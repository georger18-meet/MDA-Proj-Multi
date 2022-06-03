using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CheckMeasurement : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private string _measurementTitle;

    [SerializeField] private List<Measurements> measurementList;
       // list string of all actions
       // foreach of the action if the action name is the string
       // choose action

    private int _measurement;

    public void CheckMeasurementAction(int measurementNumber)
    {
        if (!_actionManager.CheckIfPlayerJoined())
            return;

        foreach (Measurements measurement in measurementList)
        {
            if ((Measurements)measurementNumber == measurement)
            {
                // Must be on the same order as the enum list (from 0 - 8)
                _measurement = _actionManager.CurrentPatientData.GetMeasurementName(measurementNumber);
            }
        }

        //switch (_measurementTitle.ToLower())
        //{
        //    case "bpm":
        //        _measurementTitle = "Heart Rate:";
        //        _measurement = _actionManager.CurrentPatientData.BPM;
        //        break;
        //
        //    case "pain level":
        //        _measurementTitle = "pain level:";
        //        _measurement = _actionManager.CurrentPatientData.PainLevel;
        //        break;
        //
        //    case "respiratory rate":
        //        _measurementTitle = "respiratory rate:";
        //        _measurement = _actionManager.CurrentPatientData.RespiratoryRate;
        //        break;
        //
        //    case "cincinnati level":
        //        _measurementTitle = "cincinnati level:";
        //        _measurement = _actionManager.CurrentPatientData.CincinnatiLevel;
        //        break;
        //
        //    case "blood suger":
        //        _measurementTitle = "blood suger:";
        //        _measurement = _actionManager.CurrentPatientData.BloodSuger;
        //        break;
        //
        //    case "blood pressure":
        //        _measurementTitle = "blood pressure:";
        //        _measurement = _actionManager.CurrentPatientData.BloodPressure;
        //        break;
        //
        //    case "oxygen saturation":
        //        _measurementTitle = "oxygen saturation:";
        //        _measurement = _actionManager.CurrentPatientData.OxygenSaturation;
        //        break;
        //
        //    case "etco2":
        //        _measurementTitle = "ETCO2:";
        //        _measurement = _actionManager.CurrentPatientData.ETCO2;
        //        break;
        //
        //    default:
        //        break;
        //}

        _actionTemplates.ShowAlertWindow(_measurementTitle, _measurement);
        _actionTemplates.UpdatePatientLog($"Patient's {_measurementTitle} is: {_measurement}");
    }
}
