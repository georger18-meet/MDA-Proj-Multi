using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMassages : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager _AOM;
    [SerializeField] private ActionTemplates _actionTemplates;
    [SerializeField] private GameObject _player;

    public void DoHeartMassage()
    {
        if (!_AOM.CheckIfPlayerJoined())
            return;
        else
            _player.transform.position = _AOM.PlayerTreatingTr.position;

        _actionTemplates.UpdatePatientLog($"Performed Heart Massages");
        Debug.Log("Operating Heart Massage On " /*+ _actionData.Patient.name*/);
    }

    //public void DoHeartMassage(ActionData actionData)
    //{
    //    if (!actionData.AOM.CheckIfPlayerJoined())
    //        return;
    //    else
    //        actionData.Player.transform.position = actionData.AOM.PlayerTreatingTr.position;
    //
    //    Debug.Log("Operating Heart Massage On " + actionData.Patient.name);
    //}
}
