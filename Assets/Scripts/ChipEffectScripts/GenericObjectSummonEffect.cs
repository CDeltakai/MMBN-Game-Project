using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectSummonEffect : ChipEffectBlueprint
{

    public GameObject PooledObject;

  


    public override void Effect()
    {
        PooledObject.transform.localPosition = new Vector3(player.worldTransform.position.x, player.worldTransform.position.y, 0);
        PooledObject.SetActive(true);
        


    }


}
