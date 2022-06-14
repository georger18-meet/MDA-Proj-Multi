using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager Instance;

    public List<Measurements> MeasurementList;

    //public GameObject GameObject;

    [Header("Photon")]
    public PhotonView _playerPhotonView;
    public List<PhotonView> AllPatientsPhotonViews;
    public List<PhotonView> AllPlayersPhotonViews;

    #region Data References
    [Header("Data & Scripts")]
    public List<Patient> AllPatients;

    private Patient _lastClickedPatient;
    private PatientData _lastClickedPatientData;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region UnityEvents
    public void OnPatientClicked()
    {
        Debug.Log($"Attempting to Click On Patient");

        for (int i = 0; i < AllPlayersPhotonViews.Count; i++)
        {
            //if (!AllPlayersPhotonViews[i].IsMine)
            //    return;

            PlayerData myPlayerData = AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();
            _lastClickedPatient = myPlayerData.CurrentPatientNearby;

            PatientData currentPatientData = myPlayerData.CurrentPatientNearby != null ? myPlayerData.CurrentPatientNearby.PatientData : null;
            _lastClickedPatientData = currentPatientData;

            Debug.Log($"{myPlayerData.UserName} Clicked on: {myPlayerData.CurrentPatientNearby}");

            if (!myPlayerData.CurrentPatientNearby.IsPlayerJoined(myPlayerData))
            {
                UIManager.Instance.JoinPatientPopUp.SetActive(true);
            }
            else
            {
                _lastClickedPatient.PhotonView.RPC("UpdatePatientInfoDisplay", RpcTarget.AllBufferedViaServer);
                UIManager.Instance.PatientMenuParent.SetActive(true);
            }
        }
    }

    public void OnPlayerJoinPatientRPC(bool isJoined)
    {
        Debug.Log("attempting to Join Patient");

        for (int i = 0; i < AllPlayersPhotonViews.Count; i++)
        {
            //if (!AllPlayersPhotonViews[i].IsMine)
            //    return;

            PlayerData myPlayerData = AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();
            myPlayerData.PhotonView.RPC("OnJoinPatient", RpcTarget.AllBufferedViaServer, isJoined);
        }
    }

    public void OnPlayerLeavePatientRPC()
    {
        Debug.Log("attempting to Leave Patient");

        for (int i = 0; i < AllPlayersPhotonViews.Count; i++)
        {
            if (!AllPlayersPhotonViews[i].IsMine)
                return;

            PlayerData myPlayerData = AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();
            myPlayerData.PhotonView.RPC("OnLeavePatient", RpcTarget.AllBufferedViaServer);
        }
    }
    #endregion

}