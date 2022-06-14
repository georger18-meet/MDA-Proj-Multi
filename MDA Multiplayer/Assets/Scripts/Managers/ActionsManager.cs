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
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    //private void Start()
    //{
    //    for (int i = 0; i < AllPatients.Count; i++)
    //    {
    //        _allPatientsPhotonViews[i] = AllPatients[i].PhotonView;
    //    }
    //}



    #region Assignment
    // Triggered upon Clicking on the Patient
    public void OnPatientClicked()
    {
        Debug.Log($"Attempting to Click On Patient");
        foreach (PhotonView photonView in AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (desiredPlayerData.CurrentPatientNearby == null)
                {
                    return;
                }

                _lastClickedPatient = desiredPlayerData.CurrentPatientNearby;

                PatientData currentPatientData = desiredPlayerData.CurrentPatientNearby != null ? desiredPlayerData.CurrentPatientNearby.PatientData : null;
                _lastClickedPatientData = currentPatientData;

                Debug.Log($"{desiredPlayerData.UserName} Clicked on: {desiredPlayerData.CurrentPatientNearby}");

                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                {
                    UIManager.Instance.JoinPatientPopUp.SetActive(true);
                }
                else
                {
                    SetupPatientInfoDisplay();
                    UIManager.Instance.PatientMenuParent.SetActive(true);
                }
            }
        }
    }

    public void OnJoinPatientRPC(bool isJoined)
    {
        Debug.Log("attempting Join Patient");

        foreach (PhotonView photonView in AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                desiredPlayerData.CurrentPatientNearby.PhotonView.RPC("OnJoinPatient", RpcTarget.AllBuffered, isJoined);
            }
        }
    }

    public void LeavePatientRPC()
    {
        foreach (PhotonView photonView in AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                desiredPlayerData.CurrentPatientNearby.PhotonView.RPC("LeavePatient", RpcTarget.AllBuffered);
            }
        }
    }

    public void SetupPatientInfoDisplay()
    {
        UIManager.Instance.SureName.text = _lastClickedPatientData.SureName;
        UIManager.Instance.LastName.text = _lastClickedPatientData.LastName;
        UIManager.Instance.Gender.text = _lastClickedPatientData.Gender;
        UIManager.Instance.Adress.text = _lastClickedPatientData.AddressLocation;
        UIManager.Instance.InsuranceCompany.text = _lastClickedPatientData.MedicalCompany;
        UIManager.Instance.Complaint.text = _lastClickedPatientData.Complaint;

        UIManager.Instance.Age.text = _lastClickedPatientData.Age.ToString();
        UIManager.Instance.Id.text = _lastClickedPatientData.Id.ToString();
        UIManager.Instance.PhoneNumber.text = _lastClickedPatientData.PhoneNumber.ToString();
    }

    //private void OnJoinPatient(bool isJoined)
    //{
    //    foreach (PhotonView photonView in AllPlayersPhotonViews)
    //    {
    //        PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();
    //
    //        if (photonView.IsMine)
    //        {
    //            if (isJoined)
    //            {
    //                _lastClickedPatient.AddUserToTreatingLists(desiredPlayerData);
    //
    //                SetupPatientInfoDisplay();
    //
    //                UIManager.Instance.JoinPatientPopUp.SetActive(false);
    //                UIManager.Instance.PatientMenuParent.SetActive(true);
    //                UIManager.Instance.PatientInfoParent.SetActive(false);
    //
    //            }
    //            else
    //            {
    //                UIManager.Instance.JoinPatientPopUp.SetActive(false);
    //            }
    //        }
    //    }
    //}

    //private void LeavePatient()
    //{
    //    if (PlayerData.Instance.CurrentPatientNearby.PhotonView.IsMine)
    //    {
    //      Debug.Log("Attempting leave patient");
    //
    //      UIManager.Instance.CloseAllPatientWindows();
    //      PlayerData.Instance.CurrentPatientNearby.TreatingUsers.Remove(PlayerData.Instance);
    //      Debug.Log("Left Patient Succesfully");
    //    }
    //}
    #endregion
}