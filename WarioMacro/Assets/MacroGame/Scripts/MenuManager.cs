using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
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

        Ticker.lockTimescale = gameIsPaused;
    }

    public void SwitchScene(string sceneName)
    {
        Ticker.lockTimescale = false;
        gameIsPaused = false;
        SceneManager.LoadScene(sceneName);
    }
}
