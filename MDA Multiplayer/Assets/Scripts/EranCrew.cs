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
    public GameObject DropDown;

    public List<TMP_Dropdown> CrewMemberDropDownList;
    public TMP_Dropdown PlayerListDropDown1;
    public TMP_Dropdown PlayerListDropDown2;
    public TMP_Dropdown PlayerListDropDown3;



    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        //StartCoroutine(HandleDropDownUpdates(.3f));
        DropdownPlayersNickNames();
    }


    void Update()
    {

        // PopulateDropdownRoles();
        if (Input.GetKey(KeyCode.L))
        {
           // Debug.Log("ActionsManager.Instance.AllPlayersPhotonViews   " + ActionsManager.Instance.AllPlayersPhotonViews.Count);
            //DropdownPlayersNickNames();
           // Debug.Log("PhotonNetwork.PlayerList   " + PhotonNetwork.PlayerList.Length);
        }

    }

    //Maybe In corutine?
    [PunRPC]
    private void DropdownPlayersNickNames()
    {

        //List<string> value = new List<string>();

        //for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        //{
        //    if (!value.Contains(ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName))
        //    {
        //        value.Add(ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName);
        //    }
            
        //}

        //foreach (var dropdown in CrewMemberDropDownList)
        //{
        //   // dropdown.ClearOptions();
        //    dropdown.AddOptions(value);
        //}


        List<string> value = new List<string>();
        foreach (var player in PhotonNetwork.PlayerList)
        {
            value.Add(player.NickName);
        }

       // PlayerListDropDown3.ClearOptions();
        PlayerListDropDown3.AddOptions(value);

    }

    public void Click()
    {
        _photonView.RPC("GiveRoles",RpcTarget.AllBufferedViaServer, GetHenyonIndex());
    }

    public int GetHenyonIndex()
    {
        int Index = 0;

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            if (DropDown.GetComponentInChildren<TextMeshProUGUI>().text == ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName)
            {
                Index = i;
            }

        }
        return Index;
    }



    IEnumerator HandleDropDownUpdates(float nextUpdate)
    {
        yield return new WaitForSeconds(nextUpdate);

        _photonView.RPC("DropdownPlayersNickNames", RpcTarget.AllBufferedViaServer);


        StartCoroutine(HandleDropDownUpdates(nextUpdate));
    }

    [PunRPC]
    public void GiveRoles(int index)
    {
        foreach (var player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            player.GetComponent<PlayerData>().IsHenyon10 = false;
        }

        PlayerData roleIndex = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        roleIndex.IsHenyon10 = true;
    }

    public void ShowEranRoomMenu()
    {
        RoomEranMenuUI.gameObject.SetActive(true);
    }

    public void CloseEranRoomMenu()
    {
        RoomEranMenuUI.gameObject.SetActive(false);
    }
}
