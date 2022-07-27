using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeartMassages : Action
{
    [Header("Component's Data")]
    [SerializeField] private int _newHeartRate;
    
    [Header("Alert")]
    [SerializeField] private string _alertTitle;
    [SerializeField] private string _alertText;

    [Header("Conditions")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    private Animator _playerAnimator;
    private string _playerName;

    private IEnumerator WaitToFinishCPR()
    {
        yield return new WaitForSeconds(4);

        _playerAnimator.SetBool("Administering Cpr", false);
        //ActionTemplates.Instance.UpdatePatientLog(localPlayerData.CrewIndex, localPlayerData.UserName, $"{_playerName} has finished Administering Heart Massages");
    }

    public void DoHeartMassage()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            _playerAnimator = LocalPlayerData.gameObject.transform.GetChild(5).GetComponent<Animator>();

            LocalPlayerData.transform.SetPositionAndRotation(PatientChestPosPlayerTransform.position, PatientChestPosPlayerTransform.rotation);

            _playerAnimator.SetBool("Administering Cpr", true);
            CurrentPatient.PhotonView.RPC("ChangeHeartRateRPC", RpcTarget.All, _newHeartRate);

            StartCoroutine(WaitToFinishCPR());

            TextToLog = $" Administering Heart Massages";

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
