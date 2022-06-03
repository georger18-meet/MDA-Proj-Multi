using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerPlaceHolder
{
    /*
    #region Assignment
    // Triggered upon Clicking on the Patient
    public void SetOperatingCrewCheck(PlayerData playerData, Patient currentPatientScript, GameObject player, GameObject joinPatientPopUp, GameObject patientMenu)
    {
        
        if (player == null)
        {
            return;
        }
        else if (!currentPatientScript.OperatingUserCrew.ContainsKey(playerData.UserName))
        {
            SetupPatientInfoDisplay();
            joinPatientPopUp.SetActive(true);
        }
        else if (currentPatientScript.OperatingUserCrew.ContainsKey(playerData.UserName))
        {
            SetupPatientInfoDisplay();
            patientMenu.SetActive(true);
        }
    }

    // triggered upon pressing "yes" in patient Join pop-up
    public void ConfirmOperation(bool confirm, Patient currentPatientScript, GameObject joinPatientPopUp, GameObject patientMenu, GameObject patientInfo)
    {
        if (confirm)
        {
            // need to verify that set operating crew is setting an empty group of maximum 4 and insitialize it with current player
            SetOperatingCrew(currentPatientScript.OperatingUserCrew);
            joinPatientPopUp.SetActive(false);
            patientMenu.SetActive(true);
            patientInfo.SetActive(false);

        }
        else
        {
            _joinPatientPopUp.SetActive(false);
        }
    }

    private void SetOperatingCrew(Dictionary<string, int> operatingUserCrew)
    {
        if (!_currentPatientScript.OperatingUserCrew.ContainsKey(_playerData.UserName))
        {
            _currentPatientScript.OperatingUserCrew.Add(_playerData.UserName, _playerData.CrewIndex);
            _currentPatientScript.DisplayDictionary();
        }
    }
    #endregion

    #region Patient Menu Events
    // clost menu
    public void OpenCloseMenu(GameObject menu)
    {
        if (menu.activeInHierarchy)
            menu.SetActive(false);
        else
            menu.SetActive(true);

        print("Close Patient Menu");
    }


    // paitent background info: name, weghit, gender, adress...
    public void PatientInfo()
    {
        OpenCloseMenu(_patientInfoParent);

        print("Patient Information");
    }

    // list of actions done on the patient by players, aranged by time stamp
    public void Log()
    {
        OpenCloseMenu(_actionLogParent);
        print("Log Window");
    }

    // open up a form that follows this reference: https://drive.google.com/file/d/1EScLHzpHT_YOk02lS_jzjErDfSGRWj2x/view?usp=sharing
    public void TagMiun()
    {
        print("Tag Miun");
    }

    // take current player out of their crew's list
    public void LeavePatient()
    {
        OpenCloseMenu(_patientMenuParent);

        if (_currentPatientScript.OperatingUserCrew.ContainsKey(_playerData.UserName))
        {
            for (int i = 0; i < _currentPatientScript.OperatingUsers.Count; i++)
            {
                if (_currentPatientScript.OperatingUsers[i] == _playerData.UserName)
                {
                    _currentPatientScript.OperatingUsers.RemoveAt(i);
                    _currentPatientScript.OperatingCrews.RemoveAt(i);
                }
            }

            _currentPatientScript.OperatingUserCrew.Remove(_playerData.UserName);
        }

        print("Leave Patient");
    }
    #endregion

    #region Player Action Events
    public void OpenNoBagActionMenu()
    {
        _actionsOperatingHandler.OpenNoBagActionMenu(_basicActionMenuParent);
    }

    public void CallAction(int actionNumInList)
    {
        if (_currentPatientInfoSo != null)
            _actionsOperatingHandler.RunAction(actionNumInList, _currentPatientScript);
    }
    #endregion
    public void OpenNoBagActionMenu(GameObject noBagActionMenu)
    {
        if (!noBagActionMenu.activeInHierarchy)
            noBagActionMenu.SetActive(true);
        else
            noBagActionMenu.SetActive(false);
    }

    public void RunAction(int actionIndex, Patient patient)
    {
        switch (actionIndex)
        {
            case 0:
                GetPainLevel(patient);
                break;
            case 1:
                DoHeartMassage(patient);
                break;
            case 2:
                Defibrillator(patient);
                break;
            default:
                break;
        }
    }

    // Pain Level
    public void GetPainLevel(Patient patient)
    {
        patient.PatientInfoSO.PainLevel = patient.PatientInfoSO.PainPlaceholderAnswer;
        Debug.Log(patient.name + "'s Pain Level: " + patient.PatientInfoSO.PainLevel);
    }

    // Heart Massage
    public void DoHeartMassage(Patient patient)
    {
        Debug.Log("Operating Heart Massage On " + patient.name);
    }

    // Defibrillator
    public void Defibrillator(Patient patient)
    {
        Debug.Log("CLEAR!!! Defibrillator On " + patient.name);
    }
    */
}
