using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeMeasurement : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private string _measurementTitle;
    [SerializeField] private int _newMeasurement;

    public void ApplyMeasurementAction(int measurementNumber)
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                // loops throughout measurementList and catches the first element that is equal to measurementNumber
                Measurements measurements = _actionManager.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);
                desiredPlayerData.CurrentPatientNearby.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, measurementNumber, _newMeasurement);
                //desiredPlayerData.CurrentPatientNearby.PatientData.SetMeasurementByIndex(measurementNumber, _newMeasurement);

                _actionTemplates.ShowAlertWindow(_measurementTitle, _newMeasurement);
                _actionTemplates.UpdatePatientLog($"Patient's {_measurementTitle} was changed");
                break;
            }
        }
    }
}
