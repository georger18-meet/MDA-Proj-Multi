using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{
    #region Script References
    [Header("Data & Scripts")]
    public PatientData PatientData;
    public List<ActionSequence> ActionSequences;
    #endregion

    #region Material References
    public Renderer PatientRenderer;
    #endregion

    #region Public fields
    //public Dictionary<string, int> OperatingUserCrew = new Dictionary<string, int>();
    public Animation PatientAnimation;
    #endregion

    #region private serialized fields
    [Header("Joined Crews & Players Lists")]
    public List<PlayerData> NearbyUsers;
    public List<PlayerData> TreatingUsers;
    public List<PlayerData> AllUsersTreatedThisPatient;
    public List<int> TreatingCrews;
    public List<int> AllCrewTreatedThisPatient;
    #endregion

    private void Start()
    {
        ActionsManager.Instance.AllPatients.Add(this);
        PatientRenderer.material = PatientData.FullyClothedMaterial;
    }

    private void Update()
    {

    }


    public void AddUserToTreatingLists(object currentPlayer)
    {
        PlayerData currentPlayerData = currentPlayer != null ? currentPlayer as PlayerData : null;

        if (currentPlayerData == null)
        {
            return;
        }


        for (int i = 0; i < 1; i++)
        {
            if (TreatingUsers.Contains(currentPlayerData))
            {
                continue;
            }
            else
            {
                TreatingUsers.Add(currentPlayerData);
                AllUsersTreatedThisPatient.Add(currentPlayerData);
            }

            if (TreatingCrews.Contains(currentPlayerData.CrewIndex))
            {
                return;
            }
            else
            {
                TreatingCrews.Add(currentPlayerData.CrewIndex);
                AllCrewTreatedThisPatient.Add(currentPlayerData.CrewIndex);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerData possiblePlayer = other.GetComponent<PlayerData>();

        if (possiblePlayer == null)
        {
            return;
        }
        else if (!NearbyUsers.Contains(possiblePlayer))
        {
            NearbyUsers.Add(possiblePlayer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerData possiblePlayer = other.GetComponent<PlayerData>();

        if (possiblePlayer != null)
        {

            if (!NearbyUsers.Contains(possiblePlayer))
            {
                return;
            }
            else
            {
                NearbyUsers.Remove(possiblePlayer);
            }
        }
    }

    public bool IsPlayerJoined(PlayerData playerData)
    {
        Debug.Log("Attempting to check if player is joined");

        if (TreatingUsers.Contains(playerData))
        {
            Debug.Log("Checked if player is joined, it is true");
            return true;
        }
        else
        {
            Debug.Log("Checked if player is joined, it is false");
            return false;
        }
    }

    public void OnInteracted()
    {
        ActionsManager.Instance.OnPatientClicked();
    }

    //public void SetOperatingCrewCheck(GameObject patient)
    //{
    //    PatientData _currentPatientInfoSo = patient != null ? patient.GetComponent<PatientData>() : null;
    //
    //    if (_currentPatientInfoSo == null)
    //    {
    //        return;
    //    }
    //
    //    if (patient.CompareTag("Patient"))
    //    {
    //        _currentPatientScript = patient.GetComponent<PatientV2>();
    //        GetPatientInfo();
    //    }
    //
    //    if (_player == null)
    //    {
    //        return;
    //    }
    //    else if (!_currentPatientScript.OperatingUserCrew.ContainsKey(PlayerData.UserName))
    //    {
    //        _joinPatientPopUp.SetActive(true);
    //    }
    //    else if (_currentPatientScript.OperatingUserCrew.ContainsKey(PlayerData.UserName))
    //    {
    //        SetupPatientInfoDisplay();
    //        _patientMenuParent.SetActive(true);
    //    }
    //}

    /*public PlayerData GetPlayerData(object collidingObject)
    * {
    *     GameObject collidingGameObject = collidingObject as GameObject;
    * 
    *     if (!collidingGameObject.CompareTag("Player"))
    *     {
    *         return null;
    *     }
    *     else
    *     {
    *         PlayerData lastEnteredPlayer = collidingGameObject.GetComponent<PlayerData>();
    *         NearbyUsers.Add(lastEnteredPlayer);
    *         return lastEnteredPlayer;
    *     }
    * }
    */

    //private void SetOperatingCrew(Dictionary<string, int> operatingUserCrew)
    //{
    //    if (!OperatingUserCrew.ContainsKey(PlayerData.UserName))
    //    {
    //        OperatingUserCrew.Add(PlayerData.UserName, PlayerData.CrewIndex);
    //        DisplayDictionary();
    //    }
    //}

    //public void DisplayDictionary()
    //{
    //    CurrentlyTreatingUser.Clear();
    //    CurrentlyTreatingCrew.Clear();
    //
    //    foreach (KeyValuePair<string, int> diction in OperatingUserCrew)
    //    {
    //        Debug.Log("Key = {" + diction.Key + "} " + "Value = {" + diction.Value + "}");
    //        CurrentlyTreatingUser.Add(diction.Key);
    //        CurrentlyTreatingCrew.Add(diction.Value);
    //    }
    //}
}
