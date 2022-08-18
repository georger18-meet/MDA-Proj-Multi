using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonitorSprites { HeartMonitor, ECG }

[System.Serializable]
public class NewPatientData
{
    [Header("Patient Informaion")]
    public string Name;
    public string SureName;
    public int Id, Age;
    public string Gender;
    public string PhoneNumber;
    public string MedicalCompany, AddressLocation, Complaint;

    [Header("Measurments")]
    [SerializeField] private PatientMeasurements _patientMeasurement;

    [Header("MonitorGraphTexture")]
    public List<Sprite> MonitorSpriteList;

    [Header("Status")]
    [SerializeField] private bool _isConscious = true;
    public bool IsConscious { get => _isConscious; set => value = _isConscious; }

    public NewPatientData()
    {

    }

    public NewPatientData(NewPatientData newPatientDataFromSO)
    {
        Name = newPatientDataFromSO.Name;
        SureName = newPatientDataFromSO.SureName;
        Id = newPatientDataFromSO.Id;
        Age = newPatientDataFromSO.Age;
        PhoneNumber = newPatientDataFromSO.PhoneNumber;
        MedicalCompany = newPatientDataFromSO.MedicalCompany;
        AddressLocation = newPatientDataFromSO.AddressLocation;
        Complaint = newPatientDataFromSO.Complaint;
        _patientMeasurement = newPatientDataFromSO._patientMeasurement;
        _isConscious = newPatientDataFromSO._isConscious;
        MonitorSpriteList = newPatientDataFromSO.MonitorSpriteList;
    }

    public void Initialize(string name, string sureName, int id, int age, string gender, string phoneNum, string medicalCompany, string adress, string complaint, string[] measurements)
    {
        Name = name;
        SureName = sureName;
        Id = id;
        Age = age;
        Gender = gender;
        PhoneNumber = phoneNum;
        MedicalCompany = medicalCompany;
        AddressLocation = adress;
        Complaint = complaint;

        _patientMeasurement.Initialize(measurements);
    }
    public string GetMeasurement(int x) => _patientMeasurement.GetMeasurement((Measurements)x);
    public string GetMeasurement(Measurements x) => _patientMeasurement.GetMeasurement(x);
    public void SetPatientMeasurement(string[] x) => _patientMeasurement.SetMeasurementValues(x);
}
