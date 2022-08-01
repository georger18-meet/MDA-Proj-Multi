using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeartMassages : Action
{
    [Header("Component's Data")]
    [SerializeField] private GameObject _heartMassagesWindow;
    [SerializeField] private int _newHeartRate;
    
    [Header("Alert")]
    [SerializeField] private string _alertTitle;
    [SerializeField] private string _alertText;

    private Animator _playerAnimator;
    private string _playerName;

    private IEnumerator WaitToFinishCPR()
    {
        yield return new WaitForSeconds(4);

        _playerAnimator.SetBool("Administering Cpr", false);
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

            _heartMassagesWindow.SetActive(true);
            //StartCoroutine(WaitToFinishCPR());

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
