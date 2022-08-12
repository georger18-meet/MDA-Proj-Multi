using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class EranCrew : MonoBehaviour
{

    public Canvas RoomEranMenuUI;
    //public GameObject DropDown;

    public List<TMP_Dropdown> CrewMemberDropDownList;



    void Start()
    {
        
    }


    void Update()
    {
        // PopulateDropdownRoles();
        if (Input.GetKey(KeyCode.L))
        {
            Debug.Log("ActionsManager.Instance.AllPlayersPhotonViews   " + ActionsManager.Instance.AllPlayersPhotonViews.Count);
            PopulateDropdownRoles();
            Debug.Log("PhotonNetwork.PlayerList   " + PhotonNetwork.PlayerList.Length);

        }

    }


    private void PopulateDropdownRoles()
    {

        List<string> value = new List<string>();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            value.Add(PhotonNetwork.PlayerList[i].NickName); 
        }


        foreach (var dropdown in CrewMemberDropDownList)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(value);
        }





        //var menuIndex = DropDown.GetComponentInChildren<TMP_Dropdown>().value;
        //List<TMP_Dropdown.OptionData> menuOptions = DropDown.GetComponentInChildren<TMP_Dropdown>().options;
        //string value = menuOptions[menuIndex].text;
        //string value;



        //for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        //{
        //   value[i] = ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName;

        //}


    }

    public void ShowEranRoomMenu()
    {
        RoomEranMenuUI.gameObject.SetActive(true);
    }

    public void CloseEranRoomMenu()
    {
        RoomEranMenuUI.gameObject.SetActive(false);
    }
}
