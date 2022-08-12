using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;


public abstract class BStageEntity : MonoBehaviour
{
    protected BattleStageHandler stageHandler;
    protected Transform worldTransform;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] public TextMeshProUGUI healthText;

    public Vector3Int currentCellPos;
    public abstract bool isGrounded{get;set;}
    public abstract bool isStationary{get;}
    public abstract bool isStunnable{get;}
    public abstract int maxHP{get;}

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


}
