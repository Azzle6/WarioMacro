using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutOfJailManager : MonoBehaviour
{
    
    public OutOfJailElement[] jailedUI = new OutOfJailElement[12];
    public GameObject[] rows = new GameObject[6];
    public List<GameObject> activeRows = new List<GameObject>() ;
    public EventSystemFocus eventSystemFocus;
    public ScrollRect scrollRect;
    public bool isOpen;
    public GameObject jailPanel;
    private GameObject go;
    public int currentRowId = 0;
    public int lastRowId = 1;    
    
    
    private void Update()
    {
        if (!isOpen) return;
        GameObject currentGo = EventSystem.current.currentSelectedGameObject;

        if (currentGo == null) 
            return;
        if (activeRows[currentRowId] != currentGo.transform.parent.gameObject)
        {
            lastRowId = currentRowId;
            for(int i = 0;i<activeRows.Count;i++)
            {
                
                if (activeRows[i] == currentGo.transform.parent.gameObject)
                    currentRowId = i;
            }
        }

        switch (currentRowId+lastRowId)
        {
            case 1:
                scrollRect.verticalNormalizedPosition = 1;
                break;
            case 3:
                scrollRect.verticalNormalizedPosition = 0.75f;
                break;
            case 5:
                scrollRect.verticalNormalizedPosition = 0.5f;
                break;
            case 7:
                scrollRect.verticalNormalizedPosition = 0.25f;
                break;
            case 9:
                scrollRect.verticalNormalizedPosition = 0;
                break;
            default:
                scrollRect.verticalNormalizedPosition = 1;
                break;
        }
        
        
        if(InputManager.GetKeyDown(ControllerKey.B, true) && isOpen) CloseJail();

    }
    public void SetJailed()
    {
        foreach (var go in rows) go.SetActive(true);
        var count = 0;
        foreach (var element in jailedUI)
        {
            if(CharacterManager.instance.imprisonedCharacters.Any(i =>i.character == element.character))
            {
                element.gameObject.SetActive(true);
                Debug.Log("obj active");
                element.SetJail(CharacterManager.instance.imprisonedCharacters.First(i =>i.character == element.character));
            }
            else
            {
                element.gameObject.SetActive(false);
            }
        }
        SetRowsUI();
        Debug.Log("obj uwu");
        if(jailedUI.Any(c => c.gameObject.activeSelf))
        {
            Debug.Log("selected");
            EventSystem.current.SetSelectedGameObject(jailedUI.First(c => c.gameObject.activeSelf).gameObject);
        }
    }

    public void SetRowsUI()
    {
        foreach (var go in rows)
        {
            if (go.transform.GetChild(0).gameObject.activeSelf == false &&
                go.transform.GetChild(1).gameObject.activeSelf == false)
            {
                go.SetActive(false);
            }
            else
            {
                activeRows.Add(go);
            }
        }
    }
    
    
    public void OpenJail()
    {
        jailPanel.SetActive(true);
        isOpen = true;
        InputManager.lockInput = true;
        SetJailed();
    }
    
    public void CloseJail()
    {
        jailPanel.SetActive(false);
        isOpen = false;
        activeRows.Clear();
        InputManager.lockInput = false;
        GameController.OnInteractionEnd();
        AudioManager.MacroPlayRandomSound("BarmanExit");
    }
}
