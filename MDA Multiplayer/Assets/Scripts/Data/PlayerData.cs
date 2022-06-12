using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    [Header("Photon")]
    [SerializeField] private PhotonView _photonView;

    [field: SerializeField] public string UserName { get; set; }
    [field: SerializeField] public string CrewName { get; set; }
    [field: SerializeField] public int UserIndexInCrew { get; set; }
    [field: SerializeField] public int CrewIndex { get; set; }
    [field: SerializeField] public Roles UserRole { get; set; }
    [field: SerializeField] public Patient CurrentPatientNearby { get; set; }

    [field: SerializeField] public Animation PlayerAnimation;

    private void Awake()
    {
        if (_photonView.IsMine)
        {
            Instance = this;
        }
        //else
        //{
        //    Destroy(this);
        //}
    }
}
