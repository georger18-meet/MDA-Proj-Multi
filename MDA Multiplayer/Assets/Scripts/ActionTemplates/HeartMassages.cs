using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeartMassages : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerActions _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;
    [SerializeField] private Animator _playerAnimator;

    public void DoHeartMassage()
    {
        foreach (PhotonView photonView in GameManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                //PlayerData.Instance.transform.position = _actionManager.PlayerTreatingTr.position;
                //PlayerData.Instance.transform.rotation = _actionManager.PlayerTreatingTr.rotation;
                //_playerAnimator.Play(,)
                // change heart rate after x seconds

                _actionTemplates.UpdatePatientLog($"Performed Heart Massages");
                Debug.Log("Operating Heart Massage On " /*+ _actionData.Patient.name*/);
            }
        }
    }
}
