using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Instructor : MonoBehaviour
{
    [field: SerializeField] public string UserName { get; set; }
    [field: SerializeField] public string CrewName { get; set; }
    [field: SerializeField] public int UserIndexInCrew { get; set; }
    [field: SerializeField] public int CrewIndex { get; set; }
    [field: SerializeField] public bool IsCrewLeader { get; set; }
    [field: SerializeField] public bool IsInstructor { get; set; }
    [field: SerializeField] public Roles UserRole { get; set; }
    [field: SerializeField] public Color CrewColor { get; set; }
    [field: SerializeField] public Patient CurrentPatientNearby { get; set; }
    [field: SerializeField] public Animation PlayerAnimation { get; set; }
    private void StartAran()
    {
        foreach (PlayerData playerData in ActionsManager.Instance.AllPlayerData)
        {
            playerData.CrewIndex = 0;
            CrewColor = Color.white;
            IsCrewLeader = false;
        }
    }
}
