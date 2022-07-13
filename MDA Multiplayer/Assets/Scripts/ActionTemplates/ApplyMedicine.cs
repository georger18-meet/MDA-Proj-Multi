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

    private string _alertTitle = "Applied Medicine:";

    public void OnApplyMedicineRPC(int measurementNumber)
    {
        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            PlayerData myPlayerData = ActionsManager.Instance.AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();

            if (!myPlayerData.CurrentPatientNearby.IsPlayerJoined(myPlayerData))
                return;

            Patient currentPatient = myPlayerData.CurrentPatientNearby;

            currentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.AllBufferedViaServer, measurementNumber, _newMeasurement);

            ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _medicineToApply);
            ActionTemplates.Instance.UpdatePatientLog(PhotonNetwork.NickName, $"{_alertTitle} {_medicineToApply} on {currentPatient.PatientData.Name} {currentPatient.PatientData.SureName}");
        }
    }
}
