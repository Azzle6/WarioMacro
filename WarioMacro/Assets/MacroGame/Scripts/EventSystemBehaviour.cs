using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class EventSystemBehaviour : MonoBehaviour
{
    private GameObject previousSelectedGO;
    private GameObject currentSelectedGO;

    private void Update()
    {
        if (EventSystem.current == null) return;
        
        currentSelectedGO = EventSystem.current.currentSelectedGameObject;
        if (currentSelectedGO == null)
        {
            EventSystem.current.SetSelectedGameObject(previousSelectedGO);
        }
        else
        {
            previousSelectedGO = currentSelectedGO;
        }

    }
}
