using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager Instance;

    public List<Measurements> MeasurementList;

    #region Data References
    [Header("Data & Scripts")]
    
    public List<Patient> AllPatients;

    private Patient _lastClickedPatient;
    private PatientData _lastClickedPatientData;
    #endregion


    #region MonoBehaviour Callbacks
    private void Awake()
    {
        //if (_photonView.isMine)
        //{
            Instance = this;
        //}
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
            UIManager.Instance.JoinPatientPopUp.SetActive(true);
        }
        else
        {
            SetupPatientInfoDisplay();
            UIManager.Instance.PatientMenuParent.SetActive(true);
        }
    }

    public void OnJoinPatient(bool isJoined)
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
        Debug.Log("Attempting leave patient");

        // if (_photonView.isMine)
        // {
        UIManager.Instance.CloseAllPatientWindows();
            PlayerData.Instance.CurrentPatientTreating.TreatingUsers.Remove(PlayerData.Instance);
            Debug.Log("Left Patient Succesfully");
        // }
    }
    #endregion
}