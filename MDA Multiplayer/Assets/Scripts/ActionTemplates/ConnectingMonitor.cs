using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectingMonitor : Action
{
    [Header("Prefab References")]
    [SerializeField] private GameObject _monitor;

    [Header("Component's Data")]
    [SerializeField] private GameObject _monitorGraphWindow;
    [SerializeField] private SpriteRenderer _monitorGraphRenderer;
    [SerializeField] private Sprite _newMonitorGraphSprite;

    [Header("Alert")]
    [SerializeField] private string _alertTitle;
    [SerializeField] private string _alertText;

    [Header("Conditions")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private GameObject _player;
    
    public void Defibrillation()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            _player = LocalPlayerData.gameObject;
            _player.transform.position = CurrentPatient.ChestPosPlayerTransform.position;

            GameObject monitor = PhotonNetwork.Instantiate(_monitor.name, CurrentPatient.ChestPosEquipmentTransform.position, CurrentPatient.ChestPosEquipmentTransform.rotation);

            _monitorGraphWindow.SetActive(true);

            if (CurrentPatientData.MonitorGraphTexture = _newMonitorGraphSprite)
            {
                LocalPlayerPhotonView.RPC("SetMonitorGraphRPC", RpcTarget.AllViaServer, _monitorGraphRenderer, _newMonitorGraphSprite);
            }
            

            TextToLog = $"Connected Defibrilator to {CurrentPatientData.Name} {CurrentPatientData.SureName}";

            if (_showAlert)
            {
                ShowTextAlert(_alertTitle, _alertText);
            }

            if (_updateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
