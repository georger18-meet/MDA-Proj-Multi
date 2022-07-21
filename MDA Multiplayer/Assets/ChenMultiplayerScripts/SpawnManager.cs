using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    [Header("PlayerPrefabs")]
    [SerializeField] private GameObject _playerMalePrefab;
    //[SerializeField] private GameObject _playerFemalePrefab;

    [Header("PatientPrefabs")]
    [SerializeField] private GameObject _patientMalePrefab;
    [SerializeField] private GameObject _patientFemalePrefab;//, _patientMaleOldPrefab, _patientFemaleOldPrefab, _patientKid, _patientToddler;

    [Header("VehiclesPrefabs")]
    //[SerializeField] private GameObject _ambulancePrefab;
    [SerializeField] private GameObject _natanPrefab;

    [Header("GeneralPrefabs")]
    [SerializeField] private GameObject _crewRoomColliderPrefab;

    [Header("Transform Positions In Scene")]
    [SerializeField] private Transform _patientMalePosTransform;
    [SerializeField] private Transform _patientFemalePosTransform;
    [SerializeField] private Transform /*_ambulancePosTransform,*/ _natanPosTransform;
    [SerializeField] private Transform _crewRoomOneColliderPosTransform, _crewRoomTwoColliderPosTransform; // crew rooms collider positions in scene

    public float _minX, _minZ, _maxX, _maxZ;

    void Start()
    {
        Vector3 randomPos = new Vector3(Random.Range(_minX,_maxX),1f,Random.Range(_minZ,_maxZ));
        PhotonNetwork.Instantiate(_playerMalePrefab.name, randomPos, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(_patientMalePrefab.name, _patientMalePosTransform.position, _patientMalePrefab.transform.rotation);
        PhotonNetwork.InstantiateRoomObject(_patientFemalePrefab.name, _patientFemalePosTransform.position, _patientFemalePrefab.transform.rotation);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject(_natanPrefab.name, _natanPosTransform.position, _natanPrefab.transform.rotation);
            PhotonNetwork.InstantiateRoomObject(_crewRoomColliderPrefab.name,  _crewRoomOneColliderPosTransform.position, _crewRoomOneColliderPosTransform.rotation);
            PhotonNetwork.InstantiateRoomObject(_crewRoomColliderPrefab.name,  _crewRoomTwoColliderPosTransform.position, _crewRoomTwoColliderPosTransform.rotation);
        }   
    }
}
