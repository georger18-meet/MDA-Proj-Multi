using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.ExceptionServices;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Random = UnityEngine.Random;

public class CrewRoomManager : MonoBehaviour,IPunObservable
{
    public GameObject RoomDoorBlocker;
    public Canvas RoomCrewMenuUI;
    public TextMeshProUGUI CrewMemberNameText1, CrewMemberNameText2, CrewMemberNameText3, CrewMemberNameText4;
    public List<TextMeshProUGUI> listOfUiNames;
    public List<TMP_Dropdown> CrewMemberRoleDropDownList;
    public TMP_Dropdown CrewLeaderDropDown;

    public List<PhotonView> _playersInRoomList;
    public int _playersMaxCount = 4;
    //public int _crewRoomIndex;
    private Color crewColor;

    public int _crewRoomIndex;
    public static int _crewRoomIndexStatic;

    private PhotonView _photonView;
    private Vector3 _vestPos = new Vector3(0f, 0.295f, -0.015f);

    [SerializeField] private string _prefixName;

    [SerializeField] private GameObject _patientMale, _patientFemale;

    [SerializeField] private bool isUsed;
    
    //[SerializeField] private GameObject _crewRoomDoor;

    private void Awake()
    {
        _crewRoomIndexStatic = 0;
        _photonView = GetComponent<PhotonView>();
        PopulateDropdownRoles();
        RoomCrewMenuUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        _crewRoomIndexStatic++;
        _crewRoomIndex = _crewRoomIndexStatic;
    }


    private void BlockRoomAccess()
    {
        if (_playersInRoomList.Count >= _playersMaxCount)
        {
            RoomDoorBlocker.SetActive(true);
        }
        else
        {
            RoomDoorBlocker.SetActive(false);
        }
    }

    private bool CheckIfAlreadyInList(GameObject player)
    {
        bool playerFound = false;

        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            if (_playersInRoomList[i].ViewID == player.GetPhotonView().ViewID)
            {
                playerFound = true;
            }
        }

        return playerFound;
    }

    private void RefreshCrewUITexts()
    {
        // Crew Roles UI
        for (int i = 0; i < _playersInRoomList.Count; i++)
        {

            //if (_playersInRoomList.Contains())
            //{
                
            //}


            listOfUiNames[i].text = _playersInRoomList[i].Owner.NickName;
            



            //switch (i)
            //{
            //    case 0:
            //        CrewMemberNameText1.text = _prefixName + "1" + _playersInRoomList[i].Owner.NickName;
            //        break;
            //    case 1:
            //        CrewMemberNameText2.text = _prefixName + "2" + _playersInRoomList[i].Owner.NickName;
            //        break;
            //    case 2:
            //        CrewMemberNameText3.text = _prefixName + "3" + _playersInRoomList[i].Owner.NickName;
            //        break;
            //    case 3:
            //        CrewMemberNameText4.text = _prefixName + "4" + _playersInRoomList[i].Owner.NickName;
            //        break;
            //    default:
            //        break;
            //}
        }

        // Crew Leader UI
        List<string> nicknamesList = new List<string>();
        foreach (var player in _playersInRoomList)
        {
            nicknamesList.Add(player.Owner.NickName);
        }

        CrewLeaderDropDown.ClearOptions();
        CrewLeaderDropDown.AddOptions(nicknamesList);
    }

        private void PopulateDropdownRoles()
    {
        string[] roles = Enum.GetNames(typeof(Roles));
        List<string> rolesList = new List<string>(roles);

        foreach (var dropdown in CrewMemberRoleDropDownList)
        {
            dropdown.AddOptions(rolesList);
        }
    }

    public void CreateCrewSubmit()
    {
        var color = Random.ColorHSV();
        _photonView.RPC("CrewCreateSubmit_RPC", RpcTarget.AllBufferedViaServer, GetCrewRolesByEnum(), GetCrewLeaderIndex());
        _photonView.RPC("ChangeCrewColors", RpcTarget.AllBufferedViaServer, new Vector3(color.r, color.g, color.b));

        //_photonView.RPC("CrewLeaderIsChosen", RpcTarget.AllBufferedViaServer, GetCrewLeader());
    }

    public int[] GetCrewRolesByEnum()
    {
        int[] roles = new int[_playersInRoomList.Count];

        for (int i = 0; i < roles.Length; i++)
        {
            roles[i] = CrewMemberRoleDropDownList[i].value;
        }
        return roles;
    }

    public int GetCrewLeaderIndex()
    {
        int leaderIndex = 0;

        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            if (CrewLeaderDropDown.GetComponentInChildren<TextMeshProUGUI>().text == _playersInRoomList[i].Owner.NickName)
            {
                leaderIndex = i;
            }

        }
        return leaderIndex;
    }

    // Show Hide MenuUI
    // --------------------
    public void ShowCrewRoomMenu()
    {
        _photonView.RPC("ShowCrewUI_RPC", RpcTarget.AllBufferedViaServer);

        // RoomCrewMenuUI.gameObject.SetActive(true);
        //RefreshCrewUITexts();
    }

    public void HideCrewRoomMenu()
    {
        _photonView.RPC("CloseCrewUI_RPC", RpcTarget.AllBufferedViaServer);

        // RoomCrewMenuUI.gameObject.SetActive(false);
    }

    public void SpawnPatient()
    {
        if (_crewRoomIndex < 2)
        {
            switch (_crewRoomIndex)
            {
                case 1:
                    PhotonNetwork.Instantiate(_patientMale.name, GameManager.Instance.IncidentPatientSpawns[_crewRoomIndex].position, GameManager.Instance.IncidentPatientSpawns[_crewRoomIndex].rotation);
                    GameManager.Instance.CurrentIncidentsTransforms.Add(GameManager.Instance.IncidentPatientSpawns[_crewRoomIndex]);
                    break;

                case 2:
                    PhotonNetwork.Instantiate(_patientFemale.name, GameManager.Instance.IncidentPatientSpawns[_crewRoomIndex].position, GameManager.Instance.IncidentPatientSpawns[_crewRoomIndex].rotation);
                    break;

                default:
                    break;
            }
        }
    }
    
    public void SpawnVehicle()
    {
        _photonView.RPC("SpawnVehicle_RPC",RpcTarget.AllBufferedViaServer);
    }


    // Collision Methods
    // --------------------
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player") && _playersInRoomList.Count < _playersMaxCount && !CheckIfAlreadyInList(other.gameObject))
        //{
        //    _photonView.RPC("AddingToRoomList_RPC", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName);
        //}

        if (other.CompareTag("test") && !_playersInRoomList.Contains(other.GetComponentInParent<PhotonView>()))
        {
            _playersInRoomList.Add(other.GetComponentInParent<PhotonView>());
            _photonView.RPC("AddtoUi_RPC", RpcTarget.AllBufferedViaServer);

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("test") && !_playersInRoomList.Contains(other.GetComponentInParent<PhotonView>()))
        {
            _playersInRoomList.Add(other.GetComponentInParent<PhotonView>());
        }
        //if (other.CompareTag("Player") && _playersInRoomList.Count < _playersMaxCount && !CheckIfAlreadyInList(other.gameObject))
        //{
        //    _photonView.RPC("AddingToRoomList_RPC", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player") && _playersInRoomList.Count < _playersMaxCount && CheckIfAlreadyInList(other.gameObject))
        //{
        //    _photonView.RPC("RemovingFromRoomList_RPC", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName);
        //}
        if (other.CompareTag("test") && _playersInRoomList.Contains(other.GetComponentInParent<PhotonView>()))
        {
            _playersInRoomList.Remove(other.GetComponentInParent<PhotonView>());
            // _photonView.RPC("RemoveFromUi_RPC", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName);
            TestingRemove();
        }
    }

    // PUN RPC Methods
    // --------------------
    [PunRPC]
    void AddingToRoomList_RPC(string currentPlayer)
    {
        PhotonView currentPlayerData = ActionsManager.Instance.GetPlayerPhotonViewByNickName(currentPlayer);


        if (currentPlayerData == null)
        {
            Debug.LogError("CurrentPlayer is Null");

            return;
        }

        for (int i = 0; i < 1; i++)
        {
            if (_playersInRoomList.Contains(currentPlayerData))
            {
                continue;
            }
            else
            {
                _playersInRoomList.Add(currentPlayerData);
            }
        }
        //BlockRoomAccess();
        Debug.LogError("Added to room");
       
    }

    [PunRPC]
    void RemovingFromRoomList_RPC(string currentPlayer)
    {
        PhotonView myPlayer = ActionsManager.Instance.GetPlayerPhotonViewByNickName(currentPlayer);

        if (_playersInRoomList.Contains(myPlayer))
        {
            _playersInRoomList.Remove(myPlayer);
        }

        Debug.LogError("Remove from room");

    }

    [PunRPC]
    void CrewCreateSubmit_RPC(int[] roleIndex, int leaderIndex)
    {
        int indexInCrewCounter = 0;
        for (int i = 0; i < roleIndex.Length; i++)
        {
            PlayerData desiredPlayerData = _playersInRoomList[i].GetComponent<PlayerData>();
            desiredPlayerData.CrewIndex = _crewRoomIndex;
            //desiredPlayerData.CrewIndex = ActionsManager.Instance.NextCrewIndex;
            desiredPlayerData.UserIndexInCrew = indexInCrewCounter;
            desiredPlayerData.UserRole = (Roles)roleIndex[i];
            //MeshFilter vest = desiredPlayerData.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<MeshFilter>();
            //vest.gameObject.SetActive(true);
            //vest.mesh = ActionsManager.Instance.Vests[(int)desiredPlayerData.UserRole].mesh;
            indexInCrewCounter++;
        }   

        foreach (PhotonView player in _playersInRoomList)
        {
            player.GetComponent<PlayerData>().IsCrewLeader = false;
        }

        PlayerData leaderToBe = _playersInRoomList[leaderIndex].GetComponent<PlayerData>();
        leaderToBe.IsCrewLeader = true;

        //SpawnVehicle();

        ActionsManager.Instance.NextCrewIndex++;
    }

    [PunRPC]
    void ChangeCrewColors(Vector3 randomColor)
    {
        crewColor = new Color(randomColor.x, randomColor.y, randomColor.z);

        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            PlayerData currentPlayerData = _playersInRoomList[i].GetComponent<PlayerData>();
            NameTagDisplay desiredPlayerName = _playersInRoomList[i].GetComponentInChildren<NameTagDisplay>();

            desiredPlayerName.text.color = crewColor;
            currentPlayerData.CrewColor = crewColor;
        }
    }


    [PunRPC]
    void SpawnVehicle_RPC()
    {
        VehicleChecker currentPosVehicleChecker = ActionsManager.Instance.VehiclePosTransforms[_crewRoomIndex - 1].GetComponent<VehicleChecker>();

        if (!currentPosVehicleChecker.IsPosOccupied)
        {
           /* GameObject natan = */PhotonNetwork.InstantiateRoomObject(ActionsManager.Instance.NatanPrefab.name, ActionsManager.Instance.VehiclePosTransforms[_crewRoomIndex - 1].position, ActionsManager.Instance.NatanPrefab.transform.rotation);
           // natan.GetComponent<CarControllerSimple>().OwnerCrew = _crewRoomIndex;
        }
    }

    [PunRPC]
    void ShowCrewUI_RPC()
    {
        RoomCrewMenuUI.gameObject.SetActive(true);

    }
    [PunRPC]
    void AddtoUi_RPC()
    {
        RefreshCrewUITexts();
    }
    [PunRPC]
    void RemoveFromUi_RPC(string currentPlayer)
    {
        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            if (listOfUiNames[i].text.Contains(currentPlayer))
            {
                listOfUiNames[i].text.Remove(currentPlayer[i]);
                Debug.Log("Test1");
            }
            Debug.Log("Test2");
        }
        Debug.Log("Test3");

    }

    void TestingRemove()
    {
        for (int i = 0; i < _playersInRoomList.Count; i++)
        {
            if (listOfUiNames[i].text.Contains(_playersInRoomList[i].Owner.NickName))
            {
               // listOfUiNames[i].text.Remove(_playersInRoomList[i].Owner.NickName);
                Debug.Log("Test4");
            }
            Debug.Log("Test5");
        }
        Debug.Log("Test6");

    }


    [PunRPC]
    void CloseCrewUI_RPC()
    {
        RoomCrewMenuUI.gameObject.SetActive(false);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CrewMemberNameText1.text);
            stream.SendNext(CrewMemberNameText2.text);
            stream.SendNext(CrewMemberNameText3.text);
            stream.SendNext(CrewMemberNameText4.text);
            stream.SendNext(_prefixName);
        }
        else
        {
            CrewMemberNameText1.text = (string)stream.ReceiveNext();
            CrewMemberNameText2.text = (string)stream.ReceiveNext();
            CrewMemberNameText3.text = (string)stream.ReceiveNext();
            CrewMemberNameText4.text = (string)stream.ReceiveNext();
            _prefixName = (string)stream.ReceiveNext();

        }
    }
}
