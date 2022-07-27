using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager Instance;

    [Header("Photon")]
    public List<PhotonView> AllPatientsPhotonViews;
    public List<PhotonView> AllPlayersPhotonViews;
    public List<PlayerData> AllPlayerData;

    #region Data References
    [Header("Data & Scripts")]
    public List<Patient> AllPatients;

    [Header("VehiclesPrefabs")]
    //[SerializeField] private GameObject _ambulancePrefab;
    public GameObject NatanPrefab;

    #region Prefab References
    [Header("Equipment")]
    [SerializeField] private GameObject _headVice;
    [SerializeField] private GameObject _hat, _megaphone, _clipboard, _neckBrace, _bloodPressureSleeve, _oxyMask, _respirationBalloon;
    [SerializeField] private GameObject _armBandage, _armTourniquet, _bicepsBandage, _bicepsTourniquet, _kneeBandage, _kneeTourniquet, _shinBandage, _shinTourniquet;

    [Header("Attachments")]
    [SerializeField] private GameObject _asherman;
    [SerializeField] private GameObject _ecgSticker, _throatTube, _inTube, _venflon;

    [Header("Aids")]
    [SerializeField] private GameObject _iVPole;
    [SerializeField] private GameObject _oxyTank;

    [Header("Devices")]
    [SerializeField] private GameObject _bloodPressureDevice;
    [SerializeField] private GameObject _monitor, _respirator;

    [Header("Vests")]
    [SerializeField] private GameObject _cfrVest;
    [SerializeField] private GameObject _medicVest, _seniorMedicVest, _paramedicVest, _docVest;
    #endregion

    #endregion

    [Header("Crews")]
    public int NextCrewIndex = 0;
    public List<Transform> /*AmbulancePosTransforms,*/ NatanPosTransforms;

    private Patient _lastClickedPatient;
    private PatientData _lastClickedPatientData;

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region UnityEvents
    public void OnPatientClicked()
    {
        Debug.Log($"Attempting to Click On Patient");

        for (int i = 0; i < AllPlayersPhotonViews.Count; i++)
        {
            //if (!AllPlayersPhotonViews[i].IsMine)
            //    return;

            PlayerData myPlayerData = AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();
            _lastClickedPatient = myPlayerData.CurrentPatientNearby;

            PatientData currentPatientData = myPlayerData.CurrentPatientNearby != null ? myPlayerData.CurrentPatientNearby.PatientData : null;
            _lastClickedPatientData = currentPatientData;

            Debug.Log($"{myPlayerData.UserName} Clicked on: {myPlayerData.CurrentPatientNearby}");

            if (!myPlayerData.CurrentPatientNearby.IsPlayerJoined(myPlayerData))
            {
                _lastClickedPatient.PhotonView.RPC("UpdatePatientInfoDisplay", RpcTarget.AllBufferedViaServer);
                UIManager.Instance.JoinPatientPopUp.SetActive(true);
            }
            else
            {
                _lastClickedPatient.PhotonView.RPC("UpdatePatientInfoDisplay", RpcTarget.AllBufferedViaServer);
                UIManager.Instance.PatientInfoParent.SetActive(true);
            }
        }
    }

    public void OnPlayerJoinPatientRPC(bool isJoined)
    {
        Debug.Log("attempting to Join Patient");

        for (int i = 0; i < AllPlayersPhotonViews.Count; i++)
        {
            PlayerData myPlayerData = AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();

            if (isJoined)
            {
                myPlayerData.PhotonView.RPC("OnJoinPatient", RpcTarget.AllBufferedViaServer);
                UIManager.Instance.JoinPatientPopUp.SetActive(false);
                UIManager.Instance.PatientInfoParent.SetActive(true);
            }
            else
            {
                UIManager.Instance.JoinPatientPopUp.SetActive(false);
            }
        }
    }

    public void OnPlayerLeavePatientRPC()
    {
        Debug.Log("attempting to Leave Patient");

        for (int i = 0; i < AllPlayersPhotonViews.Count; i++)
        {
            PlayerData myPlayerData = AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();
            myPlayerData.PhotonView.RPC("OnLeavePatient", RpcTarget.AllBufferedViaServer);
            UIManager.Instance.CloseAllPatientWindows();
        }
    }
    #endregion

}