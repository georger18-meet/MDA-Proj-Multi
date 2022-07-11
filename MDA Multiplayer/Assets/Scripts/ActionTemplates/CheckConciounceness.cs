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
    
    private string _conciouncnessState;

    public void CheckConciouncenessAction()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                Patient currentPatient = desiredPlayerData.CurrentPatientNearby;

                _conciouncnessState = currentPatient.PatientData.IsConscious ? _caseConciouce : _caseNotConciouce;

                ActionTemplates.Instance.ShowAlertWindow(_alertTitle, _conciouncnessState);
                ActionTemplates.Instance.UpdatePatientLog($"{_alertTitle} {_alertText} {_conciouncnessState}");
                break;
            }
        }
    }
}
