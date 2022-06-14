using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class ActionTemplates : MonoBehaviour
{
    public static ActionTemplates Instance;

    [SerializeField] private DocumentationLogManager _docLog;
    [SerializeField] private GameObject _alertWindow;
    [SerializeField] private TextMeshProUGUI _alertTitle, _alertText;
    [SerializeField] private float _alertTimer;

    #region Most Basic Tools
    public void OpenCloseDisplayWindow(GameObject window)
    {
        Debug.Log($"Attempting to Open/Close {window.name}");
        // if (_photonView.IsMine)
        // {
            if (window.activeInHierarchy)
            {
                window.SetActive(false);
                Debug.Log($"Closed {window.name}");
            }
            else
            {
                window.SetActive(true);
                Debug.Log($"Opened {window.name}");
            }
        // }

        
    }

    public void CreateDestroyWindow(GameObject window)
    {
        // if (_photonView.IsMine)
        // {
            if (window.activeInHierarchy)
                Instantiate(window);
            else
                Destroy(window);
        // }
    }

    public void UpdateDisplayWindow(GameObject window, TextMeshProUGUI text, int newValue)
    {
        TextMeshProUGUI oldText = text;
        text.text = newValue.ToString();

        print($"Updated {oldText} in {window.name} to {text}");
    }

    public void ShowAlertWindow(string measurementTitle, int measurement)
    {
        _alertTimer = 0;
        _alertWindow.SetActive(true);

        _alertTitle.text = measurementTitle;
        _alertText.text = measurement.ToString();

        if (_alertTimer > 3)
            _alertWindow.SetActive(false);
    }

    public void ShowAlertWindow(string AlertTitle, string alertContent)
    {
        _alertTimer = 0;
        _alertWindow.SetActive(true);

        _alertTitle.text = AlertTitle;
        _alertText.text = alertContent.ToString();

        if (_alertTimer > 3)
            _alertWindow.SetActive(false);
    }

    public void ChangeMeasurement(int oldValue, int newValue)
    {
        oldValue = newValue;

        print($"Changed {oldValue} to {newValue}");
    }

    public void SpawnEquipment(GameObject additionalEquipment, Transform desiredPositionTransform)
    {
        Vector3 desiredPos = desiredPositionTransform.position;

        Instantiate(additionalEquipment, desiredPos, Quaternion.identity);

        print($"Spawned {additionalEquipment.name} at {desiredPos}");
    }

    public void MoveCharacter(Transform characterTransform, Transform desiredPositionTransform)
    {
        Vector3 oldPos = characterTransform.position;
        Vector3 newPos = desiredPositionTransform.position;

        characterTransform.position = newPos;

        print($"Moved character from {oldPos} to {newPos}");
    }

    // need fixing
    public void PlayAnimationOnCharacter(Transform characterTransform, Animation animation)
    {
        animation.Play();

        print($"Played animation {animation} on {characterTransform.name}");
    }

    public void ChangeCharacterTextrues(Texture currentTexture, Texture newTexture)
    {
        currentTexture = newTexture;

        print($"Changed Textures: {newTexture} instead of {currentTexture}");
    }

    public void UpdatePatientLog(string textToLog)
    {
        _docLog.LogThisText(textToLog);
    } 
    #endregion

    private void Awake()
    {
        //if (_photonView.isMine)
        //{
            Instance = this;
        //}
    }

    // not sure about this - patient bool - isConsious vs if is currently conscious
    public void CheckStatus(bool isConscious, bool isPatientConscious)
    {

    }

    private void Update()
    {
        _alertTimer += Time.deltaTime;
    }
}
