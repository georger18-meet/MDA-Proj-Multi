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


    public List<PhotonView> CtRoomList;
    public  List<PhotonView> ShockRoomList;
    public List<PhotonView> ChildrenRoomList;
    public List<PhotonView> EmergencyRoomList;

    //public List<PhotonView> AllPatientsPhotonViews;

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
        ShockRoomList = new List<PhotonView>();
        CtRoomList = new List<PhotonView>();
        ChildrenRoomList = new List<PhotonView>();
        EmergencyRoomList = new List<PhotonView>();
    }





    public void AddPatientToRooms(PhotonView patient, EvacRoom enumRoom)
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


    public void DestroyPatient(PhotonView patient)
    {
        patient.gameObject.SetActive(false);
        
    }
    
    public void DropDown_IndexChanged(int index)
    {
        EvacRoom name = (EvacRoom)index;
        _destinationName.text = name.ToString();

    }

  

}
