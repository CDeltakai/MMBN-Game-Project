using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;


public abstract class BStageEntity : MonoBehaviour
{
    protected BattleStageHandler stageHandler;
    public Transform worldTransform;
    protected SpriteRenderer spriteRenderer;
    protected Shader shaderGUItext;
    protected Shader shaderSpritesDefault;
    [SerializeField] public TextMeshProUGUI healthText;

    public abstract bool isGrounded{get;set;}
    public abstract bool isStationary{get;}
    public abstract bool isStunnable{get;}
    public abstract int maxHP{get;}
    public abstract ETileTeam team{get;}

    public Vector3Int currentCellPos;
    [SerializeField] public int currentHP;
    [SerializeField] public int shieldPoints;
    [SerializeField] public float DefenseMultiplier = 1;
    [SerializeField] public float AttackMultiplier = 1;


    public virtual void Awake()
    {
        stageHandler = FindObjectOfType<BattleStageHandler>();
        worldTransform = transform.parent.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public virtual void hurtEntity(int damage,
                                   bool lightAttack,
                                   bool hitStun,
                                   bool pierceCloaking = false,
                                   EStatusEffects statusEffect = EStatusEffects.Default)
    {
        if(damage >= currentHP)
        {
            currentHP = 0;
            healthText.text = currentHP.ToString();
            stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = false;
            healthText.enabled = false;
            StartCoroutine(DestroyEntity());
            return;
        }
        currentHP = currentHP - (int)(damage * DefenseMultiplier);
        healthText.text = currentHP.ToString();
        return; 
    }
    

    public virtual IEnumerator DestroyEntity()
    {
        yield return new WaitForSeconds(0.0005f);
        var vfx = Addressables.InstantiateAsync("VFX_Destruction_Explosion", transform.parent.transform.position, 
                                                transform.rotation, transform.parent.transform);
        yield return new WaitForSeconds(0.320f);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }

    public void setCellPosition(int x, int y)
    {
            stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
            currentCellPos.Set(x, y, currentCellPos.z);
            stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
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


}
