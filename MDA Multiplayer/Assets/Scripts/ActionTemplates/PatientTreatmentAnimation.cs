using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PatientTreatmentAnimation : MonoBehaviour
{
    [SerializeField] private string _animationName;
    [SerializeField] private float _animationWaitTime;

    private Animator _patientAnimator;
    private string _patientName;

    private IEnumerator WaitToFinishAnimation()
    {
        yield return new WaitForSeconds(_animationWaitTime);

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
                _patientAnimator = currentPatient.GetComponent<Animator>();

                _patientAnimator.SetBool(_animationName, true);
                StartCoroutine(WaitToFinishAnimation());

                _patientName = photonView.Owner.NickName;
                ActionTemplates.Instance.UpdatePatientLog(PhotonNetwork.NickName, $"{photonView.Owner.NickName} is Administering Heart Massages");
                Debug.Log("Operating Heart Massage On " /*+ _actionData.Patient.name*/);
                break;
            }
        }
    }
}
