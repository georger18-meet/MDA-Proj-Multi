using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Clothing
{
    FullyClothed, ShirtOnly, PantsOnly, UnderwearOnly
}

public class ChangeClothing : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionsManager _actionManager;
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private Clothing _clothing;
    [SerializeField] private string _textureToChange, _alertContent;


    public void ChangeClothingAction(int measurementNumber)
    {
        if (!PlayerData.Instance.CurrentPatientTreating.IsPlayerJoined(PlayerData.Instance))
            return;

        // loops throughout measurementList and catches the first element that is equal to measurementNumber
        Measurements measurements = _actionManager.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);

        switch (_clothing)
        {
            case Clothing.FullyClothed:
                PlayerData.Instance.CurrentPatientTreating.PatientRenderer.material = PlayerData.Instance.CurrentPatientTreating.PatientData.FullyClothedMaterial;
                break;

            case Clothing.ShirtOnly:
                PlayerData.Instance.CurrentPatientTreating.PatientRenderer.material = PlayerData.Instance.CurrentPatientTreating.PatientData.ShirtOnlyMaterial;
                break;

            case Clothing.PantsOnly:
                PlayerData.Instance.CurrentPatientTreating.PatientRenderer.material = PlayerData.Instance.CurrentPatientTreating.PatientData.PantsOnlyMaterial;
                break;

            case Clothing.UnderwearOnly:
                PlayerData.Instance.CurrentPatientTreating.PatientRenderer.material = PlayerData.Instance.CurrentPatientTreating.PatientData.UnderwearOnlyMaterial;
                break;

            default:
                break;
        }

        _actionTemplates.ShowAlertWindow(_textureToChange, _alertContent);
        _actionTemplates.UpdatePatientLog($"Patient's {_textureToChange} is: {_alertContent}");
    }
}
