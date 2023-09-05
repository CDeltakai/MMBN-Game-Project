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
        if (chipToBuff.chipSORef.GetChipType() == EChipTypes.Active)
        {
            if(chipToBuff.ObjectSummon != null)
            {
                ObjectSummonAttributes objectSummonAttributes = chipToBuff.ObjectSummon.GetComponentInChildren<ObjectSummonAttributes>();


                if(objectSummonAttributes == null)
                {
                    Debug.LogWarning("Object Summon GetComponentInChildren<ObjectSummonAttributes>() returned null");
                }else
                {
                    print("WhiteCapsule attempted to buff object summon: " +objectSummonAttributes.gameObject.name);
                }



                objectSummonAttributes.AdditionalStatusEffects.Add(EStatusEffects.Paralyzed);

            }
            chipToBuff.effectPrefab.GetComponent<ChipEffectBlueprint>().AdditionalStatusEffects.Add(EStatusEffects.Paralyzed);
        }else
        {
            print("Buff chips have no effect on Non-attack chips");
        }

    }
}
