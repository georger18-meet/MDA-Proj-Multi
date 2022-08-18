using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckMeasurement : Action
{
    [SerializeField] private bool _showAlert;
    
    [Header("Component's Data")]
    [SerializeField] private Measurements _measurement;
    [SerializeField] private string _measurementName, _measurementNameForAlert;


    private string _currentMeasurement;

    public void CheckMeasurementAction()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            _currentMeasurement = CurrentPatientData.GetMeasurement((int)_measurement);

            TextToLog = $"Checked Patient's {_measurementName}, it is {_currentMeasurement}";

            if (_showAlert)
            {
                ShowTextAlert(_measurementNameForAlert, _currentMeasurement);
            }

            if (_shouldUpdateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
