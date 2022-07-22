using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ApplyMedicine : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private string _medicineToApply;
    [SerializeField] private string  _measurementTitle;
    [SerializeField] private int _newMeasurement;

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private string _alertTitle = "Applied Medicine:";

    public void OnApplyMedicineRPC(int measurementNumber)
    {
        // loops through all players photonViews
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            // execute only if this instance if of the local player
            if (photonView.IsMine)
            {
                // Get local PlayerData
                PlayerData localPlayerData = photonView.GetComponent<PlayerData>();

                // check if local player joined with a Patient
                if (!localPlayerData.CurrentPatientNearby.IsPlayerJoined(localPlayerData))
                    return;

                Patient currentPatient = localPlayerData.CurrentPatientNearby;
                PatientData currentPatientData = currentPatient.PatientData;

                currentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.AllBufferedViaServer, measurementNumber, _newMeasurement);

                if (_showAlert)
                {
                    ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _medicineToApply);
                }

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog(localPlayerData.CrewIndex, localPlayerData.UserName, $"{_alertTitle} {_medicineToApply} on    {currentPatientData.Name} {currentPatientData.SureName}");
                }

                break;
            }
        }
    }
}
