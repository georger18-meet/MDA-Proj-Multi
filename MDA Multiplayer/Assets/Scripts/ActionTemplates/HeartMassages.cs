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
    private string _playerName;
    private float _cprCounter = 0f, _cprTimeLimit = 0.1f;

    private IEnumerator WaitToFinishCPR()
    {
        yield return new WaitForSeconds(4);

        _playerAnimator.SetBool("Administering Cpr", false);
        _actionTemplates.UpdatePatientLog($"{_playerName} has finished Administering Heart Massages");
        _cprTimeLimit = 0f;
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

                StartCoroutine(WaitToFinishCPR());
                _playerName = photonView.Owner.NickName;

                _actionTemplates.UpdatePatientLog($"{photonView.Owner.NickName} is Administering Heart Massages");
                Debug.Log("Operating Heart Massage On " /*+ _actionData.Patient.name*/);
            }
        }
    }
}
