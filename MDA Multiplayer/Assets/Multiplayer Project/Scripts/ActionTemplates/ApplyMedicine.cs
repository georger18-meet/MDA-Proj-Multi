using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMedicine : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private string _medicineToApply;
    [SerializeField] private string  _measurementTitle, _alertTitle;
    [SerializeField] private int _newMeasurement;

    public void ApplyMedicineAction(int measurementNumber)
    {
        if (!PlayerData.Instance.CurrentPatientTreating.IsPlayerJoined(PlayerData.Instance))
            return;

        // loops throughout measurementList and catches the first element that is equal to measurementNumber
        Measurements measurements = _actionManager.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);
        PlayerData.Instance.CurrentPatientTreating.PatientData.SetMeasurementName(measurementNumber, _newMeasurement);

        _actionTemplates.ShowAlertWindow(_alertTitle, _medicineToApply);
        _actionTemplates.UpdatePatientLog($"Applied {_medicineToApply} on Patient");
    }
}
