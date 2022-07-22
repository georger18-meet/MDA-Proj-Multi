using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckConciounceness : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private string _alertTitle;
    [SerializeField] private string _alertText;
    [SerializeField] private string _caseConciouce;
    [SerializeField] private string _caseNotConciouce;

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private string _conciouncnessState;

    public void CheckConciouncenessAction()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData localPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!localPlayerData.CurrentPatientNearby.IsPlayerJoined(localPlayerData))
                    return;

                Patient currentPatient = localPlayerData.CurrentPatientNearby;

                _conciouncnessState = currentPatient.PatientData.IsConscious ? _caseConciouce : _caseNotConciouce;

                if (_showAlert)
                {
                    ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _conciouncnessState);
                }

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog(localPlayerData.CrewIndex, localPlayerData.UserName, $"{_alertTitle} {_alertText} {_conciouncnessState}");
                }
                break;
            }
        }
    }
}
