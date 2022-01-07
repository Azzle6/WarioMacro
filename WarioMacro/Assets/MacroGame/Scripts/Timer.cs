using System;
using UnityEngine;
using UnityEngine.Playables;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject timerGO;
    private PlayableDirector director;

    private void Awake()
    {
        director = timerGO.GetComponent<PlayableDirector>();
    }

    public void StartTimer()
    {
        timerGO.SetActive(true);
        director.Play();
    }

    public void StopTimer()
    {
        director.Stop();
        timerGO.SetActive(false);
    }
}
