using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Patient : MonoBehaviour
{
    #region Script References
    [Header("Data & Scripts")]
    public PatientData PatientData;
    public List<ActionSequence> ActionSequences;
    #endregion

    #region Material References
    [SerializeField] private Material InitialShirt, InitialPants;
    [SerializeField] private Renderer Shirt, Pants;
    #endregion

    #region Public fields
    public Animation PatientAnimation;
    #endregion

    #region private serialized fields
    [Header("Joined Crews & Players Lists")]
    public List<PlayerData> NearbyUsers;
    public List<PlayerData> TreatingUsers;
    public List<int> TreatingCrews;

    [Header("Treaters BackLog")]
    public List<PlayerData> AllUsersTreatedThisPatient;
    public List<int> AllCrewTreatedThisPatient;
    #endregion


    private PhotonView _photonView;
    public PhotonView GetphotonView => _photonView;


    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        ActionsManager.Instance.AllPatients.Add(this);
        PatientData.PatientShirtMaterial = InitialShirt;
        PatientData.PatientPantsMaterial = InitialPants;
    }

    private void Update()
    {
        Shirt.material = PatientData.PatientShirtMaterial;
        Pants.material = PatientData.PatientPantsMaterial;
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
}
