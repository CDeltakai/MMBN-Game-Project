using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using testExtensions;

public class GenericRayCastEffect : ChipEffectBlueprint
{


    public override void Effect()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies"));
        if(hitInfo)
        {
            BStageEntity entity = hitInfo.transform.gameObject.GetComponent<BStageEntity>();

            applyChipDamage(entity);
        }
        

    }




}
