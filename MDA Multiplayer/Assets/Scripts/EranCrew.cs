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
    private OwnershipTransfer _transfer;

    #region Pikod10 Variables
    [Header("Pikod10")]
    public GameObject Pikod10EranMenuUI;

    [SerializeField] private GameObject DropDown1;
    [SerializeField] private GameObject DropDown2;
    [SerializeField] private GameObject DropDown3;

    //public List<TMP_Dropdown> CrewMemberDropDownList;
    [SerializeField] private TMP_Dropdown PlayerListDropDown1;
    [SerializeField] private TMP_Dropdown PlayerListDropDown2;
    [SerializeField] private TMP_Dropdown PlayerListDropDown3;
    #endregion

    #region Mokdan Variables
    [Header("Metargel")]
    public GameObject MetargelEranMenuUI;

    [SerializeField] private GameObject MetargelDropDown;
    [SerializeField] private TMP_Dropdown MetargelPlayerListDropDown;
    #endregion

    void Start()
    {
        _transfer = GetComponent<OwnershipTransfer>();
        _photonView = GetComponent<PhotonView>();
    }

    #region Metargel Methods
    public int GetMokdanIndex()
    {
        int Index = 0;

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            if (MetargelDropDown.GetComponentInChildren<TextMeshProUGUI>().text == ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName)
            {
                Index = i;
            }

        }
        return Index;
    }

    public void GiveMokdanRoleClick()
    {
        _photonView.RPC("GiveMokdanRole", RpcTarget.AllBufferedViaServer, GetMokdanIndex());
    }

    public void ShowMetargelMenu()
    {
        _transfer.TvOwner();
        MetargelEranMenuUI.SetActive(true);
        updatePlayerListCoroutine = StartCoroutine(HandleDropDownUpdates(0.5f));

    }

    public void CloseMetargelRoomMenu()
    {
        StopCoroutine(updatePlayerListCoroutine);
        MetargelEranMenuUI.SetActive(false);
    }
    #endregion

    #region Pikod10 Methods
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

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            if (DropDown1.GetComponentInChildren<TextMeshProUGUI>().text == ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName)
            {
                Index = i;
            }

        }
        return Index;
    }
    public int GetPinoyeIndex()
    {
        int Index = 0;

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            if (DropDown2.GetComponentInChildren<TextMeshProUGUI>().text == ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName)
            {
                Index = i;
            }

        }
        return Index;
    }

    public int GetHenyonIndex()
    {
        int Index = 0;

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            if (DropDown3.GetComponentInChildren<TextMeshProUGUI>().text == ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName)
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
            if (ActionsManager.Instance.AllPlayersPhotonViews.Count != PlayerListDropDown3.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNames", RpcTarget.AllBufferedViaServer);

            }
            if (ActionsManager.Instance.AllPlayersPhotonViews.Count != PlayerListDropDown2.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNames", RpcTarget.AllBufferedViaServer);

            }
            if (ActionsManager.Instance.AllPlayersPhotonViews.Count != PlayerListDropDown1.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNames", RpcTarget.AllBufferedViaServer);

            }
            if (ActionsManager.Instance.AllPlayersPhotonViews.Count != MetargelPlayerListDropDown.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNames", RpcTarget.AllBufferedViaServer);

            }
            yield return new WaitForSeconds(nextUpdate);
        }
   
        // StartCoroutine(HandleDropDownUpdates(nextUpdate));
    }

    //opened By clicking on Sign
    private Coroutine updatePlayerListCoroutine;

    public void ShowPikod10RoomMenu(bool isPikod10)
    {
        if (isPikod10)
        {
            _transfer.TvOwner();
            Pikod10EranMenuUI.SetActive(true);
            updatePlayerListCoroutine = StartCoroutine(HandleDropDownUpdates(0.5f));
        }
    }

    public void CloseEranRoomMenu()
    {
        StopCoroutine(updatePlayerListCoroutine);
        Pikod10EranMenuUI.SetActive(false);
    }
    #endregion

    #region PunRpc
    //Maybe In corutine? 
    [PunRPC]
    private void DropdownPlayersNickNames()
    {
        List<string> value = new List<string>();
        foreach (PhotonView player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            value.Add(player.Owner.NickName);
        }

        PlayerListDropDown1.ClearOptions();
        PlayerListDropDown1.AddOptions(value);
        PlayerListDropDown2.ClearOptions();
        PlayerListDropDown2.AddOptions(value);
        PlayerListDropDown3.ClearOptions();
        PlayerListDropDown3.AddOptions(value);

        //Metargel Player list
        MetargelPlayerListDropDown.ClearOptions();
        MetargelPlayerListDropDown.AddOptions(value);
    }

    [PunRPC]
    public void GiveMokdanRole(int index)
    {
        //foreach (var player in ActionsManager.Instance.AllPlayersPhotonViews)
        //{
        //    player.GetComponent<PlayerData>().IsMokdan = false;
        //}
        PlayerData roleIndex = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        roleIndex.IsMokdan = true;
    }

    [PunRPC]
    public void GiveHenyonRole(int index)
    {
        foreach (PhotonView player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            player.GetComponent<PlayerData>().IsHenyon10 = false;
        }

        PlayerData roleIndex = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        roleIndex.IsHenyon10 = true;
    }

    [PunRPC]
    public void GiveRefuaRole(int index)
    {
        foreach (PhotonView player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            player.GetComponent<PlayerData>().IsRefua10 = false;
        }

        PlayerData roleIndex = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>(); 
        roleIndex.IsRefua10 = true;
    }

    [PunRPC]
    public void GivePinoyeRole(int index)
    {
        foreach (PhotonView player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            player.GetComponent<PlayerData>().IsPinoye10 = false;
        }

        PlayerData roleIndex = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        roleIndex.IsPinoye10 = true;
    }
    #endregion
}
