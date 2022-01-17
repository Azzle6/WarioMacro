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

    private MoveDirection GetDirection(float verticalAxis, float horizontalAxis)
    {
        
        if (!alreadyMoved)
        {
            alreadyMoved = true;
            if (verticalAxis < -0.2f)
                return MoveDirection.Down;
            if (verticalAxis > 0.2f)
                return MoveDirection.Up;
            if (horizontalAxis < -0.2f)
                return MoveDirection.Left;
            if (horizontalAxis > 0.2f)
                return MoveDirection.Right;
            alreadyMoved = false;
            return MoveDirection.None;
        }
        
        if (verticalAxis > -0.2f && verticalAxis < 0.2f && horizontalAxis > -0.2f 
            && horizontalAxis < 0.2f)
        {
            alreadyMoved = false;
        }
        
        return MoveDirection.None;
    }

    private void OnEnable()
    {
        eventSys = EventSystem.current;
        eventSys.SetSelectedGameObject(firstSelected);
    }
 
    private void Update()
    {
        float verticalAxis = InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL);
        float horizontalAxis = InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL);

        Move(GetDirection(verticalAxis, horizontalAxis));
    }
}
