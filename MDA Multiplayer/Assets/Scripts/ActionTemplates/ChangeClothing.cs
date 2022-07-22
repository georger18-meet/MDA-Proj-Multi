using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeClothing : Action
{
    [Header("Component's Data")]
    [SerializeField] private Clothing _clothing;
    [SerializeField] private string _alertTitle, _alertText;

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    public void ChangeClothingAction()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            CurrentPatient.PhotonView.RPC("ChangeClothingRPC", RpcTarget.AllBufferedViaServer, (int)_clothing);
            TextToLog = $"Patient's {_alertTitle} is: {_alertText}";
            ActionTemplates.Instance.UpdatePatientLog(LocalPlayerCrewIndex, LocalPlayerName, TextToLog);
            //LogText(TextToLog);
        }

        //foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        //{
        //    if (photonView.IsMine)
        //    {
        //        PlayerData localPlayerData = photonView.GetComponent<PlayerData>();
        //
        //        if (!localPlayerData.CurrentPatientNearby.IsPlayerJoined(localPlayerData))
        //            return;
        //
        //        //localPlayerData.CurrentPatientNearby.PhotonView.RPC("ChangeClothingRPC", RpcTarget.AllBufferedViaServer, (int)_clothing);
        //
        //        
        //        break;
        //    }
        //}
    }
}
