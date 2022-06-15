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

    public void ApplyMedicineAction(int measurementNumber)
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                // loops throughout measurementList and catches the first element that is equal to measurementNumber
                Measurements measurements = ActionsManager.Instance.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);
                desiredPlayerData.CurrentPatientNearby.PatientData.SetMeasurementName(measurementNumber, _newMeasurement);

                ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _medicineToApply);
                ActionTemplates.Instance.UpdatePatientLog($"Applied {_medicineToApply} on Patient");
            }
        }
    }
}
