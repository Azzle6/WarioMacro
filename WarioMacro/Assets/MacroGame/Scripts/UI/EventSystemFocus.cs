using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class EventSystemFocus : MonoBehaviour
{
    private static readonly List<EventSystemFocus> instances = new List<EventSystemFocus>();
    
    [SerializeField] public GameObject firstSelected;
    private EventSystem eventSys;
    private EventSystemFocus previouslyActive;
    private bool alreadyMoved;
    
    private void Move(MoveDirection direction)
    {
        if (direction == MoveDirection.None) return;

        GameObject currentlySelected = eventSys.currentSelectedGameObject;
        
        // Move using unity's navigation system
        var data = new AxisEventData(eventSys)
        {
            moveDir = direction,
            selectedObject = currentlySelected
        };

        ExecuteEvents.Execute(data.selectedObject, data, ExecuteEvents.moveHandler);

        // Play sound if selected button has changed
        if (currentlySelected != data.selectedObject)
        {
            AudioManager.MacroPlaySound("MenusHover", 0);
        }
    }

    private MoveDirection GetDirection()
    {
        // Get Player input
        MoveDirection dir = InputManager.GetDirection(true, true);

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
        return dir;
    }

    private void Awake()
    {
        instances.Add(this);
    }

    private void OnEnable()
    {
        eventSys = EventSystem.current;
        eventSys.SetSelectedGameObject(firstSelected);

        foreach (EventSystemFocus instance in instances.Where(instance => instance != this && instance.isActiveAndEnabled))
        {
            instance.enabled = false;
            previouslyActive = instance;
        }
    }

    private void OnDisable()
    {
        if (previouslyActive == null) return;
        
        previouslyActive.enabled = true;
        previouslyActive = null;
    }

    private void Update()
    {
        Move(GetDirection());
    }
}
