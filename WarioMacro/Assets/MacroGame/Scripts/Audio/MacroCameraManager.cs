using UnityEngine;

// ReSharper disable once CheckNamespace
public class MacroCameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 backgroundScale;
    [SerializeField] private AudioListener mainAudioListener;
    private AudioListener[] audioListeners;
    private float cameraHeight;
    private float cameraWidth;
    private int other;

    private void FollowPlayer()
    {
        Vector3 position = player.transform.position;
        mainCam.transform.position = new Vector3(
            Mathf.Clamp(
                position.x,
                -(backgroundScale.x/2 - cameraWidth/2),
                (backgroundScale.x/2 - cameraWidth/2)
            ), 
            Mathf.Clamp(
                position.y,
                -(backgroundScale.y/2 - cameraHeight/2),
                (backgroundScale.y/2 - cameraHeight/2)
            ), 
            -10);
    }

    private void Start()
    {
        cameraHeight = 2f * mainCam.orthographicSize;
        cameraWidth = cameraHeight * mainCam.aspect;
    }

    private void Update()
    {
        // Camera movement
        FollowPlayer();
        
        // Deactivate camera if another audio listener exists
        audioListeners = FindObjectsOfType<AudioListener>();

        if (audioListeners.Length <= 1) return;
        
        gameObject.SetActive(false);
        other = audioListeners[0] == mainAudioListener ? 1 : 0;
        audioListeners[other].gameObject.AddComponent<AudioListenerMGUnload>();
        audioListeners[other].gameObject.GetComponent<AudioListenerMGUnload>().mainCamera = gameObject;
    }
}
