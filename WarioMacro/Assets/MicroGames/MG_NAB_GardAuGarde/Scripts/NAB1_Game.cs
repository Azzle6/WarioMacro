using System.Collections;
using TMPro;
using UnityEngine;

public class NAB1_Game : MonoBehaviour, ITickable
{
    private bool result;

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

    private bool playing = true;
    private bool seen = false;
    
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
            guard1.GetComponent<NAB1_Guard>().InitAngle(17);
        }
    }

    void Update()
    {
        if (InputManager.GetKeyDown(ControllerKey.A))
        {
            player.GetComponent<NAB1_PlayerMove>().Move();
            result = true;
            playing = false;
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
        playing = false;
        result = false;
        defeatText.enabled = true;
    }

    public void OnTick()
    {
        if (GameController.currentTick == 8)
        {
            GameController.FinishGame(result);
        }

        else if(GameController.currentTick == 5)
        {
            if (!seen && result)
            {
                player.SendMessage("End", SendMessageOptions.DontRequireReceiver);
                return;
            }
            
            if (!seen && !result)
            {
                AudioManager.PlaySound(defeatSound);
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
        }

        if (GameController.currentTick > 5)
        {
            return;
        }
        
        if (playing && GameController.difficulty == 1 && GameController.currentTick % 2 == 0)
        {
            guard1.GetComponent<NAB1_Guard>().SwitchSide(animator);
        }
        
        else if (playing && GameController.difficulty == 2)
        {
            guard1.GetComponent<NAB1_Guard>().SwitchSideRandom(animator);
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
