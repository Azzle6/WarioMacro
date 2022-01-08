using UnityEngine;
using UnityEngine.Playables;

public class Timer : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    public void StartTimer()
    {
        director.Play();
    }

    public void PauseTimer()
    {
        director.Pause();
    }
    
    public void UnpauseTimer()
    {
        director.Resume();
    }

    public void StopTimer()
    {
        director.Stop();
    }
}
