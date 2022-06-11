using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EvacuationManager : MonoBehaviour
{
    public string RoomName;
    public List<GameObject> PatientsEvacuated = new List<GameObject>();
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private GameObject _currentPatient;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI.text = "Evacuate Patient To\n''" + RoomName + "''?";
        UICanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EvacuatePatient(bool choice)
    {
        if (choice)
        {
            PatientsEvacuated.Add(_currentPatient);
            _currentPatient.SetActive(false);
            UICanvas.SetActive(false);
            _currentPatient = null;
        }
        else
        {
            UICanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Patient"))
        {
            _currentPatient = other.gameObject;
            UICanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Patient"))
        {
            _currentPatient = null;
            UICanvas.SetActive(false);
        }
    }
}
