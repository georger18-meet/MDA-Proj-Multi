using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacuationNpc : MonoBehaviour
{
    private Evacuation evacuation;



    private void Start()
    {
        evacuation = GetComponentInParent<Evacuation>();

    }

    public void OnInteracted()
    {
        EvacuationManager.Instance.OnEvacuateNPCClicked();
    }

   public void EvacPatient()
    {
        //Evacuation currentPlayerData = gameObject.GetComponentInParent<Evacuation>();
        //if (currentPlayerData.NearbyPatient.Contains())
        //{
        EvacuationManager.Instance.AddPatientToRooms(evacuation.NearbyPatient[0], evacuation.RoomEnum);
           // EvacuationManager.Instance.DestroyPatient(evacuation.NearbyPatient[0]);
            EvacuationManager.Instance._evacuationUI.SetActive(false);
            //}
    }

   public void CancelEvac()
   {
        EvacuationManager.Instance._evacuationUI.SetActive(false);

    }
}
