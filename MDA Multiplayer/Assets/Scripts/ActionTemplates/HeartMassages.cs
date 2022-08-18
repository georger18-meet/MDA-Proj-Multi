using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeartMassages : Action
{
    [Header("Component's Data")]
    [SerializeField] private GameObject _heartMassagesWindow;
    private Animator _playerAnimator;

    public void DoHeartMassage()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            _playerAnimator = LocalPlayerData.gameObject.transform.GetChild(5).GetComponent<Animator>();

            LocalPlayerData.transform.SetPositionAndRotation(PatientChestPosPlayerTransform.position, PatientChestPosPlayerTransform.rotation);

            _playerAnimator.SetBool("Administering Cpr", true);

            _heartMassagesWindow.SetActive(true);

            TextToLog = $"Started Administering Heart Massages";

            if (_shouldUpdateLog)
            {
                LogText(TextToLog);
            }
        }
    }

    public void StopHeartMassages()
    {
        TextToLog = $"Stopped Administering Heart Massages";
        _playerAnimator.SetBool("Administering Cpr", false);
        _heartMassagesWindow.SetActive(false);

        if (_shouldUpdateLog)
        {
            LogText(TextToLog);
        }
    }
}
