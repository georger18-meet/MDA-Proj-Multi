using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeMeasurement : Action
{
    [SerializeField] private bool _useMedicineLog;

    [Header("Component's Data")]
    [SerializeField] private string _treatmentName;
    [SerializeField] private PatientMeasurements _patientMeasurements;
    public void ChangeMeasurementAction()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            CurrentPatient.PhotonView.RPC("SetMeasurementsRPC", RpcTarget.All, _patientMeasurements.MeasurementValues);

            if (_useMedicineLog)
            {
                TextToLog = $"Patient's took & applied {_treatmentName}";
            }
            else
            {
                TextToLog = $"did {_treatmentName} on Patient";
            }

            if (_shouldUpdateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
