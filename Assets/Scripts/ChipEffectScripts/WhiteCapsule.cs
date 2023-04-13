using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCapsule : ChipEffectBlueprint
{
    ChipLoadManager chipLoadManager;



    void Awake() 
    {
        chipLoadManager = FindObjectOfType<ChipLoadManager>();
        //chipLoadManager = ChipLoadManager.Instance;

    }

    public override void Effect()
    {
        ChipObjectReference chipToBuff = chipLoadManager.nextChipRefLoad[0];
        if (chipToBuff.chipSORef.GetChipType() == EChipTypes.Attack)
        {
            if(chipToBuff.ObjectSummon != null)
            {
                chipToBuff.ObjectSummon.GetComponentInChildren<ObjectSummonAttributes>().AddStatusEffect = EStatusEffects.Paralyzed;
            }
            chipToBuff.effectPrefab.GetComponent<ChipEffectBlueprint>().AdditionalStatusEffects.Add(EStatusEffects.Paralyzed);
        }else
        {
            print("Buff chips have no effect on Non-attack chips");
        }

    }
}
