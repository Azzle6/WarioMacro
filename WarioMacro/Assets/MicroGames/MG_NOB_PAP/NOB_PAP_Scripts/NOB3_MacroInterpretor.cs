using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB3_MacroInterpretor : MonoBehaviour, ITickable
{
    [SerializeField] private NOB3_PlankManager plankManager;
    [SerializeField] private TMPro.TMP_Text tickCountText;
    [SerializeField] private TMPro.TMP_Text resultText;
    public bool result;
    private int tickCount;
    void Awake()
    {
        tickCountText.text = "0";
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick
    }
    public void OnTick()
    {
        tickCountText.text = GameController.currentTick.ToString();
        
        if (GameController.currentTick == 5)
        {
            if (plankManager.planksLeft == 0)
            {
                result = true;
                resultText.text = "Victory";
            }
            else
            {
                result = false;
                resultText.text = "Defeat";
            }
            //Le jeu se finit, il nous reste 3 ticks pour afficher le résultat
            resultText.gameObject.SetActive(true);
        }

        if (GameController.currentTick == 8)
        {
            //Le jeu se décharge
            GameController.FinishGame(result);
        }
    }
}
