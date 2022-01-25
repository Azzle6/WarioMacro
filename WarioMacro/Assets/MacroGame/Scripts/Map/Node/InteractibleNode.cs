using UnityEngine;
using UnityEngine.Events;

public class InteractibleNode : MonoBehaviour
{
    public UnityEvent EventInteractible;

    public void StartDialog(DialogConstructor dial)
    {
        DialogManager.instance.StartDialog(dial);
    }

    public void NextMap()
    {
        GameController.instance.NextMap();
        GameController.instance.InteractiveEventEnd();
    }

    public void EndGame()
    {
        StartCoroutine(GameController.instance.ToggleEndGame(true));
        GameController.instance.InteractiveEventEnd();
    }
}
