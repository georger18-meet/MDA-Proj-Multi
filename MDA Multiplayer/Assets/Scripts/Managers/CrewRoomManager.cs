using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CrewRoomManager : MonoBehaviour
{
    public GameObject RoomDoorBlocker;

    public List<PhotonView> _playersInRoomList;
    public int _playersMaxCount = 4;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BlockRoomAccess();
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
;
        return playerFound;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _playersInRoomList.Count < _playersMaxCount && !CheckIfAlreadyInList(other.gameObject))
        {
            _playersInRoomList.Add(other.gameObject.GetPhotonView());
        }
    }
}
