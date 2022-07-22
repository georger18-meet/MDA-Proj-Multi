using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerTreatingAnimation : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string _animationName;
    [SerializeField] private float _animationEndTime;
    // usually 0 /2 /4
    [SerializeField] private int _patientColliderIndex;

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private Animator _playerAnimator;
    private string _playerName;

    private IEnumerator WaitToFinishAnimation()
    {
        yield return new WaitForSeconds(_animationEndTime);

        _playerAnimator.SetBool(_animationName, false);
        //ActionTemplates.Instance.UpdatePatientLog($"<{PhotonNetwork.NickName}>", $"{_playerName} has finished {_animationName}");
    }

    public void PlayAnimation()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData localPlayerData = photonView.GetComponent<PlayerData>();

                if (!localPlayerData.CurrentPatientNearby.IsPlayerJoined(localPlayerData))
                    return;

                Patient currentPatient = localPlayerData.CurrentPatientNearby;
                Transform patientColliderTransform = currentPatient.transform.GetChild(1).GetChild(_patientColliderIndex);
                _playerAnimator = localPlayerData.gameObject.transform.GetChild(5).GetComponent<Animator>();

                localPlayerData.transform.SetPositionAndRotation(patientColliderTransform.position, patientColliderTransform.rotation);

                _playerAnimator.SetBool(_animationName, true);
                StartCoroutine(WaitToFinishAnimation());

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog(localPlayerData.CrewIndex, localPlayerData.UserName, $" is Administering Heart Massages");
                }
                break;
            }
        }
    }
}
