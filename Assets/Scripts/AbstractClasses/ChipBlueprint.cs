using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChipEffectBlueprint : MonoBehaviour
{

    public PlayerMovement player;
    protected Transform firePoint;
    [SerializeField] public ChipSO chip;
    protected UnityEngine.GameObject ObjectSummon;
    protected int BaseDamage;
    protected EStatusEffects BaseStatusEffect;
    protected int EnergyCost;

    public int DamageModifier = 0;
    public EStatusEffects StatusEffectModifier = EStatusEffects.Default;
    public UnityEngine.GameObject SummonObjectModifier = null;
    public int EnergyCostModifier = 0;




    protected bool lightAttack;
    protected bool hitFlinch;
    protected bool pierceUntargetable;





    void Awake()
    {
        //player = PlayerMovement.Instance;

        ObjectSummon = chip.GetObjectSummon();

        BaseDamage = chip.GetChipDamage();
        lightAttack = chip.IsLightAttack();
        hitFlinch = chip.IsHitFlinch();
        pierceUntargetable = chip.IsPierceUntargetable();
        EnergyCost = chip.EnergyCost;


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

        int finalDamage = (int)((BaseDamage + DamageModifier) * player.AttackMultiplier);

        entity.hurtEntity(finalDamage,
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
