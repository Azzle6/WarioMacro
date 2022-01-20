using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC1_PlayerInput : MonoBehaviour
{

    [SerializeField] private NAC1_MiniGameController miniGameController;

    void Update()
    {
        if (miniGameController.HasStarted() && !miniGameController.GetHasEnded())
        {
            if (InputManager.GetKeyDown(ControllerKey.A))
            {
                miniGameController.CheckInputsOrder("A");
            }
            if (InputManager.GetKeyDown(ControllerKey.B))
            {
                miniGameController.CheckInputsOrder("B");
            }
            if (InputManager.GetKeyDown(ControllerKey.X))
            {
                miniGameController.CheckInputsOrder("X");
            }
            if (InputManager.GetKeyDown(ControllerKey.Y))
            {
                miniGameController.CheckInputsOrder("Y");
            }
        }
    }
}
