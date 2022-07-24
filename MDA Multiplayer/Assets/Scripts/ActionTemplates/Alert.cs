using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Alert : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private string _alertTitle;
    [SerializeField] private string _alertText;

    public void ShowAlert()
    {
        // loops through all players photonViews
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            // execute only if this instance if of the local player
            if (photonView.IsMine)
            {
                // Get local PlayerData
                PlayerData localPlayerData = photonView.GetComponent<PlayerData>();

                // check if local player joined with a Patient
                if (!localPlayerData.CurrentPatientNearby.IsPlayerJoined(localPlayerData))
                    return;

                Patient currentPatient = localPlayerData.CurrentPatientNearby;
                PatientData currentPatientData = currentPatient.PatientData;

                ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _alertText);
                ActionTemplates.Instance.UpdatePatientLog(localPlayerData.CrewIndex, localPlayerData.UserName, $"Removed foreign object from {currentPatientData.Name} {currentPatientData.SureName}");

                break;
            }
        }
    }
}
