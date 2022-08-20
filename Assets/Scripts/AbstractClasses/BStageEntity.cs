using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using System;
using DG.Tweening;



public abstract class BStageEntity : MonoBehaviour
{
    public delegate void MoveOntoTileEvent(int x, int y, BStageEntity entity);
    public event MoveOntoTileEvent moveOntoTile;
    public virtual event MoveOffTileEvent moveOnToTileOverriden;
    public bool usingOverridenMovementMethod = false;



    public delegate void MoveOffTileEvent(int x, int y, BStageEntity entity);
    public event MoveOffTileEvent moveOffTile;
    public virtual event MoveOffTileEvent moveOffTileOverriden;
    protected static TileEventManager tileEventManager;

    protected static BattleStageHandler stageHandler;
    protected SpriteRenderer spriteRenderer;
    protected static Shader shaderGUItext;
    protected static Shader shaderSpritesDefault;
    protected Animator animator;
    [SerializeField] public TextMeshProUGUI healthText;

    [HideInInspector] public Transform worldTransform;
    public abstract bool isGrounded{get;set;}
    public abstract bool isStationary{get;}
    public abstract bool isStunnable{get;}
    public abstract int maxHP{get;}
    public abstract ETileTeam team{get; set;}

    [HideInInspector] public bool isRooted = false;
    [HideInInspector] public bool isMoving = false;

    ///<summary>
    ///A non-volatile status effect is a status which cannot be overriden
    ///by another non-volatile status until the end of its duration.
    ///</summary>
    [HideInInspector] public bool nonVolatileStatus = false;
    

    public Vector3Int currentCellPos;
    [SerializeField] public int currentHP;
    [SerializeField] public int shieldPoints;
    [SerializeField] public float DefenseMultiplier = 1;
    [SerializeField] public float AttackMultiplier = 1;
    [SerializeField] public bool isInvincible = false;

    protected Color invisible;
    protected Color opaque;

    [SerializeField] protected AnimationCurve movementSpeedCurve;
    [SerializeField] protected float movementSpeedTime;
    [SerializeField] protected AnimationCurve xDistanceTimeCurve;
    [SerializeField] protected AnimationCurve yDistanceTimeCurve;

    public virtual void Awake()
    {
           
        tileEventManager = FindObjectOfType<TileEventManager>();
        stageHandler = FindObjectOfType<BattleStageHandler>();
        worldTransform = transform.parent.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = shaderSpritesDefault = Shader.Find("Sprites/Default");

        invisible = spriteRenderer.color;
        opaque = spriteRenderer.color;
        invisible.a = 0;
        opaque.a = 1;



    }

    public virtual void Start()
    {
        currentCellPos.Set((int)(Math.Round((worldTransform.position.x/1.6f), MidpointRounding.AwayFromZero)),
                            (int)transform.parent.position.y, 0);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
        healthText.text = currentHP.ToString();
    }

    ///<summary>
    ///lightAttack dictates whether the attack triggers invincibility frames.
    ///hitFlinch dictates whether the attack will trigger a flinch animation on the target.
    ///</summary>
    public virtual void hurtEntity(int damage,
                                   bool lightAttack,
                                   bool hitFlinch,
                                   bool pierceCloaking = false,
                                   EStatusEffects statusEffect = EStatusEffects.Default)
    {

        if(isInvincible)
        {return;}

        if(statusEffect != EStatusEffects.Default)
        {StartCoroutine(setStatusEffect(statusEffect, 1));}
            
        if(damage >= currentHP)
        {
            animator.speed = Mathf.Epsilon;
            currentHP = 0;
            healthText.text = currentHP.ToString();
            healthText.enabled = false;
            StartCoroutine(DestroyEntity());
            return;
        }
        currentHP = currentHP - Mathf.Clamp((int)(damage * DefenseMultiplier), 1, 999999);
        healthText.text = currentHP.ToString();
        return; 
    }
    
    public int getHealth()
    {
        return currentHP;
    }


    public virtual IEnumerator DestroyEntity()
    {
        tileEventManager.UnsubscribeEntity(this);
        yield return new WaitForSeconds(0.0005f);
        setSolidColor(Color.white);
        var vfx = Addressables.InstantiateAsync("VFX_Destruction_Explosion", transform.parent.transform.position, 
                                                transform.rotation, transform.parent.transform);
        yield return new WaitForSeconds(0.533f);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);

    }

    public void teleportToCell(int x, int y)
    {
            stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
            stageHandler.previousSeenEntity(currentCellPos.x, currentCellPos.y, this, true);

            currentCellPos.Set(x, y, 0);

            stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);

            worldTransform.localPosition = stageHandler.stageTilemap.
                                        GetCellCenterWorld(currentCellPos);
    }

    public void setSolidColor(Color color)
    {
        spriteRenderer.material.shader = shaderGUItext;
        spriteRenderer.color = color;
    }
    public void setNormalSprite()
    {
        spriteRenderer.material.shader = shaderSpritesDefault;
        spriteRenderer.color = Color.white;
    }

    ///<summary>
    ///Checks if the tile at the exact x and y position given is valid.
    ///Returns true if valid, false otherwise.
    ///</summary>
    public bool checkValidTile(int x, int y)
    {
        Vector3Int coordToCheck = new Vector3Int(x, y, 0);
        
            if(stageHandler.stageTilemap.GetTile
            (coordToCheck) == null
                ||
            stageHandler.getCustTile(coordToCheck).GetTileTeam() != team
                ||
            stageHandler.stageTiles
            [stageHandler.stageTilemap.CellToWorld(coordToCheck)].isOccupied
                ||
            (isGrounded && !stageHandler.getCustTile(coordToCheck).isPassable)
            )
            {
                return false;
            }
        return true;
    }

    ///<summary>
    ///Wrapper method for cellMove which includes checkValidTile before moving
    ///</summary>
    public void cellMove_verified(int x, int y)
    {
        if(!checkValidTile(currentCellPos.x + x, currentCellPos.y + y))
        {return;}

        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
        currentCellPos.Set(currentCellPos.x + x, currentCellPos.y + y, 0);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
        worldTransform.position = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }

    ///<summary>
    ///Moves the entity the given x and y value (x for row, y for column). 
    ///This method does not validate the tile it is attempting to move onto.
    ///</summary>
    public virtual void cellMove(int x, int y)
    {
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
        stageHandler.previousSeenEntity(currentCellPos.x, currentCellPos.y, this, true);
        moveOffTile(currentCellPos.x, currentCellPos.y, this);

        currentCellPos.Set(currentCellPos.x + x, currentCellPos.y + y, 0);

        moveOntoTile(currentCellPos.x, currentCellPos.y, this);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
        worldTransform.position = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }

 
    public Vector3Int getCurrentCellPos()
    {
        return currentCellPos;
    }



    public virtual IEnumerator setStatusEffect(EStatusEffects status, float duration)
    {

       switch (status) 
       {
        case EStatusEffects.Paralyzed:
            if(nonVolatileStatus){yield break;}
            nonVolatileStatus = true; 
            animator.speed = 0f;
            StartCoroutine(FlashColor(Color.yellow, duration));
            yield return new WaitForSeconds(duration);
            animator.speed = 1f;
            nonVolatileStatus = false;
        break;

        case EStatusEffects.Rooted:

            isRooted = true;
            yield return new WaitForSeconds(duration);
            isRooted = false;
            break;

        case EStatusEffects.Frozen:

            if(nonVolatileStatus){yield break;}
            nonVolatileStatus = true; 
            animator.speed = 0f;
            yield return new WaitForSeconds(duration);
            animator.speed = 1f;
            nonVolatileStatus = false;

        break;
       }

    }

    protected IEnumerator FlashColor(Color color, float duration)
    {
        float gracePeriod = duration;

        while (gracePeriod>=0){
            

            setSolidColor(color);
            gracePeriod -= 0.05f;

            yield return new WaitForSecondsRealtime(0.05f);
            setNormalSprite();
            gracePeriod -= 0.05f;

            yield return new WaitForSecondsRealtime(0.05f);
            
        }


    }

    protected virtual IEnumerator InvincibilityFrames(float duration)
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



    public virtual IEnumerator translateMoveCell(int x, int y, Vector2 direction)
    {
        if(isMoving)
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
                    moveOffTile(currentCellPos.x, currentCellPos.y, this);

                    currentCellPos.Set(destinationCell.x, destinationCell.y, 0);

                    moveOntoTile(currentCellPos.x, currentCellPos.y, this);
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


    public IEnumerator Shove(int x, int y)
    {
        if(isMoving){yield break;}
        Vector3Int destinationCell = new Vector3Int(currentCellPos.x + x, currentCellPos.y + y, 0);
        if(!checkValidTile(destinationCell.x, destinationCell.y))
        {yield break;}

        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
        stageHandler.previousSeenEntity(currentCellPos.x, currentCellPos.y, this, true);
        moveOffTile(currentCellPos.x, currentCellPos.y, this);

        currentCellPos.Set(currentCellPos.x + x, currentCellPos.y + y, 0);
        worldTransform.DOMove(stageHandler.stageTilemap.GetCellCenterWorld(destinationCell), 0.1f );
        yield return new WaitForSeconds(0.05f);

        moveOntoTile(currentCellPos.x, currentCellPos.y, this);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);


    }


    public virtual IEnumerator translateMove_Fixed(int x, int y)
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
                moveOffTile(currentCellPos.x, currentCellPos.y, this);

                currentCellPos.Set(destinationCell.x, destinationCell.y, 0);

                moveOntoTile(currentCellPos.x, currentCellPos.y, this);
                stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
                changedTile = true;
            }


            movementSpeedTime += Time.deltaTime;
            worldTransform.position.Set(worldTransform.position.x + xPosOnCurve, worldTransform.position.y + yPosOnCurve, 0);
            if(direction == Vector2Int.down || direction == Vector2Int.up)
            {yPosOnCurve = Mathf.Clamp(yDistanceTimeCurve.Evaluate(movementSpeedTime), 0, 1.001f);}
            else
            {xPosOnCurve = Mathf.Clamp(xDistanceTimeCurve.Evaluate(movementSpeedTime), 0, 1.601f);}
            yield return null;
        }
        movementSpeedTime = 0;
        //worldTransform.position = stageHandler.stageTilemap.
        //                            GetCellCenterWorld(currentCellPos);    
        isMoving = false;


    }


}
