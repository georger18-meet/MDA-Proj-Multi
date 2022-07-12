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
        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            PlayerData myPlayerData = ActionsManager.Instance.AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();

            if (!myPlayerData.CurrentPatientNearby.IsPlayerJoined(myPlayerData))
                return;

            Patient currentPatient = myPlayerData.CurrentPatientNearby;

            ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _alertText);
            ActionTemplates.Instance.UpdatePatientLog(PhotonNetwork.NickName, $"{myPlayerData.UserName} Removed foreign object from {currentPatient.PatientData.SureName} {currentPatient.PatientData.LastName}");
        }
    }
}
