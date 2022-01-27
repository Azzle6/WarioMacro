using UnityEngine;

public class NAB3_Player : MonoBehaviour, ITickable
{
    public float jumpForce;
    public Animator anim;
    public AudioClip audioClip;

    public NAB3_NMIGenerator enemyGen;

    GameObject gameOver;
    GameObject game;
    GameObject destroyZone;
    GameObject win;

    public bool endGame;

    int tickHolder;

    bool result = true;

    [SerializeField]
    bool isGrounded = false;

    Rigidbody2D RB;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
 
        AudioManager.Register();

        gameOver = GameObject.Find("GameOver");
        game = GameObject.Find("Game");
        destroyZone = GameObject.Find("DestroyZone");
        win = GameObject.Find("Win");
        gameOver.SetActive(false);
        win.SetActive(false);
        destroyZone.SetActive(false);
    }

    void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        endGame = false;
        enemyGen.difficulty = GameController.difficulty;
        Debug.Log(GameController.difficulty);
    }
    
    void Update()
    {
        if (InputManager.GetKeyDown(ControllerKey.A) || InputManager.GetKeyDown(ControllerKey.B) 
        || InputManager.GetKeyDown(ControllerKey.Y) || InputManager.GetKeyDown(ControllerKey.X))
        {
            if (isGrounded == true)
            {
                RB.AddForce(Vector2.up * jumpForce);
                isGrounded = false;
                anim.SetBool("isJumping",true);
                PlaySound();
            }
        }
    }
    public void OnTick()
    {
        if (!endGame)
        {
            tickHolder = GameController.currentTick + 3;
        }

        if (GameController.currentTick == 5)
        {
            GameController.StopTimer();
            game.SetActive(false);
            endGame = true;


            //result = true; --> inutile
            if (!gameOver.activeSelf)
            {
                destroyZone.SetActive(true);
                win.SetActive(true);
            }
        }

        if (endGame)
        {
            GameController.StopTimer();
            game.SetActive(false);

            if (!gameOver.activeSelf)
            {
                destroyZone.SetActive(true);
                win.SetActive(true);
            }
        }

        if (GameController.currentTick == tickHolder)
        {
            GameController.FinishGame(result);
        }

        //Debug.Log(GameController.currentTick);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ground"))
        {
            if(isGrounded == false)
            {
                isGrounded = true;
                OnLanding();
            }
        }

        /////////////////////Game Over//////////////////
        if (collision.gameObject.CompareTag("guard"))
        {
            gameOver.SetActive(true);
            game.SetActive(false);
            destroyZone.SetActive(true);

            result = false;

            endGame = true;
            
        }

        /////////////////////////////////////////////////

            /*if (destroyZone == null)
                destroyZone.SetActive(true);
            if (!destroyZone.activeSelf)
            {
                destroyZone.SetActive(true);
            }

            result = false;*/

            /*Destroy(collision.gameObject);
            Destroy(this.gameObject);*
        }

        /*if (collision.gameObject.CompareTag("coin"))
        {
            Destroy(collision.gameObject);
        }*/
    }

    public void OnLanding()
    {
        anim.SetBool("isJumping", false);
    }

    private void PlaySound()
    {
        AudioManager.PlaySound(audioClip);
    }
}
