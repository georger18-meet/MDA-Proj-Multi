using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class Patient : MonoBehaviourPunCallbacks
{
    [Header("Photon")]
    public PhotonView PhotonView;

    #region Script References
    [Header("Data & Scripts")]
    public PatientData PatientData;
    public List<ActionSequence> ActionSequences;
    #endregion

    #region Material References
    public Renderer PatientRenderer;
    #endregion

    #region Public fields
    [Header("Joined Crews & Players Lists")]
    public List<PlayerData> NearbyUsers;
    public List<PlayerData> TreatingUsers;
    public List<PlayerData> AllUsersTreatedThisPatient;
    public List<int> TreatingCrews;
    public List<int> AllCrewTreatedThisPatient;

    [Header("Treatment Positions")]
    public Transform ChestPosPlayerTransform;
    public Transform ChestPosEquipmentTransform, HeadPosPlayerTransform, HeadPosEquipmentTransform;

    public OwnershipTransfer _transfer;
    #endregion

    //public List<PlayerController> players;
    //public List<int> TreatingUsersTest;

    #region Monovehavior Callbacks
    private void Awake()
    {
        _transfer = GetComponent<OwnershipTransfer>();
        PatientRenderer.material = PatientData.FullyClothedMaterial;
        DontDestroyOnLoad(gameObject.transform);
        
    }

    private void Start()
    {
        //players = new List<PlayerController>();
        ActionsManager.Instance.AllPatients.Add(this);
        ActionsManager.Instance.AllPatientsPhotonViews.Add(PhotonView);
    }
    #endregion

    #region Collision & Triggers

    private void OnTriggerEnter(Collider other)
    {

        if (!other.TryGetComponent(out PlayerData possiblePlayer))
        {
            return;
        }
        else if (!NearbyUsers.Contains(possiblePlayer))
        {
            NearbyUsers.Add(possiblePlayer);
            _transfer.ClickToJoinPatient();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.TryGetComponent(out PlayerData possiblePlayer))
        {

            if (!NearbyUsers.Contains(possiblePlayer))
            {
                return;
            }
            else
            {
                NearbyUsers.Remove(possiblePlayer);
            }
        }
    }
    #endregion


    #region PunRPC invoke by Patient
    [PunRPC]
    public void AddUserToTreatingLists(string currentPlayer)
    {
        PlayerData currentPlayerData = GameObject.Find(currentPlayer).GetComponent<PlayerData>();
        //PlayerData currentPlayerData = currentPlayer != null ? currentPlayer as PlayerData : null;

        if (currentPlayerData == null)
        {
            return;
        }

        for (int i = 0; i < 1; i++)
        {
            if (TreatingUsers.Contains(currentPlayerData))
            {
                continue;
            }
            else
            {
                TreatingUsers.Add(currentPlayerData);

                if (AllUsersTreatedThisPatient.Contains(currentPlayerData))
                {
                    continue;
                }
                AllUsersTreatedThisPatient.Add(currentPlayerData);
            }

            if (TreatingCrews.Contains(currentPlayerData.CrewIndex))
            {
                return;
            }
            else
            {
                TreatingCrews.Add(currentPlayerData.CrewIndex);
                AllCrewTreatedThisPatient.Add(currentPlayerData.CrewIndex);
            }
        }
    }

    [PunRPC]
    public void UpdatePatientInfoDisplay()
    {
        UIManager.Instance.SureName.text = PatientData.SureName;
        UIManager.Instance.LastName.text = PatientData.LastName;
        UIManager.Instance.Gender.text = PatientData.Gender;
        UIManager.Instance.Adress.text = PatientData.AddressLocation;
        UIManager.Instance.InsuranceCompany.text = PatientData.MedicalCompany;
        UIManager.Instance.Complaint.text = PatientData.Complaint;

        UIManager.Instance.Age.text = PatientData.Age.ToString();
        UIManager.Instance.Id.text = PatientData.Id.ToString();
        UIManager.Instance.PhoneNumber.text = PatientData.PhoneNumber.ToString();
    }
    #endregion

    //public void AddUserToTreatingLists(int currentPlayer)
    //{
    //    if (!PhotonView.IsMine)
    //        return;
    //
    //    Debug.Log("currentPlayer Id IS : " + " " + currentPlayer);
    //
    //
    //    players[players.Count - 1].GetphotonView().RPC("RPC_AddUserToTreatingLists", RpcTarget.AllBufferedViaServer, currentPlayer);
    //
    //
    //
    //}

    

    public bool IsPlayerJoined(PlayerData playerData)
    {
        Debug.Log("Attempting to check if player is joined");

        if (TreatingUsers.Contains(playerData))
        {
            Debug.Log("Checked if player is joined, it is true");
            return true;
        }
        else
        {
            Debug.Log("Checked if player is joined, it is false");
            return false;
        }
    }

    public void OnInteracted()
    {
        ActionsManager.Instance.OnPatientClicked();
    }

}
