using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
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

    private IEnumerator RestoreTimeScale(bool lockTimeScale)
    {
        yield return new WaitForEndOfFrame();
        Ticker.lockTimescale = lockTimeScale;
        
    }

    public void SwitchScene(string sceneName)
    {
        Ticker.lockTimescale = false;
        gameIsPaused = false;
        SceneManager.LoadScene(sceneName);
    }
}
