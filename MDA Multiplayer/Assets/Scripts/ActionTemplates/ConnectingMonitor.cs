using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectingMonitor : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerActions _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _monitor;

    public void Defibrillation()
    {
        foreach (PhotonView photonView in GameManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                //_player.transform.position = _actionManager.PlayerTreatingTr.position;
                //MonoBehaviour.Instantiate(_monitor, _actionManager.PatientEquipmentTr.position, Quaternion.identity);

                _actionTemplates.UpdatePatientLog($"Connected Defibrilator to Patient");
                Debug.Log("CLEAR!!! Defibrillator On " /*+ _AOM.Patient.name*/);
            }
        }
    }
}
