using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshotProjectile : GenericProjectile
{
    public int ShoveDistance = 1;

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        BStageEntity target = other.gameObject.GetComponent<BStageEntity>();
        if(target != null)
        {
            if(target.Team == team){return;} //if the target is the same team as the projectile, ignore collision
            if(target.isUntargetable && !attackPayload.pierceUntargetable)
            {return;}

            target.HurtEntity(attackPayload);
            target.AttemptShove(ShoveDistance, 0);
            DestroyProjectile();
        }



    }



}
