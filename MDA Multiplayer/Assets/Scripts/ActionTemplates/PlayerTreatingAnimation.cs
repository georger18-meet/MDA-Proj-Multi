using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerTreatingAnimation : MonoBehaviour
{
    [SerializeField] private string _animationName;
    [SerializeField] private float _animationWaitTime;
    // usually 0 /2 /4
    [SerializeField] private int _patientColliderIndex;

    private Animator _playerAnimator;
    private string _playerName;

    private IEnumerator WaitToFinishAnimation()
    {
        yield return new WaitForSeconds(_animationWaitTime);

        _playerAnimator.SetBool(_animationName, false);
        ActionTemplates.Instance.UpdatePatientLog($"{_playerName} has finished {_animationName}");
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
                Transform patientColliderTransform = currentPatient.transform.GetChild(1).GetChild(_patientColliderIndex);
                _playerAnimator = desiredPlayerData.gameObject.transform.GetChild(5).GetComponent<Animator>();

                desiredPlayerData.transform.SetPositionAndRotation(patientColliderTransform.position, patientColliderTransform.rotation);

                _playerAnimator.SetBool(_animationName, true);
                StartCoroutine(WaitToFinishAnimation());

                _playerName = photonView.Owner.NickName;
                ActionTemplates.Instance.UpdatePatientLog($"{photonView.Owner.NickName} is Administering Heart Massages");
                Debug.Log("Operating Heart Massage On " /*+ _actionData.Patient.name*/);
            }
        }
    }
}
