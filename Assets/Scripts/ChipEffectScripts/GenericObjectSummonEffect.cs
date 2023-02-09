using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectSummonEffect : ChipEffectBlueprint
{

    public GameObject PooledSummonObject;
    public Vector3 PositionModifier;

  


    public override void Effect()
    {
        PooledSummonObject.transform.localPosition = new Vector3(player.worldTransform.position.x + PositionModifier.x,
                                                                player.worldTransform.position.y + PositionModifier.y, 0);
        PooledSummonObject.SetActive(true);
        


    }


}
