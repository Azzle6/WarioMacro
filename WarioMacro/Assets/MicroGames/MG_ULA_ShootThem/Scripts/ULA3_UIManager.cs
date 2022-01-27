using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ULA3_UIManager : MonoBehaviour, ITickable
{
    [SerializeField] private List<Sprite> levelEasy;
    [SerializeField] private List<Sprite> levelMedium;
    [SerializeField] private List<Sprite> levelHard;

    [SerializeField] private TMP_Text victory;
    [SerializeField] private TMP_Text defeat;

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private Animation viseurAnim;
    
    [SerializeField] private Image level;

    [SerializeField] private Transform parent;
    [SerializeField] private RectTransform viseur;
    
    [SerializeField] private GameObject cross;
    
    private int tickend = 10;
    private int target;
    private bool finish = false;
    private bool result;
    private int random;
    private int pressed=0;
    private Texture2D test;
    private Color color;
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        random = Random.Range(0, 3);
        Debug.Log("random :"+random);
        switch (GameController.difficulty)
        {
            case 1:
                target = 1;
                level.sprite = levelEasy[random];
                test = level.sprite.texture;
                viseurAnim.Play("ViseurEasy"+(random+1));
                Debug.Log("anim :"+(random+1));
                break;
            case 2:
                target = 1;
                level.sprite = levelMedium[random];
                test = level.sprite.texture;
                viseurAnim.Play("ViseurMedium"+(random+1));
                Debug.Log("anim :"+(+random+1));
                break;
            case 3:
                target = 1;
                level.sprite = levelHard[random];
                test = level.sprite.texture;
                viseurAnim.Play("ViseurHard"+(random+1));
                Debug.Log("anim :"+(random+1));
                break;
        }
    }

    private void Update()
    {
        color = test.GetPixel(Mathf.FloorToInt(viseur.anchoredPosition.x),
            Mathf.FloorToInt(viseur.anchoredPosition.y));
        if (InputManager.GetKeyDown(ControllerKey.X) || InputManager.GetKeyDown(ControllerKey.A) || InputManager.GetKeyDown(ControllerKey.B) || InputManager.GetKeyDown(ControllerKey.Y) )
        {
            if (!finish)
            {
                AudioManager.PlaySound(shootSound);
                Canvas.Instantiate(cross, viseur.position, Quaternion.identity, parent);
            }
            
            if (color == Color.red)
            {
                target = 0;
                        
            }
            else
            {
                finish = true;
            }
        }

        if (target == 0 || pressed>=2)
        {
            finish = true;
        }
    }

    public void OnTick()
    {
        if (GameController.currentTick == 5 && !finish)
        {
            finish = true;
        }
        
        if (finish && tickend==10)
        {
            GameController.StopTimer();
            tickend = GameController.currentTick;
            if (target == 0)
            {
                victory.enabled = true;
                result = true;
            }
            else
            {
                defeat.enabled = true;
                result = false;
            }
        }


        if (GameController.currentTick == tickend + 3)
        {
            GameController.FinishGame(result);
        }
    }
}
