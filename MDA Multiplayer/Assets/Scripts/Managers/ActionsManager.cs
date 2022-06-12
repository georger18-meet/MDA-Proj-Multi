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

    #region Data References
    [Header("Data & Scripts")]
    
    public List<Patient> AllPatients;

    private Patient _lastClickedPatient;
    private PatientData _lastClickedPatientData;
    #endregion

    //public UnityEvent PatientOnClick;

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

        //if (_photonView.IsMine)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(this);
        //}

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
        //if (_photonView.IsMine)
        //{
            if (PlayerData.Instance.CurrentPatientNearby == null)
            {
                return;
            }

            _lastClickedPatient = PlayerData.Instance.CurrentPatientNearby;

            PatientData currentPatientData = PlayerData.Instance.CurrentPatientNearby != null ? PlayerData.Instance.CurrentPatientNearby.PatientData : null;

            _lastClickedPatientData = currentPatientData;

            if (!PlayerData.Instance.CurrentPatientNearby.IsPlayerJoined(PlayerData.Instance))
            {
                UIManager.Instance.JoinPatientPopUp.SetActive(true);
            }
            else
            {
                SetupPatientInfoDisplay();
                UIManager.Instance.PatientMenuParent.SetActive(true);
            }
        //}
    }

    public void OnJoinPatientRPC(bool isJoined)
    {
        Debug.Log("attempting Join Patient");

        if (AllPatientsPhotonViews.Contains(PlayerData.Instance.CurrentPatientNearby.PhotonView))
        {
            Debug.Log("Found correct PhotonView");

            PlayerData.Instance.CurrentPatientNearby.PhotonView.RPC("OnJoinPatient", RpcTarget.AllBuffered, isJoined);
        }
        else
        {
            Debug.Log("Didn't found correct PhotonView");
        }
    }

    //[PunRPC]
    //private void OnJoinPatient(bool isJoined)
    //{
    //    //if (_photonView.IsMine)
    //    //{
    //        if (isJoined)
    //        {
    //            _lastClickedPatient.AddUserToTreatingLists(PlayerData.Instance);
    //
    //            SetupPatientInfoDisplay();
    //
    //            UIManager.Instance.JoinPatientPopUp.SetActive(false);
    //            UIManager.Instance.PatientMenuParent.SetActive(true);
    //            UIManager.Instance.PatientInfoParent.SetActive(false);
    //        }
    //        else
    //        {
    //            UIManager.Instance.JoinPatientPopUp.SetActive(false);
    //        }
    //    //}
    //}

    public void SetupPatientInfoDisplay()
    {
        //UIManager.Instance.SureName.text = _lastClickedPatientData.SureName;
        //UIManager.Instance.LastName.text = _lastClickedPatientData.LastName;
        //UIManager.Instance.Gender.text = _lastClickedPatientData.Gender;
        //UIManager.Instance.Adress.text = _lastClickedPatientData.AddressLocation;
        //UIManager.Instance.InsuranceCompany.text = _lastClickedPatientData.MedicalCompany;
        //UIManager.Instance.Complaint.text = _lastClickedPatientData.Complaint;
        //
        //UIManager.Instance.Age.text = _lastClickedPatientData.Age.ToString();
        //UIManager.Instance.Id.text = _lastClickedPatientData.Id.ToString();
        //UIManager.Instance.PhoneNumber.text = _lastClickedPatientData.PhoneNumber.ToString();
    }

    public void LeavePatientRPC()
    {
        PlayerData.Instance.CurrentPatientNearby.PhotonView.RPC("LeavePatient", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void LeavePatient()
    {
        if (PlayerData.Instance.CurrentPatientNearby.PhotonView.IsMine)
        {
          Debug.Log("Attempting leave patient");

          UIManager.Instance.CloseAllPatientWindows();
          PlayerData.Instance.CurrentPatientNearby.TreatingUsers.Remove(PlayerData.Instance);
          Debug.Log("Left Patient Succesfully");
        }
    }
    #endregion
}