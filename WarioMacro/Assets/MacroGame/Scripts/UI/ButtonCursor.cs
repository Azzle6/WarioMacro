using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCursor : MonoBehaviour, ISelectHandler
{
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Vector2 position;

    public void OnSelect(BaseEventData eventData)
    {
        cursor.anchoredPosition = position;
    }
}
