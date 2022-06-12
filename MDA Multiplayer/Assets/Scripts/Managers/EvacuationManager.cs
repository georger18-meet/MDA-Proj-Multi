using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EvacuationManager : MonoBehaviour
{
    [SerializeField] private string _roomName;
    [SerializeField] private GameObject _evacuationUI;
    [SerializeField] private TextMeshProUGUI _destinationName;

    [SerializeField] private List<GameObject> _patientsEvacuated = new List<GameObject>();
    private GameObject _currentPatient;

    void Start()
    {
        _destinationName.text = $"Evacuate Patient To {_roomName}?";
        _evacuationUI.SetActive(false);
    }

    public void EvacuatePatient(bool choice)
    {
        if (choice)
        {
            _patientsEvacuated.Add(_currentPatient);
            _currentPatient.SetActive(false);
            _evacuationUI.SetActive(false);
            _currentPatient = null;
        }
        else
        {
            _evacuationUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Patient"))
        {
            _currentPatient = other.gameObject;
            _evacuationUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Patient"))
        {
            _currentPatient = null;
            _evacuationUI.SetActive(false);
        }
    }
}
