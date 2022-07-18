using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CrewRoomManager : MonoBehaviour
{
    public GameObject RoomDoorBlocker;
    public GameObject RoomCrewMenuUI;
    public TextMeshProUGUI CrewMemberNameText1, CrewMemberNameText2, CrewMemberNameText3, CrewMemberNameText4;
    public List<TMP_Dropdown> CrewMemberRoleDropDownList;
    public TMP_Dropdown CrewLeaderDropDown;

    public List<PhotonView> _playersInRoomList;
    public int _playersMaxCount = 4;
    public int _crewRoomIndex;




    private PhotonView _photonView;


    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        PopulateDropdownRoles();
        RoomCrewMenuUI.SetActive(false);
    }

    void Update()
    {

    }


    private void BlockRoomAccess()
    {
        if (_playersInRoomList.Count >= _playersMaxCount)
        {
            RoomDoorBlocker.SetActive(true);
        }
        else
        {
            RoomDoorBlocker.SetActive(false);
        }
    }

    private bool CheckIfAlreadyInList(GameObject player)
    {
        bool playerFound = false;

        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            if (_playersInRoomList[i].ViewID == player.GetPhotonView().ViewID)
            {
                playerFound = true;
            }
        }

        ;
        return playerFound;
    }

    private void RefreshCrewUITexts()
    {
        // Crew Roles UI
        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            switch (i)
            {
                case 0:
                    CrewMemberNameText1.text = _playersInRoomList[i].Owner.NickName;
                    break;
                case 1:
                    CrewMemberNameText2.text = _playersInRoomList[i].Owner.NickName;
                    break;
                case 2:
                    CrewMemberNameText3.text = _playersInRoomList[i].Owner.NickName;
                    break;
                case 3:
                    CrewMemberNameText4.text = _playersInRoomList[i].Owner.NickName;
                    break;
                default:
                    break;
            }
        }

        // Crew Leader UI
        List<string> nicknamesList = new List<string>();
        foreach (var player in _playersInRoomList)
        {
            nicknamesList.Add(player.Owner.NickName);
        }

        CrewLeaderDropDown.ClearOptions();
        CrewLeaderDropDown.AddOptions(nicknamesList);
    }

    private void PopulateDropdownRoles()
    {
        string[] roles = Enum.GetNames(typeof(Roles));
        List<string> rolesList = new List<string>(roles);

        foreach (var dropdown in CrewMemberRoleDropDownList)
        {
            dropdown.AddOptions(rolesList);
        }
    }

    public void CreateCrewSubmit()
    {
        _photonView.RPC("CrewCreateSubmit_RPC", RpcTarget.AllBufferedViaServer, GetCrewRolesByEnum());
    }

    public int[] GetCrewRolesByEnum()
    {
        int[] roles = new int[_playersInRoomList.Count];

        for (int i = 0; i < roles.Length; i++)
        {
            roles[i] = CrewMemberRoleDropDownList[i].value;
        }
        return roles;

    }

    // Show Hide MenuUI
    // --------------------
    public void ShowCrewRoomMenu()
    {
        RoomCrewMenuUI.SetActive(true);
        RefreshCrewUITexts();
    }

    public void HideCrewRoomMenu()
    {
        RoomCrewMenuUI.SetActive(false);
    }


    // Collision Methods
    // --------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _playersInRoomList.Count < _playersMaxCount &&
            !CheckIfAlreadyInList(other.gameObject))
        {
            // _playersInRoomList.Add(other.gameObject.GetPhotonView());
            _photonView.RPC("AddingToRoomList_RPC", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName);
        }
    }


    // PUN RPC Methods
    // --------------------
    [PunRPC]
    void AddingToRoomList_RPC(string currentPlayer)
    {

        PhotonView currentPlayerData = GameObject.Find(currentPlayer).GetComponent<PhotonView>();
        //PlayerData currentPlayerData = currentPlayer != null ? currentPlayer as PlayerData : null;

        if (currentPlayerData == null)
        {
            return;
        }

        for (int i = 0; i < 1; i++)
        {
            if (_playersInRoomList.Contains(currentPlayerData))
            {
                continue;
            }
            else
            {
                _playersInRoomList.Add(currentPlayerData);
            }
        }

        BlockRoomAccess();
    }

    [PunRPC]
    void CrewCreateSubmit_RPC(int[] roleIndex)
    {

        //for (int i = 0; i < _playersInRoomList.Count; i++)
        //{
        //    string[] rolesStrings = Enum.GetNames(typeof(Roles));
        //    PlayerData desiredPlayerData = _playersInRoomList[i].GetComponent<PlayerData>();

        //    for (int z = 0; z < rolesStrings.Length; z++)
        //    {
        //        if (rolesStrings[z] == CrewMemberRoleDropDownList[i].GetComponentInChildren<TextMeshProUGUI>().text)
        //        {
        //            Roles rolesTemp = new Roles();
        //            desiredPlayerData.UserRole = (Roles)Enum.GetValues(rolesTemp.GetType()).GetValue(z);
        //            desiredPlayerData.CrewIndex = _crewRoomIndex;
        //        }
        //    }
        //}


        for (int i = 0; i < roleIndex.Length; i++)
        { 
            PlayerData desiredPlayerData = _playersInRoomList[i].GetComponent<PlayerData>();
            desiredPlayerData.UserRole = (Roles)roleIndex[i];
            desiredPlayerData.CrewIndex = _crewRoomIndex;
        }

        HideCrewRoomMenu();


        //private void OnTriggerEnter(Collider other)
        //{
        //    if (!other.TryGetComponent(out PhotonView possiblePlayer))
        //    {
        //        return;
        //    }
        //    else if (!_playersInRoomList.Contains(possiblePlayer))
        //    {
        //        _playersInRoomList.Add(possiblePlayer);
        //    }
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.TryGetComponent(out PhotonView possiblePlayer))
        //    {
        //        if (!_playersInRoomList.Contains(possiblePlayer))
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            _playersInRoomList.Remove(possiblePlayer);
        //        }
        //    }
        //}
    }
}
