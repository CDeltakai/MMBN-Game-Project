using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using DG.Tweening;

internal enum ChampyAnims
{
    Champy_Idle,
    Champy_Attack
}


public class Champy_RF : BStageEntity
{
    public override event OnDeathEvent DeathEvent;

    public override bool isGrounded { get; set; } = false;
    public override bool isStationary => false;
    public override bool isStunnable => true;
    public override int maxHP => 60;
    public override ETileTeam tileTeam{get;set;} = ETileTeam.Enemy;

    BoxCollider2D champyCollider;
    PlayerMovement player;
    [HideInInspector] public bool hasMoved = false;
    [HideInInspector] public bool isAttacking = false;

    Vector3Int originCellPos; 


    public override void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        champyCollider = GetComponent<BoxCollider2D>();
        currentCellPos.Set((int)(Math.Round((worldTransform.position.x/1.6f), MidpointRounding.AwayFromZero)),
                            (int)transform.parent.position.y, 0);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
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

    public IEnumerator SET_TweenMove_MaintainOccupied(int x, int y, float duration, Ease easeType)
    {
        if(hasMoved)
        {
            stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
            stageHandler.setEntityAtCell(currentCellPos.x, currentCellPos.y, this, false);

            currentCellPos.Set(x, y, currentCellPos.z);
        }        

        Vector3Int destinationCell = new Vector3Int(x, y, 0);
        if(!checkValidTile(x, y))
        {yield break;}

        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
        stageHandler.SetPreviousSeenEntity(currentCellPos.x, currentCellPos.y, this, true);
        //moveOffTileOverriden(currentCellPos.x, currentCellPos.y, this);

        currentCellPos.Set(currentCellPos.x + x, currentCellPos.y + y, 0);
        worldTransform.DOMove(stageHandler.stageTilemap.GetCellCenterWorld(destinationCell), duration ).SetEase(easeType);
        yield return new WaitForSeconds(duration * 0.5f);

        //moveOnToTileOverriden(currentCellPos.x, currentCellPos.y, this);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
        
    }

    

    public IEnumerator DashToTile(int x, int y, float duration, Ease easeType)
    {

        
        Vector3Int destinationCell = new Vector3Int(x, y, 0);



        ClaimTileOccupancy(destinationCell.x, destinationCell.y);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
        stageHandler.SetPreviousSeenEntity(currentCellPos.x, currentCellPos.y, this, true);
        originCellPos.Set(currentCellPos.x, currentCellPos.y, 0);
        //moveOffTile(currentCellPos.x, currentCellPos.y, this);

        currentCellPos.Set(x, y, 0);


            worldTransform.DOMove(stageHandler.stageTilemap.GetCellCenterWorld(destinationCell), duration).SetEase(easeType);
            yield return new WaitForSeconds(duration * 0.7f);
            //moveOntoTile(currentCellPos.x, currentCellPos.y, this);
            stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);

            

    }



    public IEnumerator AttackAnimation_old()
    {
        if(currentHP <= 0){yield break;}
        originCellPos = currentCellPos;
        setCellPosition_MaintainOccupied(player.getCellPosition().x + 1, currentCellPos.y);
        hasMoved = true;

        animator.Play(ChampyAnims.Champy_Attack.ToString());
        float delay = 0.417f;
        isAttacking = true;
        yield return new WaitForSeconds((delay + 0.3f) * objectTimeScale);
        animator.Play(ChampyAnims.Champy_Idle.ToString());
        yield return new WaitForSeconds(0.6f * objectTimeScale);

        setCellPosition_MaintainOccupied(originCellPos.x, originCellPos.y);
        hasMoved = false;

        originCellPos = currentCellPos;

        yield return new WaitForSeconds(1.25f * objectTimeScale);
        isAttacking = false;
    }

    public IEnumerator AttackAnimation()
    {
        if(currentHP <= 0){yield break;}
        //previousCellPosition = currentCellPos;
        if(!checkFreeTile(player.getCellPosition().x + 1, currentCellPos.y))
        {yield break;}

        StartCoroutine(DashToTile(player.getCellPosition().x + 1, currentCellPos.y, 0.1f, Ease.OutQuad));
        ClaimTileOccupancy(originCellPos.x, originCellPos.y);
        //hasMoved = true;
        yield return new WaitForSeconds(0.1f);

        animator.Play(ChampyAnims.Champy_Attack.ToString());
        float delay = 0.417f;
        isAttacking = true;
        yield return new WaitForSeconds((delay + 0.3f) * objectTimeScale);
        animator.Play(ChampyAnims.Champy_Idle.ToString());
        yield return new WaitForSeconds(0.6f * objectTimeScale);

        //ClearClaimedTiles();
        StartCoroutine(DashToTile(originCellPos.x, originCellPos.y, 0.1f, Ease.OutQuad));

        yield return new WaitForSeconds(0.1f);
        ClearClaimedTiles();

        //hasMoved = false;

        originCellPos = currentCellPos;

        //Cooldown before another attack can be initiated
        yield return new WaitForSeconds(1.25f * objectTimeScale);
        isAttacking = false;
    }    


    public IEnumerator ReworkedAttackAnimation()
    {
        if(currentHP <= 0){yield break;}

    }

    public override void AdditionalDestructionEvents()
    {
        StopCoroutine(AttackAnimation());
    }


    // public override IEnumerator DestroyEntity()
    // {
    //     animator.speed = Mathf.Epsilon;
    //     currentHP = 0;
    //     healthText.text = currentHP.ToString();
            
    //     if(AnimateHPCoroutine == null)
    //     {
    //         healthText.enabled = false;
    //     }
    //     fullInvincible = true;        
    //     StopCoroutine(AttackAnimation());
    //     tileEventManager.UnsubscribeEntity(this);
    //     yield return new WaitForSecondsRealtime(0.0005f);
    //     FMODUnity.RuntimeManager.PlayOneShotAttached(DestroyedSFX, this.gameObject);
    //     setSolidColor(Color.white);
    //     Instantiate(destructionVFX, transform.parent.transform.position, 
    //                 transform.rotation, transform.parent.transform);
    //     yield return new WaitForSecondsRealtime(0.533f);
    //     stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
    //     stageHandler.setCellEntity(originCellPos.x, originCellPos.y, this, false);
    //     ClearClaimedTiles();

    //     if(deathEvent != null)
    //     {
    //         deathEvent(this);
    //     }

    //     Destroy(transform.parent.gameObject);
    //     Destroy(gameObject);

    // }

    public void straightHitRegister(int damage)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast (worldTransform.position, new Vector2(-1, 0), 1.5f, LayerMask.GetMask("Player", "Player_Ally"));
        if(hitInfo)
        {

            hitInfo.collider.gameObject.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);
            print("Champy Attacked (Straight)");
            BStageEntity script = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
            if(script != null)
            {
                script.hurtEntity(damage, true, true, this);
            }
        }
    }

    public void uppercutHitRegister(int damage)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast (worldTransform.position, new Vector2(-1, 0), 1.5f, LayerMask.GetMask("Player", "Player_Ally"));
        if(hitInfo)
        {
            hitInfo.collider.gameObject.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);
            print("Champy Attacked (Uppercut)");
            BStageEntity script = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
            if(script != null)
            {
                script.hurtEntity(damage, false, true, this);
            }
        }


    }


}
