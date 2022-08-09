using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBandage : Action
{
    [SerializeField] private bool _useTourniquetInstead = false;
    public void PlaceBandageAction()
    {
        GetActionData();

        TextToLog = "Placed Bandaid on Patient";

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            CurrentPatient.SetUnusedBandages(true);
            CurrentPatient.UseTourniquet = _useTourniquetInstead;

            if (_shouldUpdateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
