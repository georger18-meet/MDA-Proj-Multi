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
        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            PlayerData myPlayerData = ActionsManager.Instance.AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();

            if (!myPlayerData.CurrentPatientNearby.IsPlayerJoined(myPlayerData))
                return;

            Patient currentPatient = myPlayerData.CurrentPatientNearby;
            PatientData currentPatientData = currentPatient.PatientData;
            currentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.AllBufferedViaServer, measurementNumber, _newMeasurement);

            if (_showAlert)
            {
                ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _medicineToApply);
            }

            if (_updateLog)
            {
                ActionTemplates.Instance.UpdatePatientLog($"<{PhotonNetwork.NickName}>", $"{_alertTitle} {_medicineToApply} on {currentPatientData.Name} {currentPatientData.SureName}");
            }
            break;
        }
    }
}
