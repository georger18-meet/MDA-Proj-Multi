using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    #region Fields
    [Header("Photon")]
    [SerializeField] public PhotonView _photonView;

    [Header("Data")]
    public PlayerData PlayerData;

    [Header("Camera")]
    private Camera _currentCamera;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Camera _vehicleCamera;
    [SerializeField] private GameObject _MiniMaCamera;
    [SerializeField] private Transform _firstPersonCameraTransform, _thirdPersonCameraTransform;

    [Header("Animation")]
    [SerializeField] private Animator _playerAnimator;

    [Header("Momvement")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Vector2 _mouseSensitivity = new Vector2(60f, 40f);
    [SerializeField] private float _turnSpeed = 90f, _walkingSpeed = 6f, _runningSpeed = 11f, _flyingSpeed = 16f;
    [SerializeField] private float _jumpForce = 3f, _flyUpwardsSpeed = 9f, _maxFlyingHeight = 100f;
    private float _stateSpeed;
    private bool _isDriving;
    public bool IsDriving { get => _isDriving; set => _isDriving = value; }
    private Vector2 _input;

    [Header("Physics")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private float _groundCheckRadius = 0.5f;
    private bool _isGrounded;
    #endregion

    #region State Machine
    private delegate void State();
    private State _stateAction;
    #endregion

    #region Monobehavior Callbacks
    private void Awake()
    {
        PlayerData = gameObject.AddComponent<PlayerData>();
        _currentCamera = _playerCamera;
    }

    private void Start()
    {
        if (_photonView.IsMine)
        {
            FreeMouse(true);
            _stateAction = UseTankIdleState;
            _MiniMaCamera.SetActive(true);
        }
        else
        {
            Destroy(this);
            _MiniMaCamera.SetActive(false);
        }
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            _stateAction.Invoke();
        }
    }
    #endregion

    #region Private Methods
    private void GetInputAxis()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void UseTankMovement()
    {
        Vector3 moveDirerction;
        float actualSpeed = Input.GetKey(KeyCode.LeftShift) ? _runningSpeed : _walkingSpeed;
        moveDirerction = actualSpeed * _input.y * transform.forward;

        if (_input.y > 0)
        {
            _playerAnimator.SetFloat("Movement Speed", actualSpeed == _walkingSpeed ? 0.5f : 1f, 0.1f, Time.deltaTime);
        }
        else
        {
            _playerAnimator.SetFloat("Movement Speed", 0f, 0.1f, Time.deltaTime);
        }

        // moves the character in diagonal direction
        _characterController.Move(moveDirerction * Time.deltaTime - Vector3.up * 0.1f);
    }

    private void UseFlyingMovement()
    {
        float yPosition = transform.position.y;
        Vector3 moveDirerction;
        moveDirerction = (Input.GetKey(KeyCode.LeftShift) ? _flyingSpeed * 2 : _flyingSpeed) * _input.y * transform.forward;

        // moves the character in diagonal direction
        _characterController.Move(moveDirerction * Time.deltaTime - Vector3.up * 0.1f);
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }

    private void UseTankRotate()
    {
        _playerAnimator.SetFloat("Rotatation Speed", _input.x, 0.1f, Time.deltaTime);
        transform.Rotate(0, _input.x * _turnSpeed * Time.deltaTime, 0);
    }

    private void UseFirstPersonMovement()
    {
        float actualSpeed = Input.GetKey(KeyCode.LeftShift) ? _runningSpeed : _walkingSpeed;
        _characterController.Move(actualSpeed * _input.x * Time.deltaTime * transform.right + actualSpeed * _input.y * Time.deltaTime * transform.forward);
    }

    private void UseFirstPersonRotate()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));

        transform.Rotate(_mouseSensitivity.x * mouseInput.x * Time.deltaTime * Vector3.up);
        _currentCamera.transform.Rotate(_mouseSensitivity.y * mouseInput.y * Time.deltaTime * Vector3.right);
    }

    private void SetFirstPersonCamera(bool value)
    {
        _currentCamera.transform.position = value ? _firstPersonCameraTransform.position : _thirdPersonCameraTransform.position;
        _currentCamera.transform.rotation = value ? _firstPersonCameraTransform.rotation : _thirdPersonCameraTransform.rotation;
    }

    private void FreeMouse(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void RotateBodyWithMouse()
    {
        if (Input.GetMouseButton(1))
        {
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));

            transform.Rotate(Vector3.up * mouseInput.x * _mouseSensitivity.x * Time.deltaTime);
            _currentCamera.transform.Rotate(Vector3.right * mouseInput.y * _mouseSensitivity.y * Time.deltaTime);
        }
    }

    private void FreeMouseWithAlt()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            FreeMouse(!Cursor.visible);
        }
    }
    #endregion

    #region States

    private void UseTankIdleState()
    {
        if (_photonView.IsMine)
        {
           //Debug.Log("Current State: Idle");

            _playerAnimator.SetFloat("Movement Speed", 0f, 0.1f, Time.deltaTime);
            _playerAnimator.SetFloat("Rotatation Speed", 0f, 0.1f, Time.deltaTime);

            GetInputAxis();

            if (_isDriving)
            {
                _stateAction = UseDrivingState;
            }

            if (_input != Vector2.zero)
            {
                FreeMouse(true);
                _stateAction = UseTankWalkingState;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                FreeMouse(false);
                SetFirstPersonCamera(true);
                _stateAction = UseFirstPersonIdleState;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _stateAction = UseFlyingIdleState;
            }

            RotateBodyWithMouse();
        }
    }

    private void UseFirstPersonIdleState()
    {
        if (_photonView)
        {
            Debug.Log("Current State: First Person Idle");

            GetInputAxis();

            if (_isDriving)
            {
                _stateAction = UseDrivingState;
            }

            if (_input != Vector2.zero)
            {
                FreeMouse(false);
                _stateAction = UseFirstPersonWalkingState;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                FreeMouse(true);
                SetFirstPersonCamera(false);
                _stateAction = UseTankIdleState;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _stateAction = UseFlyingIdleState;
            }

            UseFirstPersonRotate();
            FreeMouseWithAlt();
        }
    }

    private void UseTankWalkingState()
    {
        if (_photonView.IsMine)
        {
           // Debug.Log("Current State: Walking");

            GetInputAxis();

            if (_isDriving)
            {
                _stateAction = UseDrivingState;
            }

            if (_input == Vector2.zero)
            {
                FreeMouse(true);
                _stateAction = UseTankIdleState;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                FreeMouse(false);
                SetFirstPersonCamera(true);
                _stateAction = UseFirstPersonWalkingState;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _stateAction = UseFlyingMovingState;
            }

            RotateBodyWithMouse();
            UseTankRotate();
            UseTankMovement();
        }
    }

    private void UseFirstPersonWalkingState()
    {
        if (_photonView.IsMine)
        {
            Debug.Log("Current State: First Person Walking");

            GetInputAxis();

            if (_isDriving)
            {
                _stateAction = UseDrivingState;
            }

            if (_input == Vector2.zero)
            {
                _stateAction = UseFirstPersonIdleState;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                SetFirstPersonCamera(false);

                if (_input == Vector2.zero)
                {
                    FreeMouse(true);
                    _stateAction = UseTankIdleState;
                }
                else
                {
                    FreeMouse(true);
                    _stateAction = UseTankWalkingState;
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _stateAction = UseFlyingMovingState;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (Cursor.visible)
                {
                    FreeMouse(false);
                }
                else
                {
                    FreeMouse(true);
                }
            }

            UseFirstPersonRotate();
            UseFirstPersonMovement();
            FreeMouseWithAlt();
        }
    }

    private void UseFlyingIdleState()
    {
        if (_photonView.IsMine)
        {
            Debug.Log("Current State: FlyingIdle");

            GetInputAxis();

            if (_isDriving)
            {
                _stateAction = UseDrivingState;
            }

            if (_input != Vector2.zero)
            {
                FreeMouse(false);
                _stateAction = UseFlyingMovingState;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _stateAction = UseTankIdleState;
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.position += new Vector3(0, _flyUpwardsSpeed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                transform.position -= new Vector3(0, _flyUpwardsSpeed * Time.deltaTime, 0);
            }

            RotateBodyWithMouse();
        }
    }

    private void UseFlyingMovingState()
    {
        if (_photonView.IsMine)
        {
            Debug.Log("Current State: FlyingMoving");

            GetInputAxis();

            if (_input == Vector2.zero)
            {
                _stateAction = UseFlyingIdleState;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _stateAction = UseTankIdleState;
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.position += new Vector3(0, _flyUpwardsSpeed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                transform.position -= new Vector3(0, _flyUpwardsSpeed * Time.deltaTime, 0);
            }

            RotateBodyWithMouse();
            UseTankRotate();
            UseFlyingMovement();
        }
    }

    private void UseDrivingState()
    {
        if (_photonView.IsMine)
        {
            Debug.Log("Current State: Driving");

            _playerCamera.enabled = false;
            _vehicleCamera.enabled = true;
            _currentCamera = _vehicleCamera;
            _characterController.enabled = false;

            if (!_isDriving)
            {
                _vehicleCamera.enabled = false;
                _playerCamera.enabled = true;
                _currentCamera = _playerCamera;
                _characterController.enabled = true;
                _stateAction = UseTankIdleState;
            }
        }
    }

    private void UseTreatingState()
    {
        if (_photonView.IsMine)
        {

        }
    }
    #endregion

    //[PunRPC]
    //private void RPC_AddUserToTreatingLists(int currentPlayer)
    //{
    //    Player currentPlayerData = PhotonNetwork.LocalPlayer.Get(currentPlayer);
    //    Debug.Log("currentPlayerData ID" + " " + currentPlayerData);
    //
    //    GetComponent<PlayerData>().CurrentPatientNearby.TreatingUsersTest.Add(currentPlayerData.ActorNumber);
    //}

    #region Collisions & Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (_photonView.IsMine)
        {
            if (!other.gameObject.TryGetComponent(out Patient possiblePatient))
            {
                return;
            }
            PlayerData.CurrentPatientNearby = possiblePatient;
            UIManager.Instance.CurrentActionBarParent.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_photonView.IsMine)
        {
            if (!other.gameObject.TryGetComponent(out Patient possiblePatient))
            {
                return;
            }
            PlayerData.CurrentPatientNearby = null;
            UIManager.Instance.CurrentActionBarParent.SetActive(false);
        }
        
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        if (_photonView.IsMine)
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (_isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(_groundCheckTransform.position, _groundCheckRadius);
        }
    }
    #endregion
}
