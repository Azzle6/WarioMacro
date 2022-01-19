using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using DG.Tweening;

public class MiniGameResultPannel_UI : MonoBehaviour
{
    public enum HeaderType
    {
        GetReady,
        Success,
        Failure
    }

    [SerializeField] private RectTransform pannel;
    [SerializeField] private Animator animator;

    [Header("Nodes Group")]
    [SerializeField] GameObject bigNodePrefab;
    [SerializeField] GameObject littleNodePrefab;
    [SerializeField] HorizontalLayoutGroup pannelLayoutGroup;

    [Header("TextFields")]
    [SerializeField] private string successKeyword;
    [SerializeField] private string failureKeyword;
    [SerializeField] private string readyKeyword;
    [SerializeField] private TMP_Text whiteTextField;
    [SerializeField] private TMP_Text blueOutlineTextField;
    [SerializeField] private TMP_Text pinkOutlineTextField;

    [Header("ChekMarks")]
    [SerializeField] private Sprite successCheckMark;
    [SerializeField] private Sprite failureCheckMark;

    [Header("MoneyBag")]
    [SerializeField] private GameObject moneyBagGO;
    [SerializeField] private TMP_Text moneyTextField; 
    [SerializeField] private PlayableDirector director;

    public GameObject[] nodeArray = new GameObject[10];

    private void Start()
    {
        ClearAllNodes();
        ToggleWindow(false);
        SetStartingNodeNumber(4);
        //SetCurrentNode(true, 1);
    }

    public void ClearAllNodes()
    {

        for(int i = 0; i < pannelLayoutGroup.transform.childCount; i++)
        {
            Destroy(pannelLayoutGroup.transform.GetChild(i).gameObject);
        }
    }
    
    //Note:Fonctionne mal avec plus de 6 nodes
    public void SetStartingNodeNumber(int nodeNbr)
    {
        nodeArray = new GameObject[nodeNbr];

        if (nodeNbr < 5 )
        {
            //Spawn X big node
            pannelLayoutGroup.spacing = 0f;
            for (int i = 0; i < nodeNbr; i++)
            {
                nodeArray[i] = Instantiate(bigNodePrefab, pannelLayoutGroup.transform);
            }
        }
        else
        {
            //Spawn X little nodes
            pannelLayoutGroup.spacing = (-12*nodeNbr);
            for (int i = 0; i < nodeNbr; i++)
            {
                nodeArray[i] = Instantiate(littleNodePrefab, pannelLayoutGroup.transform);
            }
        }
    }

    public void SetCurrentNode(bool result, int currentNodeNbr)
    {
        Image checkMarkImage = nodeArray[currentNodeNbr - 1].transform.GetChild(0).GetComponent<Image>(); //Get the child Image Component of the current node
        if (result) checkMarkImage.sprite = successCheckMark; 
        else checkMarkImage.sprite = failureCheckMark;  

        checkMarkImage.enabled = true;

        Image remanentImage = nodeArray[currentNodeNbr - 1].transform.GetChild(1).GetComponent<Image>(); //Repeat For the Remanent Image
        if (result) remanentImage.sprite = successCheckMark;
        else remanentImage.sprite = failureCheckMark;

        remanentImage.enabled = true;

        nodeArray[currentNodeNbr - 1].gameObject.GetComponent<Animator>().SetTrigger("Anim"); //Get the anim, start the animation
    }

    public void SetHeaderText(bool result)
    {
        if(result)
        {
            whiteTextField.text = successKeyword;
            blueOutlineTextField.text = successKeyword;
            pinkOutlineTextField.text = successKeyword;
        }
        else
        {
            whiteTextField.text = failureKeyword;
            blueOutlineTextField.text = failureKeyword;
            pinkOutlineTextField.text = failureKeyword;
        }
    }

    public void SetHeaderText(HeaderType headerType)
    {
        var keyword = headerType == HeaderType.Success ? successKeyword
            : headerType == HeaderType.Failure ? failureKeyword
            : readyKeyword;

        whiteTextField.text = keyword;
        blueOutlineTextField.text = keyword;
        pinkOutlineTextField.text = keyword;
    }

    public void EnableMoneyBag()
    {
        moneyBagGO.SetActive(true);
    }

    IEnumerator CascadingNodeAnim()
    {
        yield return new WaitForSeconds(.8f);

        for (int i = 0; i < nodeArray.Length; i++)
        {
            nodeArray[i].gameObject.GetComponent<Animator>().SetTrigger("Anim");
            yield return new WaitForSeconds(0.33f);
        }
    }

    public void PopWindowUp()
    {
        //Tween the Window Here
        animator.SetBool("IsUp", true);
    }

    public void PopWindowDown()
    {
        //Tween the Window Here
        animator.SetBool("IsUp", false);
    }

    public void ToggleWindow(bool toogle)
    {
        pannel.gameObject.SetActive(toogle);
    }

    public void TriggerResult()
    {
        animator.SetTrigger("Result");
        StartCoroutine(CascadingNodeAnim());
        
        //Text for the MoneyBag goes here
    }
}
