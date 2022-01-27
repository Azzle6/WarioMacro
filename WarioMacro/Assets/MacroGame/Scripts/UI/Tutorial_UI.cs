using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_UI : MonoBehaviour
{

    [SerializeField] RectTransform allPannels;
    [SerializeField] RectTransform[] content;
    [SerializeField] int index = 0;

    private int currentIndex = 0;

    

    private void Update()
    {
        if (InputManager.GetKeyDown(ControllerKey.LB, true)) PreviousPage();
        if (InputManager.GetKeyDown(ControllerKey.RB, true)) NextPage();

        if (InputManager.GetKeyDown(ControllerKey.B, true)) ToggleWindow(false);
    }

    public void PreviousPage()
    {
        if (currentIndex == 0) return;

        ToggleContent(currentIndex, false);
        currentIndex--;
        ToggleContent(currentIndex, true);
    }

    public void NextPage()
    {
        if (currentIndex == index-1) return;
        ToggleContent(currentIndex, false);
        currentIndex++;
        ToggleContent(currentIndex, true);
    }

    public void ToggleWindow(bool toggle)
    {
        allPannels.gameObject.SetActive(toggle);
        if (!toggle) GameController.OnInteractionEnd();
    }

    public void ToggleContent(int contentIndex, bool toggle)
    {
        content[contentIndex].gameObject.SetActive(toggle);
    }

    public void InitTutorial()
    {
        if (index == 0)
        {
            index = content.Length;
        }
        ToggleWindow(true);

        currentIndex = 0;
        ToggleContent(currentIndex, true);
    }
}
