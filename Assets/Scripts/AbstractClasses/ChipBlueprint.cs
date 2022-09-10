using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChipBlueprint : MonoBehaviour
{

    protected PlayerMovement player;
    protected Transform firePoint;
    [SerializeField] protected ChipSO chip;
    protected GameObject ObjectSummon;
    protected int BaseDamage;
    public int AddDamage = 0;

    protected EStatusEffects BaseStatusEffect;
    public EStatusEffects AddStatusEffect = EStatusEffects.Default;

    protected bool lightAttack;
    protected bool hitFlinch;
    protected bool pierceUntargetable;





    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        firePoint = player.firePoint;
        ObjectSummon = chip.GetObjectSummon();

        BaseDamage = chip.GetChipDamage();
        lightAttack = chip.IsLightAttack();
        hitFlinch = chip.IsHitFlinch();
        pierceUntargetable = chip.IsPierceUntargetable();

    }




    public void applyChipDamage(BStageEntity entity)
    {
        EStatusEffects status;
        if(AddStatusEffect != EStatusEffects.Default)
        {
            status = BaseStatusEffect;
        }else
        {
            status = AddStatusEffect;
        }

        entity.hurtEntity((int)((BaseDamage + AddDamage) * player.AttackMultiplier),
                            lightAttack,
                            hitFlinch, 
                            player, 
                            pierceUntargetable, 
                            status);


    }


    public abstract void Effect();



    void OnDisable()
    {
        BaseDamage = chip.GetChipDamage();
        lightAttack = chip.IsLightAttack();
        hitFlinch = chip.IsHitFlinch();
        pierceUntargetable = chip.IsPierceUntargetable();
        AddDamage = 0;
        AddStatusEffect = EStatusEffects.Default;



    }



}
