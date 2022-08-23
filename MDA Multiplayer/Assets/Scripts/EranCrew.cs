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
    private Coroutine updatePlayerListCoroutine;

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

    public void GivePikud10Role()
    {
        //_photonView.RPC("GivePikud10RoleRPC", RpcTarget.AllBufferedViaServer, GetRefuaIndex());
    }

    IEnumerator HandleDropDownUpdates(float nextUpdate)
    {
        while (true)
        {
            if (ActionsManager.Instance.AllPlayersPhotonViews.Count != MetargelPlayerListDropDown.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNamesMetargel", RpcTarget.AllBufferedViaServer);
            }
            yield return new WaitForSeconds(nextUpdate);
        }
   
        // StartCoroutine(HandleDropDownUpdates(nextUpdate));
    }
    #endregion

    #region PunRpc
    [PunRPC]
    private void DropdownPlayersNickNamesMetargel()
    {
        List<string> value = new List<string>();
        foreach (PhotonView player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            value.Add(player.Owner.NickName);
        }

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
        PlayerData chosenPlayerData = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        chosenPlayerData.IsMokdan = true;
        chosenPlayerData.AssignAranRole(AranRoles.Mokdan);
    }

    [PunRPC]
    public void GivePikud10RoleRPC(int index)
    {
        foreach (PhotonView player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData playerData = player.GetComponent<PlayerData>();
            playerData.IsPikud10 = false;

            if (playerData.AranRole == AranRoles.Pikud10)
            {
                playerData.AssignAranRole(AranRoles.None);
            }
        }

        PlayerData chosenPlayerData = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        chosenPlayerData.IsPikud10 = true;
        chosenPlayerData.AssignAranRole(AranRoles.Pikud10);
    }
    #endregion
}
