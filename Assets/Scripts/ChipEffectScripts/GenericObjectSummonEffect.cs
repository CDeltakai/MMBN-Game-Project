using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectSummonEffect : ChipEffectBlueprint
{

    public UnityEngine.GameObject PooledSummonObject;
    public ObjectSummonAttributes PooledObjectAttributes;
    public Vector3 PositionModifier;

  

    private void Start() 
    {
        PooledObjectAttributes = PooledSummonObject.GetComponent<ObjectSummonAttributes>();
        PooledObjectAttributes.InheritedChipPrefab = this;        
    }
    public override void Effect()
    {
        PooledSummonObject.transform.localPosition = new Vector3(player.worldTransform.position.x + PositionModifier.x,
                                                                player.worldTransform.position.y + PositionModifier.y, 0);
        PooledSummonObject.SetActive(true);
        


    }


}
