using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeMeasurement : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private string _measurementTitle;
    [SerializeField] private int _newMeasurement;

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    public void ApplyMeasurementAction(int measurementNumber)
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                Patient currentPatient = desiredPlayerData.CurrentPatientNearby;

                // loops throughout measurementList and catches the first element that is equal to measurementNumber
                Measurements measurements = ActionsManager.Instance.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);

                currentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, measurementNumber, _newMeasurement);
                //desiredPlayerData.CurrentPatientNearby.PatientData.SetMeasurementByIndex(measurementNumber, _newMeasurement);

                if (_showAlert)
                {
                    ActionTemplates.Instance.ShowAlertWindow(_measurementTitle, _newMeasurement);
                }
                
                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog($"<{PhotonNetwork.NickName}>", $"Patient's {_measurementTitle} was changed");
                }
                break;
            }
        }
    }
}
