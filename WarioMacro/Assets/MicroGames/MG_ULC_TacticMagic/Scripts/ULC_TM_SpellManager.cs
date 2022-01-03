using UnityEngine;

public class ULC_TM_SpellManager : MonoBehaviour
{

    public static ULC_TM_SpellManager instance;
    private GameObject spell;
    
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private Transform[] enemySpawns;

    void Awake()
    {
        instance = this;
    }
    
    public void TopSpell(Object caster)
    {
        spell = ULC_TM_Pooler.instance.Pop("Spell");
        
        if (caster.GetType() == typeof(ULC_TM_Player)) PrepareSpell(playerSpawns[0].position, Vector3.right, caster);
        else if (caster.GetType() == typeof(ULC_TM_Enemy)) PrepareSpell(enemySpawns[0].position, -Vector3.right, caster);
        
        //spell.GetComponent<SpriteRenderer>().color = new Color(236/255f,219/255f,51/255f);
    }
    
    public void MidSpell(Object caster)
    {
        spell = ULC_TM_Pooler.instance.Pop("Spell");
        
        if (caster.GetType() == typeof(ULC_TM_Player)) PrepareSpell(playerSpawns[1].position, Vector3.right, caster);
        else if (caster.GetType() == typeof(ULC_TM_Enemy)) PrepareSpell(enemySpawns[1].position, -Vector3.right, caster);
        
        //spell.GetComponent<SpriteRenderer>().color = new Color(208/255f,66/255f,66/255f);
    }
    
    public void BotSpell(Object caster)
    {
        spell = ULC_TM_Pooler.instance.Pop("Spell");
        
        if (caster.GetType() == typeof(ULC_TM_Player)) PrepareSpell(playerSpawns[2].position, Vector3.right, caster);
        else if (caster.GetType() == typeof(ULC_TM_Enemy)) PrepareSpell(enemySpawns[2].position, -Vector3.right, caster);
        
        //spell.GetComponent<SpriteRenderer>().color = new Color(60/255f,219/255f,78/255f);
    }

    public void PrepareSpell(Vector3 startPos, Vector3 direction, Object caster)
    {
        spell.transform.position = startPos;
        spell.GetComponent<ULC_TM_Spell>().direction = direction;
        spell.GetComponent<ULC_TM_Spell>().caster = caster;
    }
}
