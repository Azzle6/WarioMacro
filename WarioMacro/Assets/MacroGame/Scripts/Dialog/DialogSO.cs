using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogSO", menuName = "MacroGame/DialogSO", order = 0)]
public class DialogSO : ScriptableObject
{
    public string name;
    [Space]
    public string[] dialogs;
    
}
