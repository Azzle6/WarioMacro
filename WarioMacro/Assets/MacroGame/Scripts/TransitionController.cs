using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject timerGO;
    private bool startAsyncOp;

    // ReSharper disable once MemberCanBePrivate.Global
    public void TransitionStart()
    {
        director.Play();
    }
    
    // ReSharper disable once MemberCanBePrivate.Global
    public void TransitionResume()
    {
        director.Resume();
    }

    // Used in Transition's signal receiver
    public void TransitionPause()
    {
        director.Pause();
    }

    // Used in Transition's signal receiver
    public void StartAsyncMGOp(bool toggle)
    {
        startAsyncOp = toggle;
    }
    
    public IEnumerator TransitionHandler(string sceneName, bool toLoad)
    {
        // start transition UI
        TransitionStart();
        startAsyncOp = false; 
        while (!startAsyncOp) yield return null;

        AsyncOperation asyncOp;
        if (toLoad)
        {
            GameController.instance.ShowMacroObjects(false);
            timerGO.SetActive(true);
            asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            GameController.instance.ShowMacroObjects(true);
            timerGO.SetActive(false);
            asyncOp = SceneManager.UnloadSceneAsync(sceneName);
        }

        while (!asyncOp.isDone) yield return null;

        GameController.lockTimescale = true;
        // resume transition UI
        TransitionResume();

        while (director.state == PlayState.Playing) yield return null;
        
        GameController.lockTimescale = false;
    }

    private void Awake()
    {
        GameController.Register();
    }
}
