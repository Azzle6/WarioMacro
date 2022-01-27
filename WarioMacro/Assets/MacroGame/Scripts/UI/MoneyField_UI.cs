using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyField_UI : MonoBehaviour
{
    [SerializeField] TMP_Text textField;
    [SerializeField] float waitTimebtwnChar = .003f;
    [SerializeField] float waitTimebtwnColumns = .01f;
    [SerializeField] bool debug = false;
    private char[] charArray = new char[100];


    private void Start()
    {
        SetCounterTextTyping("128950");
    }
    public char[] SetCounterTextTyping(string number)
    {
        charArray = number.ToCharArray();

        StartCoroutine(CounterTyping());
        return charArray;
    }

    string textString = string.Empty;
    string intermediaryString = string.Empty;
    IEnumerator CounterTyping()
    {
        yield return null;

        for(int i = 0; i < charArray.Length; i++)
        {

            char originChar = charArray[i];           
            int steps = int.Parse(originChar.ToString());

            if(debug)print("Origin character at position " + i + " = " + originChar + " // steps = " + steps);

            AddMiddleString(originChar.ToString());
            for(int z = 0; z <= steps; z++)
            {

                yield return new WaitForSeconds(waitTimebtwnChar);
                char[] textCharArray = textString.ToCharArray();

                string s = z.ToString();
                char[] swapCharArray = s.ToCharArray();
                if(debug)print(originChar + " // " + z);
                if(debug)print("String de Texte intermédiaire = " + ChangeStringCharAtPosition(textCharArray, i, swapCharArray));

                intermediaryString = ChangeStringCharAtPosition(textCharArray, i, swapCharArray);
                textField.text = intermediaryString;
            }

            yield return new WaitForSeconds(waitTimebtwnColumns);
        }
        textField.text = intermediaryString + " $";
        if (debug)print(" String de Texte finale = " + textString);
    }

    public void AddMiddleString(string middleString)
    {
        textString = textString + middleString;
    }

    public string ChangeStringCharAtPosition(char[] charArray, int position, char[] newchar)
    {
        if(debug)print("character swap position " + position);
        if(debug)print("char at swap position " + charArray[position]);
        if(debug)print("new char = " + newchar[0]);

        charArray[position] = newchar[0];
        string s = new string(charArray);
        return s;

    }
}
