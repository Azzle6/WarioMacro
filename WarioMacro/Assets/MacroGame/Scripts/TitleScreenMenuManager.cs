using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreenMenuManager : MonoBehaviour
{
    private GameObject previousSelectedGO;
    private GameObject currentSelectedGO;
    
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        currentSelectedGO = EventSystem.current.currentSelectedGameObject;
        if (currentSelectedGO == null)
        {
            EventSystem.current.SetSelectedGameObject(previousSelectedGO);
        }
        else
        {
            previousSelectedGO = currentSelectedGO;
        }
    }
}
