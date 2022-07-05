using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab, _patientPrefab; //_ambulancePrefab, _natanPrefab;
    [SerializeField] private Transform _patientSpawner;

    public float _minX, _minZ, _maxX, _maxZ;

    void Start()
    {
        Vector3 randomPos = new Vector3(Random.Range(_minX,_maxX),0.5f,Random.Range(_minZ,_maxZ));
        PhotonNetwork.Instantiate(_playerPrefab.name, randomPos, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(_patientPrefab.name, _patientSpawner.position, _patientPrefab.transform.rotation);
        //PhotonNetwork.InstantiateRoomObject(_patientPrefab.name, new Vector3(-255.490997f, 0.210999995f, -176.893005f), new Quaternion(-0.31172365f, 0.634687662f, 0.634687662f, 0.31172356f));
        //PhotonNetwork.InstantiateRoomObject(_natanPrefab.name, _patientSpawner.position, _patientPrefab.transform.rotation);
    }
}
