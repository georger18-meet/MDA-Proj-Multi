using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeartMassages : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;

    private Animator _playerAnimator;
    private float _cprCounter = 0, _cprTimeLimit = 2.5f;

    private void Update()
    {
        StartCoroutine(WaitToFinishCPR());

        if (_playerAnimator != null && _playerAnimator.GetBool("Administering Cpr") && _cprCounter >= _cprTimeLimit)
        {
            _playerAnimator.SetBool("Administering Cpr", false);
        }
    }

    private IEnumerator WaitToFinishCPR()
    {
        if (!(_cprCounter >= _cprTimeLimit))
        {
            for (int i = 0; i < 2; i++)
            {
                _cprTimeLimit++;
                yield return new WaitForSeconds(1);
            }
        }
    }

    public void DoHeartMassage()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                _playerAnimator = desiredPlayerData.gameObject.transform.GetChild(5).GetComponent<Animator>();

                desiredPlayerData.transform.SetPositionAndRotation(desiredPlayerData.CurrentPatientNearby.transform.GetChild(1).GetChild(0).position, desiredPlayerData.CurrentPatientNearby.transform.GetChild(1).GetChild(0).rotation);

                _playerAnimator.SetBool("Administering Cpr", true);
                desiredPlayerData.CurrentPatientNearby.PatientData.BloodPressure = 64;

                _actionTemplates.UpdatePatientLog($"Performed Heart Massages", PhotonNetwork.NickName);
                Debug.Log("Operating Heart Massage On " /*+ _actionData.Patient.name*/);
            }
        }
    }
}
