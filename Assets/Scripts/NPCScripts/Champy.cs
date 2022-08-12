using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Animations;

public class Champy : MonoBehaviour, IBattleStageEntity, IStage_MoveableEntity
{
    public string Name => "Champy";

    public int ID => 1;
    BattleStageHandler stageHandler;
    Animator animator;
    SpriteRenderer spriteRenderer;


    public bool stunnable => false;
    public bool stationary => false;

    public bool vulnerable { get; set; } = false;
    public Transform worldTransform {get; set;}
    [SerializeField]public float AttackMultiplier {get;set;} = 1;
    [SerializeField]public float DefenseMultiplier {get;set;} = 1;
    public bool isGrounded {get; set;} = false;

    public Vector3Int currentCellPos;

    [SerializeField] public int currentHP = 60;
    [SerializeField] TextMeshProUGUI healthText;

    public Shader shaderGUItext;
    public Shader shaderSpritesDefault;

    public bool hasMoved = false;


    void Awake() {
        worldTransform = transform.parent.gameObject.GetComponent<Transform>();
        currentCellPos.x = (int)(worldTransform.localPosition.x/1.6);
        currentCellPos.y = (int)(worldTransform.localPosition.y);
        
    }

    void Start()
    {
        stageHandler = FindObjectOfType<BattleStageHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthText.text = currentHP.ToString();
        animator = GetComponent<Animator>();
        //stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = true;
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);


    }

    public Transform getWorldPosition()
    {
        return worldTransform;
    }


    void Update()
    {
        worldTransform = transform.parent.gameObject.GetComponent<Transform>();
        //print(worldPosition.localPosition.ToString());

    }

    void setSolidColor(Color color)
    {
        spriteRenderer.material.shader = shaderGUItext;
        spriteRenderer.color = color;
    }
    void setNormalSprite()
    {
        spriteRenderer.material.shader = shaderSpritesDefault;
        spriteRenderer.color = Color.white;
    }

    public Vector3Int getCellPosition()
    {
        currentCellPos.x = (int)(worldTransform.localPosition.x/1.6);
        currentCellPos.y = (int)(worldTransform.localPosition.y);
        return currentCellPos;
    }

    public void hurtEntity(int damage,
                           bool lightAttack,
                           bool hitStun,
                           bool pierceCloaking = false,
                           EStatusEffects statusEffect = EStatusEffects.Default)
    {

        int postMitigationDmg = (int)(damage * DefenseMultiplier);

        if(postMitigationDmg >= currentHP)
        {
            currentHP = 0;
            
            healthText.text = "0";
            healthText.enabled = false;
            stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = false;
            animator.speed = 0;
            StartCoroutine(DestroyEntity());
            //Destroy(transform.parent.gameObject);
            //Destroy(gameObject);
            return;
        }

        currentHP -= postMitigationDmg;
        healthText.text = currentHP.ToString();
   
    }

    public void setCellPosition(int x, int y)
    {
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        currentCellPos.Set(x, y, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);

        worldTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }


    public void setCellPosition_MaintainOccupied(int x, int y)
    {

        if(hasMoved)
        {
            stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
            currentCellPos.Set(x, y, currentCellPos.z);
        }

        currentCellPos.Set(x, y, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);

        worldTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }

    public int getHealth()
    {
        return currentHP;
    }
    public void setHealth(int value)
    {
        currentHP = value;
    }

    public IEnumerator setStatusEffect(EStatusEffects status)
    {
        animator.speed = 0;
        yield return new WaitForSeconds(1.5f);
        animator.speed = 1;

    }

    IEnumerator DestroyEntity()
    {
        yield return new WaitForSeconds(0.0005f);

        var vfx = Addressables.InstantiateAsync("VFX_Destruction_Explosion", transform.parent.transform.position, 
                                                transform.rotation, transform.parent.transform);

        yield return new WaitForSeconds(0.320f);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }


    public bool checkValidTile(int x, int y, int z)
    {
        Vector3Int coordToCheck = new Vector3Int(x, y, z);
        

            if(stageHandler.stageTilemap.GetTile
            (coordToCheck) == stageHandler.PlayerTile
                ||
            stageHandler.stageTilemap.GetTile
            (coordToCheck) == null
                ||
            stageHandler.stageTiles
            [stageHandler.stageTilemap.CellToWorld(coordToCheck)].isOccupied
            )
            {
                return false;
            }

        return true;
    }

    public void cellMoveUp()
    {
        throw new System.NotImplementedException();
    }

    public void cellMoveDown()
    {
        throw new System.NotImplementedException();
    }

    public void cellMoveRight()
    {
        throw new System.NotImplementedException();
    }

    public void cellMoveLeft()
    {
        throw new System.NotImplementedException();
    }
}
