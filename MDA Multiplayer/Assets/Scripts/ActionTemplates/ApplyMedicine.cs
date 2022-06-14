using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ApplyMedicine : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerActions _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private string _medicineToApply;
    [SerializeField] private string  _measurementTitle, _alertTitle;
    [SerializeField] private int _newMeasurement;

    public void ApplyMedicineAction(int measurementNumber)
    {
        foreach (PhotonView photonView in GameManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                // loops throughout measurementList and catches the first element that is equal to measurementNumber
                Measurements measurements = GameManager.Instance.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);
                desiredPlayerData.CurrentPatientNearby.PatientData.SetMeasurementName(measurementNumber, _newMeasurement);

                _actionTemplates.ShowAlertWindow(_alertTitle, _medicineToApply);
                _actionTemplates.UpdatePatientLog($"Applied {_medicineToApply} on Patient");
            }
        }
    }
}
