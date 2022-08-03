using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChip
{

    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default, string AddressableKey = null);
    public int AdditionalDamage{get;set;}
    public int BaseDamage{get;}

    public EChipTypes ChipType{get;}

    

    public EStatusEffects statusEffect{get; set;}

    public EChipElements chipElement{get;}




    

}
