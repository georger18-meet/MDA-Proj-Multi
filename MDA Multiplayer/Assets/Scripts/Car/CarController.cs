using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;
    private float _currentSteerAngle;
    private float _currentbreakForce;
    private bool _isBreaking;
    [SerializeField] private bool _isDrivable;

    [SerializeField] private float _motorForce;
    [SerializeField] private float _breakForce;
    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private float _centerOfMassOffset;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;

    private Rigidbody _carRb;
    
    public GameObject CarHeadLights;
    private bool _carHeadLightsOn = false;

    public GameObject CarSiren/*, CarSirenLightLeft, CarSirenLightRight*/;
    public AudioSource CarSirenAudioSource;
    private bool _carSirenOn = false;

    public List<CarDoorCollision> CarDoorCollisions;

    public GameObject CarDashboardUI;

    private void Start()
    {
        _carRb = GetComponent<Rigidbody>();
        _carRb.centerOfMass = new Vector3(_carRb.centerOfMass.x, _carRb.centerOfMass.y - _centerOfMassOffset, _carRb.centerOfMass.z);
    }

    private void Update()
    {
        CheckIfDriveable();
        GetInput();
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
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
            _isBreaking = Input.GetKey(KeyCode.Space);
        }
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = _verticalInput * _motorForce;
        frontRightWheelCollider.motorTorque = _verticalInput * _motorForce;
        _currentbreakForce = _isBreaking ? _breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = _currentbreakForce;
        frontLeftWheelCollider.brakeTorque = _currentbreakForce;
        rearLeftWheelCollider.brakeTorque = _currentbreakForce;
        rearRightWheelCollider.brakeTorque = _currentbreakForce;
    }

    private void HandleSteering()
    {
        _currentSteerAngle = _maxSteerAngle * _horizontalInput;
        frontLeftWheelCollider.steerAngle = _currentSteerAngle;
        frontRightWheelCollider.steerAngle = _currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
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
                _carRb.drag = 0;
                CarDashboardUI.SetActive(true);
            }
            else if (item.SeatNum == 1 && !item.SeatOccupied)
            {
                _isDrivable = false;
                _carRb.drag = 10;
                CarDashboardUI.SetActive(false);
            }
        }
    }
}
