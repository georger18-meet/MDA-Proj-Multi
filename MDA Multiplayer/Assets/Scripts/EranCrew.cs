using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class EranCrew : MonoBehaviour
{
    private PhotonView _photonView;

    public Canvas RoomEranMenuUI;

    [SerializeField]private GameObject DropDown1;
    [SerializeField] private GameObject DropDown2;
    [SerializeField] private GameObject DropDown3;

    //public List<TMP_Dropdown> CrewMemberDropDownList;
    [SerializeField] private TMP_Dropdown PlayerListDropDown1;
    [SerializeField] private TMP_Dropdown PlayerListDropDown2;
    [SerializeField] private TMP_Dropdown PlayerListDropDown3;

    

    void Start()
    {
        _photonView = GetComponent<PhotonView>();

    }


    //Maybe In corutine?
    [PunRPC]
    private void DropdownPlayersNickNames()
    {

        //List<string> value = new List<string>();

        //for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        //{
        //        value.Add(PhotonNetwork.PlayerList[i].NickName);
        //}

        //foreach (var dropdown in CrewMemberDropDownList)
        //{
        //     dropdown.ClearOptions();
        //    dropdown.AddOptions(value);
        //}

        List<string> value = new List<string>();
        foreach (var player in PhotonNetwork.PlayerList)
        {
            value.Add(player.NickName);
        }


        PlayerListDropDown1.ClearOptions();
        PlayerListDropDown1.AddOptions(value);
        PlayerListDropDown2.ClearOptions();
        PlayerListDropDown2.AddOptions(value);
        PlayerListDropDown3.ClearOptions();
        PlayerListDropDown3.AddOptions(value);
        //Debug.Log(PlayerListDropDown3.options.Count);
    }

    public void GiveHenyonRoleClick()
    {
        _photonView.RPC("GiveHenyonRole", RpcTarget.AllBufferedViaServer, GetHenyonIndex());
    }
    public void GiveIsRefuaRoleClick()
    {
        _photonView.RPC("GiveRefuaRole", RpcTarget.AllBufferedViaServer, GetRefuaIndex());
    }
    public void GiveIsPinoyeRoleClick()
    {
        _photonView.RPC("GivePinoyeRole", RpcTarget.AllBufferedViaServer, GetPinoyeIndex());
    }


    public int GetRefuaIndex()
    {
        int Index = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (DropDown1.GetComponentInChildren<TextMeshProUGUI>().text == PhotonNetwork.PlayerList[i].NickName)
            {
                Index = i;
            }

        }
        return Index;
    }

    public int GetPinoyeIndex()
    {
        int Index = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (DropDown2.GetComponentInChildren<TextMeshProUGUI>().text == PhotonNetwork.PlayerList[i].NickName)
            {
                Index = i;
            }

        }
        return Index;
    }

    public int GetHenyonIndex()
    {
        int Index = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (DropDown3.GetComponentInChildren<TextMeshProUGUI>().text == PhotonNetwork.PlayerList[i].NickName)
            {
                Index = i;
            }

        }
        return Index;
    }


    IEnumerator HandleDropDownUpdates(float nextUpdate)
    {
        while (true)
        {
            if (PhotonNetwork.PlayerList.Length != PlayerListDropDown3.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNames", RpcTarget.AllBufferedViaServer);

            }
            if (PhotonNetwork.PlayerList.Length != PlayerListDropDown2.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNames", RpcTarget.AllBufferedViaServer);

            }
            if (PhotonNetwork.PlayerList.Length != PlayerListDropDown1.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNames", RpcTarget.AllBufferedViaServer);

            }

            yield return new WaitForSeconds(nextUpdate);
        }
   

        // StartCoroutine(HandleDropDownUpdates(nextUpdate));
    }

    [PunRPC]
    public void GiveHenyonRole(int index)
    {
        foreach (var player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            player.GetComponent<PlayerData>().IsHenyon10 = false;
        }

        PlayerData roleIndex = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        roleIndex.IsHenyon10 = true;
    }
    [PunRPC]
    public void GiveRefuaRole(int index)
    {
        foreach (var player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            player.GetComponent<PlayerData>().IsRefua10 = false;
        }

        PlayerData roleIndex = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        roleIndex.IsRefua10 = true;
    }
    [PunRPC]
    public void GivePinoyeRole(int index)
    {
        foreach (var player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            player.GetComponent<PlayerData>().IsPinoye10 = false;
        }

        PlayerData roleIndex = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        roleIndex.IsPinoye10 = true;
    }
    private Coroutine updatePlayerListCoroutine;
    //opened By clicking on Sign
    public void ShowEranRoomMenu()
    {
        RoomEranMenuUI.gameObject.SetActive(true);
        updatePlayerListCoroutine = StartCoroutine(HandleDropDownUpdates(0.5f));
    }

    public void CloseEranRoomMenu()
    {
        StopCoroutine(updatePlayerListCoroutine);
        RoomEranMenuUI.gameObject.SetActive(false);
    }
}
