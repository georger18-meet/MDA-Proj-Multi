using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacuationNpc : MonoBehaviour
{
    [SerializeField]private Evacuation evacuation;

    public void OnInteracted()
    {
        EvacuationManager.Instance.OnEvacuateNPCClicked();
    }

   public void EvacPatient()
    {
        //Evacuation currentPlayerData = gameObject.GetComponentInParent<Evacuation>();
        //if (currentPlayerData.NearbyPatient.Contains())
        //{
        if (evacuation.NearbyPatient[0]!=null)
        {
            EvacuationManager.Instance.AddPatientToRooms(evacuation.NearbyPatient[0], evacuation.RoomEnum);
            EvacuationManager.Instance.DestroyPatient(evacuation.NearbyPatient[0]);
            EvacuationManager.Instance._evacuationUI.SetActive(false);
        }
        else
        {
            return;
            
        }
            
        //}
    }

   public void CancelEvac()
   {
        EvacuationManager.Instance._evacuationUI.SetActive(false);

    }
}
