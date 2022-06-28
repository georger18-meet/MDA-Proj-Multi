using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using TMPro;


public class EvacuationManager : MonoBehaviour
{
   // [SerializeField] public EvacRoom RoomEnum;
    // public GameObject _evacuationUI;
    [SerializeField] private TMP_Text _destinationName;

    //public List<Patient> AllPatients;

    public List<Patient> CtRoomList;
    public  List<Patient> ShockRoomList;
    public List<Patient> ChildrenRoomList;
    public List<Patient> EmergencyRoomList;

    private Evacuation evacuation;

   // public List<EvacuationNpc> _evacuationNpcs;

    // public List<EvacuationManager> evacuationRooms = new List<EvacuationManager>();

    //  [SerializeField] private List<GameObject> _patientsEvacuated = new List<GameObject>();
   // private GameObject _currentPatient;

    // public List<string> roomNames = new List<string>() { "Room 1", "Room 2", "Room 3", "Room 4" };
    
    public static EvacuationManager Instance;

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
    }

    void Start()
    {
       // _destinationName.text = $"Evacuate Patient To {_roomName}?";
       // _evacuationUI.SetActive(false);
       // Debug.Log((int)RoomEnum);
        ShockRoomList = new List<Patient>();
        CtRoomList = new List<Patient>();
        ChildrenRoomList = new List<Patient>();
        EmergencyRoomList = new List<Patient>();

}

//public EvacuationManager(EvacRoom RoomEnum)
//{
//    this.RoomEnum = RoomEnum;
//}

//public bool HasRelationToRoom(string otherRoom)
//{
//    foreach (EvacuationManager evacuationManager in evacuationRooms)
//    {
//        if (evacuationManager.RoomEnum.ToString() == otherRoom)
//        {
//            Debug.Log(otherRoom);
//            return true;
//        }
//    }
//    Debug.Log(otherRoom);
//    return false;
//}

public void AddPatientToRooms(Patient patient, EvacRoom enumRoom)
    {
        switch (enumRoom)
        {
            case EvacRoom.Children_Room:
                ChildrenRoomList.Add(patient);
                break;
            case EvacRoom.CT_Room:
                CtRoomList.Add(patient);
                break;
            case EvacRoom.Emergency_Room:
                EmergencyRoomList.Add(patient);
                break;
            case EvacRoom.Shock_Room:
                ShockRoomList.Add(patient);
                break;
        }
    }

    public void DestroyPatient(Patient patient)
    {
        patient.gameObject.SetActive(false);
        
    }
    //public void EvacuatePatient(bool choice)
    //{
    //    if (choice)
    //    {
    //        _patientsEvacuated.Add(_currentPatient);
    //        _currentPatient.SetActive(false);
    //        _evacuationUI.SetActive(false);
    //        _currentPatient = null;
    //    }
    //    else
    //    {
    //        _evacuationUI.SetActive(false);
    //    }
    //}


  


    //public void AddUserToEvacuationLists(int currentPatient)
    //{
    //    Patient currentPlayerData = _currentPatient.GetComponent<Patient>();
    //    //PlayerData currentPlayerData = currentPlayer != null ? currentPlayer as PlayerData : null;

    //    if (currentPlayerData == null)
    //    {
    //        return;
    //    }

    //    for (int i = 0; i < 1; i++)
    //    {


    //    }




    //}

    public void DropDown_IndexChanged(int index)
    {
        EvacRoom name = (EvacRoom)index;
        _destinationName.text = name.ToString();

    }

  

}
