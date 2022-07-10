using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CalmPatientDown : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private string _measurementTitle;
    [SerializeField] private int _newMeasurement;

    //public void CalmPatientDownAction()
    //{
    //    foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
    //    {
    //        PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();
    //
    //        if (photonView.IsMine)
    //        {
    //            if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
    //                return;
    //
    //            // loops throughout measurementList and catches the first element that is equal to measurementNumber
    //            Measurements measurements = ActionsManager.Instance.MeasurementList.FirstOrDefault(item => item == //(Measurements)measurementNumber);
    //            desiredPlayerData.CurrentPatientNearby.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, //measurementNumber, _newMeasurement);
    //            //desiredPlayerData.CurrentPatientNearby.PatientData.SetMeasurementByIndex(measurementNumber, //_newMeasurement);
    //
    //            ActionTemplates.Instance.ShowAlertWindow(_measurementTitle, _newMeasurement);
    //            ActionTemplates.Instance.UpdatePatientLog($"Patient's {_measurementTitle} was changed");
    //        }
    //    }
    //}
}
