using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMeasurement : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager AOM;
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private string _measurementTitle;
    [SerializeField] private int _newMeasurement;

    public void ApplyMeasurementAction()
    {
        if (!AOM.CheckIfPlayerJoined())
            return;

        switch (_measurementTitle.ToLower())
        {
            case "bpm":
                _measurementTitle = "bpm:";
                AOM.CurrentPatientData.BPM = _newMeasurement;
                break;

            case "pain level":
                _measurementTitle = "pain level:";
                AOM.CurrentPatientData.PainLevel = _newMeasurement;
                break;

            case "respiratory rate":
                _measurementTitle = "respiratory rate:";
                AOM.CurrentPatientData.RespiratoryRate = _newMeasurement;
                break;

            case "cincinnati level":
                _measurementTitle = "cincinnati level:";
                AOM.CurrentPatientData.CincinnatiLevel = _newMeasurement;
                break;

            case "blood suger":
                _measurementTitle = "blood suger:";
                AOM.CurrentPatientData.BloodSuger = _newMeasurement;
                break;

            case "blood pressure":
                _measurementTitle = "blood pressure:";
                AOM.CurrentPatientData.BloodPressure = _newMeasurement;
                break;

            case "oxygen saturation":
                _measurementTitle = "oxygen saturation:";
                AOM.CurrentPatientData.OxygenSaturation = _newMeasurement;
                break;

            case "etco2":
                _measurementTitle = "ETCO2:";
                AOM.CurrentPatientData.ETCO2 = _newMeasurement;
                break;

            default:
                break;
        }

        _actionTemplates.ShowAlertWindow(_measurementTitle, _newMeasurement);
        _actionTemplates.UpdatePatientLog($"Patient's {_measurementTitle} was changed");
    }
}
