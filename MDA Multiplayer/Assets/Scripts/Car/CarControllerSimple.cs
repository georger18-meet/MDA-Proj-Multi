using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerSimple : MonoBehaviour
{
    private float _verticalInput;
    private float _horizontalInput;
    private float _currentbreakForce;
    private bool _isBreaking;
    private bool _isMovingBackwards;
    [SerializeField] private bool _isDrivable;

    [SerializeField] private float _forwardSpeed = 20;
    [SerializeField] private float _reverseSpeed = 15;
    [SerializeField] private float _turningSpeed = 2;
    [SerializeField] private float _breakForce;
    [SerializeField] private float _centerOfMassOffset;



    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;

    private Rigidbody _carRb;

    public GameObject CarHeadLights;
    private bool _carHeadLightsOn = false;

    public GameObject CarSiren;
    public AudioSource CarSirenAudioSource;
    private bool _carSirenOn = false;

    public List<CarDoorCollision> CarDoorCollisions;

    public GameObject CarDashboardUI;

    private void Start()
    {
        _carRb = GetComponent<Rigidbody>();
        _carRb.centerOfMass = new Vector3(_carRb.centerOfMass.x, _centerOfMassOffset, _carRb.centerOfMass.z);
    }

    private void Update()
    {
        CheckIfDriveable();
        GetInput();
        CheckIsMovingBackwards();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        if (_isDrivable)
        {
            _verticalInput = Input.GetAxis("Vertical");
            _horizontalInput = Input.GetAxis("Horizontal");
            _isBreaking = Input.GetKey(KeyCode.Space);
        }
    }

    private void HandleMotor()
    {
        float moveSpeed = _verticalInput *= _verticalInput > 0 ? _forwardSpeed : _reverseSpeed;
        _carRb.AddForce(transform.forward * moveSpeed, ForceMode.Acceleration);
    }

    private void ApplyBreaking(float moveSpeed)
    {
    }

    private void HandleSteering()
    {
        float turnSpeed = _horizontalInput * _turningSpeed * Time.deltaTime * _carRb.velocity.magnitude;
        if (_isMovingBackwards)
        {
            turnSpeed = -turnSpeed;
        }
        transform.Rotate(0, turnSpeed, 0, Space.World);
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(Transform wheelTransform)
    {
        if (_isMovingBackwards)
        {
            wheelTransform.Rotate(-_carRb.velocity.magnitude, 0, 0);
        }
        else
        {
            wheelTransform.Rotate(_carRb.velocity.magnitude, 0, 0);
        }
    }

    private void CheckIsMovingBackwards()
    {
        if (_carRb.angularVelocity.y > 0)
        {
            _isMovingBackwards = true;
        }
        else
        {
            _isMovingBackwards = false;
        }
    }


    public void ToggleHeadlights()
    {
        if (_carHeadLightsOn)
        {
            _carHeadLightsOn = false;
            CarHeadLights.SetActive(false);
            CarSiren.GetComponent<Animator>().enabled = false;
            //CarSirenLightLeft.SetActive(false);
            //CarSirenLightRight.SetActive(false);
        }
        else
        {
            _carHeadLightsOn = true;
            CarHeadLights.SetActive(true);
            CarSiren.GetComponent<Animator>().enabled = true;
            //CarSirenLightLeft.SetActive(true);
            //CarSirenLightRight.SetActive(true);
        }
    }

    public void ToggleSiren()
    {
        if (_carSirenOn)
        {
            _carSirenOn = false;
            CarSirenAudioSource.Stop();
        }
        else
        {
            _carSirenOn = true;
            CarSirenAudioSource.Play();
        }
    }


    private void CheckIfDriveable()
    {
        foreach (var item in CarDoorCollisions)
        {
            if (item.SeatNum == 1 && item.SeatOccupied)
            {
                _isDrivable = true;
                _carRb.isKinematic = false;
                CarDashboardUI.SetActive(true);
            }
            else if (item.SeatNum == 1 && !item.SeatOccupied)
            {
                _isDrivable = false;
                _carRb.isKinematic = true;
                CarDashboardUI.SetActive(false);
            }
        }
    }
}
