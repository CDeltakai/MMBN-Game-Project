using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectSummonEffect : ChipBlueprint
{
    public override void Effect()
    {

        Instantiate(ObjectSummon, new Vector2(player.worldTransform.position.x + 1.6f, player.worldTransform.position.y), transform.rotation);


    }


}
