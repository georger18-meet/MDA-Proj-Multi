using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoorCollision : MonoBehaviour
{
    public bool IsDoorOpen = false;
    public bool IsSeatOccupied = false;
    public int SeatNumber;
    public GameObject CollidingPlayer;
    public Transform SeatPosition;


  [SerializeField] private CarControllerSimple _carController;
    private Animator _doorAnimator;

    void Start()
    {
        _doorAnimator = GetComponent<Animator>();
        
    }

    void Update()
    {
        //GetComponent<Collider>().enabled = IsDoorOpen;
        EnterExitVehicle();
    }

    public void OpenCloseDoorToggle(int number)
    {
        if (IsDoorOpen)
        {
            UIManager.Instance.CurrentActionBarParent = number switch
            {
                0 => UIManager.Instance.AmbulanceActionBarParent,
                1 => UIManager.Instance.NatanActionBarParent,
                _ => UIManager.Instance.AmbulanceActionBarParent,
            };

            IsDoorOpen = false;
            _doorAnimator.SetBool("IsDoorOpen", false);
        }
        else if (!IsDoorOpen)
        {
            UIManager.Instance.CurrentActionBarParent = number switch
            {
                0 => UIManager.Instance.AmbulanceActionBarParent,
                1 => UIManager.Instance.NatanActionBarParent,
                _ => UIManager.Instance.AmbulanceActionBarParent,
            };

            IsDoorOpen = true;
            _doorAnimator.SetBool("IsDoorOpen", true);
            if (IsSeatOccupied)
            {
                EnterExitToggle(number);
            }
        }
    }

    private void EnterExitVehicle()
    {
        if (CollidingPlayer != null)
        {
            if (IsSeatOccupied)
            {
                CollidingPlayer.transform.SetPositionAndRotation(SeatPosition.position, SeatPosition.rotation);
            }
        }
    }

    public void EnterExitToggle(int number)
    {
        if (IsDoorOpen && CollidingPlayer != null)
        {
            PlayerController playerController = CollidingPlayer.GetComponent<PlayerController>();

            if (!IsSeatOccupied)
            {
                _carController._transfer.CarDriver();
                Debug.Log("supposed to drive");
                OpenCloseDoorToggle(number);
                IsSeatOccupied = true;
                playerController.IsDriving = true;
                // use player driving state
            }
            else if (IsSeatOccupied)
            {
                Debug.Log("NOT supposed to drive");
                IsSeatOccupied = false;
                CollidingPlayer.transform.position = gameObject.transform.position;
                playerController.IsDriving = false;
                // use player driving state
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !IsSeatOccupied)
        {
            CollidingPlayer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsSeatOccupied)
        {
            CollidingPlayer = null;
        }
    }
}
