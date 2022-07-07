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
    public static UIManager Instance;

    #region Player UI
    [Header("Player UI Parents")]
    public GameObject CurrentActionBarParent;
    public GameObject AmbulanceActionBarParent, NatanActionBarParent;
    #endregion

    #region Patient UI 
    [Header("Patient UI Parents")]
    public GameObject JoinPatientPopUp;
    public GameObject PatientMenuParent, PatientInfoParent, ActionLogParent;

    [Header("Patient UI Texts")]
    public TextMeshProUGUI SureName;
    public TextMeshProUGUI LastName, Id, Age, Gender, PhoneNumber, InsuranceCompany, Adress, Complaint; /*IncidentAdress*/
    #endregion

    [SerializeField]
    public GameObject MapWindow, ContentPanel;

    //#region EventSystem
    //[Header("EventSystem")]
    //[SerializeField] private EventSystem _eventSystem;
    //private GameObject? _lastSelectedGameObject;
    //private GameObject _currentSelectedGameObject;
    //#endregion

    #region Evacuation UI

    [Header("Evacuation UI Drop Down")]
    public TMP_Dropdown _dropDown;

    public GameObject EvacPatientPopUp;
    #endregion

    #region Car UI
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
        CurrentActionBarParent = AmbulanceActionBarParent;
    }

    public void CloseAllPatientWindows()
    {
        JoinPatientPopUp.SetActive(false);
        PatientMenuParent.SetActive(false);
        PatientInfoParent.SetActive(false);
        ActionLogParent.SetActive(false);
        EvacPatientPopUp.SetActive(false);
    }

    public void PauseHomeBtn()
    {
        MapWindow.SetActive(false);
        ContentPanel.SetActive(true);
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

    private void Start()
    {
        AddRoomToList();
    }


    //Evacuation DropDown in UI 
    void AddRoomToList()
    {
        string[] enumNames = Enum.GetNames(typeof(EvacRoom));
        List<string> roomNames = new List<string>(enumNames);


        _dropDown.AddOptions(roomNames);
    }


}
