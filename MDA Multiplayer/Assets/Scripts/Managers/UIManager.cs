using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    #region Player UI
    [Header("Player UI Parents")]
    public GameObject CurrentActionBarParent;
    public GameObject AmbulanceActionBarParent, NatanActionBarParent, BasicActionMenuParent;
    #endregion

    #region Patient UI 
    [Header("Patient UI Parents")]
    public GameObject JoinPatientPopUp;
    public GameObject PatientMenuParent, PatientInfoParent, ActionLogParent;

    [Header("Patient UI Texts")]
    public TextMeshProUGUI SureName;
    public TextMeshProUGUI LastName, Id, Age, Gender, PhoneNumber, InsuranceCompany, Adress, Complaint; /*IncidentAdress*/
    #endregion

    #region EventSystem
    [Header("EventSystem")]
    [SerializeField] private EventSystem _eventSystem;
    private GameObject? _lastSelectedGameObject;
    private GameObject _currentSelectedGameObject;
    #endregion

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

        _lastSelectedGameObject = _currentSelectedGameObject;
    }

    private void Start()
    {
        // if photonview.isMine
        // 

        CurrentActionBarParent = AmbulanceActionBarParent;
    }

    public void CloseAllPatientWindows()
    {
        JoinPatientPopUp.SetActive(false);
        PatientMenuParent.SetActive(false);
        PatientInfoParent.SetActive(false);
        ActionLogParent.SetActive(false);
    }

    public void GetLastGameObjectSelected()
    {
        if (_eventSystem.currentSelectedGameObject != _currentSelectedGameObject)
        {
            _lastSelectedGameObject = _currentSelectedGameObject;
            _currentSelectedGameObject = _eventSystem.currentSelectedGameObject;

            Debug.Log($"{_currentSelectedGameObject.name}");
        }
    }
}
