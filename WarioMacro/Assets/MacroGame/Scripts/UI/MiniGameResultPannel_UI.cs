using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using DG.Tweening;
using GameTypes;

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
    [SerializeField] private PlayableAsset moneyGain;
    [SerializeField] private PlayableAsset moneyLose;

    [Header("CharacterAnim")]
    [SerializeField] private GameObject charaApparitionGO;
    [SerializeField] private SpriteListSO portraitsList;
    [SerializeField] private Image charaSpecialistSprite;

    [Header("TypesSO")]
    [SerializeField] private TypeSO brute;
    [SerializeField] private TypeSO alchemist;
    [SerializeField] private TypeSO acrobat;
    [SerializeField] private TypeSO expert;
    [SerializeField] private TypeSO ghost;
    [SerializeField] private TypeSO technomancer;

    [Header("NodeAnim")]
    [SerializeField] private float resultAnimBtwnTime = 0.33f;
    [SerializeField] private float resultAnimWaitTime = 0.8f;
    [SerializeField] private float spawnAnimBtwnTime = 0.66f;
    [SerializeField] private float spawnAnimWaitTime = 2.3f;

    public GameObject[] nodeArray = new GameObject[10];
    private GameObject[] expertNodeArray = new GameObject[6];

    [Header("Test Variables")]
    public bool debug = false;
    public int testNodeNbr;
    public int[] nodeMGDomain;

    private int currentNode;

    private void Start()
    {
        ClearAllNodes();
        ToggleWindow(false);

        if(debug)
        {
            ToggleWindow(true);
            SetStartingNodeNumber(testNodeNbr, nodeMGDomain);
        }
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

        if (nodeNbr < 5)
        {
            //Spawn X big node
            if (nodeNbr == 3) pannelLayoutGroup.spacing = 25f;
            else pannelLayoutGroup.spacing = 0f;

            for (int i = 0; i < nodeNbr; i++)
            {
                nodeArray[i] = Instantiate(bigNodePrefab, pannelLayoutGroup.transform);
            }
        }
        else if (nodeNbr < 6)
        {
            //Spawn X little nodes
            pannelLayoutGroup.spacing = -40f;
            for (int i = 0; i < nodeNbr; i++)
            {
                nodeArray[i] = Instantiate(littleNodePrefab, pannelLayoutGroup.transform);
            }
        }
        else if (nodeNbr >= 6)
        {
            //Spawn X little nodes
            pannelLayoutGroup.spacing = (-12 * nodeNbr);
            for (int i = 0; i < nodeNbr; i++)
            {

                nodeArray[i] = Instantiate(littleNodePrefab, pannelLayoutGroup.transform);
                nodeArray[i].transform.localScale = new Vector2(.6f, .6f);
            }
        }
    }

    //Override de la fonction pour accepter les experts
    public void SetStartingNodeNumber(int nodeNbr, int[]expertSpec)
    {
        nodeArray = new GameObject[nodeNbr];

        if (nodeNbr == 3)
        {
            pannelLayoutGroup.spacing = 25f;
            for (int i = 0; i < nodeNbr; i++)
            {
                SpawnNode(i, bigNodePrefab, new Vector2(0.85f, 0.85f), expertSpec[i]);
            }
        }
        else if (nodeNbr == 4)
        {
            pannelLayoutGroup.spacing = 0f;
            for (int i = 0; i < nodeNbr; i++)
            {
                SpawnNode(i, bigNodePrefab, new Vector2(0.85f, 0.85f), expertSpec[i]);
            }
        }
        else if (nodeNbr == 5)
        {
            pannelLayoutGroup.spacing = -40f;
            for (int i = 0; i < nodeNbr; i++)
            {
                SpawnNode(i, bigNodePrefab, new Vector2(0.75f, 0.75f), expertSpec[i]);
            }
        }
        else if (nodeNbr == 6)
        {
            pannelLayoutGroup.spacing = (-12 * nodeNbr);
            for (int i = 0; i < nodeNbr; i++)
            {
                SpawnNode(i, littleNodePrefab, new Vector2(0.60f, 0.60f), expertSpec[i]);
            }
        }

        StartCoroutine(CasdaingNodeSpawnAnim());
    }

    private void SpawnNode(int index, GameObject nodePrefab, Vector2 localScale, int expertType)
    {
        nodeArray[index] = Instantiate(nodePrefab, pannelLayoutGroup.transform);
        nodeArray[index].transform.localScale = localScale;

        //Expert is Set Here
        GameObject plus = nodeArray[index].transform.GetChild(0).gameObject;
        GameObject expertHover = nodeArray[index].transform.GetChild(1).gameObject;

        plus.SetActive(false);
        expertHover.gameObject.SetActive(false);

        switch (expertType)
        {
            case 0 :
                plus.SetActive(false);
                expertHover.gameObject.SetActive(false);
                break;

            case GameTypes.SpecialistType.Brute :
                //plus.gameObject.SetActive(true);
                //expertHover.gameObject.SetActive(true);

                //Set things
                expertHover.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = brute.logoSimple;
                expertHover.GetComponent<Image>().color = brute.typeColor;

                expertNodeArray[index] = nodeArray[index];
                break;

            case GameTypes.SpecialistType.Acrobat:
                //plus.gameObject.SetActive(true);
                //expertHover.gameObject.SetActive(true);

                expertHover.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = acrobat.logoSimple;
                expertHover.GetComponent<Image>().color = acrobat.typeColor;
                expertNodeArray[index] = nodeArray[index];
                break;

            case GameTypes.SpecialistType.Alchemist:
                //plus.gameObject.SetActive(true);
                //expertHover.gameObject.SetActive(true);

                expertHover.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = alchemist.logoSimple;
                expertHover.GetComponent<Image>().color = alchemist.typeColor;
                expertNodeArray[index] = nodeArray[index];
                break;

            case GameTypes.SpecialistType.Expert:
                //plus.gameObject.SetActive(true);
                //expertHover.gameObject.SetActive(true);

                expertHover.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = expert.logoSimple;
                expertHover.GetComponent<Image>().color = expert.typeColor;
                expertNodeArray[index] = nodeArray[index];
                break;

            case GameTypes.SpecialistType.Ghost:
                //plus.gameObject.SetActive(true);
                //expertHover.gameObject.SetActive(true);

                expertHover.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ghost.logoSimple;
                expertHover.GetComponent<Image>().color = ghost.typeColor;
                expertNodeArray[index] = nodeArray[index];
                break;

            case GameTypes.SpecialistType.Technomancer:
                //plus.gameObject.SetActive(true);
                //expertHover.gameObject.SetActive(true);

                expertHover.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = technomancer.logoSimple;
                expertHover.GetComponent<Image>().color = technomancer.typeColor;
                expertNodeArray[index] = nodeArray[index];
                break;
        }

        //Nodes Anim Here
    }

    public void SetCurrentNode(bool result)
    {
        Image checkMarkImage = nodeArray[currentNode].transform.GetChild(2).GetComponent<Image>(); //Get the child Image Component of the current node
        if (result) checkMarkImage.sprite = successCheckMark;
        else checkMarkImage.sprite = failureCheckMark;

        checkMarkImage.enabled = true;

        Image remanentImage = nodeArray[currentNode].transform.GetChild(3).GetComponent<Image>(); //Repeat For the Remanent Image
        if (result) remanentImage.sprite = successCheckMark;
        else remanentImage.sprite = failureCheckMark;

        remanentImage.enabled = true;

        nodeArray[currentNode].gameObject.GetComponent<Animator>().SetTrigger("Anim"); //Get the anim, start the animation
        currentNode++;
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

    private void SetHeaderText(HeaderType headerType)
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
        yield return new WaitForSeconds(resultAnimWaitTime);

        for (int i = 0; i < nodeArray.Length; i++)
        {
            nodeArray[i].gameObject.GetComponent<Animator>().SetTrigger("Anim");
            yield return new WaitForSeconds(resultAnimBtwnTime);
        }
    }

    IEnumerator CasdaingNodeSpawnAnim()
    {
        yield return new WaitForSeconds(spawnAnimWaitTime);

        for (int i = 0; i < expertNodeArray.Length; i++)
        {
            if(expertNodeArray[i] != null)
            {
                GameObject plus = expertNodeArray[i].transform.GetChild(0).gameObject;
                GameObject expertHover = expertNodeArray[i].transform.GetChild(1).gameObject;

                plus.SetActive(true);
                expertHover.gameObject.SetActive(true);
                expertNodeArray[i].gameObject.GetComponent<Animator>().SetTrigger("Spawn");
            }

            yield return new WaitForSeconds(spawnAnimBtwnTime);
        }
    }

    public IEnumerator CharaApparition(int nodeType)
    {
        if(nodeType <= 1) yield break;

        Debug.Log(CharacterManager.instance.SpecialistOfTypeInTeam(nodeType));

        Character selectedChara = null;

        foreach (Character chara in CharacterManager.instance.playerTeam)
        {
            if (chara.characterType == nodeType) selectedChara = chara;
        }



        Debug.Log(selectedChara);

        if (selectedChara == null) yield break;
        AudioManager.MacroPlaySound(selectedChara.GetMGSoundName());
        charaSpecialistSprite.sprite = selectedChara.fullSizeSprite;
        charaApparitionGO.SetActive(true);

        yield return new WaitForSeconds(2);
        charaApparitionGO.SetActive(false);
        yield return null;
    }

    public void PopWindowUp()
    {
        //Tween the Window Here
        ((RectTransform) transform).DOAnchorPosY(0, 0.5f);
        //transform.DOMoveY(0, 0.5f);
        //animator.SetBool("IsUp", true);
    }

    public void PopWindowDown()
    {
        //Tween the Window Here
        ((RectTransform) transform).DOAnchorPosY(-800, 0.5f); //.DOMoveY(-800, 0.5f);
        //animator.SetBool("IsUp", false);
    }

    public void ToggleWindow(bool toogle)
    {
        pannel.gameObject.SetActive(toogle);
    }

    public void Init(int microGameCount)
    {
        ToggleWindow(true);
        SetHeaderText(HeaderType.GetReady);
        ClearAllNodes();
        SetStartingNodeNumber(microGameCount);
        PopWindowUp();
        currentNode = 0;
    }

    public void Init(int microGameCount, int[] expertSpec)
    {
        ToggleWindow(true);
        SetHeaderText(HeaderType.GetReady);
        ClearAllNodes();
        SetStartingNodeNumber(microGameCount, expertSpec);
        PopWindowUp();
        currentNode = 0;
    }

    public void TriggerResult(bool result)
    {
        if (!result)
        {
            director.playableAsset = moneyLose;
        }
        else director.playableAsset = moneyGain;

        animator.SetTrigger("Result");
        StartCoroutine(CascadingNodeAnim());

        //Text for the MoneyBag goes here
    }
}
