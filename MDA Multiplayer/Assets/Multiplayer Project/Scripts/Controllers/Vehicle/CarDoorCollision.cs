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

    private Animator _doorAnimator;

    void Start()
    {
        _doorAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        GetComponent<Collider>().enabled = IsDoorOpen;
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
            if (!IsSeatOccupied)
            {
                PlayerController playerController = CollidingPlayer.GetComponent<PlayerController>();
                OpenCloseDoorToggle(number);
                IsSeatOccupied = true;
                // use player driving state
            }
            else if (IsSeatOccupied)
            {
                IsSeatOccupied = false;
                CollidingPlayer.transform.position = gameObject.transform.position;
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
