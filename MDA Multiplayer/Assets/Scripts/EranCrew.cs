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


        PlayerListDropDown3.ClearOptions();
        PlayerListDropDown3.AddOptions(value);
        
        //foreach (var playername in value)
        //{
        //   // PlayerListDropDown3.options.Add(new TMP_Dropdown.OptionData() { text = playername });
        //    if (!PlayerListDropDown3.options.Contains(new TMP_Dropdown.OptionData() { text = playername }))
        //    {
        //    }
        //}


        Debug.Log(PlayerListDropDown3.options.Count);
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
        while (true)
        {
            if (PhotonNetwork.PlayerList.Length!=PlayerListDropDown3.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNames", RpcTarget.AllBufferedViaServer);

            }
            yield return new WaitForSeconds(nextUpdate);
        }

        // StartCoroutine(HandleDropDownUpdates(nextUpdate));
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
