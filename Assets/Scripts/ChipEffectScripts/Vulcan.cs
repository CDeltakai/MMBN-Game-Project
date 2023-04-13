using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulcan : ChipEffectBlueprint
{
    [SerializeField] GameObject projectile;
    

    public override void Effect()
    {
        effectProperties = new EffectPropertyContainer(DamageModifier, 
        StatusEffectModifier, 
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
        bullet.IsProjectileAlways = true;
        bullet.pierceCount = 1;
        bullet.velocityInSlowMotion = new Vector2(40, 0);



        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, 100f, LayerMask.GetMask("Enemies", "Obstacle"));
        if(hitInfo)
        {
            BStageEntity entity = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
            bullet.hitPosition = hitInfo;
            //bullet.endPosition = new Vector2(16, hitInfo.point.x);
            bullet.endPosition = new Vector2(player.currentCellPos.x + 18f, player.currentCellPos.y);


        }
        bullet.StartBullet();        

    }

    public override void OnActivationEffect(BStageEntity target)
    {
        applyChipDamage(target);
    }


}
