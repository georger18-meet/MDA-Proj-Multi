using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMassages : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;
    [SerializeField] private Animator _playerAnimator;

    public void DoHeartMassage()
    {
        if (!PlayerData.Instance.CurrentPatientTreating.IsPlayerJoined(PlayerData.Instance))
            return;

        //PlayerData.Instance.transform.position = _actionManager.PlayerTreatingTr.position;
        //PlayerData.Instance.transform.rotation = _actionManager.PlayerTreatingTr.rotation;
        //_playerAnimator.Play(,)
        // change heart rate after x seconds

        _actionTemplates.UpdatePatientLog($"Performed Heart Massages");
        Debug.Log("Operating Heart Massage On " /*+ _actionData.Patient.name*/);
    }
}
