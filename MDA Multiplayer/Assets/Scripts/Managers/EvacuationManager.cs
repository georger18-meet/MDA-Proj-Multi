using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


public class EvacuationManager : MonoBehaviour
{
    [SerializeField] public EvacRoom RoomEnum;
    [SerializeField] private string _roomName;
    [SerializeField] private GameObject _evacuationUI;
    [SerializeField] private TMP_Text _destinationName;

    #region Evacuation Rooms Lists

     public List<Patient> CtRoomList;
     public List<Patient> ShockRoomList;
     public List<Patient> ChildrenRoomList;
     public List<Patient> EmergencyRoomList;

     public List<Patient> NearbyPatient;

    //public List<Patient> AllPatients;

    public List<EvacuationManager> evacuationRooms = new List<EvacuationManager>();

    #endregion
    [SerializeField] private List<GameObject> _patientsEvacuated = new List<GameObject>();
    private GameObject _currentPatient;

  // public List<string> roomNames = new List<string>() { "Room 1", "Room 2", "Room 3", "Room 4" };

    void Start()
    {
       // _destinationName.text = $"Evacuate Patient To {_roomName}?";
        _evacuationUI.SetActive(false);
        // Debug.Log((int)RoomEnum);

    }

    public EvacuationManager(EvacRoom RoomEnum)
    {
        this.RoomEnum = RoomEnum;
    }

    public bool HasRelationToRoom(string otherRoom)
    {
        foreach (EvacuationManager evacuationManager in evacuationRooms)
        {
            if (evacuationManager.RoomEnum.ToString() == otherRoom)
            {
                Debug.Log(otherRoom);
                return true;
            }
        }
        Debug.Log(otherRoom);
        return false;
    }

    public void AddPatientToRooms()
    {
        
            Debug.Log($"Attempting to Click On NPC");


    }
    



    public void EvacuatePatient(bool choice)
    {
        if (choice)
        {
            _patientsEvacuated.Add(_currentPatient);
            _currentPatient.SetActive(false);
            _evacuationUI.SetActive(false);
            _currentPatient = null;
        }
        else
        {
            _evacuationUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    


        if (!other.TryGetComponent(out Patient possiblePatient))
        {
            return;
        }
        else if (!NearbyPatient.Contains(possiblePatient))
        {
            NearbyPatient.Add(possiblePatient);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Patient possiblePatient))
        {

            if (!NearbyPatient.Contains(possiblePatient))
            {
                return;
            }
            else
            {
                NearbyPatient.Remove(possiblePatient);
            }
        }
    }

    public void AddUserToEvacuationLists(string currentPlayer)
    {
        Patient currentPlayerData = GameObject.Find(currentPlayer).GetComponent<Patient>();
        //PlayerData currentPlayerData = currentPlayer != null ? currentPlayer as PlayerData : null;

        if (currentPlayerData == null)
        {
            return;
        }

        for (int i = 0; i < 1; i++)
        {
           
        }
    }

    public void DropDown_IndexChanged(int index)
    {
        EvacRoom name = (EvacRoom)index;
        _destinationName.text = name.ToString();

    }

  

}
