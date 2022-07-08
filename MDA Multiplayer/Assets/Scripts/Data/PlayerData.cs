using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerData : MonoBehaviour
{
    public PhotonView PhotonView => gameObject.GetPhotonView();

    [field: SerializeField] public string UserName { get; set; }
    [field: SerializeField] public string CrewName { get; set; }
    [field: SerializeField] public int UserIndexInCrew { get; set; }
    [field: SerializeField] public int CrewIndex { get; set; }
    [field: SerializeField] public bool IsJoinedNearbyPatient { get => CurrentPatientNearby.IsPlayerJoined(this); }
    [field: SerializeField] public bool IsCrewLeader { get; set; }
    [field: SerializeField] public Roles UserRole { get; set; }
    [field: SerializeField] public Patient CurrentPatientNearby { get; set; }
    [field: SerializeField] public Animation PlayerAnimation { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ActionsManager.Instance.AllPlayersPhotonViews.Add(PhotonView);
    }

    #region PunRPC invoked by Player
    [PunRPC]
    private void OnJoinPatient()
    {
        CurrentPatientNearby.PhotonView.RPC("AddUserToTreatingLists", RpcTarget.AllBufferedViaServer, UserName);
    }

    [PunRPC]
    private void OnLeavePatient()
    {
        Debug.Log("Attempting leave patient");
        CurrentPatientNearby.TreatingUsers.Remove(this);
        Debug.Log("Left Patient Succesfully");
    }

    [PunRPC]
    private void OnApplyMedicine(int measurementNumber, int _newMeasurement)
    {
        // loops throughout measurementList and catches the first element that is equal to measurementNumber
        Measurements measurements = ActionsManager.Instance.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);

        CurrentPatientNearby.PatientData.SetMeasurementByIndex(2, _newMeasurement);
    }
    #endregion
}
