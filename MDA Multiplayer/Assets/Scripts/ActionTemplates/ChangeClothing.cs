using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Clothing
{
    FullyClothed, ShirtOnly, PantsOnly, UnderwearOnly
}

public class ChangeClothing : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ActionTemplates _actionTemplates;

    [Header("Component's Data")]
    [SerializeField] private Clothing _clothing;
    [SerializeField] private string _textureToChange, _alertContent;

    public void ChangeClothingAction(int measurementNumber)
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                    return;

                // loops throughout measurementList and catches the first element that is equal to measurementNumber
                Measurements measurements = ActionsManager.Instance.MeasurementList.FirstOrDefault(item => item == (Measurements)measurementNumber);

                switch (_clothing)
                {
                    case Clothing.FullyClothed:
                        desiredPlayerData.CurrentPatientNearby.PatientRenderer.material = desiredPlayerData.CurrentPatientNearby.PatientData.FullyClothedMaterial;
                        break;

                    case Clothing.ShirtOnly:
                        desiredPlayerData.CurrentPatientNearby.PatientRenderer.material = desiredPlayerData.CurrentPatientNearby.PatientData.ShirtOnlyMaterial;
                        break;

                    case Clothing.PantsOnly:
                        desiredPlayerData.CurrentPatientNearby.PatientRenderer.material = desiredPlayerData.CurrentPatientNearby.PatientData.PantsOnlyMaterial;
                        break;

                    case Clothing.UnderwearOnly:
                        desiredPlayerData.CurrentPatientNearby.PatientRenderer.material = desiredPlayerData.CurrentPatientNearby.PatientData.UnderwearOnlyMaterial;
                        break;

                    default:
                        break;
                }

                _actionTemplates.ShowAlertWindow(_textureToChange, _alertContent);
                _actionTemplates.UpdatePatientLog($"Patient's {_textureToChange} is: {_alertContent}");
            }
        }
    }
}
