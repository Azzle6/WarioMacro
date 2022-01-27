using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

using TMPro;

namespace MiniGame.DeadPoule
{
    public enum Difficulty { Easy = 3, Medium = 4, Hard = 6 }
    public enum GameState { Loading, Showing, Playing, Unloading }

    [System.Serializable]
    public class Sound
    {
        public AudioClip clip;
        [Range(0.2f, 1f)] public float volume = 0.75f;
        [Range(0f, 1f)] public float delay = 0f;
    }

    [DefaultExecutionOrder(1)]
    public class NOC3_MinigameController : MonoBehaviour
    {
        [SerializeField] private GameControllerSO gameConfig;
        [SerializeField] private GameObject winVFX;

        [SerializeField] private Sound winSound;
        [SerializeField] private Sound loseSound;

        [Space, SerializeField] private GameObject defaultGraphics;
        [SerializeField] private GameObject chicken;
        [SerializeField] private GameObject wand;

        [Space, SerializeField] private GameObject[] buttons;
        [SerializeField] private Transform buttonSequenceStart;

        public static Difficulty Difficulty { get; private set; }
        private int countDownTracker = 2;

        private List<string> buttonsInternalName = new List<string> { "NOC3_A", "NOC3_B", "NOC3_X", "NOC3_Y" };

        private byte[] internalSequence;
        private byte[] playerSequence;
        private int rngTracker;
        public static int PlayerSequenceTracker { get; private set; }

        private GameObject[] buttonReferences;
        public static GameState GameState { get; private set; }

        private bool doneOnce;

        public static bool HasWon { get; private set; }
        public static bool FailedSequenceBeforeEnd { get; private set; }


        // DEBUG
        private int playerSequenceTrackerDebug;

        private void OnEnable()
        {
            NOC3_BPMController.OnReachingEndGame += ReceiveEngGameEvent;
        }

        private void OnDisable()
        {
            NOC3_BPMController.OnReachingEndGame -= ReceiveEngGameEvent;
        }

        private void Start()
        {
            FailedSequenceBeforeEnd = false; 
            PlayerSequenceTracker = 0; 
            Difficulty = gameConfig.currentDifficulty == 1 ? Difficulty.Easy : gameConfig.currentDifficulty == 2 ? Difficulty.Medium : Difficulty.Hard;
            HasWon = false;
            GameState = GameState.Showing;

            internalSequence = new byte[(int)Difficulty];
            playerSequence = new byte[(int)Difficulty];
            buttonReferences = new GameObject[(int)Difficulty];

            chicken.SetActive(false);
            defaultGraphics.SetActive(false);
            wand.SetActive(false);

            for (int i = 0; i < playerSequence.Length; i++)
            {
                playerSequence[i] = 100;
            }

            ShowRandomButtonSequence();
        }

        private void Update()
        {
            playerSequenceTrackerDebug = PlayerSequenceTracker;
            if (GameState == GameState.Playing)
            {
                ProcessPlayerInputs();
            }
        }


        private void ShowRandomButtonSequence() 
        {
            for (int i = 0; i < (int)Difficulty; i++)
            {
                int index = Random.Range(0, buttons.Length - 1);

                if (rngTracker == index && i > 0) // sort of filtered randomness
                {
                    index = Random.Range(0, buttons.Length - 1);
                }
                rngTracker = index;

                buttonReferences[i] = Instantiate(buttons[index], buttonSequenceStart.position + new Vector3(2f * i, 0), Quaternion.identity);
                internalSequence[i] = (byte)buttonsInternalName.IndexOf(buttons[index].gameObject.name);
            }

            StartCoroutine(nameof(CountDown));
        }

        private IEnumerator CountDown()
        {
            yield return new WaitForSeconds(1f);

            countDownTracker -= 1;
            if (countDownTracker > 0)
            {
                StartCoroutine(nameof(CountDown));
            }
            else
            {
                GameState = GameState.Playing;

                if (!doneOnce)
                {
                    doneOnce = true;
                    defaultGraphics.SetActive(true);
                    wand.SetActive(true);
                }

                HideButtonSequence();
                StopCoroutine(nameof(CountDown));
            }
        }

        private void ProcessPlayerInputs()
        {
            if (PlayerSequenceTracker > (int)Difficulty - 1) return;

            /* Debug.Log(eventSystem.currentSelectedGameObject.name);

            for (int i = 0; i < buttonsInternalName.Count; i++)
            {
                if (eventSystem.currentSelectedGameObject.name == buttonsInternalName[i])
                {
                    Check(buttonsInternalName.IndexOf(buttonsInternalName[i]));
                }
            } */

            // highly professional code
            if (InputManager.GetKeyDown(ControllerKey.A))
            {
                Check(buttonsInternalName.IndexOf("NOC3_A"));
            }
            else if (InputManager.GetKeyDown(ControllerKey.B))
            {
                Check(buttonsInternalName.IndexOf("NOC3_B"));
            }
            else if (InputManager.GetKeyDown(ControllerKey.X))
            {
                Check(buttonsInternalName.IndexOf("NOC3_X"));
            }
            else if (InputManager.GetKeyDown(ControllerKey.Y))
            {
                Check(buttonsInternalName.IndexOf("NOC3_Y"));
            }
        }

        private void Check(int buttonIndex)
        {
            playerSequence[PlayerSequenceTracker] = (byte)buttonIndex;

            if (playerSequence[PlayerSequenceTracker] == internalSequence[PlayerSequenceTracker])
            {
                wand.transform.Rotate(new Vector3(0f, 0f, -40f / (int)Difficulty));
            }
            else
            {
                FailedSequenceBeforeEnd = true;
                HasWon = false;
            }

            PlayerSequenceTracker++;
        }

        private void HideButtonSequence()
        {
            for (int i = 0; i < buttonReferences.Length; i++)
            {
                buttonReferences[i].SetActive(false);
            }
        }

        public static bool checkEquality<T>(T[] first, T[] second)
        {
            return Enumerable.SequenceEqual(first, second);
        }

        private void ReceiveEngGameEvent()
        {
            if (!FailedSequenceBeforeEnd)
            {
                byte[] values = new byte[(byte)Difficulty];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = playerSequence[i];
                }

                HasWon = checkEquality(internalSequence, values);
            }

            // send to macro game
            // WIN
            if (HasWon)
            {
                wand.transform.rotation = Quaternion.Euler(0f, 0f, -5f); // badass audiovisual feedback with a "yeaaa" sound
                chicken.SetActive(true);
                defaultGraphics.SetActive(false);
                Instantiate(winVFX, transform.position, Quaternion.identity);
                NOC3_AudioManager.PlaySound(winSound.clip, winSound.volume, winSound.delay);
            }
            // LOSE
            else
            {
                NOC3_AudioManager.PlaySound(loseSound.clip, loseSound.volume, loseSound.delay);
            }

            GameState = GameState.Unloading;
        }
    }
}
