using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckMeasurement : Action
{
    [Header("Component's Data")]
    [SerializeField] private string _measurementNameEnglish;
    [SerializeField] private string _measurementName;
    [SerializeField] private List<Measurements> measurementList;

    [Header("Conditions")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;
    private int _measurement;

    public void CheckMeasurementAction(int measurementNumber)
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            _measurement = CurrentPatientData.GetMeasurement(measurementNumber);

            TextToLog = $"Checked {CurrentPatientData.Name} {CurrentPatientData.SureName}'s {_measurementNameEnglish}, it is {_measurement}";

            if (_showAlert)
            {
                ShowNumAlert(_measurementName, _measurement);
            }

            if (_updateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
