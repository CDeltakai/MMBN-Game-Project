using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airshot : ChipEffectBlueprint
{
 
    public override void Effect()
    {


        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies", "Obstacle"));
        if(hitInfo)
        {

            BStageEntity target = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
            if(target == null)
            {return;}

            applyChipDamage(target);
            StartCoroutine(target.Shove(1, 0));            
        }
        
    



    }




}
