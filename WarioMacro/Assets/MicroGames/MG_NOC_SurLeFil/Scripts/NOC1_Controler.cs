using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NOC1_Controler : MonoBehaviour, ITickable
{
    [SerializeField] private GameObject stopZone;
    [SerializeField] float speed;
    [SerializeField] GameObject winner;
    [SerializeField] GameObject looser;
    [SerializeField] private Animator animator;
    [SerializeField] List<AudioClip> grincements;
    [SerializeField] private AudioClip sonDefaite;
    [SerializeField] private AudioClip sonVictoire;
    [SerializeField] private AudioClip sonFeu;
    public SpriteRenderer spriteRender;
    public Sprite sprite1;
    public Sprite sprite2;
    private string ButtonDown;
    private string ButtonBefore;
    private bool result;
    private bool falling;
    private bool done;
    private int canFall;
    private int felt = 0;
    private int actualTick = 10;
    private bool isWalking = false;
    private bool songIn = false;
    private bool sprite = false;
    private AudioClip actualSong;
    

    void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        AudioManager.PlaySound(sonFeu);
        result = false;
        falling = false;
        done = false;
        int difficulty = GameController.difficulty;
        if (difficulty == 1)
        {
            canFall = 2;
        }
        else if (difficulty == 2)
        {
            canFall = 1;
        }
        else if (difficulty == 3)
        {
            canFall = 0;
        } 
    }
    
    void Update()
    {
        if (isWalking == true && done == false && songIn == false)
        {
            StartCoroutine(WalkinkSound());
        }
        if (InputManager.GetKeyDown(ControllerKey.RB) && falling == false && done == false)
        {
            if (ButtonBefore == "RB")
            {
                StartCoroutine(IsFalling());
            }
            else
            {
                ButtonBefore = "RB";
                Walk();
            }
        }
        else if (InputManager.GetKeyDown(ControllerKey.LB) && falling == false && done == false)
        {
            if (ButtonBefore == "LB")
            {
                StartCoroutine(IsFalling());
            }
            else
            {
                ButtonBefore = "LB";
                Walk();
            }
        }
    }

    void ChangeSprite()
    {
        if (sprite == true)
        {
            spriteRender.sprite = sprite1;
            sprite = false;
        }
        else if (sprite == false)
        {
            spriteRender.sprite = sprite2;
            sprite = true;
        }
        
    }

    void Done()
    {
        done = true;
        actualTick = GameController.currentTick;
        AudioManager.StopSound(actualSong);
        if (result == false)
        {
            looser.SetActive(true);
            AudioManager.PlaySound(sonDefaite);
        }
        else
        {
            winner.SetActive(true);
            AudioManager.PlaySound(sonVictoire);
        }
    }
    void Walk()
    {
        isWalking = true;
        transform.position += transform.right * speed;
        ChangeSprite();
    }

    private void OnTriggerEnter(Collider stopZone)
    {
        result = true;
        Done();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void OnTick()
    {
        if (GameController.currentTick == 5 && done == false)
        {
            actualTick = GameController.currentTick;
            Done();
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (GameController.currentTick == actualTick + 3)
        {
            AudioManager.StopSound(sonFeu);
            GameController.FinishGame(result);
        }
        if (done == true)
        {
            GameController.StopTimer();
        }
    }

    private IEnumerator IsFalling()
    {
        falling = true;
        if (felt == canFall)
        {
            falling = false;
            result = false;
            Done();
            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            animator.Play("NOC_Chute");
            yield return new WaitForSeconds(2);
            animator.Play("void");
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            felt++;
            if (GameController.currentTick % 2 == 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                animator.Play("NOC_BordChute");
                yield return new WaitForSeconds(1);
                animator.Play("void");
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                animator.Play("NOC_BordChuteBas");
                yield return new WaitForSeconds(1);
                animator.Play("void");
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            }
            ButtonBefore = "None";
            falling = false;
        }
    }

    private IEnumerator WalkinkSound()
    {
        songIn = true;
        actualSong = grincements[UnityEngine.Random.Range(0, 4)];
        AudioManager.PlaySound(actualSong, 1);
        yield return new WaitForSeconds(1);
        songIn = false;
    }
}
