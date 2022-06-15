using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerData : MonoBehaviour
{
    public PhotonView PhotonView => gameObject.GetPhotonView();

    [field: SerializeField] public string UserName { get; set; }
    [field: SerializeField] public string CrewName { get; set; }
    [field: SerializeField] public int UserIndexInCrew { get; set; }
    [field: SerializeField] public int CrewIndex { get; set; }
    [field: SerializeField] public Roles UserRole { get; set; }
    [field: SerializeField] public Patient CurrentPatientNearby { get; set; }
    [field: SerializeField] public Animation PlayerAnimation { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ActionsManager.Instance.AllPlayersPhotonViews.Add(PhotonView);
    }

    #region PunRPC invoked by Player
    [PunRPC]
    private void OnJoinPatient(bool isJoined)
    {
        if (isJoined)
        {
            CurrentPatientNearby.PhotonView.RPC("AddUserToTreatingLists", RpcTarget.AllBufferedViaServer, UserName);
            UIManager.Instance.JoinPatientPopUp.SetActive(false);
            UIManager.Instance.PatientMenuParent.SetActive(true);
            UIManager.Instance.PatientInfoParent.SetActive(false);
        }
        else
        {
            UIManager.Instance.JoinPatientPopUp.SetActive(false);
        }
    }

    [PunRPC]
    private void OnLeavePatient()
    {
        if (PhotonView.IsMine)
        {
            Debug.Log("Attempting leave patient");

            UIManager.Instance.CloseAllPatientWindows();
            CurrentPatientNearby.TreatingUsers.Remove(this);
            Debug.Log("Left Patient Succesfully");
        }
    }
    #endregion
}
