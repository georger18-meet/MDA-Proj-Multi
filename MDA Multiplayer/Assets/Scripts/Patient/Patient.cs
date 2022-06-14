using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using WebSocketSharp;

public class Patient : MonoBehaviourPun
{
    #region Script References
    [Header("Data & Scripts")]
    public PatientData PatientData;
    public List<ActionSequence> ActionSequences;
    #endregion

    #region Material References
    [SerializeField] private Material InitialShirt, InitialPants;
    [SerializeField] private Renderer Shirt, Pants;
    #endregion

    #region Public fields
    public Animation PatientAnimation;
    #endregion

    #region private serialized fields
    [Header("Joined Crews & Players Lists")]
    public List<PlayerData> NearbyUsers;
    public List<PlayerData> TreatingUsers;
    public List<int> TreatingCrews;

    [Header("Treaters BackLog")]
    public List<PlayerData> AllUsersTreatedThisPatient;
    public List<int> AllCrewTreatedThisPatient;
    #endregion


    public float playerDistance;
    public OwnershipTransfer Transfer;
    private PhotonView _photonView;

    //Chen's Tests
    public List<int> TreatingUsersTest;

    public List<PlayerController> players;
    //   private PhotonNetwork currentPlayerData;
    public PhotonView GetphotonView => _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        //  Transfer = GetComponent<OwnershipTransfer>();
    }

    private void Start()
    {
        ActionsManager.Instance.AllPatients.Add(this);
        PatientData.PatientShirtMaterial = InitialShirt;
        PatientData.PatientPantsMaterial = InitialPants;

        players = new List<PlayerController>();
    }


    

    private void Update()
    {
        Shirt.material = PatientData.PatientShirtMaterial;
        Pants.material = PatientData.PatientPantsMaterial;


        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("TreatingUsersTest List is : " + "" + TreatingUsersTest.Count);
            Debug.Log("players List is : " + "" + players.Count);
        }
    }


    public void AddUserToTreatingLists(int currentPlayer)
    {
        if (!_photonView.IsMine)
            return;

        Debug.Log("currentPlayer Id IS : " + " " + currentPlayer);

       
        players[players.Count-1].GetphotonView.RPC("RPC_AddUserToTreatingLists",RpcTarget.AllViaServer, currentPlayer);


        
    }



    
    private void RPC_AddUserToTreatingLists(int currentPlayer)
    {

        Player currentPlayerData = PhotonNetwork.LocalPlayer.Get(currentPlayer);
            Debug.Log("currentPlayerData ID" + " " + currentPlayerData);

            TreatingUsersTest.Add(currentPlayerData.ActorNumber);
        

        //if (_photonView.AmOwner)
        //{
        //    PhotonView currentPlayerData = PhotonView.Find(currentPlayer);
        //    Debug.Log("currentPlayerData ID" + " " + currentPlayerData);

        //    TreatingUsersTest.Add(currentPlayerData.ViewID);
        //}


        //for (int i = 0; i < TreatingUsersTest.Count; i++)
        //{
        //    if (TreatingUsersTest.Contains(currentPlayerData.ViewID))
        //    {
        //        Debug.Log("Didnt add To List");

        //        continue;
        //    }
        //    TreatingUsersTest.Add(currentPlayerData.ViewID);
        //        // AllUsersTreatedThisPatient.Add(currentPlayerData);
        //        Debug.Log("Added To List");



        //}
    }


    public List<PlayerData> ConvertList(PlayerData[] PlayerDataArray)
    {
        int counter = PlayerDataArray.Length;

        List<PlayerData> playerlist = new List<PlayerData>();

        for (int i = 0; i < counter; i++)
        {

            playerlist.Add(PlayerDataArray[i]);


        }

        return playerlist;

    }

        [PunRPC]
    private void RPC_AddUserToTreatingLists(object currentPlayer)
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerController possiblePlayer = other.GetComponent<PlayerController>();
            if (!players.Contains(possiblePlayer))
            {
                players.Add(possiblePlayer);
                Debug.Log("DINGGGGGGGGGGGG" +" "+ possiblePlayer);
            }
        }




        //if (possiblePlayer == null)
        //{
        //    return;
        //}
        //else if (!NearbyUsers.Contains(possiblePlayer))
        //{
        //    NearbyUsers.Add(possiblePlayer);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController possiblePlayer = other.GetComponent<PlayerController>();
            if (players.Contains(possiblePlayer))
            {
                players.Remove(possiblePlayer);
            }
        }
        //PlayerData possiblePlayer = other.GetComponent<PlayerData>();

        //if (possiblePlayer != null)
        //{
        //    if (!NearbyUsers.Contains(possiblePlayer))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        NearbyUsers.Remove(possiblePlayer);
        //    }
        //}
    }


    //public Collider GetPlayerCollider()
    //{
    //    _origin = transform.position;

    //    Physics.queriesHitTriggers = false;
    //    RaycastHit[] hits = Physics.SphereCastAll(_origin, sphereRadius, _direction, _maxDistance);
    //    RaycastHit hit;

    //    foreach  (out hit in hits)
    //    {
    //        currentHitObjects.Add(hit.transform.gameObject);
    //        _currentHitDistance = hit.distance;
    //    }

    //    return hits.;


    //    if (TryGetComponent(out PlayerData playerData))
    //    {
            
    //    }

    //    return null;
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
        // Transfer.ClickToJoinPatient();
        
            ActionsManager.Instance.OnPatientClicked();

        

    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting) //if its the owner does things
    //    {

    //        stream.SendNext(PhotonNetwork.LocalPlayer.ActorNumber);
    //        // stream.SendNext(currentPlayerData.Owner.TagObject);
    //    }
    //    else if (stream.IsReading) // if its the other clients
    //    {
    //        PhotonNetwork.LocalPlayer.ActorNumber = (int)stream.ReceiveNext();
    //        //currentPlayerData.Owner.TagObject = (Player)stream.ReceiveNext();
    //    }
    //}


    public PlayerData[] ListSender(List<PlayerData> PlayerDataList)
    {
        int listcounter = PlayerDataList.Count;

        PlayerData[] player = new PlayerData[listcounter];


        for (int i = 0; i < player.Length; i++)
        {
            player[i] = PlayerDataList[i];


        }

        return player;
    }



}
