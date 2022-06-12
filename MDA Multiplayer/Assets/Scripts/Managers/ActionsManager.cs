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
    [SerializeField] private PhotonView _photonView;

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

    #region Assignment
    // Triggered upon Clicking on the Patient
    public void OnPatientClicked()
    {
        if (_photonView.IsMine)
        {
            if (PlayerData.Instance.CurrentPatientTreating == null)
            {
                return;
            }

            _lastClickedPatient = PlayerData.Instance.CurrentPatientTreating;

            PatientData currentPatientData = PlayerData.Instance.CurrentPatientTreating != null ? PlayerData.Instance.CurrentPatientTreating.PatientData : null;

            _lastClickedPatientData = currentPatientData;

            if (!PlayerData.Instance.CurrentPatientTreating.IsPlayerJoined(PlayerData.Instance))
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

    public void OnJoinPatient(bool isJoined)
    {
        if (_photonView.IsMine)
        {
            if (isJoined)
            {
                _lastClickedPatient.AddUserToTreatingLists(PlayerData.Instance);

                SetupPatientInfoDisplay();

                UIManager.Instance.JoinPatientPopUp.SetActive(false);
                UIManager.Instance.PatientMenuParent.SetActive(true);
                UIManager.Instance.PatientInfoParent.SetActive(false);
            }
            else
            {
                UIManager.Instance.JoinPatientPopUp.SetActive(false);
            }
        }
    }

    private void SetupPatientInfoDisplay()
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

    public void LeavePatient()
    {
        if (_photonView.IsMine)
        {
            Debug.Log("Attempting leave patient");

            UIManager.Instance.CloseAllPatientWindows();
            PlayerData.Instance.CurrentPatientTreating.TreatingUsers.Remove(PlayerData.Instance);
            Debug.Log("Left Patient Succesfully");
        }
    }
    #endregion
}