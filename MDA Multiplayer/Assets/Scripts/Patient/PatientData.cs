using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Patient Data",menuName = "Patient Data") ]
public class PatientData : ScriptableObject
{
    // General Info
    [Header("Patient Informaion")]
    public string SureName;
    public string LastName;
    public int Id, Age;
    public string Gender;
    public int PhoneNumber;
    public string MedicalCompany, AddressLocation, Complaint;

    // Health Data
    [Header("Measurments")]
    public int BPM;
    public int PainLevel, RespiratoryRate, CincinnatiLevel, BloodSuger, BloodPressure, OxygenSaturation, ETCO2;

    // Appearance
    [Header("Appearance")]
    public Material PatientShirtMaterial;
    public Material PatientPantsMaterial;

    // Catch Measurement Name
    private List<int> measurementName;

    public int GetMeasurementName(int index)
    {
        measurementName = new List<int>() { BPM, PainLevel, RespiratoryRate, CincinnatiLevel, BloodSuger, BloodPressure, OxygenSaturation, ETCO2 };

        return measurementName[index];
    }

    public void SetMeasurementName(int index, int value)
    {
        measurementName = new List<int>() { BPM, PainLevel, RespiratoryRate, CincinnatiLevel, BloodSuger, BloodPressure, OxygenSaturation, ETCO2 };
        measurementName[index] = value;

        Measurements measurements = (Measurements)index;

        // to be replaced
        switch (measurements)
        {
            case Measurements.BPM:
                BPM = measurementName[index];
                break;

            case Measurements.PainLevel:
                PainLevel = measurementName[index];
                break;

            case Measurements.RespiratoryRate:
                RespiratoryRate = measurementName[index];
                break;

            case Measurements.CincinnatiLevel:
                CincinnatiLevel = measurementName[index];
                break;

            case Measurements.BloodSuger:
                BloodSuger = measurementName[index];
                break;

            case Measurements.BloodPressure:
                BloodPressure = measurementName[index];
                break;

            case Measurements.OxygenSaturation:
                OxygenSaturation = measurementName[index];
                break;

            case Measurements.ETCO2:
                ETCO2 = measurementName[index];
                break;
        }
    }
}
