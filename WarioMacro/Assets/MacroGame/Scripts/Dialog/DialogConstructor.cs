using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogConstructor : MonoBehaviour
{
    public Sprite chara;
    public string name;
    [Space]
    public string[] dialogs;

    public Response[] Responses;
    public bool InteractionEndWhenDialogEnd = true;
}

[System.Serializable]
public class Response
{
    public string ButtonResponse;
    public Button.ButtonClickedEvent ButtonEvent;
}


