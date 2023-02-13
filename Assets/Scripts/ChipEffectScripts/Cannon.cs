using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using testExtensions;

public class Cannon : ChipEffectBlueprint
{

    [SerializeField] GameObject projectile;

    public override void Effect()
    {
        GameObject bullet = Instantiate(projectile, new Vector2(player.currentCellPos.x + 1.6f, player.currentCellPos.y), transform.rotation );
        

        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies"));
        if(hitInfo)
        {
            BStageEntity entity = hitInfo.transform.gameObject.GetComponent<BStageEntity>();

            applyChipDamage(entity);
        }
        

    }




}
