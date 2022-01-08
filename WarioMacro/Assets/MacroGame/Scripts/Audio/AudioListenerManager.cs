using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private AudioListener mainAudioListener;
    private AudioListener[] audioListeners;
    private int other;

    private void Update()
    {
        audioListeners = FindObjectsOfType<AudioListener>();

        if (audioListeners.Length <= 1) return;
        
        mainCamera.SetActive(false);
        other = audioListeners[0] == mainAudioListener ? 1 : 0;
        audioListeners[other].gameObject.AddComponent<AudioListenerMGUnload>();
        audioListeners[other].gameObject.GetComponent<AudioListenerMGUnload>().mainCamera = mainCamera;
    }
}
