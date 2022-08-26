using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks,IInRoomCallbacks
{
    public static GameManager Instance;
    private PhotonView _photonView;
    public Transform[] IncidentPatientSpawns;
    public List<Transform> CurrentIncidentsTransforms;
    public PhotonView Pikud10View; 
    public PhotonView Redua10View; 
    public PhotonView Pinuy10View; 
    public PhotonView Henyon10View; 
    public RenderTexture Pikud10TextureRenderer;
    public Camera Pikud10Camera;
    public Material LineMaterial;



    public List<int> usedValues = new List<int>();
    public List<string> usedNamesValues = new List<string>();
    public List<PhotonView> NatanCarList = new List<PhotonView>();

    //[SerializeField] private int multiplayerScene;
    //[SerializeField] private int currentScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            Instance = this;

        }
        DontDestroyOnLoad(this.gameObject);

    }

    //public override void OnEnable()
    //{
    //    base.OnEnable();
    //    PhotonNetwork.AddCallbackTarget(this);
    //    SceneManager.sceneLoaded += OnFinshedLoading;
    //}

    //public override void OnDisable()
    //{
    //    base.OnDisable();
    //    PhotonNetwork.RemoveCallbackTarget(this);
    //    SceneManager.sceneLoaded -= OnFinshedLoading;
    //}

    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        OnEscape(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var car in NatanCarList)
            {
                Debug.Log(car.GetComponent<CarControllerSimple>()._randomNumber+" "+ car.GetComponent<CarControllerSimple>()._randomName);
                
            }
        }
    }

    private void OnEscape(bool paused)
    {
        ChangeCursorMode(paused);
        //GameMenuMode(paused);
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

    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    Debug.Log("We are now in a room");
      
    //    OnEscape(false);

    //}

    //public void ExitGame()
    //{
    //    SceneManager.LoadScene(0);
    //    //#if UNITY_EDITOR
    //    //        UnityEditor.EditorApplication.isPlaying = false;
    //    //#else
    //    //         Application.Quit();
    //    //#endif
    //}

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect(); // disconnected from the master client

         while (PhotonNetwork.IsConnected)
            yield return null;

         
        SceneManager.LoadScene(0);
    }

 

    public void OnPlayerDisconnect()
    {
        Debug.Log("attempting to Leave Room and Disconnecting");

    
        DisconnectPlayer();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            //PlayerData myPlayerData = ActionsManager.Instance.AllPlayersPhotonViews[i].gameObject.GetComponent<PlayerData>();
            //ActionsManager.Instance.AllPlayersPhotonViews.Remove(myPlayerData.PhotonView);

            if (ActionsManager.Instance.AllPlayersPhotonViews[i].OwnerActorNr == otherPlayer.ActorNumber)
            {
                if (ActionsManager.Instance.AllPlayersPhotonViews[i].GetComponent<PhotonView>().IsMine == true && PhotonNetwork.IsConnected == true)
                    PhotonNetwork.Destroy(ActionsManager.Instance.AllPlayersPhotonViews[i]);
            }
        }



        Debug.Log(otherPlayer.NickName + " has left the room");
        Debug.Log(ActionsManager.Instance.AllPlayersPhotonViews.Count);
    }

    //void OnFinshedLoading(Scene scene, LoadSceneMode mode)
    //{
    //    currentScene = scene.buildIndex;
    //}

}







