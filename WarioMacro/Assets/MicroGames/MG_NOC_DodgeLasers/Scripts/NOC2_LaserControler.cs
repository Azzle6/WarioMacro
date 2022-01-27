using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using Random = UnityEngine.Random;

public class NOC2_LaserControler : MonoBehaviour
{
    [HideInInspector] public int difficulty;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector3[] cameraBounderies;
    [HideInInspector] public int cameraBounderiesIndex;
    [HideInInspector] public float startAngle;
    [SerializeField] private float[] speed, rotationSpeed, colorUpdateSpeed, colorEnd1Speed, colorEnd2Speed;
    public float minRot, maxRot;
    [SerializeField] private LineRenderer laser;
    [SerializeField] private GameObject laserPoint;
    [HideInInspector] public float currentAngle;
    [SerializeField] private Color StartColor1, StartColor2, EndColor1, EndColor2;
    private Coroutine limitCheckCoroutine, limitRotationCoroutine, colorEndCoroutine;
    private float timeElapsed = 0;
    private bool playerHere, shooted;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") playerHere = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") playerHere = false;
    }

    void Update()
    {
        if (NOC2_LaserManager.instance.currentTick < 5)
        {
            Move();
            DrawLaserUpdate();
            ColorUpdate();
        }
        else
        {
            if (colorEndCoroutine == null) colorEndCoroutine = StartCoroutine(ColorEnd());
        }
    }

    private void Move()
    {
        if (Time.timeScale == 0) return;
        //Bouger 
        transform.localPosition += direction * Time.deltaTime * 1 / Time.timeScale * speed[difficulty];

        //Vérifier la position por rapport aux bordures de l'écran et changer de direction si trop proche de la bordure
        if (limitCheckCoroutine == null) limitCheckCoroutine = StartCoroutine(LimitPositionCheck());
        if (difficulty == 4)
            if (Random.Range(1, 1000) == 500)
                direction *= -1;
            //Rotation
            if (difficulty == 0) currentAngle = 0;
        transform.rotation = Quaternion.Euler(0, 0, startAngle + currentAngle);
        currentAngle += Time.deltaTime * 1 / Time.timeScale * rotationSpeed[difficulty];

        // Vérifier si la rotation à atteint son degre maximal
        if (limitRotationCoroutine == null) limitRotationCoroutine = StartCoroutine(LimitRotation());
        if (difficulty == 2)
            if (Random.Range(1, 1000) == 500)
                rotationSpeed[difficulty] *= -1;
    }

    private IEnumerator LimitPositionCheck()
    {
        while (this.enabled)
        {
            if (direction.x == 0)
            {
                if (Mathf.Abs(transform.localPosition.y - Camera.main.transform.position.y) > Mathf.Abs(cameraBounderies[0].y))
                {
                    direction *= -1;
                    yield return new WaitUntil(() =>
                        Mathf.Abs(transform.localPosition.y - Camera.main.transform.position.y) < Mathf.Abs(cameraBounderies[0].y));
                }

            }
            else
            {
                if (Mathf.Abs(transform.localPosition.x - Camera.main.transform.position.x) > Mathf.Abs(cameraBounderies[1].x))
                {
                    direction *= -1;
                    yield return new WaitUntil(() =>
                        Mathf.Abs(transform.localPosition.x - Camera.main.transform.position.x) < Mathf.Abs(cameraBounderies[1].x));
                }
            }
            yield return null;
        }
    }

    private IEnumerator LimitRotation()
    {
        while (this.enabled)
        {
            if (currentAngle <= minRot || currentAngle >= maxRot)
            {
                rotationSpeed[difficulty] *= -1;
                yield return new WaitUntil(() => currentAngle > minRot && currentAngle < maxRot);
            }
            yield return null;   
        }
    }

    private void DrawLaserUpdate()
    {
        float maxDuration = 4;

        for (int i = 1; i < GetComponentsInChildren<SpriteRenderer>().Length; i++)
        {
            //Debug.Log(timeElapsed);
            Vector2 _maxSize = Vector2.up * 2f *
                (Vector2.Distance(-cameraBounderies[cameraBounderiesIndex], cameraBounderies[cameraBounderiesIndex]));
            GetComponentsInChildren<SpriteRenderer>()[i].size = Vector2.Lerp(new Vector2(0.58f, 0), _maxSize + new Vector2(0.58f, 5f), timeElapsed / (maxDuration / 2f));
        }

        if (timeElapsed >= maxDuration / 2 && NOC2_LaserManager.instance.currentTick > 0)
        {
            laser.enabled = true;
            laser.SetColors(StartColor1, StartColor1);
            laser.SetPosition(0, transform.position);
            
            Vector2 _maxSize = new Vector2(0, 5) + Vector2.up * 4f * Vector2.Distance(-cameraBounderies[cameraBounderiesIndex], cameraBounderies[cameraBounderiesIndex]);
            
            laserPoint.transform.localPosition =
                Vector2.Lerp(Vector2.zero, _maxSize, (timeElapsed - (maxDuration / 2f)) / (maxDuration / 2f));
            laser.SetPosition(1, laserPoint.transform.position);
        }

        if (timeElapsed >= 4 - NOC2_LaserManager.instance.soundDelay[GameController.difficulty - 1] && !shooted)
        {
            shooted = true;
            NOC2_LaserManager.instance.PlaySound(GameController.difficulty - 1);
        }
        
        timeElapsed += Time.deltaTime;
    }

    private void ColorUpdate()
    {
        Color currentColor = StartColor1;
        currentColor = ColorLimitation(StartColor1 + (StartColor2 * Mathf.Sin(timeElapsed * colorUpdateSpeed[difficulty])), StartColor1, StartColor2);
        for (int i = 1; i < GetComponentsInChildren<SpriteRenderer>().Length; i++)
        {
            GetComponentsInChildren<SpriteRenderer>()[i].color = currentColor;
        }
    }

    private IEnumerator ColorEnd()
    {
        bool result = false;
        if (playerHere) result = true;
        NOC2_LaserManager.instance.AddResult(result);
        
        laserPoint.transform.localPosition = Vector2.up * ((Vector2.Distance(Vector2.zero, cameraBounderies[cameraBounderiesIndex] * 16)));
        laser.SetPosition(1, laserPoint.transform.position);
        laser.SetPosition(0, transform.position);
        for (int i = 1; i < GetComponentsInChildren<SpriteRenderer>().Length - 1; i++)
        {
            GetComponentsInChildren<SpriteRenderer>()[i].enabled = false;
        }

        while (NOC2_LaserManager.instance.currentTick >= 5)
        {
            laser.SetColors(EndColor1, EndColor1);
            yield return new WaitForSecondsRealtime(colorEnd1Speed[0]);
            laser.SetColors(EndColor2, EndColor2);
            yield return new WaitForSecondsRealtime(colorEnd2Speed[0]);
        }

        laser.enabled = false;
        colorEndCoroutine = null;
    }

    private Color ColorLimitation(Color currentColor, Color minColor, Color maxColor)
    {
        Color outColor = Color.black;
        outColor.r = Mathf.Clamp(currentColor.r, minColor.r, maxColor.r);
        outColor.g = Mathf.Clamp(currentColor.g, minColor.g, maxColor.g);
        outColor.b = Mathf.Clamp(currentColor.b, minColor.b, maxColor.b);
        outColor.a = Mathf.Clamp(currentColor.a, minColor.a, maxColor.a);
        return outColor;
    }
}
