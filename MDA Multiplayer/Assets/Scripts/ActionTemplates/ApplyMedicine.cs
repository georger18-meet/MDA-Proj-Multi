using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ApplyMedicine : Action
{
    [Header("Component's Data")]
    [SerializeField] private int _newMeasurement;

    [Header("Alert")]
    [SerializeField] private string _medicineToApply;
    [SerializeField] private string  _measurementTitle;

    [Header("Conditions")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private string _alertTitle = "Applied Medicine:";

    public void OnApplyMedicineRPC(int measurementNumber)
    {
        // need fixing

        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            CurrentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.AllBufferedViaServer, measurementNumber, _newMeasurement);

            TextToLog = $"{_alertTitle} {_medicineToApply} on {CurrentPatientData.Name} {CurrentPatientData.SureName}";

            if (_showAlert)
            {
                ShowNumAlert(_alertTitle, _newMeasurement);
            }

            if (_updateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
