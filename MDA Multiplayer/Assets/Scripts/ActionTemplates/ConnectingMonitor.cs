using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectingMonitor : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;
    [SerializeField] private GameObject _monitor;

    private GameObject _player;
    
    public void Defibrillation()
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                _player = desiredPlayerData.gameObject;
                _player.transform.position = desiredPlayerData.CurrentPatientNearby.ChestPosPlayerTransform.position;
                PhotonNetwork.Instantiate(_monitor.name, desiredPlayerData.CurrentPatientNearby.ChestPosEquipmentTransform.position, Quaternion.identity);
                //MonoBehaviour.Instantiate(_monitor, _actionManager.PatientEquipmentTr.position, Quaternion.identity);

                _actionTemplates.UpdatePatientLog($"Connected Defibrilator to Patient", PhotonNetwork.NickName);
                Debug.Log("CLEAR!!! Defibrillator On " /*+ _AOM.Patient.name*/);
            }
        }
    }
}
