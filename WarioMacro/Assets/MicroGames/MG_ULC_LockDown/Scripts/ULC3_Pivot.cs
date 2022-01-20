using UnityEngine;

public class ULC3_Pivot : MonoBehaviour
{
    
    [SerializeField] private float anglePerStep;
    [SerializeField] private int numberOfSteps;
    
    [HideInInspector] public int currentPos = 0;
    [HideInInspector] public int unlockPos;

    public bool isSelected = false;
    
    [HideInInspector] public Vector2 leftStickInput;
    [HideInInspector] public Vector2 lastStickInput;

    private float totalAngle = 0;

    [SerializeField] private AnimationCurve pitchCurve;

    void Start()
    {
        unlockPos = Random.Range(1, numberOfSteps);
    }
    
    void FixedUpdate()
    {
        if (isSelected)
        {
            if (lastStickInput != leftStickInput)
            {
                float angle = Vector2.SignedAngle(lastStickInput, leftStickInput);
                totalAngle += angle;
            
                //If the stick has been turn enough to pass a step
                if (totalAngle >= anglePerStep || totalAngle <= -anglePerStep)
                {
                    if (totalAngle >= anglePerStep)
                    {
                        currentPos++;
                        transform.Rotate(transform.forward, -360f/numberOfSteps);
                    }
                    else
                    {
                        currentPos--;
                        transform.Rotate(transform.forward, 360f/numberOfSteps);
                    }

                    //Clamp currentPos between 0 and the total number of steps
                    if (currentPos == numberOfSteps) currentPos = 0;
                    else if (currentPos < 0) currentPos = numberOfSteps - 1;
                
                    //Check if we're on the unlock position
                    if (currentPos == unlockPos) ULC3_AudioManager.instance.PlaySFX("Yes",1);
                    else ULC3_AudioManager.instance.PlaySFX("Nope",1+GetPitchFromInterval());
                    
                    totalAngle = 0;
                }
            }
        }
    }

    //Return a pitch value modulation based on the distance between current position and unlock position
    private float GetPitchFromInterval()
    {
        float distance;
        if (Mathf.Abs(currentPos - unlockPos) <= numberOfSteps / 2) distance = Mathf.Abs(currentPos - unlockPos);
        else distance = numberOfSteps - Mathf.Abs(currentPos - unlockPos);

        return pitchCurve.Evaluate(distance);
    }
}
