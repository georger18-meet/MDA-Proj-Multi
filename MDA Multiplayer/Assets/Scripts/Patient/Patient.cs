using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
//using System.Linq;

public class Patient : MonoBehaviourPunCallbacks
{
    #region Script References
    [Header("Data & Scripts")]
    public PatientData PatientData;
    #endregion

    #region Public fields
    [SerializeField]
    public Dictionary<string, int> OperatingUserCrew = new Dictionary<string, int>();
    public Animation PatientAnimation;
    #endregion

    #region private serialized fields
    [Header("Joined Crews & Players Lists")]
    [SerializeField] public List<string> OperatingUsers = new List<string>();
    [SerializeField] public List<int> OperatingCrews = new List<int>();
    #endregion


    private GameObject _player;
    private MakeItAButton _makeItAButton;

    private PhotonView _photonView;
    private void Awake()
    {
        _makeItAButton = GetComponent<MakeItAButton>();
        _photonView = GetComponent<PhotonView>();
    }


    //public void DisplayDictionary()
    //{
    //    _photonView.RPC("RPC_DisplayDictionary",RpcTarget.AllBufferedViaServer);

    //}

    //[PunRPC]
    public void DisplayDictionary()
    {

        OperatingUsers.Clear();
        OperatingCrews.Clear();

        foreach (KeyValuePair<string, int> diction in OperatingUserCrew)
        {
            Debug.Log("Key = {" + diction.Key + "} " + "Value = {" + diction.Value + "}");
            OperatingUsers.Add(diction.Key);
            OperatingCrews.Add(diction.Value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

            if (_player == null)
            {
                if (other.CompareTag("Player"))
                {
                    _player = other.gameObject;
                    _makeItAButton.EventToCall.AddListener(delegate
                    {
                        _player.GetComponent<ActionsManager>().SetOperatingCrewCheck(this.gameObject);
                      // _player.GetComponent<ActionsManager>().ConfirmOperation(_player);
                    });
                    
                }
            }
        
    }




    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = null;
            _makeItAButton.EventToCall.RemoveListener(delegate { _player.GetComponent<ActionsManager>().SetOperatingCrewCheck(this.gameObject); });
        }
    }

    // refactor for player to do on patient

    #region getting patient data
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (_player == null)
    //    {
    //        if (other.CompareTag("Player"))
    //        {
    //            _player = other.gameObject;
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        _player = null;
    //    }
    //}

    //private void GetPatientInfo()
    //{
    //    if (_currentPatientScript.CheckIfPlayerJoined())
    //    {
    //        AmbulanceActionPanel.SetActive(true);
    //        _patientInfoSO = _currentPatientScript.PatientInfoSO;
    //    }
    //    else
    //    {
    //        AmbulanceActionPanel.SetActive(false);
    //        _patientInfoSO = null;
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Patient"))
    //    {
    //        _currentPatientScript = other.gameObject.GetComponent<Patient>();
    //        GetPatientInfo();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Patient"))
    //    {
    //        AmbulanceActionPanel.SetActive(false);
    //        _currentPatientScript = null;
    //        _patientInfoSO = null;
    //    }
    //}
    #endregion

    //// For Use Externally
    //public bool CheckIfPlayerJoined()
    //{
    //    bool playerIsJoined = false;
    //    if (_player != null)
    //    {
    //        if (OperatingUserCrew.ContainsKey(_player.GetComponent<CrewMember>().UserName))
    //        {
    //            playerIsJoined = true;
    //        }
    //    }
    //    return playerIsJoined;
    //}
}
