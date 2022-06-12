using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera _playerCamera;

    [Header("Interaction")]
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private GameObject _indicatorIcon;
    [SerializeField] private AudioSource _indicatorSound;
    [SerializeField] private float _raycastDistance = 10f;

    private void Update()
    {
        Interact();
    }

    public RaycastHit Interact()
    {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, _raycastDistance, _interactableLayer))
        {
            Debug.DrawLine(ray.origin, raycastHit.point, Color.cyan, _raycastDistance);

            _indicatorIcon.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                _indicatorSound.Play();
                raycastHit.transform.GetComponent<MakeItAButton>().EventToCall.Invoke();

                Debug.Log($"Interacted with {raycastHit.transform.name}");
            }
        }
        else
        {
            _indicatorIcon.SetActive(false);
        }

        return raycastHit;
    }
}
