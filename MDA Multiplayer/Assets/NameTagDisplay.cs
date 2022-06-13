using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

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
    }
}
