using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectingMonitor : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] private GameObject _monitor;

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private GameObject _player;
    
    public void Defibrillation()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData localPlayerData = photonView.GetComponent<PlayerData>();

                if (!localPlayerData.CurrentPatientNearby.IsPlayerJoined(localPlayerData))
                    return;

                Patient currentPatient = localPlayerData.CurrentPatientNearby;
                PatientData currentPatientData = currentPatient.PatientData;

                _player = localPlayerData.gameObject;
                _player.transform.position = currentPatient.ChestPosPlayerTransform.position;

                GameObject monitor = PhotonNetwork.Instantiate(_monitor.name, currentPatient.ChestPosEquipmentTransform.position, currentPatient.ChestPosEquipmentTransform.rotation);

                photonView.RPC("UpdatePatientLogRPC", RpcTarget.AllViaServer, $"Connected Defibrilator to Patient {localPlayerData.CurrentPatientNearby.PatientData.Name} {localPlayerData.CurrentPatientNearby.PatientData.SureName}");

                // alert

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog(localPlayerData.CrewIndex, localPlayerData.UserName, $"Connected Defibrilator to {currentPatientData.Name} {currentPatientData.SureName}");
                }
                break;
            }
        }
    }
}
