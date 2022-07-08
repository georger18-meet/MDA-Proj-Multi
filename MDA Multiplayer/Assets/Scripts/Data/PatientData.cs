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
    public int HeartRateBPM;
    public int PainLevel, RespiratoryRate, CincinnatiLevel, BloodSuger, BloodPressure, OxygenSaturation, ETCO2;

    // Choose Appearance Material
    [Header("Appearance Material")]
    public Material FullyClothedMaterial;
    public Material ShirtOnlyMaterial, PantsOnlyMaterial, UnderwearOnlyMaterial;

    // Catch Measurement Name
    [Header("Measurement Name")]
    public List<int> MeasurementName;

    // Catch Clothing Material
    [Header("Clothing Material")]
    public List<Material> ClothingMaterial;

    public int GetMeasurementName(int index)
    {
        MeasurementName = new List<int>() { HeartRateBPM, PainLevel, RespiratoryRate, CincinnatiLevel, BloodSuger, BloodPressure, OxygenSaturation, ETCO2 };

        return MeasurementName[index];
    }

    private Material GetClothingMaterial(int index)
    {
        ClothingMaterial = new List<Material>() { FullyClothedMaterial, ShirtOnlyMaterial, PantsOnlyMaterial, UnderwearOnlyMaterial };

        return ClothingMaterial[index];

    }

    public void SetMeasurementByIndex(int index, int value)
    {
        MeasurementName = new List<int>() { HeartRateBPM, PainLevel, RespiratoryRate, CincinnatiLevel, BloodSuger, BloodPressure, OxygenSaturation, ETCO2 };
        MeasurementName[index] = value;

        Measurements measurements = (Measurements)index;

        // to be replaced
        switch (measurements)
        {
            case Measurements.BPM:
                HeartRateBPM = MeasurementName[index];
                break;

            case Measurements.PainLevel:
                PainLevel = MeasurementName[index];
                break;

            case Measurements.RespiratoryRate:
                RespiratoryRate = MeasurementName[index];
                break;

            case Measurements.CincinnatiLevel:
                CincinnatiLevel = MeasurementName[index];
                break;

            case Measurements.BloodSuger:
                BloodSuger = MeasurementName[index];
                break;

            case Measurements.BloodPressure:
                BloodPressure = MeasurementName[index];
                break;

            case Measurements.OxygenSaturation:
                OxygenSaturation = MeasurementName[index];
                break;

            case Measurements.ETCO2:
                ETCO2 = MeasurementName[index];
                break;
        }
    }
}
