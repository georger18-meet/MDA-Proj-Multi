using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerActions : MonoBehaviour
{
    #region MonoBehaviour Callbacks

    #endregion

    #region Assignment
    // Triggered upon Clicking on the Patient
    public void OnPatientClicked()
    {
        Debug.Log($"Attempting to Click On Patient");
        foreach (PhotonView photonView in GameManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                if (desiredPlayerData.CurrentPatientNearby == null)
                {
                    return;
                }

                GameManager.Instance.LastClickedPatient = desiredPlayerData.CurrentPatientNearby;

                PatientData currentPatientData = desiredPlayerData.CurrentPatientNearby != null ? desiredPlayerData.CurrentPatientNearby.PatientData : null;
                GameManager.Instance.LastClickedPatientData = currentPatientData;

                Debug.Log($"{desiredPlayerData.UserName} Clicked on: {desiredPlayerData.CurrentPatientNearby}");

                if (!desiredPlayerData.CurrentPatientNearby.IsPlayerJoined(desiredPlayerData))
                {
                    UIManager.Instance.JoinPatientPopUp.SetActive(true);
                }
                else
                {
                    SetupPatientInfoDisplay();
                    UIManager.Instance.PatientMenuParent.SetActive(true);
                }
            }
        }
    }

    public void OnJoinPatientRPC(bool isJoined)
    {
        Debug.Log("attempting Join Patient");

        foreach (PhotonView photonView in GameManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();

            if (photonView.IsMine)
            {
                desiredPlayerData.CurrentPatientNearby.PhotonView.RPC("OnJoinPatient", RpcTarget.AllBuffered, isJoined);
            }
        }
    }

    public void LeavePatientRPC()
    {
        foreach (PhotonView photonView in GameManager.Instance.AllPlayersPhotonViews)
        {
            PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();
            desiredPlayerData.CurrentPatientNearby.PhotonView.RPC("LeavePatient", RpcTarget.AllBuffered);
        }
    }

    public void SetupPatientInfoDisplay()
    {
        UIManager.Instance.SureName.text = GameManager.Instance.LastClickedPatientData.SureName;
        UIManager.Instance.LastName.text = GameManager.Instance.LastClickedPatientData.LastName;
        UIManager.Instance.Gender.text = GameManager.Instance.LastClickedPatientData.Gender;
        UIManager.Instance.Adress.text = GameManager.Instance.LastClickedPatientData.AddressLocation;
        UIManager.Instance.InsuranceCompany.text = GameManager.Instance.LastClickedPatientData.MedicalCompany;
        UIManager.Instance.Complaint.text = GameManager.Instance.LastClickedPatientData.Complaint;

        UIManager.Instance.Age.text = GameManager.Instance.LastClickedPatientData.Age.ToString();
        UIManager.Instance.Id.text = GameManager.Instance.LastClickedPatientData.Id.ToString();
        UIManager.Instance.PhoneNumber.text = GameManager.Instance.LastClickedPatientData.PhoneNumber.ToString();
    }

    //private void OnJoinPatient(bool isJoined)
    //{
    //    foreach (PhotonView photonView in AllPlayersPhotonViews)
    //    {
    //        PlayerData desiredPlayerData = photonView.GetComponent<PlayerData>();
    //
    //        if (photonView.IsMine)
    //        {
    //            if (isJoined)
    //            {
    //                _lastClickedPatient.AddUserToTreatingLists(desiredPlayerData);
    //
    //                SetupPatientInfoDisplay();
    //
    //                UIManager.Instance.JoinPatientPopUp.SetActive(false);
    //                UIManager.Instance.PatientMenuParent.SetActive(true);
    //                UIManager.Instance.PatientInfoParent.SetActive(false);
    //
    //            }
    //            else
    //            {
    //                UIManager.Instance.JoinPatientPopUp.SetActive(false);
    //            }
    //        }
    //    }
    //}

    //private void LeavePatient()
    //{
    //    if (PlayerData.Instance.CurrentPatientNearby.PhotonView.IsMine)
    //    {
    //      Debug.Log("Attempting leave patient");
    //
    //      UIManager.Instance.CloseAllPatientWindows();
    //      PlayerData.Instance.CurrentPatientNearby.TreatingUsers.Remove(PlayerData.Instance);
    //      Debug.Log("Left Patient Succesfully");
    //    }
    //}
    #endregion
}