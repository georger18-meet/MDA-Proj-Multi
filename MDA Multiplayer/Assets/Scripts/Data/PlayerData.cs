using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    [Header("Photon")]
    public PhotonView PhotonView;

    [field: SerializeField] public string UserName { get; set; }
    [field: SerializeField] public string CrewName { get; set; }
    [field: SerializeField] public int UserIndexInCrew { get; set; }
    [field: SerializeField] public int CrewIndex { get; set; }
    [field: SerializeField] public Roles UserRole { get; set; }
    [field: SerializeField] public Patient CurrentPatientNearby { get; set; }
    [field: SerializeField] public Animation PlayerAnimation { get; set; }

    private GameObject _playerGameObject;
    public GameObject PlayerGameObject => _playerGameObject;

    private void Awake()
    {
        if (PhotonView.IsMine)
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }
    }

    private void Start()
    {
        ActionsManager.Instance.AllPlayersPhotonViews.Add(PhotonView);
        _playerGameObject = gameObject;
    }
}
