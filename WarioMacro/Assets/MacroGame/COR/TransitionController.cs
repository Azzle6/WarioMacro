using UnityEngine;
using UnityEngine.Playables;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    public bool isReady;
    public bool isDone => director.time == director.duration;

    public void TransitionStart()
    {
        director.Play();
    }

    public void TransitionPause()
    {
        director.Pause();
    }

    public void TransitionResume()
    {
        director.Resume();
    }
    private void Update()
    {
        
    }
}
