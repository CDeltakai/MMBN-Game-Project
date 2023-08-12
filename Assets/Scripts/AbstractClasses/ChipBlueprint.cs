using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EffectProperties
{

    public int DamageModifier;
    public EStatusEffects StatusEffectModifier;
    public List<EStatusEffects> AdditionalStatusEffects;
    public bool lightAttack;
    public bool hitFlinch;
    public bool pierceUntargetable; 
    public AttackElement attackElement;


    public EffectProperties(int dmgMod, 
                            EStatusEffects statEffect, 
                            List<EStatusEffects> additionalStatEffects, 
                            bool lightAttack, 
                            bool hitFlinch, 
                            bool pierceUntargetable, 
                            AttackElement attackElement)
    {
        DamageModifier = dmgMod;
        StatusEffectModifier = statEffect;
        AdditionalStatusEffects = additionalStatEffects;
        this.lightAttack = lightAttack;
        this.hitFlinch = hitFlinch;
        this.pierceUntargetable = pierceUntargetable;
        this.attackElement = attackElement;


    }


}


public abstract class ChipEffectBlueprint : MonoBehaviour
{

    public delegate void TriggeredEffectEvent();
    public event TriggeredEffectEvent triggeredEffect;

    
    public PlayerMovement player;
    protected Transform firePoint;
    [SerializeField] public ChipSO chip;
    protected GameObject ObjectSummon;
    [HideInInspector]
    public int BaseDamage{get; protected set;}
    protected EStatusEffects BaseStatusEffect;
    protected int EnergyCost;

    public GameObject SummonObjectModifier = null;
    public int EnergyCostModifier = 0;
    public List<Vector2Int> RangeModifier = new List<Vector2Int>();




    public int DamageModifier = 0;
    public EStatusEffects StatusEffectModifier;
    public List<EStatusEffects> AdditionalStatusEffects = new List<EStatusEffects>();
    protected bool lightAttack;
    protected bool hitFlinch;
    protected bool pierceUntargetable;

    public EffectProperties effectProperties = new EffectProperties(0, 
                                                                    EStatusEffects.Default, 
                                                                    new List<EStatusEffects>(),  
                                                                    false, 
                                                                    false, 
                                                                    false, 
                                                                    AttackElement.Normal);



    protected virtual void AdditionalAwakeEvents(){}

    void Awake()
    {

        ObjectSummon = chip.GetObjectSummon();
        BaseStatusEffect = chip.GetStatusEffect();
        BaseDamage = chip.GetChipDamage();
        lightAttack = chip.IsLightAttack();
        hitFlinch = chip.IsHitFlinch();
        pierceUntargetable = chip.IsPierceUntargetable();
        EnergyCost = chip.EnergyCost;

        effectProperties.DamageModifier = DamageModifier;
        effectProperties.StatusEffectModifier = BaseStatusEffect;
        effectProperties.hitFlinch = hitFlinch;
        effectProperties.pierceUntargetable = pierceUntargetable;


        AdditionalAwakeEvents();

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

    /// <summary>
    /// Wrapper method for applying damage to an entity with an attack payload
    /// </summary>
    /// <param name="entity"></param>
    public void applyChipDamage(BStageEntity entity)
    {

        if(StatusEffectModifier == EStatusEffects.Default)
        {
            StatusEffectModifier = BaseStatusEffect;
        }

        int finalDamage = (int)((BaseDamage + effectProperties.DamageModifier) * player.AttackMultiplier);

        AttackPayload attackPayload = new AttackPayload(finalDamage,
                                                        effectProperties.lightAttack,
                                                        effectProperties.hitFlinch,
                                                        effectProperties.pierceUntargetable,
                                                        player,
                                                        effectProperties.StatusEffectModifier,
                                                        effectProperties.AdditionalStatusEffects,
                                                        chip.GetChipElement());

        entity.hurtEntity(attackPayload);

    }

    


    protected void SummonObjects()
    {
        
    }


    ///<summary>
    ///The main function that gets called whenever the chip is activated. This method should contain all the necessary statements
    ///and calls to make the chip work. 
    ///</summary>
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
        AdditionalStatusEffects.Clear();
        SummonObjectModifier = null;
        EnergyCostModifier = 0;



    }



}
