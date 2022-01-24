using System;
using TMPro;
using UnityEngine;

public class NodePrevisualisation : MonoBehaviour
{
    public bool onScreen { get; private set; }
    
    [SerializeField] private Vector3 showPos;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private TextMeshProUGUI[] texts;
    
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
        onScreen = !onScreen;
        ((RectTransform) transform).localPosition = onScreen ? showPos : hidePos;
        InputManager.lockInput = onScreen;
    }

    private void Start()
    {
        Show();
    }

    private void Update()
    {
        float input = InputManager.GetAxis(ControllerAxis.RIGHT_TRIGGER, true);

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
