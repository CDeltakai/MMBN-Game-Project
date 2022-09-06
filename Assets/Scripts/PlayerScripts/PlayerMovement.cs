using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Pathfinding.Util;
using System;
using DG.Tweening;

public class PlayerMovement : BStageEntity
{

#region Initialized Script Classes

    TimeManager timeManager;
    ChipLoadManager chipLoadManager;
    PlayerInput playerInput;
    PlayerChipAnimations playerChipAnimations; 
    ChipEffects chipEffect;
    [SerializeField] PlayerVFXController VFXController;
    ChipSelectScreenMovement chipSelectScreenMovement;

#endregion

    public delegate void UsedChipEvent();
    public event UsedChipEvent usedChip;


    [SerializeField] public ChipSO activeChip;
    [SerializeField] public List<ChipSO> PlayerChipQueue = new List<ChipSO>();

    public override event MoveOffTileEvent moveOnToTileOverriden;
    public override event MoveOffTileEvent moveOffTileOverriden;




    //firePoint is used for effects or chips that use raycasts to deal their effect.
    [SerializeField] Transform firePoint;
    [HideInInspector] public BoxCollider2D boxCollider2D;
    [SerializeField] bool SuperArmor = false;

    bool isAlive = true;
    bool isUsingChip = false;
    bool uninterruptibleAction = false;
    float animationLength;


    public int shotDamage = 5;
    public int shotDamageMultiplier = 1;
    [SerializeField] float chargeDuration = 0.1f;


    public string Name => "Megaman";


    public int ID => 3;

    public bool vulnerable { get;set;} = false;
    public override bool isGrounded { get ; set ; } = true;
    public override bool isStationary => false;
    public override bool isStunnable => true;
    public override int maxHP => 9999;
    public override ETileTeam team { get;set;} = ETileTeam.Player;

    Coroutine UseChipCoroutine = null;
    Coroutine ParryCoroutine = null;


  [Header("Experimental Features")]
    [SerializeField] bool useTranslateMovement = false;
    [SerializeField] bool useTweenMovement = false;


    public override void Start()
    {


        usingOverridenMovementMethod = true;
        chipEffect = FindObjectOfType<ChipEffects>();
        chipSelectScreenMovement = FindObjectOfType<ChipSelectScreenMovement>();
        playerChipAnimations = GetComponent<PlayerChipAnimations>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        stageHandler = FindObjectOfType<BattleStageHandler>();


        chipLoadManager = GetComponent<ChipLoadManager>();

        playerInput = GetComponent<PlayerInput>();

        timeManager = FindObjectOfType<TimeManager>();


        spriteRenderer = GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");

        invisible = spriteRenderer.color;
        opaque = spriteRenderer.color;
        invisible.a = 0;
        opaque.a = 1;
        
        currentCellPos.Set((int)(Math.Round((worldTransform.position.x/1.6f), MidpointRounding.AwayFromZero)),
                            (int)transform.parent.position.y, 0);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
        healthText.text = currentHP.ToString();

        
    }


//Tied to Megaman_Shoot animation
    void Shoot()
    {
      RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies","Obstacle"));

      if(hitInfo)
      {

          BStageEntity target = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
          if(target == null)
          {return;}
          target.hurtEntity(shotDamage * shotDamageMultiplier, true, false, this);
          shotDamageMultiplier = 1;
      }

    }


    void Update()
    {        
        if(currentHP <= 0)
        {
            isAlive = false;
        }
        if(!isMoving && !isUsingChip)
        {simpleMove();}else{return;}

       if(Keyboard.current.spaceKey.wasPressedThisFrame)
       {
        print("Attempted slow motion");
        timeManager.SlowMotion();
       }
       if(Keyboard.current.spaceKey.wasReleasedThisFrame)
       {
        print("Canceled slowmotion");
        timeManager.cancelSlowMotion();
       }

        
    }






    IEnumerator OnUseChipIEnumerator()
    {

        if (chipLoadManager.nextChipLoad.Count == 0)
        {Debug.Log("Chip Queue Empty");
        isUsingChip = false;
        UseChipCoroutine = null;

        yield break;}

        isUsingChip = true;


        if(chipLoadManager.nextChipLoad[0].GetChipType() != EChipTypes.Passive && chipLoadManager.nextChipLoad[0].GetChipType() != EChipTypes.Special )
        {

            playerChipAnimations.playAnimationEnum(chipLoadManager.nextChipLoad[0].GetChipEnum(), chipLoadManager.nextChipLoad[0].GetAnimationDuration());
        } 

        if(chipLoadManager.nextChipLoad[0].GetChipType() == EChipTypes.Special)
        {
            chipEffect.ApplyChipEffectV3();
        }

        yield return new WaitForSecondsRealtime(chipLoadManager.nextChipLoad[0].GetAnimationDuration() + 0.05f);
        chipLoadManager.nextChipLoad.Clear();
        chipLoadManager.calcNextChipLoad();

        if(usedChip != null){usedChip();}
        
        isUsingChip = false;
        UseChipCoroutine = null;
    }



    IEnumerator teleMoveWithDelay(int x, int y, float delay)
    {
        if(isMoving)
        {yield break;}
        if(!checkValidTile(currentCellPos.x + x, currentCellPos.y + y))
        {
            yield break;
        }
        isMoving = true; 
        animator.SetTrigger("Move");
        yield return new WaitForSeconds(delay);

        cellMove(x, y);
        isMoving = false;
    }

    Ease movementEase = Ease.OutCubic;
    void simpleMove()
    {
        if(ChipSelectScreenMovement.GameIsPaused)
        {return;}
        if(!isAlive){return;}
        if(isUsingChip){
            print("is using chip, cannot move");
            return;}
        if(isRooted){
            print("is rooted, cannot move");
            return;}
        if(isMoving){
            print("is already moving, cannot move again");
            return;}
        int index = animator.GetLayerIndex("Base Layer");
        
        if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            if(useTranslateMovement)
            {StartCoroutine(translateMoveCell(1, 0, Vector2.right));}
            else if(useTweenMovement)
            {
                isMovingCoroutine = StartCoroutine(TweenMove(1, 0, 0.1f, movementEase));
                
            }
            else
            {StartCoroutine(teleMoveWithDelay(1, 0, 0.106f));}

        }
        if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            if(useTranslateMovement)
            {StartCoroutine(translateMoveCell(-1, 0, Vector2.left));}
            else if(useTweenMovement)
            {
                isMovingCoroutine = StartCoroutine(TweenMove(-1, 0, 0.1f, movementEase));
            }            
            else{StartCoroutine(teleMoveWithDelay(-1, 0, 0.106f));}             
        }
        if(Keyboard.current.wKey.wasPressedThisFrame)
        {
            if(useTranslateMovement)
            {StartCoroutine(translateMoveCell(0, 1, Vector2.up));}
            else if(useTweenMovement)
            {
                isMovingCoroutine = StartCoroutine(TweenMove(0, 1, 0.1f, movementEase));
            }            
            else{StartCoroutine(teleMoveWithDelay(0, 1, 0.106f));}               
        }
        if(Keyboard.current.sKey.wasPressedThisFrame)
        {
            if(useTranslateMovement)
            {StartCoroutine(translateMoveCell(0, -1, Vector2.down));}
            else if(useTweenMovement)
            {
                isMovingCoroutine = StartCoroutine(TweenMove(0, -1, 0.1f, movementEase));
            }            
            else{StartCoroutine(teleMoveWithDelay(0, -1, 0.106f));}            
        }

    }
   





    public override void hurtEntity(int damage,
        bool lightAttack,
        bool hitFlinch,
        BStageEntity attacker,
        bool pierceCloaking = false,
        EStatusEffects statusEffect = EStatusEffects.Default)
    {
        if(isUntargetable){return;}

        if(!SuperArmor && hitFlinch ){
            animator.Play(EMegamanAnimations.Megaman_Hurt.ToString());
            StartCoroutine(ChangeAnimState(EMegamanAnimations.Megaman_Idle.ToString(), EMegamanAnimations.Megaman_Hurt.ToString()));
            StartCoroutine(setStatusEffect(EStatusEffects.Rooted, 0.111f));

        }


        if(damage >= 10)
        {
            isAnimatingHP = true;

            if(AnimateHPCoroutine != null)
            {
                StopCoroutine(AnimateHPCoroutine);
            }

            AnimateHPCoroutine = StartCoroutine(animateHP(currentHP, currentHP - Mathf.Clamp((int)(damage * DefenseMultiplier), 1, 999999)));

        }
        
        if(damage * DefenseMultiplier >= currentHP)
        {
            isAlive = false;
            currentHP = 0;
            return;
        }

        currentHP = currentHP - Mathf.Clamp((int)(damage * DefenseMultiplier), 1, 999999);
        if(AnimateHPCoroutine == null)
        {
            healthText.text = currentHP.ToString();
        }

        if(!lightAttack){
        StartCoroutine(InvincibilityFrames(1f));
        }
        
        return;
    }




    IEnumerator ChangeAnimState(string stateName, string transitionState)
    {
        yield return new WaitForSeconds(GetAnimationLength(transitionState));
        animator.Play(stateName);
    }

    float GetAnimationLength(string stateName)
    {
        int index = animator.GetLayerIndex("Base Layer");
        animationLength = animator.GetCurrentAnimatorStateInfo(index).length;
        return animationLength * 2f;
    }

    protected override IEnumerator InvincibilityFrames(float duration)
    {
        float gracePeriod = duration;
        isUntargetable = true;

        while (gracePeriod>=0){
            

            spriteRenderer.color = invisible;
            gracePeriod -= 0.05f;

            yield return new WaitForSecondsRealtime(0.05f);
            spriteRenderer.color = opaque;
            gracePeriod -= 0.05f;

            yield return new WaitForSecondsRealtime(0.05f);
            
        }

        isUntargetable = false;
    }


    public Vector3Int getCellPosition()
    {
        return currentCellPos;
    }



    public override IEnumerator setStatusEffect(EStatusEffects status, float duration)
    {

       switch (status) 
       {
        case EStatusEffects.Paralyzed: 
            animator.speed = 0f;
            yield return new WaitForSecondsRealtime(duration);
            animator.speed = 1f;
            break;

        case EStatusEffects.Rooted:
            isRooted = true;
            yield return new WaitForSecondsRealtime(duration);
            isRooted = false;
            break;

       }

    }


    public override IEnumerator translateMoveCell(int x, int y, Vector2 direction)
    {
        if(isMoving || isUsingChip)
        {yield break;}
        Vector3Int destinationCell = new Vector3Int(currentCellPos.x + x, currentCellPos.y + y, 0);
        if(!checkValidTile(destinationCell.x, destinationCell.y))
        {
            yield break;
        }
        isMoving = true;
        movementSpeedTime = 0;
        float speed = movementSpeedCurve.Evaluate(movementSpeedTime);
        float maxDistance = Vector3.Distance(stageHandler.stageTilemap.GetCellCenterWorld(currentCellPos),
         stageHandler.stageTilemap.GetCellCenterWorld(destinationCell));
        float currentDistance;
        float percentageDone = 0;
        float maxPercentage;
        float speedMultiplier = 1;
        bool changedTile = false;
        if(Mathf.Approximately(direction.y, Vector2.up.y) || Mathf.Approximately(direction.y, Vector2.down.y))
        {maxPercentage = 0.75f;
        speedMultiplier = 0.6f;}
        else
        {maxPercentage = 0.85f;}

            while(percentageDone <= maxPercentage)
            {

                if(percentageDone >= 0.3 && !changedTile)
                {
                    stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
                    stageHandler.previousSeenEntity(currentCellPos.x, currentCellPos.y, this, true);
                    moveOffTileOverriden(currentCellPos.x, currentCellPos.y, this);

                    currentCellPos.Set(destinationCell.x, destinationCell.y, 0);

                    moveOnToTileOverriden(currentCellPos.x, currentCellPos.y, this);
                    stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
                    changedTile = true;
                }

                worldTransform.Translate(direction * (speed*speedMultiplier) * Time.deltaTime);
                movementSpeedTime += Time.deltaTime;
                speed = movementSpeedCurve.Evaluate(movementSpeedTime);

                currentDistance = Vector3.Distance(worldTransform.position,
                stageHandler.stageTilemap.GetCellCenterWorld(destinationCell));

                percentageDone = (Mathf.Clamp((maxDistance - currentDistance), 0, maxDistance)/maxDistance);
                    

                speed = movementSpeedCurve.Evaluate(movementSpeedTime);
                //yield return null unblocks the thread and allows update to draw the next frame before 
                //returning to this coroutine.
                yield return null;
            }
        movementSpeedTime = 0;
        worldTransform.position = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
        isMoving = false;
    }

    public override IEnumerator translateMove_Fixed(int x, int y)
    {
        if(isMoving)
        {yield break;}
        Vector3Int destinationCell = new Vector3Int(currentCellPos.x + x, currentCellPos.y + y, 0);
        if(!checkValidTile(destinationCell.x, destinationCell.y))
        {
            yield break;
        }
        isMoving = true;
        Vector2Int direction = new Vector2Int(x, y);
        movementSpeedTime = 0;
        float yPosOnCurve = 0;
        float xPosOnCurve = 0;
        bool changedTile = false;


        while(movementSpeedTime <= 0.1f)
        {

            if(movementSpeedTime >= 0.05f && !changedTile)
            {
                stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
                stageHandler.previousSeenEntity(currentCellPos.x, currentCellPos.y, this, true);
                moveOffTileOverriden(currentCellPos.x, currentCellPos.y, this);

                currentCellPos.Set(destinationCell.x, destinationCell.y, 0);

                moveOnToTileOverriden(currentCellPos.x, currentCellPos.y, this);
                stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
                changedTile = true;
            }


            movementSpeedTime += Time.deltaTime;
            if(direction == Vector2Int.down || direction == Vector2Int.up)
            {yPosOnCurve = Mathf.Clamp(yDistanceTimeCurve.Evaluate(movementSpeedTime), 0, 1.001f);}
            else
            {xPosOnCurve = Mathf.Clamp(xDistanceTimeCurve.Evaluate(movementSpeedTime), 0, 1.601f);}
            worldTransform.position = new Vector3 (worldTransform.position.x + xPosOnCurve, worldTransform.position.y + yPosOnCurve, 0);

            yield return null;
        }
        movementSpeedTime = 0;
        //worldTransform.position = stageHandler.stageTilemap.
        //                            GetCellCenterWorld(currentCellPos);    
        isMoving = false;


    }


#region Input Actions

    Coroutine VFXCoroutine = null;
    public void OnFire(InputAction.CallbackContext context)
    {


        print(context);

        if(ChipSelectScreenMovement.GameIsPaused)
        {return;}        
        if(isMoving){return;}

        if(context.performed)
        {
            VFXCoroutine = StartCoroutine(VFXController.playVFXanim(true, PlayerVFXAnims.BasicShot_Charging, PlayerVFXAnims.BasicShot_FullyCharged, chargeDuration));
        }

        if(context.canceled)
        {
            if(context.duration >= chargeDuration)
            {
                shotDamageMultiplier = 10;
            }

           if(VFXCoroutine != null)
           {
                print("Stopping VFXCoroutine");
                StopCoroutine(VFXCoroutine);
           } 
            VFXCoroutine = StartCoroutine(VFXController.playVFXanim(false));

            print("Megaman fired buster shot");
            animator.SetTrigger("Shoot");
            VFXCoroutine = null;
        }
    }

    public void OnOpenDeck(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            chipSelectScreenMovement.ToggleChipMenu();
        }

    }

    public void OnUseChip(InputAction.CallbackContext context)
    {
        if(ChipSelectScreenMovement.GameIsPaused)
        {return;}        
        if(isUsingChip){return;}
        if(isMoving){return;}
        if(UseChipCoroutine != null)
        {return;}

        if(context.started)
        {
            UseChipCoroutine = StartCoroutine(OnUseChipIEnumerator());
        }

    }


    public void OnParry(InputAction.CallbackContext context)
    {
    
        if(ChipSelectScreenMovement.GameIsPaused)
        {return;}        
        if(isUsingChip){return;}
        if(isMoving){return;}






    }



#endregion



}
