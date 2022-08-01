using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Longsword : MonoBehaviour, IChip
{
    public Transform firePoint;
    public int BaseDamage {get;set;} = 80;

    public int AdditionalDamage{get; set;} = 0;

    public EChipTypes ChipType => EChipTypes.Active;
    public EStatusEffects statusEffect {get;set;} = EStatusEffects.Default;
    public EChipElements chipElement => EChipElements.Blade;

    PlayerMovement player;

    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default)
    {
        throw new System.NotImplementedException();
    }

    
}
