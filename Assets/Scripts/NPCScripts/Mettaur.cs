
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Mettaur : MonoBehaviour, IBattleStageEntity, IStage_MoveableEntity
{

    BoxCollider2D mettaurCollider;
    BattleStageHandler stageHandler;
    Animator animator;
    float animationLength;
    string currentAnimation;
    public Transform parentTransform;
    [SerializeField] GameObject mettaurProjectile;
    [SerializeField] public int currentHP = 40;
    [SerializeField] TextMeshProUGUI healthText;


    public const string METTAUR_IDLE = "Mettaur Idle";
    public const string METTAUR_ATTACK = "Mettaur Attack";
    public const string METTAUR_DEFEND = "Mettaur Defend";
    private float time = 0.0f;
    public bool isAttacking = false;

    public string Name => "Mettaur";

    public int ID => 0;

    public bool stunnable => true;
    public bool stationary => false;
    public bool vulnerable {get;set;} = false;

    public Transform worldTransform {get;set;}
    public float AttackMultiplier {get;set;} = 1;
    public float DefenseMultiplier {get;set;}= 1;

    public Vector3Int currentCellPos;



    void Start()
    {
        //vulnerable = false;
        currentCellPos.z = 0;
        mettaurCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        healthText.text = currentHP.ToString();
        parentTransform = transform.parent.gameObject.GetComponent<Transform>();
        stageHandler = FindObjectOfType<BattleStageHandler>();
        

        currentCellPos.x = (int)(parentTransform.localPosition.x/1.6f);
        currentCellPos.y = (int)parentTransform.localPosition.y;

        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
        


    }

    public IEnumerator ChangeAnimationState(string stateName)
    {
        animator.Play(stateName);
        float delay = GetAnimationLength(METTAUR_ATTACK);
        isAttacking = true;
        yield return new WaitForSeconds(delay * 9);
        animator.Play(METTAUR_IDLE);
        yield return new WaitForSecondsRealtime(2);
        isAttacking = false;
    }


    public void Attack()
    {
        if(isAttacking){return;};

        StartCoroutine(ChangeAnimationState(METTAUR_ATTACK));    
    }

    float GetAnimationLength(string stateName)
    {
        int index = animator.GetLayerIndex("Base Layer");
        animationLength = animator.GetCurrentAnimatorStateInfo(index).length;
        return animationLength * 1.1f;
    }


    void Update()
    {
        //Debug.Log("Mettaur Current CellPos: " + currentCellPos.ToString());

        //time += Time.deltaTime;
 
        //if(isAttacking){return;};

        //StartCoroutine(ChangeAnimationState(METTAUR_ATTACK)); 
    }

    void fireProjectile()
    {
        Instantiate(mettaurProjectile, new Vector2 (parentTransform.position.x - 1.6f, parentTransform.position.y), transform.rotation);
    }



    public void cellMoveRight()
    {
        if(!checkValidTile(currentCellPos.x + 1, currentCellPos.y, currentCellPos.z))
        {return;}

        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        currentCellPos.Set(currentCellPos.x + 1, currentCellPos.y, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);

        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
                                    //isMoving = false;
    }
    public void cellMoveLeft()
    {

        if(!checkValidTile(currentCellPos.x - 1, currentCellPos.y, currentCellPos.z))
        {return;}

        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        currentCellPos.Set(currentCellPos.x - 1, currentCellPos.y, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);

        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
                                    //isMoving = false;
    }
    public void cellMoveUp()
    {
        if(!checkValidTile(currentCellPos.x, currentCellPos.y + 1, currentCellPos.z))
        {return;}
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        currentCellPos.Set(currentCellPos.x, currentCellPos.y + 1, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);

        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
                                    //isMoving = false;
    }
    public void cellMoveDown()
    {

        if(!checkValidTile(currentCellPos.x, currentCellPos.y - 1, currentCellPos.z))
        {return;}

        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        currentCellPos.Set(currentCellPos.x, currentCellPos.y - 1, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);

        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
                                    //isMoving = false;
    }
    

    public int getHealth()
    {
        return currentHP;
    }

    public void setHealth(int value){currentHP = value;}

    public void setHealthText(int number)
    {
        number = currentHP;
        healthText.text = number.ToString();

    }

    public Vector3Int getCellPosition()
    {
        return currentCellPos;
    }

    public void setCellPosition(int x, int y)
    {
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        currentCellPos.Set(x, y, currentCellPos.z);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }

    public void hurtEntity(int damage,
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
            animator.speed = 0;
            StartCoroutine(DestroyEntity());
            //Destroy(transform.parent.gameObject);
            //Destroy(gameObject);
            return;

        }

        currentHP = currentHP - (int)(damage * DefenseMultiplier);
        healthText.text = currentHP.ToString();
        
        return;    
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

        var vfx = Addressables.InstantiateAsync("VFX_Destruction_Explosion", parentTransform.position, transform.rotation, transform.parent.transform);
        
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
}
