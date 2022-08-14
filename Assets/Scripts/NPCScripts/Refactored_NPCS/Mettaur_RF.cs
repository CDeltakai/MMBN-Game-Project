using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

internal enum MettaurAnims
{
    Mettaur_Attack,
    Mettaur_Idle,
}


public class Mettaur_RF : BStageEntity
{
    public override bool isGrounded {get;set;} = true;
    public override bool isStationary => false;
    public override bool isStunnable => true;
    public override int maxHP => 40;
    public override ETileTeam team{get; set;} = ETileTeam.Enemy;
    public bool isAttacking = false;
    public const float attackAnimationDuration = 1.167f;
    BoxCollider2D mettaurCollider;



    // Start is called before the first frame update
    void Start()
    {
        mettaurCollider = GetComponent<BoxCollider2D>();
        currentCellPos.Set((int)(Math.Round((worldTransform.position.x/1.6f), MidpointRounding.AwayFromZero)),
                            (int)transform.parent.position.y, 0);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
        currentHP = maxHP;
        healthText.text = currentHP.ToString();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator AttackAnimation()
    {
        isAttacking = true;
        animator.Play(MettaurAnims.Mettaur_Attack.ToString(), 0);
        yield return new WaitForSeconds(attackAnimationDuration);
        animator.Play(MettaurAnims.Mettaur_Idle.ToString(), 0);
        isAttacking = false;


    }

    void fireProjectile()
    {
        float direction;
        if(team == ETileTeam.Enemy)
        {direction = -1.6f;}
        else
        {direction = 1.6f;};

        Addressables.InstantiateAsync
        ("mettaurProjectile", new Vector2(worldTransform.position.x + direction, worldTransform.position.y), transform.rotation);
    }

}
