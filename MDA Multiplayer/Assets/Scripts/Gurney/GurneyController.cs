using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GurneyController : MonoBehaviour
{
    public bool TakeOutBed;
    public bool Folded;
    public bool HasPatient;
    public GameObject Patient;
    public GameObject Player;
    public GameObject GurneyUnFolded, GurneyFolded, GurneyGFX;
    public GameObject InteractionsBar;
    public Transform PlayerHoldPos;
    public Transform PatientPosOnBed;
    public Transform PatientPosOffBed;
    public Transform GurneyPosInCar, GurneyPosOutCar;
    public TextMeshProUGUI _TakeReturnText, FollowUnfollowText, PlaceRemovePatientText;

    private bool _isFollowingPlayer;
    //private bool _isFacingTrolley = false;
    private bool _inCar;

    // Start is called before the first frame update
    void Start()
    {
        InteractionsBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        AlwaysChecking();
    }

    private void AlwaysChecking()
    {
        // In Car
        if (_inCar)
        {
            GurneyGFX.GetComponent<BoxCollider>().isTrigger = true;
            Folded = true;
        }
        else if (!_inCar)
        {
            GurneyGFX.GetComponent<BoxCollider>().isTrigger = false;
            Folded = false;
        }

        // Fold
        FoldUnfold();

        // Follow Player
        FollowPlayer();

        // Take Out Bed
        TakeOutReturnBed();
    }

    public void ShowInteractionsToggle()
    {
        if (InteractionsBar.activeInHierarchy)
        {
            InteractionsBar.SetActive(false);
        }
        else if (!InteractionsBar.activeInHierarchy)
        {
            InteractionsBar.SetActive(true);
        }
    }

    public void FoldUnfoldToggle()
    {
        if (Folded)
        {
            Folded = false;
        }
        else if (!Folded)
        {
            Folded = true;
        }
    }

    private void FoldUnfold()
    {
        if (Folded)
        {
            GurneyUnFolded.SetActive(false);
            GurneyFolded.SetActive(true);
        }
        else if (!Folded)
        {
            GurneyUnFolded.SetActive(true);
            GurneyFolded.SetActive(false);
        }
    }

    public void FollowPlayerToggle()
    {
        if (Player != null && TakeOutBed)
        {
            if (_isFollowingPlayer)
            {
                _isFollowingPlayer = false;
            }
            else if (!_isFollowingPlayer)
            {
                _isFollowingPlayer = true;
            }
            InteractionsBar.SetActive(false);
        }
    }

    private void FollowPlayer()
    {
        if (Player != null)
        {
            if (_isFollowingPlayer)
            {
                Player.transform.position = PlayerHoldPos.position;
                //if (!_isFacingTrolley)
                //{
                //var lookPos = transform.position - Player.transform.position;
                //lookPos.y = 0f;
                //var rotation = Quaternion.LookRotation(lookPos);
                //Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, rotation, Time.deltaTime * 10f);
                //print($"{Mathf.Abs((Player.transform.rotation.y * Mathf.Rad2Deg) - (rotation.y * Mathf.Rad2Deg))}");
                //if (Mathf.Abs((Player.transform.rotation.y * Mathf.Rad2Deg) - (rotation.y * Mathf.Rad2Deg)) <= 2)
                //{
                //    _isFacingTrolley = true;
                //    gameObject.transform.SetParent(Player.transform);
                //    FollowUnfollowText.text = "Detach \n Bed";
                //}
                //}
                Player.transform.LookAt(transform.position);
                gameObject.transform.SetParent(Player.transform);
                FollowUnfollowText.text = "Detach \n Bed";
            }
            else if (!_isFollowingPlayer)
            {
                //_isFacingTrolley = false;
                gameObject.transform.SetParent(null);
                FollowUnfollowText.text = "Attach \n Bed";
            }
        }
    }

    public void PutRemovePatient()
    {
        if (Patient != null && !HasPatient)
        {
            HasPatient = true;
            Patient.GetComponent<BoxCollider>().enabled = false;
            Patient.transform.position = PatientPosOnBed.position;
            Patient.transform.rotation = PatientPosOnBed.rotation;
            Patient.transform.SetParent(this.transform);
            PlaceRemovePatientText.text = "Drop \n Patient";
            InteractionsBar.SetActive(false);
        }
        else if (Patient != null && HasPatient && !_inCar)
        {
            HasPatient = false;
            Patient.GetComponent<BoxCollider>().enabled = true;
            Patient.transform.position = PatientPosOffBed.position;
            Patient.transform.SetParent(null);
            PlaceRemovePatientText.text = "Place \n Patient";
            InteractionsBar.SetActive(false);
        }
    }

    public void TakeOutReturnBedToggle()
    {
        if (_inCar && !TakeOutBed)
        {
            TakeOutBed = true;
            InteractionsBar.SetActive(true);
            transform.position = GurneyPosOutCar.position;
            transform.rotation = GurneyPosOutCar.rotation;
            FollowPlayerToggle();
        }
        else if (_inCar && TakeOutBed)
        {
            TakeOutBed = false;
            InteractionsBar.SetActive(false);
        }
    }

    private void TakeOutReturnBed()
    {
        if (_inCar && !TakeOutBed)
        {
            _isFollowingPlayer = false;
            transform.position = GurneyPosInCar.position;
            transform.rotation = GurneyPosInCar.rotation;
            transform.SetParent(GurneyPosInCar);
            _TakeReturnText.text = "Take Out";
        }
        else if (_inCar && TakeOutBed)
        {
            _TakeReturnText.text = "Return";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.gameObject;
        }
        if (other.CompareTag("Patient"))
        {
            if (!HasPatient)
            {
                Patient = other.gameObject;
            }
        }
        if (other.CompareTag("Car"))
        {
            _inCar = true;
        }
        if (other.CompareTag("Evac"))
        {
            Patient.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player = null;
            //InteractionsBar.SetActive(false);
        }
        if (other.CompareTag("Patient"))
        {
            if (!HasPatient)
            {
                Patient = null;
            }
        }
        if (other.CompareTag("Car"))
        {
            _inCar = false;
        }
        if (other.CompareTag("Evac"))
        {
            Patient.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
