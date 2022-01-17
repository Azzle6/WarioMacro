using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    public static bool GameIsPaused;
    
    private void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            DisplayMenu();
        }
    }

    public void DisplayMenu()
    {
        GameIsPaused = !GameIsPaused;
        menu.SetActive(GameIsPaused);
        menu.transform.GetChild(0).gameObject.SetActive(true);
        menu.transform.GetChild(1).gameObject.SetActive(false);
        float lastTimeScale = 1;
        if (GameIsPaused)
        {
             lastTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else Time.timeScale = lastTimeScale;
    }

    public void SwitchScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
