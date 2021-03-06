using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using System.Xml;

public enum EvacRoom { CT_Room, Emergency_Room, Children_Room, Shock_Room, }

public class UIManager : MonoBehaviour
{
    //#region EventSystem
    //[Header("EventSystem")]
    //[SerializeField] private EventSystem _eventSystem;
    //private GameObject? _lastSelectedGameObject;
    //private GameObject _currentSelectedGameObject;
    //#endregion

    public static UIManager Instance;
    [SerializeField] private Vector3 _leaderMenuOffset;
    [SerializeField] private bool _isLeaderMenuOpen;

    #region Player UI
    [Header("Player UI Parents")]
    public GameObject CurrentActionBarParent;
    public GameObject MapWindow, ContentPanel;
    public GameObject AmbulanceBar, NatanBar;
    public GameObject AmbulanceNoBagPanel, AmbulanceAmbuPanel, AmbulanceKidsAmbuPanel, AmbulanceMedicPanel, AmbulanceDefibrilationPanel, AmbulanceOxygenPanel, AmbulanceMonitorPanel;
    public GameObject NatanNoBagPanel, NatanAmbuPanel, NatanKidsAmbuPanel, NatanMedicPanel, NatanQuickDrugsPanel, NatanOxygenPanel, NatanMonitorPanel;
    public GameObject TeamLeaderMenu;
    public GameObject TeamLeaderNavigationBtn;

    #endregion

    #region Patient UI 
    [Header("Patient UI Parents")]
    public GameObject JoinPatientPopUp;
    public GameObject PatientInfoParent, ActionLogParent;

    [Header("Patient UI Texts")]
    public TextMeshProUGUI SureName;
    public TextMeshProUGUI LastName, Id, Age, Gender, PhoneNumber, InsuranceCompany, Adress, Complaint;
    #endregion

    #region Evacuation UI

    [Header("Evacuation UI Drop Down")]
    public TMP_Dropdown _dropDown;

    public GameObject EvacPatientPopUp;
    #endregion

    #region Vehicle UI
    [Header("Vehicle UI Texts")]
    public GameObject VehicleUI;
    
    #endregion

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

        //_lastSelectedGameObject = _currentSelectedGameObject;
        CurrentActionBarParent = AmbulanceBar;
    }

    private void Start()
    {
        AddRoomToList();
    }

    // catch last gameObject to fire an event
    //public GameObject GetLastGameObjectSelected()
    //{
    //    Debug.Log($"Attemting to get last client who tried to join patient");
    //
    //    if (_eventSystem.currentSelectedGameObject != _currentSelectedGameObject)
    //    {
    //        _lastSelectedGameObject = _currentSelectedGameObject;
    //        _currentSelectedGameObject = _eventSystem.currentSelectedGameObject;
    //
    //        //Debug.Log($"{_currentSelectedGameObject.name}");
    //        return _currentSelectedGameObject;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    public void CloseAllPatientWindows()
    {
        JoinPatientPopUp.SetActive(false);
        PatientInfoParent.SetActive(false);
        ActionLogParent.SetActive(false);
        EvacPatientPopUp.SetActive(false);
    }

    public void CloseAllAmbulanceBags(GameObject currentWindow)
    {
        if (!currentWindow.activeInHierarchy)
        {
            AmbulanceNoBagPanel.SetActive(false);
            AmbulanceAmbuPanel.SetActive(false);
            AmbulanceKidsAmbuPanel.SetActive(false);
            AmbulanceMedicPanel.SetActive(false);
            AmbulanceDefibrilationPanel.SetActive(false);
            AmbulanceOxygenPanel.SetActive(false);
            AmbulanceMonitorPanel.SetActive(false);
        }
    }

    public void CloseAllNatanBags(GameObject currentWindow)
    {
        if (!currentWindow.activeInHierarchy)
        {
            NatanNoBagPanel.SetActive(false);
            NatanAmbuPanel.SetActive(false);
            NatanKidsAmbuPanel.SetActive(false);
            NatanMedicPanel.SetActive(false);
            NatanQuickDrugsPanel.SetActive(false);
            NatanOxygenPanel.SetActive(false);
            NatanMonitorPanel.SetActive(false);
        }
    }

    public void PauseHomeBtn()
    {
        MapWindow.SetActive(false);
        ContentPanel.SetActive(true);
    }

    //Evacuation DropDown in UI 
    void AddRoomToList()
    {
        string[] enumNames = Enum.GetNames(typeof(EvacRoom));
        List<string> roomNames = new List<string>(enumNames);

        _dropDown.AddOptions(roomNames);
    }

    public void PullLeaderMenuDown()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                Debug.Log("Trying To Pull Leader Menu");

                if (!_isLeaderMenuOpen && desiredPlayerData.IsCrewLeader)
                {
                    TeamLeaderMenu.transform.position -= _leaderMenuOffset;

                    _isLeaderMenuOpen = true;
                }
                else if (_isLeaderMenuOpen && desiredPlayerData.IsCrewLeader)
                {
                    TeamLeaderMenu.transform.position += _leaderMenuOffset;
                    _isLeaderMenuOpen = false;
                }
            }
        }
    }
}
