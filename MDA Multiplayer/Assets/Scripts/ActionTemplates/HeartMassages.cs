using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeartMassages : MonoBehaviour
{
    private Animator _playerAnimator;
    private string _playerName;

    private IEnumerator WaitToFinishCPR()
    {
        yield return new WaitForSeconds(4);

        _playerAnimator.SetBool("Administering Cpr", false);
        ActionTemplates.Instance.UpdatePatientLog($"{_playerName} has finished Administering Heart Massages");
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

                //desiredPlayerData.CurrentPatientNearby.PatientData.BloodPressure = 64;

                StartCoroutine(WaitToFinishCPR());
                _playerName = photonView.Owner.NickName;

                ActionTemplates.Instance.UpdatePatientLog($"{photonView.Owner.NickName} is Administering Heart Massages");
                Debug.Log("Operating Heart Massage On " /*+ _actionData.Patient.name*/);
                break;
            }
        }
    }
}
