using System;
using TMPro;
using UnityEngine;

public class NodePrevisualisation : MonoBehaviour
{
    [SerializeField] private Vector3 showPos;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private TextMeshProUGUI[] texts;

    private bool onScreen = true;
    private bool buttonPressed;

    public void SetTexts(float[] percentages)
    {
        if (percentages.Length != texts.Length)
        {
            Debug.LogError("Float array has a different size than text array.");
        }

        for (int i = 0; i < percentages.Length; i++)
        {
            texts[i].text = percentages[i] + "%";
        }
    }
    
    public void Show()
    {
        ((RectTransform) transform).localPosition = onScreen ? hidePos : showPos;
        onScreen = !onScreen;
    }

    private void Update()
    {
        float input = InputManager.GetAxis(ControllerAxis.RIGHT_TRIGGER);

        if (input < 0.5f)
        {
            buttonPressed = false;
            return;
        }

        if (buttonPressed) return;
        
        buttonPressed = true;
        Show();
    }
}
