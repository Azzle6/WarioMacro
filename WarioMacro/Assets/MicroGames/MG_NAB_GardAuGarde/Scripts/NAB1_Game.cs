using System.Collections;
using TMPro;
using UnityEngine;

public class NAB1_Game : MonoBehaviour, ITickable
{

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject guardPrefab;
    [SerializeField] private GameObject container;
    
    [SerializeField] private TMP_Text defeatText;
    
    [SerializeField] private AudioClip defeatSound;


    private GameObject guard1;
    private GameObject guard2;

    private Animator animator;
    private Animator animator2;

    private int angle1;
    private int angle2;
    private bool moving;
    private int maxTick = 8;

    private bool playing = true;
    private bool seen = false;
    private bool result;
    
    private void Start()
    {
        GameManager.Register();
        AudioManager.Register();
        GameController.Init(this);
        InstantiateGuard();
    }

    private void InstantiateGuard()
    {
        if (GameController.difficulty == 3)
        {
            guard1 = Instantiate(guardPrefab,new Vector3(-1,0,0), Quaternion.identity, container.transform);
            animator = guard1.GetComponent<Animator>();
            guard1.GetComponent<NAB1_Guard>().InitAngle(-33);
            
            guard2 = Instantiate(guardPrefab, new Vector3(1,0,0), Quaternion.identity, container.transform);
            animator2 = guard2.GetComponent<Animator>();
            guard2.GetComponent<NAB1_Guard>().InitAngle(-3);
        }
        else
        {
            guard1 = Instantiate(guardPrefab, new Vector3(0,0,0), Quaternion.identity, container.transform);
            animator = guard1.GetComponent<Animator>();
            if (GameController.difficulty == 2)
            {
                guard1.GetComponent<NAB1_Guard>().InitAngle(17);
            }
            else
            {
                guard1.GetComponent<NAB1_Guard>().InitAngle(0);
            }
        }
    }

    void Update()
    {
        if (InputManager.GetKeyDown(ControllerKey.A) || InputManager.GetKeyDown(ControllerKey.B) || InputManager.GetKeyDown(ControllerKey.X) || InputManager.GetKeyDown(ControllerKey.Y))
        {
            player.GetComponent<NAB1_PlayerMove>().Move();
            moving = true;
            result = true;
        }
        
        if (GameController.difficulty == 3)
        {
            seen = guard1.GetComponent<NAB1_Guard>().seen || guard2.GetComponent<NAB1_Guard>().seen;
        }
        
        else
        {
            seen = guard1.GetComponent<NAB1_Guard>().seen;
        }

        if (!seen) return;
        moving = false;
        playing = false;
        result = false;
        defeatText.enabled = true;
    }

    public void OnTick()
    {
        
        if ((result || seen) && (GameController.currentTick < 5) && playing)
        {
            GameController.StopTimer();
            maxTick = GameController.currentTick + 4;
        }
        
        if (GameController.currentTick == maxTick)
        {
            GameController.FinishGame(result);
        }

        if (!seen && result && playing)
        {
            player.SendMessage("End", SendMessageOptions.DontRequireReceiver);
            playing = false;
            return;
        }

        if(GameController.currentTick == 5 && (!seen && !result && !moving))
        {
            guard1.GetComponent<NAB1_Guard>().SwitchLight();
            AudioManager.PlaySound(defeatSound);
            playing = false;
            defeatText.enabled = true;
            player.SendMessage("Spotted", SendMessageOptions.DontRequireReceiver);
            player.GetComponentInChildren<Transform>().rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y + 180, player.transform.rotation.z);
            if (GameController.difficulty == 3)
            {
                guard2.GetComponent<Animator>().SetTrigger("Spot");
                guard2.GetComponent<NAB1_Guard>().MoveIdle(guard2.GetComponent<Animator>());
                guard2.GetComponentInParent<Transform>().LookAt(player.transform.position);
            }
            guard1.GetComponent<Animator>().SetTrigger("Spot");
            guard1.GetComponent<NAB1_Guard>().MoveIdle(guard1.GetComponent<Animator>());
            guard1.GetComponentInParent<Transform>().LookAt(player.transform.position);
        }

        if (playing && GameController.difficulty == 1)
        {
            guard1.GetComponent<NAB1_Guard>().SwitchLight();
        }
        
        else if (playing && GameController.difficulty == 2)
        {
            guard1.GetComponent<NAB1_Guard>().SwitchSide(animator);
        }
        
        else if (playing && GameController.difficulty == 3 && GameController.currentTick % 2 == 0)
        {

            guard1.GetComponent<NAB1_Guard>().SwitchSideAsTwo(animator);
        }
        
        else if (playing && GameController.difficulty == 3 && GameController.currentTick % 2 == 1)
        {
            guard2.GetComponent<NAB1_Guard>().SwitchSideAsTwo(animator2);
        }
    }
}
