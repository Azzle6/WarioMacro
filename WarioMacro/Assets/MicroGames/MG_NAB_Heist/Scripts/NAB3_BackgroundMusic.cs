using UnityEngine;

public class NAB3_BackgroundMusic : MonoBehaviour
{
    private static NAB3_BackgroundMusic instance = null;
    public static NAB3_BackgroundMusic Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
