using System.Collections;
using UnityEditor;
using UnityEngine;

public class ULC5_Snipe : MonoBehaviour
{
    private Transform player;

    [SerializeField] private SpriteRenderer[] sightSps;
    [SerializeField] private SpriteRenderer[] impactSps;
    
    [SerializeField] private float targetTime;
    [SerializeField] private float timeBeforeSnipe;
    [SerializeField] private float impactTime;

    void Awake() {
        player = FindObjectOfType<ULC5_Player>().transform;
    }
    
    void Start() {
        StartCoroutine(TargetCoroutine());
    }

    void Update() {
        if (targetTime > 0) transform.position = player.position;
    }

    private IEnumerator TargetCoroutine() {
        
        while (targetTime > 0)
        {
            float stepMove = targetTime / Time.deltaTime;
            for (int i = 0; i < sightSps.Length; i++)
            {
                sightSps[i].color = new Color(1, 1, 1, Mathf.Clamp(sightSps[i].color.a+0.02f,0,0.5f));
                Vector3 newPos = (transform.position - sightSps[i].transform.position)/stepMove;
                if (newPos.magnitude > 0f) sightSps[i].transform.localPosition += newPos;
            }
            
            yield return new WaitForSeconds(Time.deltaTime);
            targetTime -= Time.deltaTime;
        }

        //Make sure all pos are correctly set
        for (int i = 0; i < sightSps.Length; i++) sightSps[i].transform.position = transform.position;
        StartCoroutine(SnipeCoroutine());
    }

    private IEnumerator SnipeCoroutine() {
        for (int i = 0; i < sightSps.Length; i++) sightSps[i].color = Color.white;
        yield return new WaitForSeconds(timeBeforeSnipe);
        for (int i = 0; i < sightSps.Length; i++) sightSps[i].sprite = null;

        impactSps[0].transform.position = new Vector3(sightSps[0].transform.position.x,0,0);
        impactSps[1].transform.position = new Vector3(0,sightSps[0].transform.position.y,0);

        impactSps[0].enabled = true;
        impactSps[1].enabled = true;
        
        float stepAlpha = impactTime / Time.deltaTime;
        impactSps[0].GetComponent<BoxCollider2D>().enabled = true;
        impactSps[1].GetComponent<BoxCollider2D>().enabled = true;
        while (impactTime > 0)
        {
            impactSps[0].color = new Color(1, 1, 1, impactSps[0].color.a - 1/stepAlpha);
            impactSps[1].color = new Color(1, 1, 1, impactSps[0].color.a);
            yield return new WaitForSeconds(Time.deltaTime);
            impactTime -= Time.deltaTime;
        }
        
        Destroy(gameObject);
    }
}
