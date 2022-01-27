using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    [SerializeField] private GameObject dialogGO;
    [SerializeField] private GameObject ButtonsParent;
    [SerializeField] private TMP_Text textZone;
    [SerializeField] private DialogConstructor curDial;
    [SerializeField] private GameObject ButtonTemplate;
    private GameObject[] Buttons;
    private IEnumerator currentCoroutine;
    private bool isInDialog;
    private int curIndex;

    private void Awake()
    {
        instance = this;
        
    }

    public void StartDialog(DialogConstructor currentDial)
    {
        if (instance.isInDialog)
        {
            Debug.Log("player is already in a dialog.");
            return;
        }

        curDial = currentDial;
        isInDialog = true;
        dialogGO.SetActive(true);

        Buttons = SetupButtons();

        currentCoroutine = WriteNextSentence();
        StartCoroutine(currentCoroutine);
    }

    IEnumerator WriteNextSentence()
    {
        textZone.text = "";

        if (curDial.dialogs.Length == 0)
        {
            Debug.Log("Dialogues vides !");
            yield break;
        }
        for (int i = 0; i < curDial.dialogs[curIndex].Length; i++)
        {
            textZone.text = string.Concat(textZone.text, curDial.dialogs[curIndex][i]);
            yield return new WaitForSeconds(0.02f);
        }
        
        if(curIndex < curDial.dialogs.Length - 1) curIndex++;
        else
        {
            LastDialog();
            while (!InputManager.GetKeyDown(ControllerKey.A, true) && (!curDial.canBeCanceled || !InputManager.GetKeyDown(ControllerKey.B, true)))
            {
                yield return null;
            }
            //yield return new WaitUntil(() => InputManager.GetKeyDown(ControllerKey.A, true));
            Debug.Log("QUIT");
            FinishDialog();
            yield break;
        }

        yield return new WaitUntil(() => InputManager.GetKeyDown(ControllerKey.A, true));

        currentCoroutine = WriteNextSentence();
        StartCoroutine(currentCoroutine);
    }

    private void LastDialog()
    {
        ButtonsParent.SetActive(true);
    }

    private void FinishDialog()
    {
        StopCoroutine(currentCoroutine);
        currentCoroutine = null;
        dialogGO.SetActive(false);
        curIndex = 0;
        isInDialog = false;
        Ticker.lockTimescale = false;
        if(curDial.InteractionEndWhenDialogEnd) GameController.OnInteractionEnd();
        foreach (GameObject but in Buttons)
        {
            but.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(but);
        }

    }

    GameObject[] SetupButtons()
    {
        List<GameObject> buttons = new List<GameObject>();
        
        foreach (Response resp in curDial.Responses)
        {
            GameObject but = Instantiate(ButtonTemplate, ButtonsParent.transform);
            
            Button butComponent = but.GetComponent<Button>();
            but.GetComponentInChildren<TMP_Text>().text = resp.ButtonResponse;
            butComponent.onClick = resp.ButtonEvent;
            butComponent.onClick.AddListener(FinishDialog);

            buttons.Add(but);
            
        }

        if (curDial.Responses.Length == 0)
        {
            GameObject but = Instantiate(ButtonTemplate, ButtonsParent.transform);
            Button butComponent = but.GetComponent<Button>();
            but.GetComponentInChildren<TMP_Text>().text = "Quit";
            butComponent.onClick.AddListener(FinishDialog);
            buttons.Add(but);
            //ButtonsParent.SetActive(false);
            //return buttons.ToArray();
        }
        
        ButtonsParent.SetActive(false);
        ButtonsParent.GetComponent<EventSystemFocus>().firstSelected = buttons[0];
        return buttons.ToArray();
    }
    
}
