using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClipping : MonoBehaviour
{
    public LayerMask CollisionLayers;
    public Transform Target;
    public Transform _destinationObj;

    public bool Colliding = false;
    public Vector3[] AdjustedCameraClipPoints;
    public Vector3[] DesiredCameraClipPoints;

    Camera _cam;

    public bool SmoothFollow = true;
    public float Smooth = 0.05f;
    public float AdjustmentDistance = -8;

    Vector3 _destination = Vector3.zero;
    Vector3 _adjustedDestination = Vector3.zero;
    Vector3 _camVel = Vector3.zero;

    public bool DrawDesiredCollisionLines = true;
    public bool DrawAdjustedCollisionLines = true;


    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<Camera>();
        AdjustedCameraClipPoints = new Vector3[5];
        DesiredCameraClipPoints = new Vector3[5];

        _destination = _destinationObj.position;

        // Initializing
        RefreshCamClipPoints(transform.position, transform.rotation, ref AdjustedCameraClipPoints);
        RefreshCamClipPoints(_destination, transform.rotation, ref DesiredCameraClipPoints);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        RefreshCamClipPoints(transform.position, transform.rotation, ref AdjustedCameraClipPoints);
        RefreshCamClipPoints(_destination, transform.rotation, ref DesiredCameraClipPoints);

        // Draw Debug Lines
        for (int i = 0; i < 5; i++)
        {
            if (DrawDesiredCollisionLines)
            {
                Debug.DrawLine(Target.position, DesiredCameraClipPoints[i], Color.white);
            }
            if (DrawAdjustedCollisionLines)
            {
                Debug.DrawLine(Target.position, AdjustedCameraClipPoints[i], Color.green);
            }
        }

        CheckColliding(Target.position);
        AdjustmentDistance = GetAdjustedDistanceWithRayFrom(Target.position);

        MoveToTarget();
    }


    private void MoveToTarget()
    {
        if (Colliding)
        {
            _adjustedDestination = Vector3.forward * AdjustmentDistance;
            _adjustedDestination += Target.position;

            if (SmoothFollow)
            {
                transform.position = Vector3.SmoothDamp(transform.position, _adjustedDestination, ref _camVel, Smooth);
            }
            else
            {
                transform.position = _adjustedDestination;
            }
        }
        else
        {
            if (SmoothFollow)
            {
                transform.position = Vector3.SmoothDamp(transform.position, _destination, ref _camVel, Smooth);
            }
            else
            {
                transform.position = _destination;
            }
        }
    }


    public void RefreshCamClipPoints(Vector3 camPosition, Quaternion atRotation, ref Vector3[] intoArray)
    {
        if (!_cam)
        {
            return;
        }

        // Clear intoArray Contents
        intoArray = new Vector3[5];

        float z = _cam.nearClipPlane;
        float x = Mathf.Tan(_cam.fieldOfView / 2) * z;
        float y = x / _cam.aspect;

        // Adding And Rotating Points Relative To Camera:
        // Top Left Clip Point
        intoArray[0] = (atRotation * new Vector3(-x, y, z)) + camPosition;
        // Top Right Clip Point
        intoArray[1] = (atRotation * new Vector3(x, y, z)) + camPosition;
        // Bottom Left Clip Point
        intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + camPosition;
        // Bottom Right Clip Point
        intoArray[3] = (atRotation * new Vector3(x, -y, z)) + camPosition;
        // Cam Position
        intoArray[4] = camPosition - _cam.transform.forward;
    }

    private bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
    {
        for (int i = 0; i < clipPoints.Length; i++)
        {
            Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
            float distance = Vector3.Distance(clipPoints[i], fromPosition);

            if (Physics.Raycast(ray, distance, CollisionLayers))
            {
                return true;
            }
        }

        return false;
    }

    public float GetAdjustedDistanceWithRayFrom(Vector3 from)
    {
        float distance = -1;

        for (int i = 0; i < DesiredCameraClipPoints.Length; i++)
        {
            Ray ray = new Ray(from, DesiredCameraClipPoints[i] - from);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (distance == -1)
                {
                    distance = hit.distance;
                }
                else
                {
                    if (hit.distance < distance)
                    {
                        distance = hit.distance;
                    }
                }
            }
        }

        if (distance == -1)
        {
            return 0;
        }
        else
        {
            return distance;
        }
    }

    public void CheckColliding(Vector3 targetPos)
    {
        if (CollisionDetectedAtClipPoints(DesiredCameraClipPoints, targetPos))
        {
            Colliding = true;
        }
        else
        {
            Colliding = false;
        }
    }
}
