using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager Instance;

    public List<Measurements> MeasurementList;

    #region Data References
    [Header("Data & Scripts")]
    [SerializeField] private UIManager _uIManager;
    
    public List<Patient> AllPatients;

    private Patient _lastClickedPatient;
    private PatientData _lastClickedPatientData;
    private PlayerData _playerData;
    #endregion

    private PhotonView _photonView;
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
            Destroy(this.gameObject);
        }

    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }
    #endregion

    #region Assignment
    // Triggered upon Clicking on the Patient



    public void OnPatientClicked()
    {
        
        if (_playerData.CurrentPatientTreating == null)
        {
            return;
        }

        _lastClickedPatient = _playerData.CurrentPatientTreating;

        PatientData currentPatientData = _playerData.CurrentPatientTreating != null ? _playerData.CurrentPatientTreating.PatientData : null;

        _lastClickedPatientData = currentPatientData;

        if (!_playerData.CurrentPatientTreating.IsPlayerJoined(_playerData))
        {
            _uIManager.JoinPatientPopUp.SetActive(true);
        }
        else
        {
            SetupPatientInfoDisplay();
            _uIManager.PatientMenuParent.SetActive(true);
        }
    }

    public void AddingPlayerIndex(int playerIndex, int patientID)
    {
        PlayerData[] playerData = GameObject.FindObjectsOfType<PlayerData>();

        PlayerData addedToPatient = null;

        Patient treatedPatient = GetPatientByViewID(patientID);

        for (int i = 0; i < playerData.Length; i++)
        {
            if (playerData[i].GetPhotonView.ViewID==playerIndex)
            {
                addedToPatient = playerData[i];
                break;
            }
        }

        if (addedToPatient&& treatedPatient)
        {
            
        }
    }

    private Patient GetPatientByViewID(int viewID)
    {
        

        for (int i = 0; i < AllPatients.Count; i++)
        {
            if (AllPatients[i].GetphotonView.ViewID == viewID)
            {
                return AllPatients[i];
            }

        }

        return null;

    }


    public void OnJoinPatient(bool isJoined)
    {
        if (isJoined)
        {

            _lastClickedPatient.AddUserToTreatingLists(_playerData);

            SetupPatientInfoDisplay();

            _uIManager.JoinPatientPopUp.SetActive(false);
           // _uIManager.PatientMenuParent.SetActive(true);
            _uIManager.PatientInfoParent.SetActive(false);
        }
        else
        {
            _uIManager.JoinPatientPopUp.SetActive(false);
        }
    }

    private void SetupPatientInfoDisplay()
    {
        _uIManager.SureName.text = _lastClickedPatientData.SureName;
        _uIManager.LastName.text = _lastClickedPatientData.LastName;
        _uIManager.Gender.text = _lastClickedPatientData.Gender;
        _uIManager.Adress.text = _lastClickedPatientData.AddressLocation;
        _uIManager.InsuranceCompany.text = _lastClickedPatientData.MedicalCompany;
        _uIManager.Complaint.text = _lastClickedPatientData.Complaint;

        _uIManager.Age.text = _lastClickedPatientData.Age.ToString();
        _uIManager.Id.text = _lastClickedPatientData.Id.ToString();
        _uIManager.PhoneNumber.text = _lastClickedPatientData.PhoneNumber.ToString();
    }

    public void LeavePatient()
    {
        Debug.Log("Attempting leave patient");

        // if (_photonView.isMine)
        // {
            _uIManager.CloseAllPatientWindows();
            _playerData.CurrentPatientTreating.TreatingUsers.Remove(_playerData);
            Debug.Log("Left Patient Succesfully");
        // }
    }
    #endregion
}