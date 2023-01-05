using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectSummonEffect : ChipEffectBlueprint
{

    public GameObject PooledObject;

  


    public override void Effect()
    {
        PooledObject.SetActive(true);
        PooledObject.transform.localPosition = new Vector3(player.worldTransform.position.x + 1.6f, player.worldTransform.position.y, 0);
        


    }


}
