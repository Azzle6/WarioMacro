using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    [SerializeField]private GameObject dialogGO;
    [SerializeField] private GameObject ButtonsParent;
    [SerializeField] private TMP_Text textZone;
    private int curIndex;
    [SerializeField]private DialogConstructor curDial;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject finishButton;
    [SerializeField] private GameObject ButtonTemplate;
    private bool isInDialog;
    private GameObject[] Buttons;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        StartDialog(curDial);
    }

    public void StartDialog(DialogConstructor currentDial)
    {
        if (isInDialog)
        {
            Debug.Log("player is already in a dialog.");
            return;
        }

        curDial = currentDial;
        isInDialog = true;
        dialogGO.SetActive(true);
        finishButton.SetActive(false);
        continueButton.SetActive(true);
        
        
        Buttons = SetupButtons();
        
        StartCoroutine(WriteNextSentence());
    }

    IEnumerator WriteNextSentence()
    {
        textZone.text = "";
        
        
        for (int i = 0; i < curDial.dialogs[curIndex].Length; i++)
        {
            textZone.text = string.Concat(textZone.text, curDial.dialogs[curIndex][i]);
            yield return new WaitForSeconds(0.1f);
        }
        
        if(curIndex < curDial.dialogs.Length - 1) curIndex++;
        else
        {
            LastDialog();
            yield return new WaitUntil(() => !MenuManager.gameIsPaused && InputManager.GetKeyDown(ControllerKey.A));
            FinishDialog();
            yield break;
        }

        
        yield return new WaitUntil(() => !MenuManager.gameIsPaused && InputManager.GetKeyDown(ControllerKey.A));
        
        StartCoroutine(WriteNextSentence());
    }

    void LastDialog()
    {
        continueButton.SetActive(false);
        finishButton.SetActive(true);
        ButtonsParent.SetActive(true);
    }

    void FinishDialog()
    {
        dialogGO.SetActive(false);
        curIndex = 0;
        isInDialog = false;
        Ticker.lockTimescale = false;
    }

    GameObject[] SetupButtons()
    {
        List<GameObject> buttons = new List<GameObject>();
        foreach (Response resp in curDial.Responses)
        {
            //GameObject but = ButtonTemplate;
            
            GameObject but = Instantiate(ButtonTemplate, ButtonsParent.transform);
            
            Button butComponent = but.GetComponent<Button>();
            butComponent.onClick = resp.ButtonEvent;
            butComponent.onClick.AddListener(delegate { FinishDialog(); });
            
            
            buttons.Add(but);
            
        }

        if (curDial.Responses.Length == 0)
        {
            GameObject but = Instantiate(ButtonTemplate, ButtonsParent.transform);
            Button butComponent = but.GetComponent<Button>();
            butComponent.onClick.AddListener(delegate { FinishDialog(); });
            buttons.Add(but);
            ButtonsParent.SetActive(false);
            return buttons.ToArray();
        }
        
        ButtonsParent.SetActive(false);
        ButtonsParent.GetComponent<EventSystemFocus>().firstSelected = buttons[0];
        return buttons.ToArray();
    }
    
}
