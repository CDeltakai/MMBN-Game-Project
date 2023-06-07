using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This class is used for chips that only summon another object on activation.
///Additional logic for what the summoned object does should be written on a seperate class(es).
///</summary>
public class GenericObjectSummonEffect : ChipEffectBlueprint
{

    public GameObject PooledSummonObject;
    public ObjectSummonAttributes PooledObjectAttributes;
    public Vector3 PositionModifier;





    private void Start() 
    {
        if(PooledSummonObject.GetComponent<ObjectSummonAttributes>() == null)
        {
           PooledObjectAttributes = PooledSummonObject.GetComponentInChildren<ObjectSummonAttributes>();
        }else
        {
            PooledObjectAttributes = PooledSummonObject.GetComponent<ObjectSummonAttributes>();            
        }


        PooledObjectAttributes.InheritedChipPrefab = this.gameObject.GetComponent<ChipEffectBlueprint>();      
    }

    public override void Effect()
    {
        PooledSummonObject.transform.localPosition = new Vector3(player.worldTransform.position.x + PositionModifier.x,
                                                                player.worldTransform.position.y + PositionModifier.y, 0);
        PooledSummonObject.SetActive(true);
        


    }


}
