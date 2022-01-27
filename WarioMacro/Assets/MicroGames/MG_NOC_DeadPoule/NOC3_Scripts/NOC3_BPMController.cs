using UnityEngine;

namespace MiniGame.DeadPoule
{
    [DefaultExecutionOrder(0)]
    public class NOC3_BPMController : MonoBehaviour, ITickable
    {
        [SerializeField] private Sound tickSound;

        public static System.Action OnReachingEndGame;
        private bool reachedMaxTickCount;

        private float startTime;
        private int currentTick;
        private bool callDone; 

        private void Start()
        {
            startTime = Time.time;
            GameManager.Register();
            GameController.Init(this);
        }

        public void OnTick()
        {

            if (reachedMaxTickCount) return;

            Debug.Log("tick " + GameController.currentTick);
            
            if (NOC3_MinigameController.PlayerSequenceTracker == (int)NOC3_MinigameController.Difficulty || NOC3_MinigameController.FailedSequenceBeforeEnd)
            {
                if (!callDone)
                {
                    Debug.Log("final gameplay tick " + GameController.currentTick);

                    callDone = true; 
                    OnReachingEndGame();
                    GameController.StopTimer();
                    currentTick = GameController.currentTick;
                }

                if (GameController.currentTick - currentTick == 3)
                {
                    Debug.Log("post-final gameplay tick " + GameController.currentTick);
                    GameController.FinishGame(NOC3_MinigameController.HasWon);
                }                
            }

            if (callDone) return;

            if (GameController.currentTick <= 5 && Time.time > Time.fixedDeltaTime + Mathf.Epsilon) // second part to avoid tick sound when currentTick == 0
            {
                NOC3_AudioManager.PlaySound(tickSound.clip, tickSound.volume, tickSound.delay);
                Debug.Log("reading not call done");
                
                if (GameController.currentTick == 5)
                {
                    Debug.Log("capped final gameplay tick");
                    GameController.StopTimer();
                    OnReachingEndGame();
                }
            }
            
            if (GameController.currentTick == 8)
            {
                Debug.Log("reached max tick count");

                reachedMaxTickCount = true;
                GameController.FinishGame(NOC3_MinigameController.HasWon);
            }
        }
    }
}
