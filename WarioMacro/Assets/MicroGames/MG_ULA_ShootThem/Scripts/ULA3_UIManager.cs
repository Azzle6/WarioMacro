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
    [SerializeField] private Transform viseur;
    
    [SerializeField] private GameObject cross;
    
    private int tickend = 10;
    private int target;
    private bool finish = false;
    private bool result;
    private int random;
    private int pressed=0;
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        random = Random.Range(0, 3);
        switch (GameController.difficulty)
        {
            case 1:
                target = 1;
                level.sprite = levelEasy[random];
                viseurAnim.Play("ViseurEasy"+(random+1));
                break;
            case 2:
                target = 1;
                level.sprite = levelMedium[random];
                viseurAnim.Play("ViseurMedium"+(random+1));
                break;
            case 3:
                target = 2;
                level.sprite = levelHard[random];
                viseurAnim.Play("ViseurHard"+(random+1));
                break;
        }
    }

    private void Update()
    {
        if (InputManager.GetKeyDown(ControllerKey.X))
        {
            if (!finish)
            {
                AudioManager.PlaySound(shootSound);
                Canvas.Instantiate(cross, viseur.position, Quaternion.identity, parent);
            }
            
            switch (GameController.difficulty)
            {
                case 1:
                    switch (random+1)
                    {
                        case 1:
                            if (GameController.currentTick == 4)
                            {
                                target = 0;
                            }
                            else
                            {
                                finish = true;
                            }

                            break;
                        case 2:
                            if (GameController.currentTick == 3)
                            {
                                target = 0;
                            }
                            else
                            {
                                finish = true;
                            }
                            break;
                        case 3:
                            if (GameController.currentTick == 4)
                            {
                                target = 0;
                            }
                            else
                            {
                                finish = true;
                            }
                            break;
                    }
                    break;
                    
                case 2:
                    switch (random+1)
                    {
                        case 1:
                            if (GameController.currentTick == 3)
                            {
                                target = 0;
                            }
                            else
                            {
                                finish = true;
                            }

                            break;
                        
                        case 2:
                            if (GameController.currentTick == 3)
                            {
                                target = 0;
                            }
                            else
                            {
                                finish = true;
                            }

                            break;
                        
                        case 3:
                            if (GameController.currentTick == 4)
                            {
                                target = 0;
                            }
                            else
                            {
                                finish = true;
                            }

                            break;
                    }
                    break;
                case 3:
                    switch (random+1)
                    {
                        case 1:
                            if (GameController.currentTick == 1 && pressed==0)
                            {
                                target -= 1;
                                pressed++;
                            } else if (GameController.currentTick == 4 && pressed==1)
                            {
                                target -= 1;
                                pressed++;
                            }else
                            {
                                pressed++;
                            }

                            break;
                        
                        case 2:
                            if (GameController.currentTick == 2 && pressed==0)
                            {
                                target -= 1;
                                pressed++;
                            } else if (GameController.currentTick == 4 && pressed==1)
                            {
                                target -= 1;
                                pressed++;
                            }else
                            {
                                pressed++;
                            }

                            break;
                        
                        case 3:
                            if (GameController.currentTick == 0 && pressed==0)
                            {
                                target -= 1;
                                pressed++;
                            } else if (GameController.currentTick == 3 && pressed==1)
                            {
                                target -= 1;
                                pressed++;
                            }else
                            {
                                pressed++;
                            }

                            break;
                    }
                    break;

            }
        }

        if (target == 0 || pressed>=2)
        {
            finish = true;
        }
    }

    public void OnTick()
    {
        Debug.Log(GameController.currentTick);
        
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
            Debug.Log(result);
        }
    }
}
