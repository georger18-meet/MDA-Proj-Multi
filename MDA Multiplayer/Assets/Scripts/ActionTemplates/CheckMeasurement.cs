using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckMeasurement : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private string _alertTitle;

    [SerializeField] private List<Measurements> measurementList;

    private int _measurement;

    public void CheckMeasurementAction(int measurementNumber)
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {

            if (photonView.IsMine)
            {
                PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();
                
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                // loops throughout measurementList and catches the first element that is equal to measurementNumber
                Measurements measurements = ActionsManager.Instance.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);

                _measurement = desiredPlayerData.CurrentPatientNearby.PatientData.GetMeasurementName(measurementNumber);

                Debug.Log($"{_alertTitle} {_measurement}");
                ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _measurement);
                ActionTemplates.Instance.UpdatePatientLog(PhotonNetwork.NickName, $"Patient's {_alertTitle} is: {_measurement}");
                break;
            }
        }
    }
}
