using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] private AudioSource _indicatorSound;
    [SerializeField] private GameObject _indicatorIcon;

    public Transform Cam;
    public Transform Cam1stPersonTempHead;
    public GameObject VC3rdPerson;
    public GameObject VC1stPerson;
    public LayerMask InteractableMask;
    public Transform GroundCheck;
    public Animator PlayerAnimator;
    public GameObject BeanModel;
    private CharacterController _characterController;

    public float MoveSpeed = 4f;
    public float RotationSmoothTime = 0.1f;
    public float Gravity = -9.81f;
    public float GroundDistance = 0.5f;
    public LayerMask GroundMask;
    public float JumpHeight = 3f;
    public float SprintMultiplier = 2f;
    public float MaxFlyingHeight = 100f;
    public bool InControl = true;
    public bool EnableJump = true;
    public bool EnableSprint = true;
    public bool EnableCrouch = true;
    public bool EnableFly = false;
    public bool IsFPS = false;
    public bool GamingStyleMovement = true;

    private float _rotationSmoothVelocity;
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _isMoving;
    private bool _isSprinting;
    private float _TotalMoveSpeed;
    private bool _isCrouching;
    private Vector3 _standingOffset;
    private Vector3 _crouchOffset;
    private float _standingHeight;
    private float _crouchHeight;

    public float FPSMouseSensitivity = 100f;
    private float _yHeadRotation = 0f;
    private bool _isCursorFree;


    [SerializeField] private GameObject UIPanel;

    private PhotonView _photonView;
    //--------------------------------
    // Main Unity Methods
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _photonView = GetComponent<PhotonView>();

    }

    // Start is called before the first frame update
    void Start()
    {

        
        _standingHeight = _characterController.height;
        _standingOffset = _characterController.center;
        _crouchHeight = _standingHeight / 2;
        _crouchOffset.y = -(_crouchHeight / 2);

        if (!GamingStyleMovement)
        {
            _isCursorFree = true;
        }

        if (!_photonView.IsMine)
        {
            UIPanel.gameObject.SetActive(false);
            Cam.parent.gameObject.SetActive(false);
            //Cam.parent.transform.gameObject.SetActive(false);
            //Cam.parent.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (_photonView.IsMine)
        {
            FreeCursorToggle();
            FreeCursor();
            CameraFPSControls();
            if (InControl)
            {
                _characterController.enabled = true;
                MovePlayer();
                ApplyGravity();
                Jump();
                Crouch();
                Fly();
                if (!EnableFly)
                {
                    AnimationController();
                }
            }
            else
            {
                _characterController.enabled = false;
                AnimationController();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                TogglePOV();
            }

            CheckInteraction();
        }

        
            
        
       
    }


    //--------------------------------
    // Created Methods


    public void CameraFPSControls()
    {
        if (InControl)
        {
            _yHeadRotation = 0f;
            Cam1stPersonTempHead.localRotation = Quaternion.Euler(0f, _yHeadRotation, 0f);
        }
        else if (!InControl)
        {
            float mouseX = Input.GetAxis("Mouse X") * FPSMouseSensitivity * Time.deltaTime;
            _yHeadRotation += mouseX;
            _yHeadRotation = Mathf.Clamp(_yHeadRotation, -90f, 90f);
            Cam1stPersonTempHead.localRotation = Quaternion.Euler(0f, _yHeadRotation, 0f);
        }
    }

    public void MovePlayer()
    {
        // Calculate Direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (IsFPS)
        {
            float mouseX = Input.GetAxis("Mouse X") * FPSMouseSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);
        }

        // If There's Input
        if (direction.magnitude >= 0.1f)
        {
            if (IsFPS)
            {
                _isMoving = true;
                Sprint();
                Vector3 moveDirFPS = transform.right * horizontal + transform.forward * vertical;
                _characterController.Move(moveDirFPS * _TotalMoveSpeed * Time.deltaTime);
            }
            else
            {
                _isMoving = true;
                // Rotate Player to Direction of Movement
                // *(The "+ Cam.eulerAngles.y" Makes the Player Face Look Direction)
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
                float angle;
                if (GamingStyleMovement)
                {
                    angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationSmoothVelocity, RotationSmoothTime);
                }
                else
                {
                    angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationSmoothVelocity, RotationSmoothTime * 4);
                }
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // Move Towards Look Direction
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                // Apply Sprint
                Sprint();
                // Move Player
                _characterController.Move(moveDir.normalized * _TotalMoveSpeed * Time.deltaTime);
            }
        }
        else
        {
            _isMoving = false;
        }
    }

    public void ApplyGravity()
    {
        if (!EnableFly)
        {
            _isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2;
            }

            _velocity.y += Gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded && EnableJump)
        {
            _velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }
    }

    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && EnableSprint)
        {
            _isSprinting = true;
        }
        else
        {
            _isSprinting = false;
        }

        if (_isSprinting)
        {
            _TotalMoveSpeed = MoveSpeed * SprintMultiplier;
        }
        else
        {
            _TotalMoveSpeed = MoveSpeed;
        }
    }

    public void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && EnableCrouch && _isGrounded)
        {
            _isCrouching = true;
        }
        else
        {
            _isCrouching = false;
        }

        if (_isCrouching)
        {
            _characterController.height = _crouchHeight;
            _characterController.center = _crouchOffset;
        }
        else
        {
            _characterController.height = _standingHeight;
            _characterController.center = _standingOffset;
        }
    }

    public void Fly()
    {
        if (Input.GetKeyDown(KeyCode.G) && EnableFly)
        {
            EnableFly = false;
            PlayerAnimator.gameObject.SetActive(true);
            BeanModel.gameObject.SetActive(false);
            MoveSpeed /= 8;
        }
        else if (Input.GetKeyDown(KeyCode.G) && !EnableFly)
        {
            EnableFly = true;
            PlayerAnimator.gameObject.SetActive(false);
            BeanModel.gameObject.SetActive(true);
            MoveSpeed *= 8;
        }
        if (EnableFly)
        {
            Sprint();

            Vector3 moveDir = Vector3.zero;
            if (Input.GetKey(KeyCode.E) && !(_characterController.transform.position.y >= MaxFlyingHeight))
            {
                moveDir.y = 1f;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                moveDir.y = -1f;
            }
            else
            {
                moveDir.y = 0f;
            }
            _characterController.Move(moveDir.normalized * _TotalMoveSpeed * Time.deltaTime);
        }
    }

    public void SetPOV(string pov)
    {
        if (pov == "1st")
        {
            IsFPS = true;
            VC1stPerson.SetActive(true);
            VC3rdPerson.SetActive(false);
        }
        else if (pov == "3rd")
        {
            IsFPS = false;
            VC3rdPerson.SetActive(true);
            VC1stPerson.SetActive(false);
        }
    }

    public void TogglePOV()
    {
        if (IsFPS)
        {
            IsFPS = false;
            VC3rdPerson.SetActive(true);
            VC1stPerson.SetActive(false);
        }
        else
        {
            IsFPS = true;
            VC1stPerson.SetActive(true);
            VC3rdPerson.SetActive(false);
        }
    }

    public RaycastHit CheckInteraction()
    {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 10f, InteractableMask))
        {
            //Our custom method. 
            if (raycastHit.transform.gameObject.GetComponent<Collider>().enabled)
            {
                _indicatorIcon.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Object Hit: " + raycastHit.transform.gameObject.name);
                    Vector3 pointToLook = ray.GetPoint(Vector3.Distance(ray.origin, raycastHit.transform.position));
                    Debug.DrawLine(ray.origin, pointToLook, Color.cyan, 5f);
                    _indicatorSound.Play();
                    raycastHit.transform.gameObject.GetComponent<MakeItAButton>().EventToCall.Invoke();
                }
            }
        }
        else
        {
            _indicatorIcon.SetActive(false);
        }

        return raycastHit;
    }

    private void FreeCursorToggle()
    {
        if (!GamingStyleMovement)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _isCursorFree = false;
            }
            if (Input.GetMouseButtonUp(1))
            {
                _isCursorFree = true;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                _isCursorFree = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                _isCursorFree = true;
            }
        }
    }

    private void FreeCursor()
    {
        if (!GamingStyleMovement)
        {
            if (!_isCursorFree)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cam.GetComponent<CinemachineBrain>().enabled = true;

                float targetAngle = Cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationSmoothVelocity, RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Cam.GetComponent<CinemachineBrain>().enabled = false;
            }
        }
        else
        {
            if (!_isCursorFree)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cam.GetComponent<CinemachineBrain>().enabled = true;

            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Cam.GetComponent<CinemachineBrain>().enabled = false;
            }
        }
    }

    public void AnimationController()
    {
        if (_isMoving && !_isSprinting)
        {
            PlayerAnimator.SetBool("IsWalking", true);
            PlayerAnimator.SetBool("IsRunning", false);
        }
        else if (_isMoving && _isSprinting)
        {
            PlayerAnimator.SetBool("IsWalking", true);
            PlayerAnimator.SetBool("IsRunning", true);
        }
        else
        {
            PlayerAnimator.SetBool("IsWalking", false);
            PlayerAnimator.SetBool("IsRunning", false);
        }

        if (_velocity.y > 0 && !_isGrounded)
        {
            PlayerAnimator.SetBool("IsJumping", true);
        }
        else if (_velocity.y < 0)
        {
            PlayerAnimator.SetBool("IsJumping", false);
        }

        if (_isGrounded)
        {
            PlayerAnimator.SetBool("IsGrounded", true);
        }
        else
        {
            PlayerAnimator.SetBool("IsGrounded", false);
        }

        if (!InControl)
        {
            PlayerAnimator.SetBool("IsGrounded", true);
            PlayerAnimator.SetBool("IsJumping", false);
            PlayerAnimator.SetBool("IsWalking", false);
            PlayerAnimator.SetBool("IsRunning", false);

        }
    }


    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (_isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(GroundCheck.position, GroundDistance);
    }
}
