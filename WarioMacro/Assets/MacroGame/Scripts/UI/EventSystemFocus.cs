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
        // Get Player input
        MoveDirection dir = InputManager.GetDirection(true);

        // Reset possibility to move
        if (dir == MoveDirection.None)
        {
            alreadyMoved = false;
            return dir;
        }

        // Don't move if done already to avoid skipping buttons
        if (alreadyMoved) return MoveDirection.None;
        
        // Return new direction
        alreadyMoved = true;
        AudioManager.MacroPlaySound("MenusHover", 0);
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
