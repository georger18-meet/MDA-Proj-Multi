using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ApplyMedicine : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private string _medicineToApply;
    [SerializeField] private string  _measurementTitle, _alertTitle;
    [SerializeField] private int _newMeasurement;

    public void OnApplyMedicineRPC(int measurementNumber)
    {
        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            PlayerData myPlayerData = ActionsManager.Instance.AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();
            myPlayerData.PhotonView.RPC("OnApplyMedicine", RpcTarget.AllBufferedViaServer, measurementNumber, _newMeasurement);

            ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _medicineToApply);
            ActionTemplates.Instance.UpdatePatientLog($"Applied {_medicineToApply} on Patient");
        }
    }
}
