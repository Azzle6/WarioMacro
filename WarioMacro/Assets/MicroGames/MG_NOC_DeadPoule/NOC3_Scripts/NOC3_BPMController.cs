using UnityEngine;

namespace MiniGame.DeadPoule
{
    [DefaultExecutionOrder(0)]
    public class NOC3_BPMController : MonoBehaviour, ITickable
    {
        [SerializeField] private Sound tickSound;

        public static System.Action OnReachingEndGame;
        private bool gameEnd;

        private float startTime;

        private void Start()
        {
            startTime = Time.time;
            GameManager.Register();
            GameController.Init(this);
        }

        public void OnTick()
        {
            Debug.Log("tick");
            // 2 seconds of countdown to read the keyword
            // weird tick call on start from GameController that is not considered as a real tick (currentTick == 5 does not count that first weird tick) 
            if (Time.time - startTime < 0.025f || gameEnd) return;

            if (GameController.currentTick <= 5)
            {
                NOC3_AudioManager.PlaySound(tickSound.clip, tickSound.volume, tickSound.delay);

                if (GameController.currentTick == 5)
                {
                    Debug.Log("tick 5");
                    GameController.StopTimer();
                }
            }
            else if (GameController.currentTick == 6)
            {
                OnReachingEndGame();
            }
            else if (GameController.currentTick == 8)
            {
                Debug.Log("tick 8");

                gameEnd = true;
                GameController.FinishGame(NOC3_MinigameController.HasWon);
            }
        }
    }
}
