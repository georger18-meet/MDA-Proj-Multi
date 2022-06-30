using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EvacuationNpc : MonoBehaviour
{
    private Evacuation evacuation;

    public GameObject _evacuationUI;


    private void Start()
    {
        evacuation = GetComponentInParent<Evacuation>();
    }

    public void OnInteracted()
    {
        OnEvacuateNPCClicked();
    }

    public void OnEvacuateNPCClicked()
    {
        Debug.Log($"Attempting to Click On Npc");
        _evacuationUI.SetActive(true);
    }

    public void EvacPatient()
    {
        EvacuationManager.Instance.AddPatientToRooms(evacuation.NearbyPatient[0], evacuation.RoomEnum);
        EvacuationManager.Instance.DestroyPatient(evacuation.NearbyPatient[0]);
        evacuation.NearbyPatient.Clear();
        _evacuationUI.SetActive(false);
    }

    public void CancelEvac()
   {
        _evacuationUI.SetActive(false);

    }
}
