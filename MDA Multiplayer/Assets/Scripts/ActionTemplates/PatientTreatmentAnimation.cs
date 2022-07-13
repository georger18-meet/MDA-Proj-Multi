using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PatientTreatmentAnimation : MonoBehaviour
{
    [Header("Animation's Data")]
    [SerializeField] private string _animationName;
    [SerializeField] private float _animationEndTime;

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = false;

    private Animator _patientAnimator;
    private string _patientName;

    private IEnumerator WaitToFinishAnimation()
    {
        yield return new WaitForSeconds(_animationEndTime);

        _patientAnimator.SetBool(_animationName, false);
        ActionTemplates.Instance.UpdatePatientLog(PhotonNetwork.NickName, $"{_patientName} has finished {_animationName}");
    }

    public void PlayAnimation()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                Patient currentPatient = desiredPlayerData.CurrentPatientNearby;
                PatientData currentPatientData = currentPatient.PatientData;
                _patientAnimator = currentPatient.GetComponent<Animator>();

                _patientAnimator.SetBool(_animationName, true);
                StartCoroutine(WaitToFinishAnimation());

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog($"<{PhotonNetwork.NickName}>", $" {currentPatientData.Name} is Reciving Heart Massages");
                }
                break;
            }
        }
    }
}
