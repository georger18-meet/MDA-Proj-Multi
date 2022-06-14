using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Realtime;
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
            Destroy(this.gameObject);
        }

    }

    private void Start()
    {
    }
    #endregion

    #region Assignment
    // Triggered upon Clicking on the Patient

  

    public void OnPatientClicked()
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


        for (int i = 0; i < playerData.Length; i++)
        {
            if (playerData[i].GetPhotonView.ViewID==playerIndex)
            {
                addedToPatient = playerData[i];
                break;
            }
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


    public void TestingJoin()
    {



    }






    public void OnJoinPatient(bool isJoined)
    {
        
            if (isJoined)
            {
            Debug.Log("PlayerData.Instance.GetPhotonView.ViewID" + " " + PhotonNetwork.LocalPlayer.ActorNumber);

            PlayerData.Instance.CurrentPatientTreating.AddUserToTreatingLists(PhotonNetwork.LocalPlayer.ActorNumber);
                SetupPatientInfoDisplay();

                UIManager.getInstance.JoinPatientPopUp.SetActive(false);
            // _uIManager.PatientMenuParent.SetActive(true);
                  UIManager.getInstance.PatientInfoParent.SetActive(false);
            }
            else
            {
                UIManager.getInstance.JoinPatientPopUp.SetActive(false);
            }
        
    }

    [PunRPC]
    private void RPC_OnJoinPatient(bool isJoined)
    {

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

            _uIManager.CloseAllPatientWindows();
            PlayerData.Instance.CurrentPatientTreating.TreatingUsers.Remove(PlayerData.Instance);
            Debug.Log("Left Patient Succesfully");
      
    }
    #endregion
}