using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_RF : BStageEntity
{

    public override bool isGrounded { get; set; } = true;
    public override bool isStationary => false;
    public override bool isStunnable => true;
    public override int maxHP => 9999;
    public override ETileTeam team{get;set;} = ETileTeam.Enemy;


    BoxCollider2D playerCollider;
    PlayerChipAnimations playerChipAnimations; 
    ChipEffects chipEffect;
    ChipSelectScreenMovement chipSelectScreen;
    ChipLoadManager chipLoadManager;
    [SerializeField] Transform firePoint;


    Color invisible;
    Color opaque;

    bool isAlive = true;
    bool isMoving = false;
    bool isUsingChip = false;
    bool SuperArmor = false;
    float animationLength;




    // Start is called before the first frame update
    public override void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        chipLoadManager = GetComponent<ChipLoadManager>();
        chipEffect = GetComponent<ChipEffects>();
        chipSelectScreen = FindObjectOfType<ChipSelectScreenMovement>();
        playerChipAnimations = GetComponent<PlayerChipAnimations>();

        invisible = spriteRenderer.color;
        opaque = spriteRenderer.color;
        invisible.a = 0;
        opaque.a = 1;

        currentCellPos.Set((int)(Math.Round((worldTransform.position.x/1.6f), MidpointRounding.AwayFromZero)),
                            (int)transform.parent.position.y, 0);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
        healthText.text = currentHP.ToString();



    }

    // Update is called once per frame
    void Update()
    {
        
        if(currentHP <= 0)
        {
            isAlive = false;
        }
        if(!isMoving)
        {simpleMove();}else{return;}


    }

    void OnFire()
    {
        
            animator.SetTrigger("Shoot");
        
    }


    public void Shoot()
    {
      RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies","Obstacle"));

      if(hitInfo)
      {

          BStageEntity target = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
          if(target == null)
          {return;}
          target.hurtEntity(5, true, false);
                
      }
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

    void simpleMove()
    {
        if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            if(!checkValidTile(1, 0))
            {return;}

            animator.SetTrigger("Move");
            isMoving = true;
            animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            //Debug.Log("animation length = " + animationLength);
            Invoke("cellMoveRight", 0.104f);
            
        }
        if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            if(!checkValidTile(-1, 0))
            {return;}

            animator.SetTrigger("Move");
            isMoving = true;
            animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            //Debug.Log("animation length = " + animationLength);
            Invoke("cellMoveLeft", 0.104f);
            
        }
        if(Keyboard.current.wKey.wasPressedThisFrame)
        {
            if(!checkValidTile(0, 1))
            {return;}
            animator.SetTrigger("Move");
            isMoving = true;
            animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            //Debug.Log("animation length = " + animationLength);
            Invoke("cellMoveUp", animationLength);
            
        }
        if(Keyboard.current.sKey.wasPressedThisFrame)
        {
            if(!checkValidTile(0, -1))
            {return;}

            animator.SetTrigger("Move");
            isMoving = true;
            animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            //Debug.Log("animation length = " + animationLength);
            Invoke("cellMoveDown", animationLength);
        }
    }

    void OnOpenDeck()
    {
        chipSelectScreen.EnableChipMenu();
    }


    IEnumerator animatedMove()
    {
        yield return 1;
    }


}
