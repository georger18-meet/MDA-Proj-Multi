using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class NameTagDisplay : MonoBehaviour
{
    [SerializeField] private PhotonView playerPhotonView;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        if (playerPhotonView.IsMine) // if we are the local player we disable the text
        {
            gameObject.SetActive(false);
        }
        text.text = playerPhotonView.Owner.NickName;
        //playerPhotonView.Owner.NickName = PlayerData.Instance.UserName;
        //PlayerData.Instance.PlayerGameObject.name = playerPhotonView.Owner.NickName;
    }
}
