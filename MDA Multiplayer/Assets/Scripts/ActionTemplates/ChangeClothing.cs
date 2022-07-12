using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeClothing : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private Clothing _clothing;
    [SerializeField] private string _alertTitle, _alertText;

    public void ChangeClothingAction()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                desiredPlayerData.CurrentPatientNearby.PhotonView.RPC("ChangeClothingRPC", RpcTarget.All, (int)_clothing);

                ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _alertText);
                ActionTemplates.Instance.UpdatePatientLog(PhotonNetwork.NickName, $"Patient's {_alertTitle} is: {_alertText}");
            }
        }
    }
}
