using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA3_LineController_LAC : MonoBehaviour
{
    public NAA3_MicroGameController_LAC MC_Controller;
    public LineRenderer line;
    public Transform startPoint, endPoint;
    public float bpmFactor; // between 0 & 1
    [Range(0, 3)]
    public float baseAmp, baseFreq;
    public float ampMult, freqMult;

    float amp, freq;
    

    float freqTimer = 0;
    // Update is called once per frame
    public float minBlinkFreq = 0.2f, maxBlinkFreq = 1.5f;

    public float blinkDuration = 0.2f;
    [HideInInspector]
    public float blinkFreq ;

    float blinkTime;
    public bool isBlinking;
    private void Start()
    {
        blinkTime = Random.Range(0, blinkFreq);
    }
    void Update()
    {
        

        bpmFactor = Mathf.Clamp((MC_Controller.currentBpm() / MC_Controller.minimumBpmRequire), 0, 1);
        

        amp = baseAmp * Mathf.Lerp(1, baseAmp, bpmFactor);
        freq = baseFreq * Mathf.Lerp(1, baseFreq, bpmFactor);
        blinkDuration = 0.2f;
        freqTimer += Time.unscaledDeltaTime;
        if (freqTimer > freq)
        {
            UpdateRopePoint();
            freqTimer = 0;
        }
        
        blinkFreq = Mathf.Lerp(minBlinkFreq, maxBlinkFreq,1 - bpmFactor );
        if(Time.time - (blinkTime/Time.timeScale) > ((isBlinking)?blinkDuration : blinkFreq))
        {
            //Debug.Log("Bmp Ratio : " + MC_Controller.currentBpm());
            blinkTime = Time.time;
            isBlinking = !isBlinking;

            line.enabled = !isBlinking;

        }

        if (!MC_Controller.isPlaying && !!MC_Controller.result)
            Unchained();
    }


    void UpdateRopePoint()
    {
        Vector3 startEnd = (startPoint.position - endPoint.position);
        for (int i = 0; i < line.positionCount; i++)
        {
            Vector3 pos = startEnd * ((float)i / (line.positionCount - 1)) + endPoint.position;

            if (i != 0 && i != line.positionCount - 1)
                pos = pos + new Vector3(-startEnd.y, startEnd.x).normalized * amp * (Random.value - 0.5f);

            line.SetPosition(i, pos);
        }
    }

    void Unchained()
    {
        line.enabled = false;
    }
}
