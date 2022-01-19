using System.Collections;
using UnityEngine;

public class ULC5_DeathBeam : MonoBehaviour {

    public static ULC5_DeathBeam instance;
    
    [SerializeField] private GameObject beamEffectPrefab;
    [SerializeField] private float beamTime;
    [SerializeField] private AudioClip blastKOSound;

    void Awake() {
        instance = this;
    }

    private GameObject beam;
    public void SummonBeam(Vector2 summonPoint, Vector2 direction) {
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        
        beam = Instantiate(beamEffectPrefab, summonPoint, Quaternion.Euler(0,0,angle), transform);
        AudioManager.PlaySound(blastKOSound,0.125f);
        StartCoroutine(BeamAnimation(beam));
    }

    private IEnumerator BeamAnimation(GameObject beam) {
        float popTime = beamTime*1/3;
        float fadeTime = beamTime*2/3;
        while (popTime > 0) {
            beam.transform.GetChild(0).transform.localScale += Vector3.up*0.01f;
            yield return new WaitForSeconds(Time.deltaTime);
            popTime -= Time.deltaTime;
            if (beam.transform.GetChild(0).transform.localScale.y >= 0.7f) popTime = 0;
        }

        float stepAlpha = fadeTime / Time.deltaTime;
        SpriteRenderer sp = beam.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        while (fadeTime > 0) {
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a - 1/stepAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
            fadeTime -= Time.deltaTime;
        }
        
        Destroy(beam);
    }
}
