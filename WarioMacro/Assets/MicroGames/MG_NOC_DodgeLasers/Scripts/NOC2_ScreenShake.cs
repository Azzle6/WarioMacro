using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class NOC2_ScreenShake : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    public void ScreenShaking(float duration)
    {
        StartCoroutine(ShakeUpdate(duration));
    }

    private IEnumerator ShakeUpdate(float duration)
    {
        Vector2 startPosition = Camera.main.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * Time.timeScale;
            float strength = curve.Evaluate(elapsedTime / duration);
            Camera.main.transform.position = startPosition + Random.insideUnitCircle * strength;
            Camera.main.transform.position += new Vector3(0, 0, -10);
            yield return null;
        }

        Camera.main.transform.position = startPosition;
        Camera.main.transform.position += new Vector3(0, 0, -10);
    }
}
