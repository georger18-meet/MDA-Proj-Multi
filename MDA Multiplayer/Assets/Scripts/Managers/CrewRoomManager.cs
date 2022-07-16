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

    public List<PhotonView> _playersInRoomList;
    public int _playersMaxCount = 4;
    private PhotonView _photonView;


    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        RoomCrewMenuUI.SetActive(false);
    }

    void Update()
    {
       // BlockRoomAccess();
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

    private void RefreshCrewNamesTexts()
    {
        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            switch (i)
            {
                case 0:
                    CrewMemberNameText1.text = _playersInRoomList[i].name;
                    break;                
                case 1:
                    CrewMemberNameText2.text = _playersInRoomList[i].name;
                    break;                
                case 2:
                    CrewMemberNameText3.text = _playersInRoomList[i].name;
                    break;                
                case 3:
                    CrewMemberNameText4.text = _playersInRoomList[i].name;
                    break;
                default:
                    break;
            }
        }
    }

    public void ShowCrewRoomMenu()
    {
        RoomCrewMenuUI.SetActive(true);
        RefreshCrewNamesTexts();
    }

    public void HideCrewRoomMenu()
    {
        RoomCrewMenuUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _playersInRoomList.Count < _playersMaxCount && !CheckIfAlreadyInList(other.gameObject))
        {
           // _playersInRoomList.Add(other.gameObject.GetPhotonView());
            _photonView.RPC("AddingToRoomList_RPC", RpcTarget.AllBufferedViaServer,PhotonNetwork.NickName);
        }
    }

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
