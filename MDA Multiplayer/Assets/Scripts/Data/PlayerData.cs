using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Roles { CFR, Medic, SeniorMedic, Paramedic, Doctor }
public enum AranRoles { None, HeadMokdan, Mokdan, Pikud10, Refua10, Henyon10, Pinuy10 }

public class PlayerData : MonoBehaviour
{
    public PhotonView PhotonView => gameObject.GetPhotonView();
    public bool IsJoinedNearbyPatient => CurrentPatientNearby.IsPlayerJoined(this);

    [field: SerializeField] public string UserName { get; set; }
    [field: SerializeField] public string CrewName { get; set; }
    [field: SerializeField] public int UserIndexInCrew { get; set; }
    [field: SerializeField] public int CrewIndex { get; set; }
    [field: SerializeField] public bool IsCrewLeader { get; set; }
    [field: SerializeField] public bool IsInstructor { get; set; }
    [field: SerializeField] public bool IsMetargel { get; set; }
    [field: SerializeField] public bool IsMokdan { get; set; }
    [field: SerializeField] public bool IsPikud10 { get; set; }
    [field: SerializeField] public bool IsRefua10 { get; set; }
    [field: SerializeField] public bool IsHenyon10 { get; set; }
    [field: SerializeField] public bool IsPinuy10 { get; set; }
    [field: SerializeField] public Roles UserRole { get; set; }
    [field: SerializeField] public AranRoles AranRole { get; set; }
    [field: SerializeField] public Color CrewColor { get; set; }
    [field: SerializeField] public Patient CurrentPatientNearby { get; set; }
    [field: SerializeField] public Animation PlayerAnimation { get; set; }

    #region MonobehaviourCallbacks
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonView.RPC("AddingPlayerToAllPlayersList", RpcTarget.AllBufferedViaServer);

        AranRole = AranRoles.None;
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
    private void OnDestroy()
    {
        ActionsManager.Instance.AllPlayersPhotonViews.Remove(PhotonView);
    }
    #endregion

    #region Private Methods
    private void TryClearAranRoleByOrder()
    {
        if (TryGetComponent(out Mokdan mokdan))
        {
            ClearAranRole(mokdan);
        }
        else if (TryGetComponent(out Pikud10 pikud10))
        {
            ClearAranRole(pikud10);
        }
        else if (TryGetComponent(out Refua10 refua10))
        {
            ClearAranRole(refua10);
        }
        else if (TryGetComponent(out Henyon10 henyon10))
        {
            ClearAranRole(henyon10);
        }
        else if (TryGetComponent(out Pinuy10 pinuy10))
        {
            ClearAranRole(pinuy10);
        }
    }
    private void ClearAranRole(Mokdan mokdan)
    {
        if (mokdan = GetComponent<Mokdan>())
            Destroy(mokdan);
    }
    private void ClearAranRole(Pikud10 pikud10)
    {
        if (pikud10 = GetComponent<Pikud10>())
            Destroy(pikud10);
    }
    private void ClearAranRole(Refua10 refua10)
    {
        if (refua10 = GetComponent<Refua10>())
            Destroy(refua10);
    }
    private void ClearAranRole(Pinuy10 pinuy10)
    {
        if (pinuy10 = GetComponent<Pinuy10>())
            Destroy(pinuy10);
    }

    private void ClearAranRole(Henyon10 henyon10)
    {
        if (henyon10 = GetComponent<Henyon10>())
            Destroy(henyon10);
    }
    #endregion

    #region Public Methods
    public void AssignAranRole(AranRoles newRole)
    {
        switch (newRole)
        {
            case AranRoles.None:
                TryClearAranRoleByOrder();
                break;
            case AranRoles.HeadMokdan:
                gameObject.AddComponent<Mokdan>();
                IsMokdan = false;
                AranRole = newRole;
                break;
            case AranRoles.Mokdan:
                gameObject.AddComponent<Mokdan>();
                IsMokdan = false;
                AranRole = newRole;
                break;
            case AranRoles.Pikud10:
                gameObject.AddComponent<Pikud10>();
                IsPikud10 = false;
                AranRole = newRole;
                break;
            case AranRoles.Refua10:
                gameObject.AddComponent<Refua10>();
                IsRefua10 = false;
                AranRole = newRole;
                break;
            case AranRoles.Henyon10:
                gameObject.AddComponent<Henyon10>();
                IsHenyon10 = false;
                AranRole = newRole;
                break;
            case AranRoles.Pinuy10:
                gameObject.AddComponent<Pinuy10>();
                IsPinuy10 = false;
                AranRole = newRole;
                break;
            default:
                break;
        }
    }
    #endregion
    #region PunRPC invoked by Player
    [PunRPC]
    void AddingPlayerToAllPlayersList()
    {
        ActionsManager.Instance.AllPlayersPhotonViews.Add(PhotonView);
    }

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

    [PunRPC]
    private void AssignAranRoleRPC(int newRoleIndex)
    {
        AranRoles newRole = (AranRoles)newRoleIndex;

        switch (newRole)
        {
            case AranRoles.None:
                TryClearAranRoleByOrder();
                break;
            case AranRoles.HeadMokdan:
                gameObject.AddComponent<Mokdan>();
                IsMokdan = false;
                AranRole = newRole;
                break;
            case AranRoles.Mokdan:
                gameObject.AddComponent<Mokdan>();
                IsMokdan = false;
                AranRole = newRole;
                break;
            case AranRoles.Pikud10:
                gameObject.AddComponent<Pikud10>();
                IsPikud10 = false;
                AranRole = newRole;
                break;
            case AranRoles.Refua10:
                gameObject.AddComponent<Refua10>();
                IsRefua10 = false;
                AranRole = newRole;
                break;
            case AranRoles.Henyon10:
                gameObject.AddComponent<Henyon10>();
                IsHenyon10 = false;
                AranRole = newRole;
                break;
            case AranRoles.Pinuy10:
                gameObject.AddComponent<Pinuy10>();
                IsPinuy10 = false;
                AranRole = newRole;
                break;
            default:
                break;
        }
    }
    #endregion
}
