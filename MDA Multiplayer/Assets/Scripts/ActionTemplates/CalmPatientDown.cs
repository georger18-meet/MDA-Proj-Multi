using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CalmPatientDown : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private int _calmDownHeartRateBy = 0;
    [SerializeField] private int _calmDownRespiratoryRateBy;

    private string _alertTitle, _alertText;
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
                _newRespiratoryRate = currentPatient.PatientData.HeartRateBPM - _calmDownHeartRateBy;

                currentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, _heartRateIndex, _newHeartRate);

                currentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, _respiratoryRate, _newRespiratoryRate);

                ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _alertText);
                ActionTemplates.Instance.UpdatePatientLog($"Patient's {patientName} has calmed down a bit");
                break;
            }
        }
    }
}
