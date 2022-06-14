using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Patient : MonoBehaviour
{
    [Header("Photon")]
    public PhotonView PhotonView;

    [Header("Data")]
    private PlayerActions _playerActions;

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
    #endregion

    //public Dictionary<string, int> OperatingUserCrew = new Dictionary<string, int>();
    //public Animation PatientAnimation;

    private void Awake()
    {
        PatientRenderer.material = PatientData.FullyClothedMaterial;
    }

    private void Start()
    {
        GameManager.Instance.AllPatients.Add(this);
        GameManager.Instance.AllPatientsPhotonViews.Add(PhotonView);
    }

    public void AddUserToTreatingLists(PlayerData currentPlayerData)
    {
        currentPlayerData = currentPlayerData != null ? currentPlayerData : null;

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

    [PunRPC]
    private void OnJoinPatient(bool isJoined)
    {
        foreach (PhotonView photonView in GameManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView != null ? photonView.GetComponent<PlayerData>() : null;
            _playerActions = photonView != null ? photonView.GetComponent<PlayerActions>() : null;

            if (photonView.IsMine)
            {
                if (isJoined)
                {
                    AddUserToTreatingLists(desiredPlayerData);

                    _playerActions.SetupPatientInfoDisplay();

                    UIManager.Instance.JoinPatientPopUp.SetActive(false);
                    UIManager.Instance.PatientMenuParent.SetActive(true);
                    UIManager.Instance.PatientInfoParent.SetActive(false);

                }
                else
                {
                    UIManager.Instance.JoinPatientPopUp.SetActive(false);
                }
            }
        }
    }

    [PunRPC]
    private void LeavePatient()
    {
        foreach (PhotonView photonView in GameManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                Debug.Log("Attempting leave patient");

                UIManager.Instance.CloseAllPatientWindows();
                TreatingUsers.Remove(desiredPlayerData);
                Debug.Log("Left Patient Succesfully");
            }
        }
    }

    public void OnInteracted()
    {
        _playerActions.OnPatientClicked();
    }
}
