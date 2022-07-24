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

    // usually 0 /2 /4
    [SerializeField] private int _patientColliderIndex;

    [Header("Alert")]
    [SerializeField] private string _alertTitle;
    [SerializeField] private string _alertText;

    [Header("Conditions")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private Animator _playerAnimator;
    private string _playerName;

    private IEnumerator WaitToFinishAnimation()
    {
        yield return new WaitForSeconds(_animationEndTime);

        _playerAnimator.SetBool(_animationName, false);
    }

    public void PlayAnimation()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            _playerAnimator = LocalPlayerData.gameObject.transform.GetChild(5).GetComponent<Animator>();

            LocalPlayerData.transform.SetPositionAndRotation(PatientChestPosPlayerTransform.position, PatientChestPosPlayerTransform.rotation);

            _playerAnimator.SetBool(_animationName, true);
            StartCoroutine(WaitToFinishAnimation());

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
