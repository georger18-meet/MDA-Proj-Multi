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

    public void ChangeClothingAction(int clothingMaterial)
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                // loops throughout measurementList and catches the first element that is equal to measurementNumber
                Clothing clothing = ActionsManager.Instance.ClothingList.FirstOrDefault(item => item == (Clothing)clothingMaterial);

                desiredPlayerData.CurrentPatientNearby.PhotonView.RPC("ChangeClothingRPC", RpcTarget.All, clothingMaterial);

                _actionTemplates.ShowAlertWindow(_textureToChange, _alertContent);
                _actionTemplates.UpdatePatientLog($"Patient's {_textureToChange} is: {_alertContent}");
            }
        }
    }
}
