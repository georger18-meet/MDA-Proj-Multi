using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeClothing : Action
{
    [Header("Component's Data")]
    [SerializeField] private Clothing _clothing;

    [Header("Alerts")]
    [SerializeField] private string _alertTitle, _alertContent;

    [Header("Conditions")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    public void ChangeClothingAction()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            CurrentPatient.PhotonView.RPC("ChangeClothingRPC", RpcTarget.AllBufferedViaServer, (int)_clothing);

            TextToLog = $"Patient's {_alertTitle} is: {_alertContent}";

            if (_showAlert)
            {
                ShowTextAlert(_alertTitle, _alertContent);
            }

            if (_updateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
