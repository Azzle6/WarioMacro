using UnityEngine;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Transform lifeBarGO;
    [Range(1, 4)]
    [SerializeField] private int life;
    
    private void Start()
    {
        for (int i = 0; i < life; i++)
        {
            lifeBarGO.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Damage()
    {
        life--;
        
        lifeBarGO.GetChild(life).gameObject.SetActive(false);
    }

    public int GetLife() => life;
}
