using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCapsule : MonoBehaviour, IChip
{
    public int AdditionalDamage { get; set;  } = 0;

    public int BaseDamage => 0;

    public EChipTypes ChipType => EChipTypes.Passive;
    public EStatusEffects chipStatusEffect {get;set;} = EStatusEffects.Default;
    public EChipElements chipElement => EChipElements.Buff;



    IChip[] otherchipsArray;
    IChip chipToBuff;

    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default, string AddressableKey = null)
    {

        otherchipsArray = GetComponents<IChip>();

        foreach(IChip chip in otherchipsArray)
        {
            print("WhiteCapsule attempted to buff chip: " +chip.GetType().ToString());
            if(chip.ChipType == EChipTypes.Active || chip.ChipType == EChipTypes.OffensiveSpecial)
            {
                chip.chipStatusEffect = EStatusEffects.Paralyzed;
                return;
            }

        }

    }

}
