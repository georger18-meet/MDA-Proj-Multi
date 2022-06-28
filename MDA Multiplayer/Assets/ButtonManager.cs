using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button[] buttons;


    private Evacuation evacuation;

    [SerializeField] private GameObject targetPatient;

    void Start()
    {
        foreach (var button in buttons)
        {
            button.onClick.AddListener((() =>
                    EvacuationManager.Instance.AddPatientToRooms(evacuation.NearbyPatient[0], evacuation.RoomEnum)));
            Debug.Log(targetPatient.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
