using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab, _patientPrefab,NatanPrefab,ColliderPrefab;
    [SerializeField] private Transform _patientSpawner, NatanSpanwner,SpawnerTest;

    public float _minX, _minZ, _maxX, _maxZ;

    void Start()
    {
        Vector3 randomPos = new Vector3(Random.Range(_minX,_maxX),0.5f,Random.Range(_minZ,_maxZ));
        PhotonNetwork.Instantiate(_playerPrefab.name, randomPos, Quaternion.identity);
       // if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.InstantiateRoomObject(_patientPrefab.name, _patientSpawner.position, _patientPrefab.transform.rotation);
            PhotonNetwork.InstantiateRoomObject(ColliderPrefab.name, SpawnerTest.position, ColliderPrefab.transform.rotation);
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.InstantiateRoomObject(NatanPrefab.name, NatanSpanwner.position, NatanPrefab.transform.rotation);
    }
}
