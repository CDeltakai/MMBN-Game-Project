using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChipEffectBlueprint : MonoBehaviour
{

    protected PlayerMovement player;
    protected Transform firePoint;
    [SerializeField] public ChipSO chip;
    protected GameObject ObjectSummon;
    protected int BaseDamage;
    protected EStatusEffects BaseStatusEffect;
    protected int EnergyCost;

    public int DamageModifier = 0;
    public EStatusEffects StatusEffectModifier = EStatusEffects.Default;
    public GameObject SummonObjectModifier = null;
    public int EnergyCostModifier = 0;




    protected bool lightAttack;
    protected bool hitFlinch;
    protected bool pierceUntargetable;





    void Awake()
    {
        player = PlayerMovement.Instance;
        player = FindObjectOfType<PlayerMovement>();
        firePoint = player.firePoint;
        ObjectSummon = chip.GetObjectSummon();

        BaseDamage = chip.GetChipDamage();
        lightAttack = chip.IsLightAttack();
        hitFlinch = chip.IsHitFlinch();
        pierceUntargetable = chip.IsPierceUntargetable();
        EnergyCost = chip.EnergyCost;


    }




    public void applyChipDamage(BStageEntity entity)
    {

        if(StatusEffectModifier == EStatusEffects.Default)
        {
            StatusEffectModifier = BaseStatusEffect;
        }

        entity.hurtEntity((int)((BaseDamage + DamageModifier) * player.AttackMultiplier),
                            lightAttack,
                            hitFlinch, 
                            player, 
                            pierceUntargetable, 
                            StatusEffectModifier);


    }


    protected void SummonObjects()
    {
        
    }

    public abstract void Effect();



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
