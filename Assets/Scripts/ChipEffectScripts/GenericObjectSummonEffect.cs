using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectSummonEffect : ChipEffectBlueprint
{

    public GameObject PooledSummonObject;

  


    public override void Effect()
    {
        PooledSummonObject.transform.localPosition = new Vector3(player.worldTransform.position.x, player.worldTransform.position.y, 0);
        PooledSummonObject.SetActive(true);
        


    }


}
