using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;

public class EvacuationNpc : MonoBehaviour
{
    private Evacuation evacuation;

    public GameObject _evacuationUI;

    private PhotonView _photonView;
    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

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
        _photonView.RPC("EvacPatient_RPC", RpcTarget.AllBufferedViaServer);
        // AllPatientsPhotonViews.PhotonView.RPC("EvacPatient_RPC", RpcTarget.AllBufferedViaServer);
    }


    [PunRPC]
    public void EvacPatient_RPC()
    {
        EvacuationManager.Instance.AddPatientToRooms(evacuation.NearbyPatient[0].PhotonView, evacuation.RoomEnum);
        EvacuationManager.Instance.DestroyPatient(evacuation.NearbyPatient[0].PhotonView);
        evacuation.NearbyPatient.Clear();
        _evacuationUI.SetActive(false);
    }

    public void CancelEvac()
   {
        _evacuationUI.SetActive(false);

    }
}
