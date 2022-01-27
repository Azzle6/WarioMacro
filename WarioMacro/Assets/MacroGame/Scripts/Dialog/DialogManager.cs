using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    static public bool FindDialog(DialogConstructor dial)
    {
        var characterNode = dial.GetComponent<CharacterNode>();
        if (characterNode != null)
        {
            var dialogLines = new string[0];

            // Recruit Phase
            if (RecruitmentController.isInRecruitmentLoop)
            {
                var currentChara = characterNode.currentChara;
                if (currentChara == null)
                {
                    Debug.LogWarning("Character node " + characterNode + " has no currentChara");
                    return false;
                }
                
                var isRecruited = CharacterManager.instance.playerTeam.Contains(characterNode.currentChara);
                dialogLines = isRecruited ? currentChara.AlreadyRecruitDialog : currentChara.RecruitDialog;
            }
            
            if (dialogLines.Length > 0)
            {
                dial.dialogs = dialogLines;
                return true;
            }
        }

        return false;
    }
    
    public static DialogManager instance;
    [SerializeField] private GameObject dialogGO;
    [SerializeField] private GameObject ButtonsParent;
    [SerializeField] private TMP_Text textZone;
    [SerializeField] private DialogConstructor curDial;
    [SerializeField] private GameObject ButtonTemplate;
    [SerializeField] private Image charaSprite;
    private GameObject[] Buttons;
    private IEnumerator currentCoroutine;
    private bool isInDialog;
    private int curIndex;
    private string[] currentCharaDialog;
    public bool isCharaDialog;
    public TMP_Text nametext;

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
        
        if(FindDialog(currentDial))
            Debug.Log("New dialogs found for: " + currentDial);
        
        if (curDial.chara != null)
        {
            charaSprite.sprite = curDial.chara;
            nametext.text = curDial.name;
            charaSprite.gameObject.SetActive(true);
        }
        else
        {
            if(curDial.chara != null) charaSprite.sprite = curDial.chara;
            charaSprite.gameObject.SetActive(false);
            nametext.text = curDial.name;
        }
        
        isInDialog = true;
        dialogGO.SetActive(true);

        Buttons = SetupButtons();

        currentCoroutine = WriteNextSentence();
        StartCoroutine(currentCoroutine);
    }

    IEnumerator WriteNextSentence()
    {
        textZone.text = "";

        string[] dialogToDisplay = isCharaDialog ? currentCharaDialog : curDial.dialogs;

        if (curDial.dialogs.Length == 0)
        {
            Debug.Log("Dialogues vides !");
            yield break;
        }
        for (int i = 0; i < dialogToDisplay[curIndex].Length; i++)
        {
            textZone.text = string.Concat(textZone.text, dialogToDisplay[curIndex][i]);
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

        StartCoroutine(FinishWithTiming());
    }

    IEnumerator FinishWithTiming()
    {
        yield return new WaitForSeconds(0.4f);
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
    
    private 

    GameObject[] SetupButtons()
    {
        List<GameObject> buttons = new List<GameObject>();
        
        foreach (Response resp in curDial.Responses)
        {
            GameObject but = Instantiate(ButtonTemplate, ButtonsParent.transform);
            
            Button butComponent = but.GetComponent<Button>();
            but.GetComponentInChildren<TMP_Text>().text = resp.ButtonResponse;
            UnityEvent butCurEvent = butComponent.onClick;
            butComponent.onClick = resp.ButtonEvent;
            butComponent.onClick.AddListener(FinishDialog);
            butComponent.onClick.AddListener(butCurEvent.Invoke);

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
