using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleNode : MonoBehaviour
{
    public UnityEvent EventInteractible;

    public void StartDialog(DialogConstructor dial)
    {
        GameController.isInActionEvent = true;
        DialogManager.instance.StartDialog(dial);
    }

    public void NextMap()
    {
        GameController.instance.WantToContinue = true;
        GameController.instance.InteractiveEventEnd();
    }

    public void EndGame()
    {
        StartCoroutine(GameController.instance.ToggleEndGame(true));
        //NotDestroyedScript.instance.EndRun(true);
        //GameController.instance.InteractiveEventEnd();
    }
}
