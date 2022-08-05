using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeMeasurement : Action
{
    [Header("Component's Data")]
    [SerializeField] private int _newMeasurement;
    [SerializeField] private Measurements _measurement; // will make thing easier in editor

    public void ChangeMeasurementAction(int measurementNumber)
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            int measurementNum = (int)_measurement; // will make thing easier in editor
            CurrentPatient.PhotonView.RPC("SetMeasurementByIndexRPC", RpcTarget.All, measurementNumber, _newMeasurement);

            TextToLog = $"Patient's {_newMeasurement} was changed";

            if (_shouldUpdateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
