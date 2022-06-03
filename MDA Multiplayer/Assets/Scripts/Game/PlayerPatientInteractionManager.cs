using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerPatientInteractionManager : MonoBehaviour
{
    [SerializeField] private PatientData _patientData;
    [SerializeField] private Patient _patientScript;

    [SerializeField] private PlayerData _playerData;
    [SerializeField] private PlayerMovement _playerScript;

    [SerializeField] private Canvas _patientMenuCanvas;
    [SerializeField] private Image _ActionBar;

    [SerializeField]
    private GameObject _playerGO, _patientGO;
    
    [SerializeField] private List<GameObject> _addedItems;
    [SerializeField] private List<Transform> _addedItemsFixedPositions;

    public void JoinPatient()
    {
        if (_playerScript.CheckInteraction().transform.tag == "Patient")
        {
            print("Managed to get raycstHit");
        }
    }
}
