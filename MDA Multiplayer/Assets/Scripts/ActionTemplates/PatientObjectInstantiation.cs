using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PatientObjectInstantiation : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] private GameObject _item;

    [Header("Item Offsets")]
    [SerializeField] private Vector3 _offsetPos;
    [SerializeField] private Quaternion _offsetRot;

    [Header("Alerts")]
    [SerializeField] private bool _showAlert = false;
    [SerializeField] private bool _updateLog = true;

    [Header("Condition")]
    [SerializeField] private bool _useColliders = false;

    private Transform _patientTransform;
    private Transform[] _equipmentColliders;

    // 0 = HeadPos, 1 = ChestPos, 2 = Knees
    public void InstantiateOnPatient(int colliderIndex)
    {
        foreach (PhotonView photonView in ActionsManager.Instance.AllPlayersPhotonViews)
        {
            if (photonView.IsMine)
            {
                PlayerData localPlayerData = photonView.GetComponent<PlayerData>();

                if (!localPlayerData.CurrentPatientNearby.IsPlayerJoined(localPlayerData))
                    return;

                Patient currentPatient = localPlayerData.CurrentPatientNearby;
                PatientData currentPatientData = currentPatient.PatientData;
                
                if (_useColliders)
                {
                    _equipmentColliders[0] = currentPatient.HeadPosEquipmentTransform;
                    _equipmentColliders[1] = currentPatient.ChestPosEquipmentTransform;
                    //_equipmentColliders[2] = currentPatient.KneesPosEquipmentTransform;

                    GameObject item = PhotonNetwork.Instantiate(_item.name, _equipmentColliders[colliderIndex].position, _equipmentColliders[colliderIndex].rotation);
                    //item.transform.SetParent(_equipmentColliders[colliderIndex]);
                }
                else
                {
                    _patientTransform = currentPatient.transform;
                    Quaternion desiredRotation = new Quaternion(currentPatient.transform.rotation.x + _offsetRot.y, currentPatient.transform.rotation.y + _offsetRot.x, currentPatient.transform.rotation.z + _offsetRot.z, currentPatient.transform.rotation.w + _offsetRot.w);

                    GameObject item = PhotonNetwork.Instantiate(_item.name, currentPatient.transform.position + _offsetPos, desiredRotation);
                    //item.transform.SetParent(_patientTransform);
                }

                if (_updateLog)
                {
                    ActionTemplates.Instance.UpdatePatientLog(localPlayerData.CrewIndex, localPlayerData.UserName, $"Placed {_item.name} on {currentPatientData.Name} {currentPatientData.SureName}");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, _offsetPos);
    }
}
