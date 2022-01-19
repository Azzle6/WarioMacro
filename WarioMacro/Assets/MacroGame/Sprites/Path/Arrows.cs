using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : MonoBehaviour
{
    public enum Arrow { LEFT, RIGHT, TOP, BOTTOM}

    public Arrow arrowDirection = Arrow.BOTTOM;
    public GameObject littleArrow;
    public GameObject bigArrow;
}
