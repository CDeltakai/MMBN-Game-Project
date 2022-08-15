using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum ChampyAnims
{
    Champy_Idle,
    Champy_Attack
}


public class Champy_RF : BStageEntity
{

    public override bool isGrounded { get; set; } = false;
    public override bool isStationary => false;
    public override bool isStunnable => true;
    public override int maxHP => 60;
    public override ETileTeam team{get;set;} = ETileTeam.Enemy;

    BoxCollider2D champyCollider;
    PlayerMovement player;
    [HideInInspector] public bool hasMoved = false;
    [HideInInspector] public bool isAttacking = false;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        champyCollider = GetComponent<BoxCollider2D>();
        currentCellPos.Set((int)(Math.Round((worldTransform.position.x/1.6f), MidpointRounding.AwayFromZero)),
                            (int)transform.parent.position.y, 0);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
        //currentHP = maxHP;
        healthText.text = currentHP.ToString();


    }

    void Update()
    {
        
    }


    public void setCellPosition_MaintainOccupied(int x, int y)
    {

        if(hasMoved)
        {
            stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
            stageHandler.setEntityAtCell(currentCellPos.x, currentCellPos.y, this, false);

            currentCellPos.Set(x, y, currentCellPos.z);
        }

        currentCellPos.Set(x, y, 0);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
        stageHandler.setEntityAtCell(currentCellPos.x, currentCellPos.y, this, true);

        worldTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }

    public IEnumerator AttackAnimation()
    {
        Vector3Int previousCellPosition = currentCellPos;
        setCellPosition_MaintainOccupied(player.getCellPosition().x + 1, currentCellPos.y);
        hasMoved = true;

        animator.Play(ChampyAnims.Champy_Attack.ToString());
        float delay = 0.417f;
        isAttacking = true;
        yield return new WaitForSeconds(delay + 0.3f);
        animator.Play(ChampyAnims.Champy_Idle.ToString());
        yield return new WaitForSeconds(1);

        setCellPosition_MaintainOccupied(previousCellPosition.x, previousCellPosition.y);
        hasMoved = false;

        previousCellPosition = currentCellPos;

        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }

    public void straightHitRegister(int damage)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast (worldTransform.position, new Vector2(-1, 0), 1, LayerMask.GetMask("Player", "Player_Ally"));
        if(hitInfo)
        {

            hitInfo.collider.gameObject.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);
            print("Champy Attacked (Straight)");
            IBattleStageEntity script = hitInfo.transform.gameObject.GetComponent<IBattleStageEntity>();
            if(script != null)
            {
                script.hurtEntity(damage, true, true);
            }
        }
    }

    public void uppercutHitRegister(int damage)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast (worldTransform.position, new Vector2(-1, 0), 1, LayerMask.GetMask("Player", "Player_Ally"));
        if(hitInfo)
        {
            hitInfo.collider.gameObject.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);
            print("Champy Attacked (Uppercut)");
            IBattleStageEntity script = hitInfo.transform.gameObject.GetComponent<IBattleStageEntity>();
            if(script != null)
            {
                script.hurtEntity(damage, false, true);
            }
        }


    }


}
