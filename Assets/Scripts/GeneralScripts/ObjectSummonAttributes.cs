using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSummonAttributes : MonoBehaviour
{
    public int AddDamage = 0;

    public EStatusEffects StatusEffect;
    public EStatusEffects AddStatusEffect = EStatusEffects.Default;
    public UnityEngine.GameObject AddObjectSummon = null;
    public ChipSO InheritedChip;
    public ChipEffectBlueprint InheritedChipPrefab;

    public virtual void ResetAttributesToInitialState()
    {
        AddDamage = 0;
        StatusEffect = InheritedChip.GetStatusEffect();
        AddStatusEffect = EStatusEffects.Default;
        AddObjectSummon = null;
    }





}
