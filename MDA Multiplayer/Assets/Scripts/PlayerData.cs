using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    [field: SerializeField] public string UserName { get; set; }
    [field: SerializeField] public string CrewName { get; set; }
    [field: SerializeField] public int UserIndexInCrew { get; set; }
    [field: SerializeField] public int CrewIndex { get; set; }
    [field: SerializeField] public Roles UserRole { get; set; }
    [field: SerializeField] public Patient CurrentPatientTreating { get; set; }

    [field: SerializeField] public Animation PlayerAnimation;

    private PhotonView _photonView;
    public PhotonView GetPhotonView => _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();


        if (_photonView.IsMine)
        {
            Instance = this;

        }
    }
    
}
