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
    private OwnershipTransfer _transfer;

    [SerializeField] private CarControllerSimple _carController;
    private Animator _doorAnimator;

    void Start()
    {
        _transfer = GetComponent<OwnershipTransfer>();
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
                0 => UIManager.Instance.AmbulanceBar,
                1 => UIManager.Instance.NatanBar,
                _ => UIManager.Instance.AmbulanceBar,
            };

            IsDoorOpen = false;
            _doorAnimator.SetBool("IsDoorOpen", false);
        }
        else if (!IsDoorOpen)
        {
            UIManager.Instance.CurrentActionBarParent = number switch
            {
                0 => UIManager.Instance.AmbulanceBar,
                1 => UIManager.Instance.NatanBar,
                _ => UIManager.Instance.AmbulanceBar,
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
                
                Debug.Log("supposed to drive");

                OpenCloseDoorToggle(number);
                IsSeatOccupied = true;
                playerController.IsDriving = true;
                
                if (SeatNumber == 0)
                {
                    _carController._transfer.CarDriver();
                    playerController.CurrentCarController = _carController;
                    //playerController.PhotonView.RPC("ChangeCharControllerStateRPC", Photon.Pun.RpcTarget.Others);
                }
                // use player driving state
            }
            else if (IsSeatOccupied)
            {
                Debug.Log("NOT supposed to drive");
                IsSeatOccupied = false;
                CollidingPlayer.transform.position = gameObject.transform.position;
                playerController.IsDriving = false;

                if (SeatNumber != 0)
                {
                    playerController.CurrentCarController = null;
                    //playerController.PhotonView.RPC("ChangeCharControllerStateRPC", Photon.Pun.RpcTarget.Others);
                }
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
