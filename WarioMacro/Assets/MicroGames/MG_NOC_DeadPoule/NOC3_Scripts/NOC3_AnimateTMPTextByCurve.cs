using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace MiniGame.DeadPoule
{
    [RequireComponent(typeof(TMP_Text))]
    public class NOC3_AnimateTMPTextByCurve : AnimateObjectByCurve
    {
        [SerializeField] private Sound popSound;
        [SerializeField, Range(0f, 0.5f)] private float onAnimStartEventsDelay = 0.2f;
        private TMP_Text tmp;

        private void Start()
        {
            tmp = GetComponent<TMP_Text>();
            tmp.enabled = true;
            initialSize = tmp.fontSize;
            Invoke(nameof(CallAnimationStartEvents), onAnimStartEventsDelay);
        }

        void FixedUpdate()
        {
            if (timeTracker > Duration && !isDone)
            {
                isDone = true;
                OnAnimationEnding?.Invoke();
                return;
            }

            Animate();
            IncreaseTimer();
        }

        private void CallAnimationStartEvents()
        {
            NOC3_AudioManager.PlaySound(popSound.clip, popSound.volume, popSound.delay);
            StartCoroutine(nameof(ShowKeyworld));
        }

        public override void Animate()
        {
            tmp.fontSize = initialSize + NOC3_AnimCurveController.EaseOutElastic(timeTracker) * Amplitude;
        }

        System.Collections.IEnumerator ShowKeyworld()
        {
            yield return new WaitForSeconds(0.1f);
            OnAnimationStarting?.Invoke();
        }
    }

    public abstract class AnimateObjectByCurve : MonoBehaviour
    {
        [SerializeField, Range(1, 5)] private float duration = 2f;
        [SerializeField, Range(40, 60)] private float amplitude = 50f;
        protected float Amplitude { get; private set; }
        protected float Duration { get; private set; }

        protected float initialSize;
        protected float timeTracker;
        protected bool isDone;

        [SerializeField] protected UnityEvent OnAnimationStarting;
        [SerializeField] protected UnityEvent OnAnimationEnding;

        private void Awake()
        {
            Amplitude = amplitude;
            Duration = duration;
        }

        /// <summary>
        /// Call this AFTER Animate
        /// </summary>
        protected void IncreaseTimer()
        {
            timeTracker += Time.fixedDeltaTime / duration;
        }

        /// <summary>
        ///  Logic for animation, chosing a custom AnimCurveController curve
        /// </summary>
        public abstract void Animate();
    }
}
