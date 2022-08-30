using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Henyon10 : MonoBehaviour
{



    private bool _isHenyon10MenuOpen;
    public Button TopMenuHandle,ShowCarsMenu;
    public GameObject Henyon10Menu;

    void Start()
    {
        UIManager.Instance.TeamLeaderMenu.SetActive(false);
        UIManager.Instance.Henyon10Menu.SetActive(true);


        ShowCarsMenu = UIManager.Instance.Henyon10CarsMenu;
        Henyon10Menu = UIManager.Instance.Henyon10Menu;
        TopMenuHandle = UIManager.Instance.Henyon10MenuHandle;
        TopMenuHandle.onClick.RemoveAllListeners();
        TopMenuHandle.onClick.AddListener(delegate { OpenCloseHenyon10Menu(); });
    }

    void Update()
    {
        
    }


    public void OpenCloseHenyon10Menu()
    {
        if (!_isHenyon10MenuOpen)
        {
            UIManager.Instance.OpenCloseTopMenu("Henyon10");
            _isHenyon10MenuOpen = true;
        }
        else
        {
            UIManager.Instance.OpenCloseTopMenu("Henyon10");
            _isHenyon10MenuOpen = false;
        }

        TopMenuHandle.onClick.RemoveAllListeners();
        TopMenuHandle.onClick.AddListener(delegate { OpenCloseHenyon10Menu(); });
    }
}
