using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Pathfinding.Util;
using System;

public class PlayerMovement : BStageEntity
{

    [SerializeField] public ChipSO activeChip;
    [SerializeField] public List<ChipSO> PlayerChipQueue = new List<ChipSO>();


    TimeManager timeManager;
    ChipLoadManager chipLoadManager;

    PlayerInput playerInput;



    [HideInInspector] public BoxCollider2D boxCollider2D;
    PlayerChipAnimations playerChipAnimations; 
    ChipEffects chipEffect;

    bool isAlive = true;
    bool isMoving = false;
    bool isUsingChip = false;
    float animationLength;

    ChipSelectScreenMovement chipSelectScreenMovement;
    [SerializeField] Transform firePoint;

    public int shotDamage = 5;

    public bool SuperArmor = false;

    public string Name => "Megaman";


    public int ID => 3;

    public bool vulnerable { get;set;} = false;
    public override bool isGrounded { get ; set ; } = true;
    public override bool isStationary => false;
    public override bool isStunnable => true;
    public override int maxHP => 9999;
    public override ETileTeam team { get;set;} = ETileTeam.Player;
    public bool Rooted;

    Color invisible;
    Color opaque;

    AnimationCurve movementSpeedCurve;


    public override void Start()
    {
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



    public void Shoot()
    {
      RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies","Obstacle"));

      if(hitInfo)
      {

          BStageEntity target = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
          if(target == null)
          {return;}
          target.hurtEntity(shotDamage, true, false);
      }
    }

    void setInvincible()
    {
        if(isInvincible){isInvincible = false; return;}
        isInvincible = true;
        
    }


    void Update()
    {        
        if(currentHP <= 0)
        {
            isAlive = false;
        }
        if(!isMoving)
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


    void OnUseChip()
    {
        if(isUsingChip){return;}
        StartCoroutine(OnUseChipIEnumerator());
    }

    IEnumerator OnUseChipIEnumerator()
    {

        if (chipLoadManager.nextChipLoad.Count == 0)
        {Debug.Log("Chip Queue Empty");
            yield break;}

        if(chipLoadManager.nextChipLoad[0].GetChipType() != EChipTypes.Passive && chipLoadManager.nextChipLoad[0].GetChipType() != EChipTypes.Special ){

        playerChipAnimations.playAnimationEnum(chipLoadManager.nextChipLoad[0].GetChipEnum(), chipLoadManager.nextChipLoad[0].GetAnimationDuration());
        isUsingChip = true;
        } 

        if(chipLoadManager.nextChipLoad[0].GetChipType() == EChipTypes.Special)
        {
            chipEffect.ApplyChipEffectV3();
        }

        yield return new WaitForSecondsRealtime(chipLoadManager.nextChipLoad[0].GetAnimationDuration());
        
        chipLoadManager.nextChipLoad.Clear();
        chipLoadManager.calcNextChipLoad();
        isUsingChip = false;
        
    }

    void OnOpenDeck()
    {
        chipSelectScreenMovement.EnableChipMenu();
    }

    IEnumerator teleMoveWithDelay(int x, int y, float delay)
    {
        if(isMoving)
        {yield break;}
        if(!checkValidTile(currentCellPos.x + x, currentCellPos.y + y))
        {
            yield break;
        }
        animator.SetTrigger("Move");
        isMoving = true; 
        yield return new WaitForSeconds(delay);

        cellMove(x, y);
        isMoving = false;
    }

    IEnumerator animatedMovewithDelay(int x, int y, float delay)
    {
        yield return 1;
    }

    void simpleMove()
    {
        if(!isAlive){return;}
        if(Rooted){return;}
        if(isMoving){return;}
        int index = animator.GetLayerIndex("Base Layer");
        
        if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            StartCoroutine(teleMoveWithDelay(1, 0, 0.106f));            
        }
        if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            StartCoroutine(teleMoveWithDelay(-1, 0, 0.106f));   
        }
        if(Keyboard.current.wKey.wasPressedThisFrame)
        {
            StartCoroutine(teleMoveWithDelay(0, 1, 0.106f));   
        }
        if(Keyboard.current.sKey.wasPressedThisFrame)
        {
            StartCoroutine(teleMoveWithDelay(0, -1, 0.106f));
        }

    }
   


    void OnFire()
    {
        animator.SetTrigger("Shoot");   
    }


    public override void hurtEntity(int damageAmount,
        bool lightAttack,
        bool hitStun,
        bool pierceCloaking = false,
        EStatusEffects statusEffect = EStatusEffects.Default)
    {
        if(isInvincible){return;}

        if(!SuperArmor || hitStun ){
            animator.Play(EMegamanAnimations.Megaman_Hurt.ToString());
        }
        
        if(damageAmount * DefenseMultiplier >= currentHP)
        {
            isAlive = false;
            currentHP = 0;
            return;
        }

        currentHP = currentHP - (int)(damageAmount * DefenseMultiplier);
        healthText.text = currentHP.ToString();

        if(!lightAttack){
        StartCoroutine(InvincibilityFrames(1f));
        }
        StartCoroutine(ChangeAnimState(EMegamanAnimations.Megaman_Idle.ToString(), EMegamanAnimations.Megaman_Hurt.ToString()));
        
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

    IEnumerator InvincibilityFrames(float duration)
    {
        float gracePeriod = duration;
        isInvincible = true;

        while (gracePeriod>=0){
            

            spriteRenderer.color = invisible;
            gracePeriod -= 0.05f;

            yield return new WaitForSecondsRealtime(0.05f);
            spriteRenderer.color = opaque;
            gracePeriod -= 0.05f;

            yield return new WaitForSecondsRealtime(0.05f);
            
        }

        isInvincible = false;
    }





    public Vector3Int getCellPosition()
    {
        return currentCellPos;
    }



    public IEnumerator setStatusEffect(EStatusEffects status)
    {

       switch (status) 
       {
        case EStatusEffects.Paralyzed: 
            animator.speed = 0f;
            yield return new WaitForSecondsRealtime(1f);
            animator.speed = 1f;
            break;

        case EStatusEffects.Rooted:
            Rooted = true;
            yield return new WaitForSecondsRealtime(1f);
            break;
        




        default :
        
            break;
       }




    }

}
