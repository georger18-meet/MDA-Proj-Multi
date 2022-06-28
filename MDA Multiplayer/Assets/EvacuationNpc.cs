using System;
using System.Collections;
using System.Collections.Generic;
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
        //Evacuation currentPlayerData = gameObject.GetComponentInParent<Evacuation>();
        //if (currentPlayerData.NearbyPatient.Contains())
        //{
        EvacuationManager.Instance.AddPatientToRooms(evacuation.NearbyPatient[0], evacuation.RoomEnum);
            EvacuationManager.Instance.DestroyPatient(evacuation.NearbyPatient[0]);
            _evacuationUI.SetActive(false);
            //}
    }

   public void CancelEvac()
   {
        _evacuationUI.SetActive(false);

    }
}
