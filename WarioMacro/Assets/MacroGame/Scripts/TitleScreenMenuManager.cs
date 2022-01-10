using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
public class TitleScreenMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
