using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    [SerializeField]private GameObject dialogGO;
    [SerializeField] private TMP_Text textZone;
    private int curIndex;
    [SerializeField]private DialogSO curDial;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject finishButton;
    private bool isInDialog;

    private void Awake()
    {
        instance = this;
    }

    public void StartDialog(DialogSO currentDial)
    {
        if (isInDialog)
        {
            Debug.Log("player is already in a dialog.");
            return;
        }

        Ticker.lockTimescale = true;
        isInDialog = true;
        dialogGO.SetActive(true);
        finishButton.SetActive(false);
        continueButton.SetActive(true);
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
    }

    void FinishDialog()
    {
        dialogGO.SetActive(false);
        curIndex = 0;
        isInDialog = false;
        Ticker.lockTimescale = false;
    }
    
}
