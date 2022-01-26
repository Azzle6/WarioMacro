using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private GameObject transitionGO;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject timerGO;
    private bool startAsyncOp;

    // ReSharper disable once MemberCanBePrivate.Global
    public void TransitionStart()
    {
        director.time = 0;
        director.Play();
    }
    
    // ReSharper disable once MemberCanBePrivate.Global
    public void TransitionResume()
    {
        director.Resume();
    }

    // Used in Transition's signal receiver
    [UsedImplicitly]
    public void TransitionPause()
    {
        director.Pause();
    }

    // Used in Transition's signal receiver
    [UsedImplicitly]
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

        AsyncOperation asyncOp = toLoad
            ? SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)
            : SceneManager.UnloadSceneAsync(sceneName);
        
        OnAsyncOpCompleted(asyncOp, sceneName, toLoad);

        while (!asyncOp.isDone) yield return null;

        Ticker.lockTimescale = true;
        // resume transition UI
        TransitionResume();

        while (director.state == PlayState.Playing) yield return null;
        
        transitionGO.SetActive(false);
        Ticker.lockTimescale = false;
    }

    private void OnAsyncOpCompleted(AsyncOperation asyncOp, string sceneName, bool toLoad)
    {
        asyncOp.completed += operation =>
        {
            if (toLoad)
            {
                GameController.instance.ShowMacroObjects(false);
                timerGO.SetActive(true);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            }
            else
            {
                GameController.instance.ShowMacroObjects(true);
                timerGO.SetActive(false);
                AudioManager.StopAllMicroSounds();
            }
        };
    }
}
