using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSummonAttributes : MonoBehaviour
{
    public int AddDamage = 0;

    public EStatusEffects StatusEffect;
    public EStatusEffects AddStatusEffect = EStatusEffects.Default;
    public List<EStatusEffects> AdditionalStatusEffects = new List<EStatusEffects>();
    public UnityEngine.GameObject AddObjectSummon = null;
    public ChipSO InheritedChip;
    public ChipEffectBlueprint InheritedChipPrefab;

    public virtual void ResetAttributesToInitialState()
    {
        AddDamage = 0;
        StatusEffect = InheritedChip.GetStatusEffect();
        AddStatusEffect = EStatusEffects.Default;
        AdditionalStatusEffects.Clear();
        AddObjectSummon = null;
    }


    public void applyDamage(BStageEntity entity)
    {
        EffectProperties effectProperties = InheritedChipPrefab.effectProperties;

        int finalDamage = (int)((InheritedChipPrefab.BaseDamage + effectProperties.DamageModifier) * InheritedChipPrefab.player.AttackMultiplier);
        print("Chip element used: " + InheritedChip.GetChipElement());

        AttackPayload attackPayload = new AttackPayload(finalDamage,
                                                        effectProperties.lightAttack,
                                                        effectProperties.hitFlinch,
                                                        effectProperties.pierceUntargetable,
                                                        InheritedChipPrefab.player,
                                                        effectProperties.StatusEffectModifier,
                                                        AdditionalStatusEffects,
                                                        InheritedChip.GetChipElement());

        entity.HurtEntity(attackPayload);


    }

    private void OnDisable()
    {
        ResetAttributesToInitialState();   
    }


}
