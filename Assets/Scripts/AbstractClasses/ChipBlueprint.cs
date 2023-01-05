using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChipEffectBlueprint : MonoBehaviour
{

    protected PlayerMovement player;
    protected Transform firePoint;
    [SerializeField] protected ChipSO chip;
    protected GameObject ObjectSummon;
    protected int BaseDamage;
    protected EStatusEffects BaseStatusEffect;

    public int AddDamage = 0;
    public EStatusEffects AddStatusEffect = EStatusEffects.Default;
    public GameObject AddSummonObject = null;




    protected bool lightAttack;
    protected bool hitFlinch;
    protected bool pierceUntargetable;





    void Awake()
    {
        player = PlayerMovement.Instance;
        //player = FindObjectOfType<PlayerMovement>();
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
        AddDamage = 0;
        AddStatusEffect = EStatusEffects.Default;
        AddSummonObject = null;



    }



}
