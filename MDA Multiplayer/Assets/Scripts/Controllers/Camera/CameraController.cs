using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
    [Header("Photon")]
    private PhotonView _photonView;

    [Header("Camera")]
    [SerializeField] private Camera _playerCamera;
    public Camera PlayerCamera => _playerCamera;

    [Header("Interaction")]
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private LayerMask _selectableLayer;
    [SerializeField] private GameObject _indicatorIcon;
    [SerializeField] private AudioSource _indicatorSound;
    [SerializeField] private float _raycastDistance = 10f;
    [SerializeField] private Outline.Mode _outlineMode;
    private Outline _currentInteractable;
    private bool _sendInteractRaycast;
    public bool SetSendIteractRaycast { set => _sendInteractRaycast = value; }
    //private Vector3 _cameraOriginalPos, _cameraCurrentPos;
    //private Quaternion _cameraOriginalRot, _cameraCurrentRot;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            Destroy(_playerCamera.gameObject);
            Destroy(this);
        }
        else
        {
            _sendInteractRaycast = true;
            //_cameraOriginalPos = _playerCamera.transform.position;
            //_cameraOriginalRot = _playerCamera.transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        if (_photonView.IsMine)
        {
            //SmoothCamera();
            if (_sendInteractRaycast)
            {
                Interact();
            }
        }
    }

    private void SmoothCamera()
    {
        //_cameraCurrentPos = _playerCamera.transform.position;
        //_cameraCurrentRot = _playerCamera.transform.rotation;

        //Vector3.Lerp();
    }


    public RaycastHit Interact()
    {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);

        // need to try other layer before ----

        if (Physics.Raycast(ray, out RaycastHit interactableRaycastHit, _raycastDistance, _interactableLayer))
        {
            if (_currentInteractable != null && interactableRaycastHit.transform.gameObject != _currentInteractable) // true if raycast hit new interactable, destroy old current outline
            {
                _currentInteractable.OutlineWidth = 0;
            }

            _currentInteractable = interactableRaycastHit.transform.GetComponent<Outline>();

            if (!_currentInteractable) // if current is null => add outline to interactrable
            {
                _currentInteractable = interactableRaycastHit.transform.gameObject.AddComponent<Outline>();
            }

            _currentInteractable.OutlineWidth = 15;
            _currentInteractable.OutlineMode = _outlineMode;

            Debug.DrawLine(ray.origin, interactableRaycastHit.point, Color.cyan, 1f);

            //_indicatorIcon.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                _indicatorSound.Play();
                interactableRaycastHit.transform.GetComponent<MakeItAButton>().EventToCall.Invoke();

                Debug.Log($"Interacted with {interactableRaycastHit.transform.name}");
            }
        }
        else
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.OutlineWidth = 0;
            }
            //_indicatorIcon.SetActive(false);
        }

        return interactableRaycastHit;
    }

    public void ToggleInteractRaycast(bool sendRaycast)
    {
        _sendInteractRaycast = sendRaycast;
    }
}
