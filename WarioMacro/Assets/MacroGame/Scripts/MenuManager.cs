using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject defaultMenu;
    [SerializeField] private GameObject settingsMenu;
    private List<MonoBehaviour> mgScripts = new List<MonoBehaviour>();
    public static bool gameIsPaused;
    
    private void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            DisplayMenu();
        }
    }

    public void DisplayMenu()
    {
        gameIsPaused = !gameIsPaused;
        menu.SetActive(gameIsPaused);
        AudioManager.MacroPlaySound(gameIsPaused ? "PauseIn" : "PauseOut", 0);

        menu.transform.GetChild(0).gameObject.SetActive(true);
        menu.transform.GetChild(1).gameObject.SetActive(false);

        StartCoroutine(RestoreTimeScale(gameIsPaused));
        /*
        if (gameIsPaused)
        {
            LockMGScripts();
        }
        else
        {
            UnlockMGScripts();
        }
        */
    }

    public void DisplaySettings()
    {
        defaultMenu.SetActive(false);
        settingsMenu.SetActive(true);
        AudioManager.MacroPlaySound("MenusValidation");
    }

    public void DisposeSettings()
    {
        settingsMenu.SetActive(false);
        defaultMenu.SetActive(true);
        AudioManager.MacroPlaySound("MenusBack");
    }
    
    public void SwitchScene(string sceneName)
    {
        Ticker.lockTimescale = false;
        InputManager.lockInput = false;
        gameIsPaused = false;
        GameController.OnInteractionEnd();
        SceneManager.LoadScene(sceneName);
    }

    /*
    private void UnlockMGScripts()
    {
        for (int i = mgScripts.Count - 1; i >= 0; i--)
        {
            mgScripts[i].enabled = true;
            mgScripts.RemoveAt(i);
        }
    }

    private void LockMGScripts()
    {
        mgScripts = new List<MonoBehaviour>();
        foreach (MonoBehaviour monoBehaviour in FindObjectsOfType<MonoBehaviour>())
        {
            Scene otherScene = monoBehaviour.gameObject.scene;
            if (otherScene == gameObject.scene || otherScene.buildIndex == -1) continue;
            if (!monoBehaviour.isActiveAndEnabled) continue;
            
            monoBehaviour.enabled = false;
            mgScripts.Add(monoBehaviour);
        }
    }
    */

    private static IEnumerator RestoreTimeScale(bool lockTimeScale)
    {
        yield return new WaitForEndOfFrame();
        Ticker.lockTimescale = lockTimeScale;
        
    }
}
