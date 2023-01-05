using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlus10 : ChipEffectBlueprint
{
    ChipLoadManager chipLoadManager;



    void Awake() 
    {
        chipLoadManager = ChipLoadManager.Instance;

    }

    public override void Effect()
    {
        ChipObjectReference chipToBuff = chipLoadManager.nextChipRefLoad[0];
        if (chipToBuff.chipSORef.GetChipType() == EChipTypes.Attack)
        {
            if(chipToBuff.ObjectSummon != null)
            {
                chipToBuff.ObjectSummon.GetComponentInChildren<ObjectSummonAttributes>().AddDamage += 10;
            }
            chipToBuff.effectPrefab.GetComponent<ChipEffectBlueprint>().AddDamage += 10;
        }else
        {
            print("Buff chips have no effect on Non-attack chips");
        }

    }


}


