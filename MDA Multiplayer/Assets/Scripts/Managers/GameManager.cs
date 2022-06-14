using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Measurements> MeasurementList;

    [Header("Photon")]
    public List<PhotonView> AllPatientsPhotonViews;
    public List<PhotonView> AllPlayersPhotonViews;

    #region Data References
    [Header("Data & Scripts")]
    public List<Patient> AllPatients;

    public Patient LastClickedPatient;
    public PatientData LastClickedPatientData;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    void Start()
    {
        OnEscape(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //PauseToggle();
        }
    }

    //public void PauseToggle()
    //{
    //    if (!GameMenuOpen)
    //    {
    //        OnEscape(true);
    //    }
    //    else if (GameMenuOpen)
    //    {
    //        OnEscape(false);
    //    }
    //}

    private void OnEscape(bool paused)
    {
        ChangeCursorMode(paused);
    }

    private void ChangeCursorMode(bool unlocked)
    {
        if (unlocked)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
        //#if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //         Application.Quit();
        //#endif
    }
}
