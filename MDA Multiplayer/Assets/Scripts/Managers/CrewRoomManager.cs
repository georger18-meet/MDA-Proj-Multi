using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.ExceptionServices;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Random = UnityEngine.Random;

public class CrewRoomManager : MonoBehaviour
{
    public GameObject RoomDoorBlocker;
    public GameObject RoomCrewMenuUI;
    public TextMeshProUGUI CrewMemberNameText1, CrewMemberNameText2, CrewMemberNameText3, CrewMemberNameText4;
    public List<TMP_Dropdown> CrewMemberRoleDropDownList;
    public TMP_Dropdown CrewLeaderDropDown;

    public List<PhotonView> _playersInRoomList;
    public int _playersMaxCount = 4;
    //public int _crewRoomIndex;
    private Color crewColor;

    public int _crewRoomIndex;
    public static int _crewRoomIndexStatic;

    private PhotonView _photonView;
    private Vector3 _vestPos = new Vector3(0f, 0.295f, -0.015f);

    private void Awake()
    {
        _crewRoomIndexStatic = 0;
        _photonView = GetComponent<PhotonView>();
        PopulateDropdownRoles();
        RoomCrewMenuUI.SetActive(false);
    }

    private void Start()
    {
        _crewRoomIndexStatic++;
        _crewRoomIndex = _crewRoomIndexStatic;
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
        var color = Random.ColorHSV();
        _photonView.RPC("CrewCreateSubmit_RPC", RpcTarget.AllBufferedViaServer, GetCrewRolesByEnum(), GetCrewLeaderIndex());
        _photonView.RPC("ChangeCrewColors", RpcTarget.AllBufferedViaServer, new Vector3(color.r, color.g, color.b));

        //_photonView.RPC("CrewLeaderIsChosen", RpcTarget.AllBufferedViaServer, GetCrewLeader());
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

    public int GetCrewLeaderIndex()
    {
        int leaderIndex = 0;

        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            if (CrewLeaderDropDown.GetComponentInChildren<TextMeshProUGUI>().text == _playersInRoomList[i].Owner.NickName)
            {
                leaderIndex = i;
            }

        }
        return leaderIndex;
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
        if (other.CompareTag("Player") && _playersInRoomList.Count < _playersMaxCount && !CheckIfAlreadyInList(other.gameObject))
        {
            _photonView.RPC("AddingToRoomList_RPC", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _playersInRoomList.Count < _playersMaxCount && CheckIfAlreadyInList(other.gameObject))
        {
            _photonView.RPC("RemovingFromRoomList_RPC", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName);
        }
    }

    // PUN RPC Methods
    // --------------------
    [PunRPC]
    void AddingToRoomList_RPC(string currentPlayer)
    {
        PhotonView currentPlayerData = GameObject.Find(currentPlayer).GetComponent<PhotonView>();

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
    void RemovingFromRoomList_RPC(string currentPlayer)
    {
        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            PhotonView myPlayer = ActionsManager.Instance.AllPlayersPhotonViews[i];

            if (myPlayer == null)
            {
                return;
            }

            for (int j = 0; j < 1; j++)
            {
                if (!_playersInRoomList.Contains(myPlayer))
                {
                    continue;
                }
                else
                {
                    _playersInRoomList.Remove(myPlayer);
                }
            }
        }
    }

    [PunRPC]
    void CrewCreateSubmit_RPC(int[] roleIndex, int leaderIndex)
    {
        int indexInCrewCounter = 0;
        for (int i = 0; i < roleIndex.Length; i++)
        {
            PlayerData desiredPlayerData = _playersInRoomList[i].GetComponent<PlayerData>();
            desiredPlayerData.CrewIndex = _crewRoomIndex;
            //desiredPlayerData.CrewIndex = ActionsManager.Instance.NextCrewIndex;
            desiredPlayerData.UserIndexInCrew = indexInCrewCounter;
            desiredPlayerData.UserRole = (Roles)roleIndex[i];
            MeshFilter vest = desiredPlayerData.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<MeshFilter>();
            vest.gameObject.SetActive(true);
            vest.mesh = ActionsManager.Instance.Vests[(int)desiredPlayerData.UserRole].mesh;
            indexInCrewCounter++;
        }   

        foreach (PhotonView player in _playersInRoomList)
        {
            player.GetComponent<PlayerData>().IsCrewLeader = false;
        }

        PlayerData leaderToBe = _playersInRoomList[leaderIndex].GetComponent<PlayerData>();
        leaderToBe.IsCrewLeader = true;
        HideCrewRoomMenu();

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject natan = PhotonNetwork.InstantiateRoomObject(ActionsManager.Instance.NatanPrefab.name, ActionsManager.Instance.NatanPosTransforms[_crewRoomIndex].position, ActionsManager.Instance.NatanPrefab.transform.rotation);
            natan.GetComponent<CarControllerSimple>().OwnerCrew = _crewRoomIndex;
        }

        ActionsManager.Instance.NextCrewIndex++;
    }

    [PunRPC]
    void ChangeCrewColors(Vector3 randomColor)
    {
        crewColor = new Color(randomColor.x, randomColor.y, randomColor.z);

        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            PlayerData currentPlayerData = _playersInRoomList[i].GetComponent<PlayerData>();
            NameTagDisplay desiredPlayerName = _playersInRoomList[i].GetComponentInChildren<NameTagDisplay>();

            desiredPlayerName.text.color = crewColor;
            currentPlayerData.CrewColor = crewColor;
        }
    }
}
