using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckMeasurement : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private string _measurementTitle;

    [SerializeField] private List<Measurements> measurementList;

    private int _measurement;

    public void CheckMeasurementAction(int measurementNumber)
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                // loops throughout measurementList and catches the first element that is equal to measurementNumber
                Measurements measurements = _actionManager.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);
                _measurement = desiredPlayerData.CurrentPatientNearby.PatientData.GetMeasurementName(measurementNumber);

                _actionTemplates.ShowAlertWindow(_measurementTitle, _measurement);
                _actionTemplates.UpdatePatientLog($"Patient's {_measurementTitle} is: {_measurement}");
            }
        }
    }
}
