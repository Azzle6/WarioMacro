using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB2_TICCounter : MonoBehaviour
{
    public GameObject tic1, tic2, tic3, tic4, tic5;

    public void TicAvance()
    {
        if(tic1 != null)
        {
            Destroy(tic1);

        }
        else if (tic2 != null)
        {
            Destroy(tic2);
        }
        else if (tic3 != null)
        {
            Destroy(tic3);
        }
        else if (tic4 != null)
        {
            Destroy(tic4);
        }
        else if (tic5 != null)
        {
            Destroy(tic5);
        }
    }

}
