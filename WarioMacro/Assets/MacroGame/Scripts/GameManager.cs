using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{

    public static void Register()
    {
        AudioManager.Register();
        InputManager.Register();
        GameController.Register();
    }

}
