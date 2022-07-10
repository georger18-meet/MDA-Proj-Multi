using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectingMonitor : MonoBehaviour
{
    [SerializeField] private GameObject _monitor;

    private GameObject _player;
    
    public void Defibrillation()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                _player = desiredPlayerData.gameObject;
                _player.transform.position = desiredPlayerData.CurrentPatientNearby.ChestPosPlayerTransform.position;
                GameObject monitor = PhotonNetwork.Instantiate(_monitor.name, desiredPlayerData.CurrentPatientNearby.ChestPosEquipmentTransform.position, desiredPlayerData.CurrentPatientNearby.ChestPosEquipmentTransform.rotation);

                photonView.RPC("UpdatePatientLogRPC", RpcTarget.AllViaServer, $"Connected Defibrilator to Patient {desiredPlayerData.CurrentPatientNearby.PatientData.SureName} {desiredPlayerData.CurrentPatientNearby.PatientData.LastName}");

                //ActionTemplates.Instance.UpdatePatientLog($"Connected Defibrilator to Patient {desiredPlayerData.CurrentPatientNearby.PatientData.SureName} {desiredPlayerData.CurrentPatientNearby.PatientData.LastName}");
                Debug.Log("CLEAR!!! Defibrillator");
            }
        }
    }
}
