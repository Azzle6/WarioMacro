using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NAC2_AnswerManager : MonoBehaviour
{
    [SerializeField] private NAC2_QuelleCouleurController qcController;

    [SerializeField] private string[] answerText;
    [SerializeField] private string[] answerColors;
    [SerializeField] private TMP_Text[] buttonTexts;
    private int goodAnswer;
    private int answer;

    private void Start()
    {
        goodAnswer = Random.Range(0,3);
    }

    private int falseAnswer;

    public void GenerateAnswers(int[] goodColors) 
    {
        Debug.Log(goodColors[0] +" / "+ goodColors[1]);
        Debug.Log("Good answer = " + goodAnswer);
        falseAnswer = 1;
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            if (i == goodAnswer)
            {
                buttonTexts[i].text = (GameController.difficulty == 3 ? "<color=" + answerColors[Random.Range(0, answerText.Length)].ToLower() + ">" : "" ) + answerText[goodColors[0]] + "</color>" +
                    (GameController.difficulty > 1 ? "/" + (GameController.difficulty == 3 ? "<color=" + answerColors[Random.Range(0, answerText.Length)].ToLower() + ">" : "") + answerText[goodColors[1]] : "");
            }
            else
            {
                buttonTexts[i].text = (GameController.difficulty == 3 ? "<color=" + answerColors[Random.Range(0, answerText.Length)].ToLower() + ">" : "") + answerText[(goodColors[0] + falseAnswer) % answerText.Length] + "</color>" +
                    (GameController.difficulty > 1 ? "/" + (GameController.difficulty == 3 ? "<color=" + answerColors[Random.Range(0, answerText.Length)].ToLower() + ">" : "") + answerText[(goodColors[1] + falseAnswer) % answerText.Length] : "");
                falseAnswer++;
            }
        }
    }

    public int GetGoodAnswer() 
    {
        return goodAnswer;
    }

    public void Answer1() 
    {
        qcController.CheckAwnser(0);
    }

    public void Anwser2() 
    {
        qcController.CheckAwnser(1);
    }
    public void Anwser3()
    {
        qcController.CheckAwnser(2);
    }
}
