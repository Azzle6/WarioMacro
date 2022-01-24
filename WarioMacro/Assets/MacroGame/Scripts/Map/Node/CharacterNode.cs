using System.Collections;
using System.Collections.Generic;
using GameTypes;
using UnityEngine;

public class CharacterNode : MonoBehaviour
{
    [GameType(typeof(SpecialistType))]
    public int type = 1;

    public Character currentChara;

    public bool HasBeenRecruit;
    
    

}
