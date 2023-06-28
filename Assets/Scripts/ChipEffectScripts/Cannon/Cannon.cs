using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : ChipEffectBlueprint
{

    [SerializeField] GameObject projectile;
    

    public override void Effect()
    {
        effectProperties = new EffectProperties(DamageModifier, 
                                                        BaseStatusEffect, 
                                                        AdditionalStatusEffects, 
                                                        lightAttack, 
                                                        hitFlinch, 
                                                        pierceUntargetable, 
                                                        chip.GetChipElement());

        BulletController bulletController = Instantiate(projectile, new Vector2(player.transform.parent.transform.position.x + 1.6f,
        player.transform.parent.transform.position.y), transform.rotation).GetComponent<BulletController>();

        

        BasicBullet bullet = bulletController.bullet;
        bullet.Damage = calcFinalDamage();
        bullet.parentChip = this;
        bullet.player = player;



        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, 100f, LayerMask.GetMask("Enemies", "Obstacle"));
        if(hitInfo)
        {
            BStageEntity entity = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
            bullet.hitPosition = hitInfo;
            bullet.endPosition = hitInfo.point;

            if(!TimeManager.isCurrentlySlowedDown)
            {
                OnActivationEffect(entity);
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
    }







}
