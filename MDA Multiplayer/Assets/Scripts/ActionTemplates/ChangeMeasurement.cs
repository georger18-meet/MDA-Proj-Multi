using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeMeasurement : Action
{
    [Header("Component's Data")]
    [SerializeField] private int _newMeasurement;

    [Header("Alert")]
    [SerializeField] private string _alertTitle, _alertText;

    [Header("Conditions")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    public void ChangeMeasurementAction(int measurementNumber)
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            CurrentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, measurementNumber, _newMeasurement);

            TextToLog = $"Patient's {_alertText} was changed";

            if (_showAlert)
            {
                ShowTextAlert(_alertTitle, _alertText);
            }

            if (_updateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
