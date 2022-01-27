using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloorPlan_UI : MonoBehaviour
{
    [SerializeField] TMP_Text textField;
    public void SetFloor(int floorNbr, bool isUnkown)
    {
        string floorString = string.Empty;
        if(!isUnkown)
        {
            string s = floorNbr.ToString();
            floorString = "Floor " + s;
        }
        else floorString = "Floor ??";

        textField.text = floorString;
    }
}
