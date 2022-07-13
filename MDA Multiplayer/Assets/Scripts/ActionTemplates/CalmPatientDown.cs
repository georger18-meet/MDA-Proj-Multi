using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CalmPatientDown : MonoBehaviour
{
    [Header("Alert Content")]
    [SerializeField] private string _alertTitle;
    [SerializeField] private string _alertText;

    [Header("Component's Data")]
    [SerializeField] private int _calmDownHeartRateBy = 0;
    [SerializeField] private int _calmDownRespiratoryRateBy;

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;


    private int _newHeartRate, _newRespiratoryRate;
    private int _heartRateIndex = 0, _respiratoryRate = 2;

    public void CalmPatientDownAction()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                Patient currentPatient = desiredPlayerData.CurrentPatientNearby;
                string patientName = currentPatient.PhotonView.Owner.NickName;

                _newHeartRate = currentPatient.PatientData.HeartRateBPM - _calmDownHeartRateBy;
                _newRespiratoryRate = currentPatient.PatientData.HeartRateBPM - _calmDownRespiratoryRateBy;

                currentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, _heartRateIndex, _newHeartRate);

                currentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, _respiratoryRate, _newRespiratoryRate);

                if (_showAlert)
                {
                    ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _alertText);
                }

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog($"<{PhotonNetwork.NickName}>", $"Patient's {patientName} has calmed down a bit");
                }
                break;
            }
        }
    }
}
