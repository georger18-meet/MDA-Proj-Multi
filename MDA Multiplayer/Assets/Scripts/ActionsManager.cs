using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ActionsManager : MonoBehaviourPunCallbacks
{
    private ActionTemplates _actionTemplates;
    private ActionsHandler _actionsHandler;

    

    #region Data References
    [Header("Data & Scripts")]
    [field: SerializeField] public PlayerData _playerData;
    
    private Patient _currentPatientScript;
    private PatientData _currentPatientData;

    public PatientData CurrentPatientData { get => _currentPatientData; }
    #endregion

    #region Prefab References
    [Header("Prefabs")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _monitor;
    #endregion

    #region Player UI
    [Header("Player UI Parents")]
    [SerializeField] private GameObject _ambulanceActionBarParent;
    [SerializeField] private GameObject _natanActionBarParent, _basicActionMenuParent;
    //[SerializeField] private GameObject

    //[Header("Player UI Texts")]
    //[SerializeField] private TextMeshProUGUI
    //[SerializeField] private TextMeshProUGUI
    #endregion

    #region Patient UI 
    // will be transmuted into scriptableObject
    [Header("Patient UI Parents")]
    [SerializeField] private GameObject _joinPatientPopUp;
    [SerializeField] private GameObject _patientMenuParent, _patientInfoParent, _actionLogParent;

    [Header("Patient UI Texts")]
    [SerializeField] private TextMeshProUGUI _sureName;
    [SerializeField] private TextMeshProUGUI _lastName, _id, _age, _gender, _phoneNumber, _insuranceCompany, _adress, _complaint; /*_incidentAdress*/

    [Header("Patient Fixed Treatment Positions")]
    public Transform PlayerTreatingTr;
    public Transform PatientEquipmentTr;
    #endregion

    #region PauseMenu
    [SerializeField]
    private GameObject _mapWindow, _contentPanel;
    #endregion

    // may be removed or changed to decouple
    [field: SerializeField]
    public string UserName, CrewName;

    [field: SerializeField]
    public int UserIndexInCrew, CrewIndex;

    [SerializeField] private List<GameObject> _ambulanceActionBtnParents, _natanActionBtnParents;


    private PhotonView _photonView;



    #region MonoBehaviour Callbacks

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            foreach (GameObject btnParent in _ambulanceActionBtnParents)
                btnParent.GetComponentInChildren<Button>().gameObject.SetActive(false);

        }
     

        if (_photonView.IsMine)
        {
            foreach (GameObject btnParent in _ambulanceActionBtnParents)
                btnParent.GetComponentInChildren<Button>().interactable = false;

            _actionsHandler = new ActionsHandler();

            // from patient scripts
            _joinPatientPopUp.SetActive(false);
            _actionLogParent.SetActive(false);
            _patientInfoParent.SetActive(false);
        }

   
    }
    #endregion

    #region Assignment
    // Triggered upon Clicking on the Patient
    public void SetOperatingCrewCheck(GameObject patient)
    {

        //if (_currentPatientInfoSo == null && patient.CompareTag("Patient"))
        //{
        //    _currentPatientScript = patient.GetComponent<Patient>();
        //    _currentPatientInfoSo = _currentPatientScript.PatientInfoSO;
        //    GetPatientInfo();
        //}

        if (_player == null)
        {
            return;
        }
        else if (!_currentPatientScript.OperatingUserCrew.ContainsKey(_playerData.UserName))
        {
            _joinPatientPopUp.SetActive(true);
        }
        else if (_currentPatientScript.OperatingUserCrew.ContainsKey(_playerData.UserName))
        {
            SetupPatientInfoDisplay();
            _patientMenuParent.SetActive(true);
        }
    }

    // triggered upon pressing "yes" in patient Join pop-up



    //private void SetOperatingCrew(Dictionary<string, int> operatingUserCrew)
    //{
    //    _photonView.RPC("RPC_SetOperatingCrew", RpcTarget.AllBufferedViaServer, operatingUserCrew);
    //}

    //[PunRPC]
    private void SetOperatingCrew(Dictionary<string, int> operatingUserCrew)
    {

        _currentPatientScript.OperatingUserCrew.Add(_playerData.UserName, _playerData.CrewIndex);
        _currentPatientScript.DisplayDictionary();

    }

    public void ConfirmOperation(bool confirm)
    {
        if (confirm)
        {
            // need to verify that set operating crew is setting an empty group of maximum 4 and insitialize it with current player
            SetOperatingCrew(_currentPatientScript.OperatingUserCrew);
            SetupPatientInfoDisplay();
            _joinPatientPopUp.SetActive(false);
            _patientMenuParent.SetActive(true);
            _patientInfoParent.SetActive(false);

        }
        else
        {
            _joinPatientPopUp.SetActive(false);
        }
    }


   

    private void SetupPatientInfoDisplay()
    {
        _sureName.text = _currentPatientData.SureName;
        _sureName.text = _currentPatientData.SureName;
        _gender.text = _currentPatientData.Gender;
        _adress.text = _currentPatientData.AddressLocation;
        _insuranceCompany.text = _currentPatientData.MedicalCompany;
        _complaint.text = _currentPatientData.Complaint;
        //_incidentAdress.text = PatientInfoSO.eventPlace;

        _age.text = _currentPatientData.Age.ToString();
        _id.text = _currentPatientData.Id.ToString();
        _phoneNumber.text = _currentPatientData.PhoneNumber.ToString();
    }

    // For Use Externally
    public bool CheckIfPlayerJoined()
    {
        bool playerIsJoined = false;
        if (_player != null)
        {
            if (_currentPatientScript.OperatingUserCrew.ContainsKey(_playerData.UserName))
            {
                playerIsJoined = true;
            }
        }
        return playerIsJoined;
    }
    #endregion

    #region Patient Menu Events
    // close menu
    public void OpenCloseMenu(GameObject menu)
    {
        if (menu.activeInHierarchy)
            menu.SetActive(false);
        else
            menu.SetActive(true);
    
        print("Open / Close Menu from AM");
    }

    public void CloseAllPatientWindows()
    {
        _joinPatientPopUp.SetActive(false);
        _patientMenuParent.SetActive(false);
        _patientInfoParent.SetActive(false);
        _actionLogParent.SetActive(false);
    }

    public void PauseHomeBtn()
    {
        _mapWindow.SetActive(false);
        _contentPanel.SetActive(true);
    }

    // paitent background info: name, weghit, gender, adress...
    public void PatientInfo()
    {
        OpenCloseMenu(_patientInfoParent);

        _sureName.text = _currentPatientData.SureName;
        _lastName.text = _currentPatientData.LastName;
        _id.text = _currentPatientData.Id.ToString();
        _age.text = _currentPatientData.Age.ToString();
        _gender.text = _currentPatientData.Gender;
        _phoneNumber.text = _currentPatientData.PhoneNumber.ToString();
        _insuranceCompany.text = _currentPatientData.MedicalCompany;
        _adress.text = _currentPatientData.AddressLocation;
        _complaint.text = _currentPatientData.Complaint;

        print("Patient Information");
    }

    // list of actions done on the patient by players, aranged by time stamp
    public void Log()
    {
        OpenCloseMenu(_actionLogParent);
        print("Log Window");
    }

    // open up a form that follows this reference: https://drive.google.com/file/d/1EScLHzpHT_YOk02lS_jzjErDfSGRWj2x/view?usp=sharing
    public void TagMiun()
    {
        print("Tag Miun");
    }

    // take current player out of their crew's list
    public void LeavePatient()
    {
        CloseAllPatientWindows();        
        if (_currentPatientScript.OperatingUserCrew.ContainsKey(_playerData.UserName))
        {
            for (int i = 0; i < _currentPatientScript.OperatingUsers.Count; i++)
            {
                if (_currentPatientScript.OperatingUsers[i] == _playerData.UserName)
                {
                    _currentPatientScript.OperatingUsers.RemoveAt(i);
                    _currentPatientScript.OperatingCrews.RemoveAt(i);
                }
            }

            _currentPatientScript.OperatingUserCrew.Remove(_playerData.UserName);
        }

        print("Leave Patient");
    }
    #endregion

    #region Player Action Events
    public void OpenNoBagActionMenu()
    {
        _actionsHandler.OpenNoBagActionMenu(_basicActionMenuParent);
    }

    public void CallAction(int actionNumInList)
    {
        if (_currentPatientData != null)
        {
            _actionsHandler.RunAction(this, _currentPatientScript, _player, _monitor, _playerData.UserRole, actionNumInList);
        }
    }
    #endregion

    #region Get Patient Data
    private void OnTriggerEnter(Collider other)
    {
        if (_photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Patient"))
            {
                for (int i = 0; i < _ambulanceActionBtnParents.Count; i++)
                {
                    _ambulanceActionBtnParents[i].GetComponentInChildren<Button>().interactable = true;
                }

                _currentPatientScript = other.gameObject.GetComponent<Patient>();
                _currentPatientData = _currentPatientScript.PatientData;
            }
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (_photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Patient"))
            {
                foreach (GameObject btnParent in _ambulanceActionBtnParents)
                    btnParent.GetComponentInChildren<Button>().interactable = false;

                _currentPatientScript = null;
                _currentPatientData = null;
            }
        }
    
    }
    
    #endregion
}
