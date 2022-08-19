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

    public delegate void MoveOffTileEvent(int x, int y, BStageEntity entity);
    public event MoveOffTileEvent moveOffTile;
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
    public bool isRooted = false;
    public bool isMoving = false;
    

    public Vector3Int currentCellPos;
    [SerializeField] public bool isInvincible = false;
    [SerializeField] public int currentHP;
    [SerializeField] public int shieldPoints;
    [SerializeField] public float DefenseMultiplier = 1;
    [SerializeField] public float AttackMultiplier = 1;

    protected Color invisible;
    protected Color opaque;

    [SerializeField] AnimationCurve movementSpeedCurve;
    [SerializeField] float movementSpeedTime;

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


    public virtual void hurtEntity(int damage,
                                   bool lightAttack,
                                   bool hitStun,
                                   bool pierceCloaking = false,
                                   EStatusEffects statusEffect = EStatusEffects.Default)
    {

        if(isInvincible)
        {return;}

        if(damage >= currentHP)
        {
            animator.speed = Mathf.Epsilon;
            currentHP = 0;
            healthText.text = currentHP.ToString();
            healthText.enabled = false;
            StartCoroutine(DestroyEntity());
            return;
        }
        currentHP = currentHP - (int)(damage * DefenseMultiplier);
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
    ///Checks if the tile at the exact x and y position given is valid
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
    public void cellMove(int x, int y)
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
        bool changedTile = false;
        if(Mathf.Approximately(direction.y, Vector2.up.y) || Mathf.Approximately(direction.y, Vector2.down.y))
        {maxPercentage = 0.75f;}
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

                worldTransform.Translate(direction * speed * Time.deltaTime);
                movementSpeedTime += Time.deltaTime;
                speed = movementSpeedCurve.Evaluate(movementSpeedTime);

                currentDistance = Vector3.Distance(worldTransform.position,
                stageHandler.stageTilemap.GetCellCenterWorld(destinationCell));

                percentageDone = 
                
                    (Mathf.Clamp((maxDistance - currentDistance), 0, maxDistance)/maxDistance);
                    
                    
                
                // percentageDone = Mathf.Clamp
                // (
                //     (Mathf.Clamp((maxDistance - currentDistance), 0, maxDistance)/maxDistance),
                //     Mathf.Epsilon,
                //     1.001f
                // );

                //print("Percentage done: " + percentageDone + "current distance:" + currentDistance.ToString());

                speed = movementSpeedCurve.Evaluate(movementSpeedTime);
                yield return null;
            }
        movementSpeedTime = 0;
        worldTransform.position = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);

        // if(x < 0 && !Mathf.Approximately(x, 0))
        // {

        //     while(transform.position.x >= stageHandler.stageTilemap.GetCellCenterWorld(destinationCell).x)
        //     {

        //         transform.Translate(direction * speed * Time.deltaTime);
        //         movementSpeedTime += Time.deltaTime;
        //         speed = movementSpeedCurve.Evaluate(movementSpeedTime);
        //         yield return null;
        //     }
        


        // }
        // else if(x > 0 && !Mathf.Approximately(x, 0))
        // {
        //     while(transform.position.x <= stageHandler.stageTilemap.GetCellCenterWorld(destinationCell).x)
        //     {
        //         transform.Translate(direction * speed * Time.deltaTime);
        //         movementSpeedTime += Time.deltaTime;
        //         speed = movementSpeedCurve.Evaluate(movementSpeedTime);
        //         yield return null;
        //     }            
        // }





        isMoving = false;
    }



}
