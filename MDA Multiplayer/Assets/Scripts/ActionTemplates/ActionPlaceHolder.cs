using UnityEngine;
using Photon.Pun;

public class ActionPlaceHolder : MonoBehaviour
{
    [Header("Component's Data")]
    [SerializeField] private string _string;

    [Header("Conditions")]
    [SerializeField] private bool _shouldUpdateLog = true;

    public void DoAction()
    {
        // loops through all players photonViews
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            // execute only if this instance if of the local player
            if (photonView.IsMine)
            {
                // Get local PlayerData
                PlayerData localPlayerData = photonView.GetComponent<PlayerData>();

                // check if local player joined with a Patient
                if (!localPlayerData.CurrentPatientNearby.IsPlayerJoined(localPlayerData))
                    return;

                // get Patient & PatientData
                Patient currentPatient = localPlayerData.CurrentPatientNearby;
                PatientData currentPatientData = currentPatient.PatientData;

                // invoke RPC with this action's logic
                currentPatient.PhotonView.RPC("", RpcTarget.AllBufferedViaServer, _string);

                // will update the log if true
                if (_shouldUpdateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog(localPlayerData.CrewIndex, localPlayerData.UserName, $"Text to Log");
                }

                // if found local player no need for loop to continue
                break;
            }
        }
    }
}
