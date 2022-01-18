using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class EventSystemFocus : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;
    private EventSystem eventSys;
    private bool alreadyMoved;

    private void Move(MoveDirection direction)
    {
        var data = new AxisEventData(eventSys)
        {
            moveDir = direction,
            selectedObject = eventSys.currentSelectedGameObject
        };

        ExecuteEvents.Execute(data.selectedObject, data, ExecuteEvents.moveHandler);
    }

    private MoveDirection GetDirection()
    {
        MoveDirection dir = InputManager.GetDirection();

        if (dir == MoveDirection.None)
        {
            alreadyMoved = false;
            return dir;
        }

        if (alreadyMoved) return MoveDirection.None;
        
        alreadyMoved = true;
        return dir;
    }

    private void OnEnable()
    {
        eventSys = EventSystem.current;
        eventSys.SetSelectedGameObject(firstSelected);
    }
 
    private void Update()
    {
        Move(GetDirection());
    }
}
