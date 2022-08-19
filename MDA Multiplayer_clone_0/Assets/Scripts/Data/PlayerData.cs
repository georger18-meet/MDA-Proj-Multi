using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Photon.Pun;

public class PlayerData : MonoBehaviour
{
    public PhotonView PhotonView => gameObject.GetPhotonView();
    public bool IsJoinedNearbyPatient { get => CurrentPatientNearby.IsPlayerJoined(this); }

    [field: SerializeField] public string UserName { get; set; }
    [field: SerializeField] public string CrewName { get; set; }
    [field: SerializeField] public int UserIndexInCrew { get; set; }
    [field: SerializeField] public int CrewIndex { get; set; }
    [field: SerializeField] public bool IsCrewLeader { get; set; }
    [field: SerializeField] public bool IsInstructor { get; set; }
    [field: SerializeField] public bool IsRefua10 { get; set; }
    [field: SerializeField] public bool IsHenyon10 { get; set; }
    [field: SerializeField] public bool IsPinoye10 { get; set; }
    [field: SerializeField] public Roles UserRole { get; set; }
    [field: SerializeField] public Color CrewColor { get; set; }
    [field: SerializeField] public Patient CurrentPatientNearby { get; set; }
    [field: SerializeField] public Animation PlayerAnimation { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        ActionsManager.Instance.AllPlayersPhotonViews.Remove(PhotonView);
    }

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        PhotonView.RPC("AddingPlayerToAllPlayersList",RpcTarget.AllBufferedViaServer);

    }

    [PunRPC]
    void AddingPlayerToAllPlayersList()
    {
        ActionsManager.Instance.AllPlayersPhotonViews.Add(PhotonView);

    }

    private void Update()
    {
        if (IsInstructor)
        {
            gameObject.AddComponent<Instructor>();
            ActionsManager.Instance.AllPlayerData.Add(this);
        }
        else if (TryGetComponent(out Instructor instructor))
        {
            if (instructor)
            {
                ActionsManager.Instance.AllPlayerData.Remove(this);
                Destroy(instructor);
            }
        }
    }

    //public void DisconnectButton()
    //{
    //    ActionsManager.Instance.AllPlayersPhotonViews.Remove(PhotonView);

    //    GameManager.Instance.DisconnectPlayer();
    //}

    #region PunRPC invoked by Player
    [PunRPC]
    private void OnJoinPatient()
    {
        CurrentPatientNearby.PhotonView.RPC("AddUserToTreatingLists", RpcTarget.AllBufferedViaServer, UserName);
    }

    [PunRPC]
    private void OnLeavePatient()
    {
        Debug.Log("Attempting leave patient");
        CurrentPatientNearby.TreatingUsers.Remove(this);
        Debug.Log("Left Patient Succesfully");
    }
    #endregion
}