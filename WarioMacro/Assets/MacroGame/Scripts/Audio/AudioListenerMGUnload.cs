using System;
using UnityEngine;

public class AudioListenerMGUnload : MonoBehaviour
{
    public GameObject mainCamera;

    private void OnDestroy()
    {
        mainCamera.SetActive(true);
    }
}
