using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CalmPatientDown : Action
{
    [Header("Alert Content")]
    [SerializeField] private string _alertTitle;
    [SerializeField] private string _alertContent;

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
        CurrentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, _heartRateIndex, _newHeartRate);
        CurrentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, _respiratoryRate, _newRespiratoryRate);

        TextToLog = $"Patient's {_alertTitle} is: {_alertContent}";

        if (_showAlert)
        {
            ShowTextAlert(_alertTitle, _alertContent);
        }

        if (_updateLog)
        {
            LogText(TextToLog);
        }
    }
}
