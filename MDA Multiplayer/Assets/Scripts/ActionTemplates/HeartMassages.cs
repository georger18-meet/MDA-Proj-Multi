using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeartMassages : MonoBehaviour
{
    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private Animator _playerAnimator;
    private string _playerName;

    private IEnumerator WaitToFinishCPR()
    {
        yield return new WaitForSeconds(4);

        _playerAnimator.SetBool("Administering Cpr", false);
        ActionTemplates.Instance.UpdatePatientLog(PhotonNetwork.NickName, $"{_playerName} has finished Administering Heart Massages");
    }

    public void DoHeartMassage()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                Patient currentPatient = desiredPlayerData.CurrentPatientNearby;
                Transform patientColliderTransform = currentPatient.transform.GetChild(1).GetChild(0);

                _playerAnimator = desiredPlayerData.gameObject.transform.GetChild(5).GetComponent<Animator>();

                desiredPlayerData.transform.SetPositionAndRotation(patientColliderTransform.position, patientColliderTransform.rotation);

                _playerAnimator.SetBool("Administering Cpr", true);
                currentPatient.PhotonView.RPC("ChangeHeartRateRPC", RpcTarget.All, 64);

                // log

                StartCoroutine(WaitToFinishCPR());
                _playerName = photonView.Owner.NickName;

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog($"<{PhotonNetwork.NickName}>", $" Administering Heart Massages");
                }
                break;
            }
        }
    }
}
