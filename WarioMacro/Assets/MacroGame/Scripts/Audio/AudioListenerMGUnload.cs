using System;
using UnityEngine;

public class AudioListenerMGUnload : MonoBehaviour
{
    public GameObject mainCamera;

    private void OnDestroy()
    {
        Debug.Log("destroyed");
        mainCamera.SetActive(true);
    }
}
