using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerTreatingAnimation : Action
{
    [Header("Data")]
    [SerializeField] private string _animationName;
    [SerializeField] private float _animationEndTime;

    [Header("Player Position")]
    PlayerTreatingPosition _playerTreatingPos;

    [Header("Alert")]
    [SerializeField] private string _alertTitle;
    [SerializeField] private string _alertText;

    [Header("Conditions")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private Animator _playerAnimator;
    private string _playerName;

    public void PlayAnimation()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            _playerAnimator = LocalPlayerData.gameObject.transform.GetChild(5).GetComponent<Animator>();

            int playerTreatingPos = (int)_playerTreatingPos;
            LocalPlayerData.transform.SetPositionAndRotation(PlayerTreatingPositions[playerTreatingPos].position, PlayerTreatingPositions[playerTreatingPos].rotation);

            _playerAnimator.SetBool(_animationName, true);

            TextToLog = $" is Administering Heart Massages";

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
