using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlus10 : MonoBehaviour, IChip
{
    public int AdditionalDamage { get; set;  } = 0;

    public int BaseDamage => 0;

    public EChipTypes ChipType => EChipTypes.Passive;
    public EStatusEffects statusEffect {get;set;} = EStatusEffects.Default;
    public EChipElements chipElement => EChipElements.Multiplier;



    IChip[] otherchipsArray;
    IChip chipToBuff;

    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default)
    {

        otherchipsArray = GetComponents<IChip>();

        foreach(IChip chip in otherchipsArray)
        {
            print("AttackPlus10 attempted to buff chip: " +chip.GetType().ToString());
            if(chip.ChipType == EChipTypes.Active || chip.ChipType == EChipTypes.OffensiveSpecial)
            {
                chip.AdditionalDamage += 10;
                return;
            }

        }

    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

 
}
