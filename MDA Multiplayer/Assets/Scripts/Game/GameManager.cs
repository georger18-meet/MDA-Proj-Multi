using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public GameObject GameMenuCanvas;
    public bool GameMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        OnEscape(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //PauseToggle();
        }
    }

    //public void PauseToggle()
    //{
    //    if (!GameMenuOpen)
    //    {
    //        OnEscape(true);
    //    }
    //    else if (GameMenuOpen)
    //    {
    //        OnEscape(false);
    //    }
    //}

    private void OnEscape(bool paused)
    {
        ChangeCursorMode(paused);
        //GameMenuMode(paused);
    }

    //private void GameMenuMode(bool mode)
    //{
    //    if (mode)
    //    {
    //        GameMenuCanvas.gameObject.SetActive(true);
    //        GameMenuOpen = true;
    //    }
    //    else
    //    {
    //        GameMenuCanvas.gameObject.SetActive(false);
    //        GameMenuOpen = false;
    //    }
    //}

    private void ChangeCursorMode(bool unlocked)
    {
        if (unlocked)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
        //#if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //         Application.Quit();
        //#endif
    }
}
