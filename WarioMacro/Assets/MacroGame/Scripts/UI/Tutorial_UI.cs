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

    // Start is called before the first frame update
    void Start()
    {
        InitTutorial();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) PreviousPage();
        if (Input.GetKeyDown(KeyCode.RightArrow)) NextPage();

        if (Input.GetKeyDown(KeyCode.Space)) ToggleWindow(false);
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

        currentIndex = 0;
        ToggleContent(currentIndex, true);
    }
}
