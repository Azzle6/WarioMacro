//_________________________________________________________________________________________
//CE SCRIPT EST UN TEMPLATE
//NE PAS MODIFIER CE SCRIPT
//COPIER COLLER LE CODE DANS UN AUTRE SCRIPT ET ENLEVER CELUI-CI DE LA SCENE
//_________________________________________________________________________________________

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMicroGame : MonoBehaviour, ITickable
{
    void Start()
    {
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick
    }

    public void OnTick()
    {
        if (GameController.currentTick == 5)
        {
            //Le jeu se finit, il nous reste 3 ticks pour afficher le résultat
        }

        if (GameController.currentTick == 8)
        {
            //Le jeu se décharge
            GameController.FinishGame(true);
        }
    }
}
