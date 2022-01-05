using UnityEngine;

public class ULC_TM_SpellManager : MonoBehaviour
{

    public static ULC_TM_SpellManager instance;
    private GameObject spell;
    
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private Transform[] enemySpawns;

    [SerializeField] private Sprite yLetter, bLetter, aLetter;
    //[SerializeField] private AudioClip ySound, bSound, aSound;

    void Awake()
    {
        instance = this;
    }
    
    public void TopSpell(Object caster)
    {
        spell = ULC_TM_Pooler.instance.Pop("Spell");
        
        if (caster.GetType() == typeof(ULC_TM_Player)) PrepareSpell(playerSpawns[0].position, Vector3.right, caster);
        else if (caster.GetType() == typeof(ULC_TM_Enemy)) PrepareSpell(enemySpawns[0].position, -Vector3.right, caster);

        spell.GetComponent<SpriteRenderer>().sprite = yLetter;
        spell.GetComponent<TrailRenderer>().startColor = new Color(241f / 255f, 231f / 255f, 70f / 255f);
        spell.GetComponent<TrailRenderer>().endColor = Color.white;
        //AudioManager.PlaySound(ySound,0.5f);
    }
    
    public void MidSpell(Object caster)
    {
        spell = ULC_TM_Pooler.instance.Pop("Spell");
        
        if (caster.GetType() == typeof(ULC_TM_Player)) PrepareSpell(playerSpawns[1].position, Vector3.right, caster);
        else if (caster.GetType() == typeof(ULC_TM_Enemy)) PrepareSpell(enemySpawns[1].position, -Vector3.right, caster);

        spell.GetComponent<SpriteRenderer>().sprite = bLetter;
        spell.GetComponent<TrailRenderer>().startColor = new Color(248f / 255f, 68f / 255f, 74f / 255f);
        spell.GetComponent<TrailRenderer>().endColor = Color.white;
        //AudioManager.PlaySound(bSound,0.5f);
    }
    
    public void BotSpell(Object caster)
    {
        spell = ULC_TM_Pooler.instance.Pop("Spell");
        
        if (caster.GetType() == typeof(ULC_TM_Player)) PrepareSpell(playerSpawns[2].position, Vector3.right, caster);
        else if (caster.GetType() == typeof(ULC_TM_Enemy)) PrepareSpell(enemySpawns[2].position, -Vector3.right, caster);

        spell.GetComponent<SpriteRenderer>().sprite = aLetter;
        spell.GetComponent<TrailRenderer>().startColor = new Color(135f / 255f, 219f / 255f, 21f / 255f);
        spell.GetComponent<TrailRenderer>().endColor = Color.white;
        //AudioManager.PlaySound(aSound,0.5f);
    }

    public void PrepareSpell(Vector3 startPos, Vector3 direction, Object caster)
    {
        spell.transform.position = startPos;
        spell.GetComponent<ULC_TM_Spell>().direction = direction;
        spell.GetComponent<ULC_TM_Spell>().caster = caster;
    }
}
