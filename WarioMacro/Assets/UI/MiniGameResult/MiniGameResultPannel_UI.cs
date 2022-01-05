using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    public GameObject[] nodeArray = new GameObject[10];
    public Animator animator;

    private void Start()
    {
        ClearAllNodes();
        ToggleWindow(false);
        //SetStartingNodeNumber(4);
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
            pannelLayoutGroup.spacing = (-10*nodeNbr);
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
}
