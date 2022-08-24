using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class OperationsRoom : MonoBehaviour, IPunObservable
{
    public GameObject MokdnMenuUI;
    private Coroutine updatePlayerListCoroutine;
    private PhotonView _photonView;
    private OwnershipTransfer _transfer;
    [SerializeField] private bool isUsed;

    [SerializeField] private GameObject _DropDown;
    [SerializeField] private TMP_Dropdown _playerListDropDown;


    void Start()
    {
        _transfer = GetComponent<OwnershipTransfer>();
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            InteractUICrew();
        }
        else
        {
            MokdnMenuUI.GetComponentInParent<CanvasGroup>().interactable = false;

        }
    }

    public int GetPikud10Index()
    {
        int Index = 0;

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            if (_DropDown.GetComponentInChildren<TextMeshProUGUI>().text == ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName)
            {
                Index = i;
            }
        }
        return Index;
    }

    public void GivePikudRoleClick()
    {
        _photonView.RPC("GivePikudRole", RpcTarget.AllBufferedViaServer, GetPikud10Index());
    }

    [PunRPC]
    public void ShowMokdanMenu_RPC()
    {
        MokdnMenuUI.SetActive(true);
        isUsed = true;
    }


    [PunRPC]
    public void CloseMokdanMenu_RPC()
    {
        isUsed = false;
        MokdnMenuUI.SetActive(false);
    }


    private void InteractUICrew()
    {
        if (isUsed)
        {
            MokdnMenuUI.GetComponentInParent<CanvasGroup>().interactable = true;

        }
    }

    public void ShowMokdanMenu()
    {
        _transfer.TvOwner();
        _photonView.RPC("ShowMokdanMenu_RPC", RpcTarget.AllBufferedViaServer);
        updatePlayerListCoroutine = StartCoroutine(HandleDropDownUpdates(0.5f));

    }

    public void CloseMokdanRoomMenu()
    {
        StopCoroutine(updatePlayerListCoroutine);
        _photonView.RPC("CloseMokdanMenu_RPC", RpcTarget.AllBufferedViaServer);
    }



    IEnumerator HandleDropDownUpdates(float nextUpdate)
    {
        while (true)
        {
            if (ActionsManager.Instance.AllPlayersPhotonViews.Count != _playerListDropDown.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNamesPikud", RpcTarget.AllBufferedViaServer);
            }

            yield return new WaitForSeconds(nextUpdate);
        }
    }



    [PunRPC]
    private void DropdownPlayersNickNamesPikud()
    {
        List<string> value = new List<string>();
        foreach (PhotonView player in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            value.Add(player.Owner.NickName);
        }
        _playerListDropDown.ClearOptions();
        _playerListDropDown.AddOptions(value);
       
    }


    [PunRPC]
    public void GivePikudRole(int index)
    {
        PlayerData chosenPlayerData = ActionsManager.Instance.AllPlayersPhotonViews[index].GetComponent<PlayerData>();
        chosenPlayerData.IsPikud10 = true;
        chosenPlayerData.AssignAranRole(AranRoles.Pikud10);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(_playerListDropDown.value);
        }
        else
        {
            _playerListDropDown.value = (int)stream.ReceiveNext();
        }
    }

}
