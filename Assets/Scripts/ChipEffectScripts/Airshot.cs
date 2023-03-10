using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airshot : ChipEffectBlueprint
{
 
    [SerializeField] GameObject projectilePrefab;

    public override void Effect()
    {
        BulletController bulletController = Instantiate(projectilePrefab, new Vector2(player.transform.parent.transform.position.x + 1.6f,
        player.transform.parent.transform.position.y), transform.rotation).GetComponent<BulletController>();

        BasicBullet bullet = bulletController.bullet;
        bullet.Damage = calcFinalDamage();
        bullet.parentChip = this;

        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies", "Obstacle"));
        if(hitInfo)
        {

            BStageEntity target = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
            if(target == null)
            {return;}

            bullet.hitPosition = hitInfo;
            bullet.endPosition = hitInfo.point;

            if(!TimeManager.isCurrentlySlowedDown)
            {
                OnActivationEffect(target);
            }            
            
        }else
        {
            bullet.endPosition = new Vector2(player.currentCellPos.x + 18f, player.currentCellPos.y);
        }
        
        StartCoroutine(bullet.MoveBullet());      



    }

    public override void OnActivationEffect(BStageEntity target)
    {
        applyChipDamage(target);
        target.AttemptShove(1, 0);

    }



}
