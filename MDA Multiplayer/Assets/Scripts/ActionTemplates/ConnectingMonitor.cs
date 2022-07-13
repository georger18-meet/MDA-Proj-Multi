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
                PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                Patient currentPatient = desiredPlayerData.CurrentPatientNearby;
                PatientData currentPatientData = currentPatient.PatientData;

                _player = desiredPlayerData.gameObject;
                _player.transform.position = currentPatient.ChestPosPlayerTransform.position;

                GameObject monitor = PhotonNetwork.Instantiate(_monitor.name, currentPatient.ChestPosEquipmentTransform.position, currentPatient.ChestPosEquipmentTransform.rotation);

                photonView.RPC("UpdatePatientLogRPC", RpcTarget.AllViaServer, $"Connected Defibrilator to Patient {desiredPlayerData.CurrentPatientNearby.PatientData.Name} {desiredPlayerData.CurrentPatientNearby.PatientData.SureName}");

                // alert

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog($"<{PhotonNetwork.NickName}>", $"Connected Defibrilator to {currentPatientData.Name} {currentPatientData.SureName}");
                }
                break;
            }
        }
    }
}
