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

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    public void ChangeClothingAction()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData localPlayerData = photonView.GetComponent<PlayerData>();

                if (!localPlayerData.CurrentPatientNearby.IsPlayerJoined(localPlayerData))
                    return;

                localPlayerData.CurrentPatientNearby.PhotonView.RPC("ChangeClothingRPC", RpcTarget.AllBufferedViaServer, (int)_clothing);

                if (_showAlert)
                {
                    ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _alertText);
                }

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog(localPlayerData.CrewIndex, localPlayerData.UserName, $"Patient's {_alertTitle} is: {_alertText}");
                }
                break;
            }
        }
    }
}
