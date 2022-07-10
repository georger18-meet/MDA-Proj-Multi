using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeClothing : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private Clothing _clothing;
    [SerializeField] private string _textureToChange, _alertContent;

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

                _actionTemplates.ShowAlertWindow(_textureToChange, _alertContent);
                _actionTemplates.UpdatePatientLog($"Patient's {_textureToChange} is: {_alertContent}");
            }
        }
    }
}
