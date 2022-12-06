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
        



        //var summonedObject = Instantiate(ObjectSummon, new Vector2(player.worldTransform.position.x + 1.6f, player.worldTransform.position.y), transform.rotation);
        //var objectStats = summonedObject.GetComponent<ObjectSummonAttributes>();
        //objectStats.AddStatusEffect = AddStatusEffect;
        //objectStats.AddDamage = AddDamage;


    }


}
