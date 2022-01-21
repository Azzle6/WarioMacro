using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA2_Surveillance_CameraDetect : MonoBehaviour
{
    [SerializeField]
    float reactionTime;
    float reactedTime;
    [SerializeField]
    float unreactTime;
    float unreactedTime;
    [SerializeField]
    float detectTime;
    float detectedTime;
    [SerializeField]
    GameObject camSprite;
    [SerializeField]
    GameObject player;
    [SerializeField]
    ContactFilter2D contactFilter;

    RaycastHit2D[] results = new RaycastHit2D[1];

    [SerializeField]
    int result;
    public bool defeat = false;
    bool detect;

    [Header("Field of View")]
    [SerializeField]
    float fov;
    [SerializeField]
    float viewDistance, angle;
    [SerializeField]
    int rayCount;

    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    Mesh mesh;
    MeshFilter mf;

    [SerializeField]
    Vector3 origin = Vector3.zero;
    [SerializeField]
    GameObject fovStart;

    public LayerMask lm;

    float angleIncrease;

    [SerializeField] AudioClip detecting, alarm;
    bool playingSound;
    private void Start()
    {
        //mf = GetComponent<MeshFilter>();
        angleIncrease = fov / rayCount;

       
    }

    private void Update()
    {
        if (!detect)
        {
            unreactTime -= Time.deltaTime;
            if(unreactTime <= 0)
            {
                AudioManager.StopSound(detecting, 0);
                playingSound = false;
            }
        }
    }

    Vector3 VectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    void VerifyHiding()
    {
        result = Physics2D.Raycast(camSprite.transform.position, player.transform.position - camSprite.transform.position, contactFilter, results);
        Debug.DrawRay(camSprite.transform.position, player.transform.position - camSprite.transform.position, Color.red, 4);

        if(results[0].collider.gameObject == player)
        {
            detectedTime += Time.deltaTime;
        }

        if (detectedTime >= detectTime)
        {
            AudioManager.StopSound(detecting, 0);
            AudioManager.PlaySound(alarm, 0.1f, 0);
            defeat = true;

        }

    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            detect = true;
            reactedTime += Time.deltaTime;
            unreactTime = reactedTime;
            if (!playingSound)
            {
                playingSound = true;
                AudioManager.PlaySound(detecting, 0.5f);
            }
            if (reactedTime >= reactionTime)
            {
                VerifyHiding();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            detect = false;
        }
    }
}
