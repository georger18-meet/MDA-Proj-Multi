using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using TMPro;


public class EvacuationManager : MonoBehaviour
{
  
    [SerializeField] private TMP_Text _destinationName;


    public List<Patient> CtRoomList;
    public  List<Patient> ShockRoomList;
    public List<Patient> ChildrenRoomList;
    public List<Patient> EmergencyRoomList;


    // Singleton Declaration
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
        ShockRoomList = new List<Patient>();
        CtRoomList = new List<Patient>();
        ChildrenRoomList = new List<Patient>();
        EmergencyRoomList = new List<Patient>();
    }

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
    
    public void DropDown_IndexChanged(int index)
    {
        EvacRoom name = (EvacRoom)index;
        _destinationName.text = name.ToString();

    }

  

}
