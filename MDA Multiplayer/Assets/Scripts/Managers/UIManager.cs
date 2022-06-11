using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Player UI
    [Header("Player UI Parents")]
    public GameObject AmbulanceActionBarParent;
    public GameObject NatanActionBarParent, BasicActionMenuParent;
    #endregion

    #region Patient UI 
    [Header("Patient UI Parents")]
    public GameObject JoinPatientPopUp;
    public GameObject PatientMenuParent, PatientInfoParent, ActionLogParent;

    [Header("Patient UI Texts")]
    public TextMeshProUGUI SureName;
    public TextMeshProUGUI LastName, Id, Age, Gender, PhoneNumber, InsuranceCompany, Adress, Complaint; /*IncidentAdress*/
    #endregion

 


    private void Start()
    {
      
        
    }

    public void CloseAllPatientWindows()
    {

        JoinPatientPopUp.SetActive(false);
        PatientMenuParent.SetActive(false);
        PatientInfoParent.SetActive(false);
        ActionLogParent.SetActive(false);
    }
}
