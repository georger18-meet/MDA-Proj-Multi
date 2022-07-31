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

    [Header("Interaction")]
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private GameObject _indicatorIcon;
    [SerializeField] private AudioSource _indicatorSound;
    [SerializeField] private float _raycastDistance = 10f;
    private GameObject _tempInteractableRef;
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
            //_cameraOriginalPos = _playerCamera.transform.position;
            //_cameraOriginalRot = _playerCamera.transform.rotation;
        }
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            //SmoothCamera();
            Interact();
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
        if (Physics.Raycast(ray, out RaycastHit raycastHit, _raycastDistance, _interactableLayer))
        {
            if (_tempInteractableRef != null && raycastHit.transform.gameObject != _tempInteractableRef)
            {
                if (_tempInteractableRef.TryGetComponent(out Outline outlineTe))
                {
                    Destroy(outlineTe);
                }
            }
            _tempInteractableRef = raycastHit.transform.gameObject;

            Debug.DrawLine(ray.origin, raycastHit.point, Color.cyan, _raycastDistance);

            _indicatorIcon.SetActive(true);

            if (raycastHit.transform.gameObject.TryGetComponent(out Outline outline))
            {
            }
            else
            {
                Outline tempOutline = _tempInteractableRef.AddComponent<Outline>();
                tempOutline.OutlineWidth = 15;
                tempOutline.OutlineMode = Outline.Mode.OutlineVisible;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _indicatorSound.Play();
                raycastHit.transform.GetComponent<MakeItAButton>().EventToCall.Invoke();

                Debug.Log($"Interacted with {raycastHit.transform.name}");
            }
        }
        else
        {
            if (_tempInteractableRef != null)
            {
                if (_tempInteractableRef.TryGetComponent(out Outline outlineTe))
                {
                    Destroy(outlineTe);
                }
            }
            _indicatorIcon.SetActive(false);
        }

        return raycastHit;
    }
}
