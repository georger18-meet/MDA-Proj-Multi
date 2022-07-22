using UnityEngine;
using Photon.Pun;

public class Action : MonoBehaviour
{
    [Header("Player Data")]
    protected PhotonView LocalPlayerPhotonView;
    protected PlayerData LocalPlayerData;

    [Header("Currently joined Patient's Data")]
    protected Patient CurrentPatient;
    protected PatientData CurrentPatientData;

    [Header("Conditions")]
    [SerializeField] protected bool _shouldUpdateLog = true;

    [Header("Documentaion")]
    protected int LocalPlayerCrewIndex;
    protected string LocalPlayerName;
    protected string TextToLog;

    public void GetActionData()
    {
        // loops through all players photonViews
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            // execute only if this instance if of the local player
            if (photonView.IsMine)
            {
                LocalPlayerPhotonView = photonView;
                // Get local PlayerData
                LocalPlayerData = photonView.GetComponent<PlayerData>();

                // check if local player joined with a Patient
                if (!LocalPlayerData.CurrentPatientNearby.IsPlayerJoined(LocalPlayerData))
                    return;

                // get Patient & PatientData
                CurrentPatient = LocalPlayerData.CurrentPatientNearby;
                CurrentPatientData = CurrentPatient.PatientData;

                // if found local player no need for loop to continue
                break;
            }
        }
    }
}
