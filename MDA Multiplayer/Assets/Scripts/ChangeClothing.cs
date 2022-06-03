using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClothing : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager AOM;
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private Texture _newTextures;
    [SerializeField] private string _textureToChange, _alertContent;

    public void CheckMeasurementAction()
    {
        if (!AOM.CheckIfPlayerJoined())
            return;

        switch (_textureToChange.ToLower())
        {
            case "upper":
                AOM.CurrentPatientData.PatientTextures = _newTextures;
                break;

            case "lower":
                AOM.CurrentPatientData.PatientTextures = _newTextures;
                break;

            case "both":
                AOM.CurrentPatientData.PatientTextures = _newTextures;
                break;

            default:
                break;
        }

        _actionTemplates.ShowAlertWindow(_textureToChange, _alertContent);
        _actionTemplates.UpdatePatientLog($"Patient's {_textureToChange} is: {_alertContent}");
    }
}
