using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EffectPropertyContainer
{

    public int DamageModifier;
    public EStatusEffects StatusEffectModifier;
    public bool lightAttack;
    public bool hitFlinch;
    public bool pierceUntargetable; 


    public EffectPropertyContainer(int dmgMod, EStatusEffects statEffect, bool lightAttack, bool hitFlinch, bool pierceUntargetable)
    {
        DamageModifier = dmgMod;
        StatusEffectModifier = statEffect;
        this.lightAttack = lightAttack;
        this.hitFlinch = hitFlinch;
        this.pierceUntargetable = pierceUntargetable;


    }


}


public abstract class ChipEffectBlueprint : MonoBehaviour
{

    public delegate void TriggeredEffectEvent();
    public event TriggeredEffectEvent triggeredEffect;

    public PlayerMovement player;
    protected Transform firePoint;
    [SerializeField] public ChipSO chip;
    protected UnityEngine.GameObject ObjectSummon;
    protected int BaseDamage;
    protected EStatusEffects BaseStatusEffect;
    protected int EnergyCost;

    public UnityEngine.GameObject SummonObjectModifier = null;
    public int EnergyCostModifier = 0;
    public List<Vector2Int> RangeModifier = new List<Vector2Int>();




    public int DamageModifier = 0;
    public EStatusEffects StatusEffectModifier = EStatusEffects.Default;
    protected bool lightAttack;
    protected bool hitFlinch;
    protected bool pierceUntargetable;

    protected EffectPropertyContainer effectProperties = new EffectPropertyContainer(0, EStatusEffects.Default, false, false, false);





    void Awake()
    {
        //player = PlayerMovement.Instance;

        ObjectSummon = chip.GetObjectSummon();

        BaseDamage = chip.GetChipDamage();
        lightAttack = chip.IsLightAttack();
        hitFlinch = chip.IsHitFlinch();
        pierceUntargetable = chip.IsPierceUntargetable();
        EnergyCost = chip.EnergyCost;

        effectProperties.DamageModifier = DamageModifier;
        effectProperties.StatusEffectModifier = StatusEffectModifier;
        effectProperties.hitFlinch = hitFlinch;
        effectProperties.pierceUntargetable = pierceUntargetable;

    }

    private void Start() 
    {
        if(player == null)
        {
            print("Chip: " + chip.GetChipName() + " did not have its player owner set on instantiation - automatically set player owner." 
            + " This is normally not a problem, however it is less efficient than setting the player owner on instantiation." );
            player = FindObjectOfType<PlayerMovement>();
        }

        firePoint = player.firePoint;        

        
    }

    private void OnEnable() 
    {
        
    }

    public int calcFinalDamage()
    {
        int finalDamage = (int)((BaseDamage + DamageModifier) * player.AttackMultiplier);
        return finalDamage;        
    }

    public void applyChipDamage(BStageEntity entity)
    {

        if(StatusEffectModifier == EStatusEffects.Default)
        {
            StatusEffectModifier = BaseStatusEffect;
        }

        int finalDamage = (int)((BaseDamage + effectProperties.DamageModifier) * player.AttackMultiplier);

        entity.hurtEntity(finalDamage,
                            effectProperties.lightAttack,
                            effectProperties.hitFlinch, 
                            player, 
                            effectProperties.pierceUntargetable, 
                            effectProperties.StatusEffectModifier);

    }

    


    protected void SummonObjects()
    {
        
    }

    public abstract void Effect();

    public virtual void OnActivationEffect(BStageEntity target){}


    void OnDisable()
    {
        BaseDamage = chip.GetChipDamage();
        lightAttack = chip.IsLightAttack();
        hitFlinch = chip.IsHitFlinch();
        pierceUntargetable = chip.IsPierceUntargetable();
        DamageModifier = 0;
        StatusEffectModifier = EStatusEffects.Default;
        SummonObjectModifier = null;
        EnergyCostModifier = 0;



    }



}
