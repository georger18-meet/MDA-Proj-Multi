using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class Patient : MonoBehaviour
{
    [Header("Photon")]
    public PhotonView PhotonView;

    #region Script References
    [Header("Data & Scripts")]
    public PatientData PatientData;
    public List<ActionSequence> ActionSequences;
    #endregion

    #region Material References
    public Renderer PatientRenderer;
    #endregion

    #region Public fields
    [Header("Joined Crews & Players Lists")]
    public List<PlayerData> NearbyUsers;
    public List<PlayerData> TreatingUsers;
    public List<PlayerData> AllUsersTreatedThisPatient;
    public List<int> TreatingCrews;
    public List<int> AllCrewTreatedThisPatient;

    [Header("Treatment Positions")]
    public Transform ChestPosPlayerTransform;
    public Transform ChestPosEquipmentTransform, HeadPosPlayerTransform, HeadPosEquipmentTransform;
    #endregion

    #region Monovehavior Callbacks
    private void Awake()
    {
        PatientRenderer.material = PatientData.FullyClothedMaterial;
        DontDestroyOnLoad(gameObject.transform);
    }

    private void Start()
    {
        //players = new List<PlayerController>();
        ActionsManager.Instance.AllPatients.Add(this);
        ActionsManager.Instance.AllPatientsPhotonViews.Add(PhotonView);
    }
    #endregion

    #region Collision & Triggers

    private void OnTriggerEnter(Collider other)
    {



        if (!other.TryGetComponent(out PlayerData possiblePlayer))
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

        if (other.TryGetComponent(out PlayerData possiblePlayer))
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
    #endregion


    #region PunRPC invoke by Patient
    [PunRPC]
    public void AddUserToTreatingLists(string currentPlayer)
    {
        PlayerData currentPlayerData = GameObject.Find(currentPlayer).GetComponent<PlayerData>();
        //PlayerData currentPlayerData = currentPlayer != null ? currentPlayer as PlayerData : null;

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

                if (AllUsersTreatedThisPatient.Contains(currentPlayerData))
                {
                    continue;
                }
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

    [PunRPC]
    public void UpdatePatientInfoDisplay()
    {
        UIManager.Instance.SureName.text = PatientData.SureName;
        UIManager.Instance.LastName.text = PatientData.LastName;
        UIManager.Instance.Gender.text = PatientData.Gender;
        UIManager.Instance.Adress.text = PatientData.AddressLocation;
        UIManager.Instance.InsuranceCompany.text = PatientData.MedicalCompany;
        UIManager.Instance.Complaint.text = PatientData.Complaint;

        UIManager.Instance.Age.text = PatientData.Age.ToString();
        UIManager.Instance.Id.text = PatientData.Id.ToString();
        UIManager.Instance.PhoneNumber.text = PatientData.PhoneNumber.ToString();
    }
    #endregion

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

    #region PunRPC Methods

    [PunRPC]
    private void ChangeHeartRateRPC(int newBPM)
    {
        PatientData.HeartRateBPM = newBPM;
    }

    [PunRPC]
    private void SetMeasurementByIndexRPC(int index, int value)
    {
        PatientData.MeasurementName = new List<int>() { PatientData.HeartRateBPM, PatientData.PainLevel, PatientData.RespiratoryRate, PatientData.CincinnatiLevel, PatientData.BloodSuger, PatientData.BloodPressure, PatientData.OxygenSaturation, PatientData.ETCO2 };
        PatientData.MeasurementName[index] = value;

        Measurements measurements = (Measurements)index;

        // to be replaced
        switch (measurements)
        {
            case Measurements.BPM:
                PatientData.HeartRateBPM = PatientData.MeasurementName[index];
                break;

            case Measurements.PainLevel:
                PatientData.PainLevel = PatientData.MeasurementName[index];
                break;

            case Measurements.RespiratoryRate:
                PatientData.RespiratoryRate = PatientData.MeasurementName[index];
                break;

            case Measurements.CincinnatiLevel:
                PatientData.CincinnatiLevel = PatientData.MeasurementName[index];
                break;

            case Measurements.BloodSuger:
                PatientData.BloodSuger = PatientData.MeasurementName[index];
                break;

            case Measurements.BloodPressure:
                PatientData.BloodPressure = PatientData.MeasurementName[index];
                break;

            case Measurements.OxygenSaturation:
                PatientData.OxygenSaturation = PatientData.MeasurementName[index];
                break;

            case Measurements.ETCO2:
                PatientData.ETCO2 = PatientData.MeasurementName[index];
                break;
        }
    }

    [PunRPC]
    private void ChangeClothingRPC(int index)
    {
        Clothing clothing = (Clothing)index;

        switch (clothing)
        {
            case Clothing.FullyClothed:

                transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = PatientData.FullyClothedMaterial;
                break;

            case Clothing.ShirtOnly:
                transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = PatientData.ShirtOnlyMaterial;
                break;

            case Clothing.PantsOnly:
                transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = PatientData.PantsOnlyMaterial;
                break;

            case Clothing.UnderwearOnly:
                transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = PatientData.UnderwearOnlyMaterial;
                break;

            default:
                break;
        }
    }
    #endregion
}
