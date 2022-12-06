using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectSummonEffect : ChipEffectBlueprint
{
    public override void Effect()
    {

        var summonedObject = Instantiate(ObjectSummon, new Vector2(player.worldTransform.position.x + 1.6f, player.worldTransform.position.y), transform.rotation);
        


    }


}
