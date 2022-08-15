using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using System;

public abstract class BStageEntity : MonoBehaviour
{
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
    [SerializeField] bool isInvincible = false;
    [SerializeField] public int currentHP;
    [SerializeField] public int shieldPoints;
    [SerializeField] public float DefenseMultiplier = 1;
    [SerializeField] public float AttackMultiplier = 1;


    public virtual void Awake()
    {
        stageHandler = FindObjectOfType<BattleStageHandler>();
        worldTransform = transform.parent.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = shaderSpritesDefault = Shader.Find("Sprites/Default");



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
            currentHP = 0;
            healthText.text = currentHP.ToString();
            healthText.enabled = false;
            animator.speed = 0;
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
        
            //stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
            //stageHandler.setEntityAtCell(currentCellPos.x, currentCellPos.y, this, false);
            stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
            currentCellPos.Set(x, y, 0);
            //stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
            //stageHandler.setEntityAtCell(currentCellPos.x, currentCellPos.y, this, true);
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

    public bool checkValidTile(int x, int y)
    {
        Vector3Int coordToCheck = new Vector3Int(x, y, 0);
        
            if(stageHandler.getCustTile(coordToCheck).GetTileTeam() != team
                ||
            stageHandler.stageTilemap.GetTile
            (coordToCheck) == null
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

    public void cellMove(int x, int y)
    {
        if(!checkValidTile(currentCellPos.x + x, currentCellPos.y + y))
        {return;}

        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, false);
        currentCellPos.Set(currentCellPos.x + x, currentCellPos.y + y, 0);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
        worldTransform.position = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }


    public void cellMoveRight()
    {
        if(!checkValidTile(currentCellPos.x + 1, currentCellPos.y))
        {return;}

        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        stageHandler.setEntityAtCell(currentCellPos.x, currentCellPos.y, this, false);

        currentCellPos.Set(currentCellPos.x + 1, currentCellPos.y, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
        stageHandler.setEntityAtCell(currentCellPos.x, currentCellPos.y, this, true);
        

        worldTransform.position = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }
    public void cellMoveLeft()
    {

        if(!checkValidTile(currentCellPos.x - 1, currentCellPos.y))
        {return;}

        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        stageHandler.setEntityAtCell(currentCellPos.x, currentCellPos.y, this, false);
        currentCellPos.Set(currentCellPos.x - 1, currentCellPos.y, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
        stageHandler.setEntityAtCell(currentCellPos.x, currentCellPos.y, this, true);

        worldTransform.position = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }
    public void cellMoveUp()
    {
        if(!checkValidTile(currentCellPos.x, currentCellPos.y + 1))
        {return;}
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        currentCellPos.Set(currentCellPos.x, currentCellPos.y + 1, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);

        worldTransform.position = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }
    public void cellMoveDown()
    {

        if(!checkValidTile(currentCellPos.x, currentCellPos.y - 1))
        {return;}

        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        currentCellPos.Set(currentCellPos.x, currentCellPos.y - 1, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);

        worldTransform.position = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }
    
    public Vector3Int getCurrentCellPos()
    {
        return currentCellPos;
    }




}
