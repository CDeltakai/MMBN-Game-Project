using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using System;

public abstract class BStageEntity : MonoBehaviour
{
    public delegate void MoveOntoTileEvent(int x, int y, BStageEntity entity);
    public event MoveOntoTileEvent moveOntoTile;

    public delegate void MoveOffTileEvent(int x, int y, BStageEntity entity);
    public event MoveOffTileEvent moveOffTile;

    TileEventManager tileEventManager;

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

    public Vector3Int currentCellPos;
    [SerializeField] public bool isInvincible = false;
    [SerializeField] public int currentHP;
    [SerializeField] public int shieldPoints;
    [SerializeField] public float DefenseMultiplier = 1;
    [SerializeField] public float AttackMultiplier = 1;


    public virtual void Awake()
    {
        tileEventManager = FindObjectOfType<TileEventManager>();
        stageHandler = FindObjectOfType<BattleStageHandler>();
        worldTransform = transform.parent.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = shaderSpritesDefault = Shader.Find("Sprites/Default");



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




}
