using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionDetection : MonoBehaviour
{
    //[SerializeField] private Vector3 _camOriginPosOffset = new Vector3(0f, 1f, -5f);
    //public Vector3 _dollyDirAdjusted;
    [SerializeField] private Transform _camZoomedTr;
    [SerializeField] private float _minDistance = 0.6f, _maxDistance = 2.9f, _smooth = 10.0f;
    [SerializeField] private float _distance;
    private Vector3 _dollyDir;
    private Vector3 _desiredCamPos;

    private void Awake()
    {
        _dollyDir = transform.localPosition.normalized;
        _distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        _desiredCamPos = transform.parent.TransformPoint(_dollyDir * _maxDistance);
    }

    private void FixedUpdate()
    {
        if (Physics.Linecast(transform.parent.position, _desiredCamPos, out RaycastHit hit))
        {
            _distance = Mathf.Clamp(hit.distance, _minDistance, _maxDistance);
        }
        else
        {
            _distance = _maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, _dollyDir * _distance, Time.deltaTime * _smooth);
    }
}