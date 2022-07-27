using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameFaceCamera : MonoBehaviour
{
    private Transform _mainCam;

    private void Start()
    {
        _mainCam = Camera.main.transform;
    }




    void Update()
    {
        //if (cam == null)
        //    cam = FindObjectOfType<Camera>();
        

        //if (cam==null)
        //    return;
        
        transform.LookAt(transform.position + _mainCam.rotation*Vector3.forward,_mainCam.rotation*Vector3.up);
        //transform.Rotate(Vector3.up*180); //to rotate the text in the right way (text was revert)
    }
}
