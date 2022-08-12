using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Pathfinding.Util;

public class PlayerMovement : MonoBehaviour, IBattleStageEntity
{

    [SerializeField] public ChipSO activeChip;
    [SerializeField] public List<ChipSO> PlayerChipQueue = new List<ChipSO>();
    [SerializeField] public List<ChipSO> buffChipQueue = new List<ChipSO>();
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] public int playerHP = 100;
    [SerializeField] public float DefenseMultiplier{get;set;} = 1;
    [SerializeField] public float AttackMultiplier {get;set;} = 1;
    BattleStageHandler stageHandler;
    SpriteRenderer spriteRenderer;
    TimeManager timeManager;
    ChipLoadManager chipLoadManager;

    PlayerInput playerInput;

    public Transform parentTransform;
    Vector3 currentPos;
    public Vector3Int currentCellPos;


    public BoxCollider2D boxCollider2D;
    PlayerChipAnimations playerChipAnimations; 
    ChipEffects chipEffect;
    Vector2 moveInput;
    Animator myAnimator;

    bool isAlive = true;
    float animationLength;
    bool isMoving = false;
    bool isUsingChip = false;

    ChipSelectScreenMovement chipSelectScreenMovement;

    BasicShot basicShot;
    public int shotDamage = 5;

    public Collider2D lastContactedCollider = null;
    public bool isInvincible = false;
    public bool SuperArmor = false;

    public string Name => "Megaman";

    public Transform worldTransform { get; set;}

    public int ID => 3;

    public bool stunnable => true;
    public bool stationary => false;
    public bool vulnerable { get;set;} = false;
    public bool isGrounded { get; set; } = true;

    public bool Rooted;

    Color invisible;
    Color opaque;

    public Shader shaderGUItext;
    public Shader shaderSpritesDefault;

    void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        parentTransform = GetComponentInParent<Transform>();
        myAnimator = GetComponent<Animator>();
        healthText.text = playerHP.ToString();
        basicShot = FindObjectOfType<BasicShot>();
        chipEffect = FindObjectOfType<ChipEffects>();
        chipSelectScreenMovement = FindObjectOfType<ChipSelectScreenMovement>();
        playerChipAnimations = GetComponent<PlayerChipAnimations>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        stageHandler = FindObjectOfType<BattleStageHandler>();
        parentTransform = transform.parent.gameObject.GetComponent<Transform>();
        currentCellPos = new Vector3Int ((int)(parentTransform.localPosition.x/1.6f), (int)parentTransform.localPosition.y,0);

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
        
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = true;


        //Vector3 testPosition = battleStageHandler.battleStageTilemap.GetCellCenterWorld(new Vector3Int(4,1,0));
        
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


    void Shoot()
    {
        basicShot.Shoot();
    }

    void setInvincible()
    {
        if(isInvincible){isInvincible = false; return;}
        isInvincible = true;
        
    }

    public Collider2D OnTriggerEnter2D(Collider2D other) 
    {
        lastContactedCollider = other;
        return other;
    }

    void Update()
    {

       //Debug.Log(currentCellPos.ToString());
        
        if(playerHP <= 0)
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


    void simpleMove()
    {
         if(!isAlive){return;}
         if(Rooted){return;}
        
        int index = myAnimator.GetLayerIndex("Base Layer");
        
        if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            if(!checkValidTile(1, 0))
            {return;}

            myAnimator.SetTrigger("Move");
            isMoving = true;
            animationLength = myAnimator.GetCurrentAnimatorStateInfo(index).length;
            //Debug.Log("animation length = " + animationLength);
            Invoke("cellMoveRight", 0.104f);
            
        }
        if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            if(!checkValidTile(-1, 0))
            {return;}

            myAnimator.SetTrigger("Move");
            isMoving = true;
            animationLength = myAnimator.GetCurrentAnimatorStateInfo(index).length;
            //Debug.Log("animation length = " + animationLength);
            Invoke("cellMoveLeft", 0.104f);
            
        }
        if(Keyboard.current.wKey.wasPressedThisFrame)
        {
            if(!checkValidTile(0, 1))
            {return;}
            myAnimator.SetTrigger("Move");
            isMoving = true;
            animationLength = myAnimator.GetCurrentAnimatorStateInfo(index).length;
            //Debug.Log("animation length = " + animationLength);
            Invoke("cellMoveUp", animationLength);
            
        }
        if(Keyboard.current.sKey.wasPressedThisFrame)
        {
            if(!checkValidTile(0, -1))
            {return;}

            myAnimator.SetTrigger("Move");
            isMoving = true;
            animationLength = myAnimator.GetCurrentAnimatorStateInfo(index).length;
            //Debug.Log("animation length = " + animationLength);
            Invoke("cellMoveDown", animationLength);
        }

    }


    public bool checkValidTile(int xDirection, int yDirection)
    {

           //     ||
           // !stageHandler.stageTilemap.GetTile<CustomTile>(coordToCheck).isPassable


        Vector3Int coordToCheck = new Vector3Int(currentCellPos.x + xDirection, currentCellPos.y + yDirection, currentCellPos.z);

            if(
            stageHandler.stageTilemap.GetTile
            (coordToCheck) == null
                ||
            stageHandler.stageTiles
            [stageHandler.stageTilemap.CellToWorld(coordToCheck)].isOccupied
                ||
            !stageHandler.stageTilemap.GetTile<CustomTile>(coordToCheck).isPassable
            ||
            stageHandler.getCustTile(coordToCheck).GetTileTeam() == ETileTeam.Enemy
            )
            {
                
                return false;
            }


        return true;
    }

    public Vector3Int getCurrentCellPos()
    {
        return currentCellPos;
    }

    void cellMoveRight()
    {
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = false;
        currentCellPos.Set(currentCellPos.x + 1, currentCellPos.y, currentCellPos.z);
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = true;
        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
                                    isMoving = false;
    }
    void cellMoveLeft()
    {
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = false;
        currentCellPos.Set(currentCellPos.x - 1, currentCellPos.y, currentCellPos.z);
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = true;
        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
                                    isMoving = false;
    }
    void cellMoveUp()
    {
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = false;
        currentCellPos.Set(currentCellPos.x, currentCellPos.y + 1, currentCellPos.z);
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = true;
        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
                                    isMoving = false;
    }
    void cellMoveDown()
    {
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = false;
        currentCellPos.Set(currentCellPos.x, currentCellPos.y - 1, currentCellPos.z);
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = true;
        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
                                    isMoving = false;
    }
    

   

    public IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(animationLength);
    }

    void OnFire()
    {
        
            myAnimator.SetTrigger("Shoot");
        
    }


    public void hurtEntity(int damageAmount,
        bool lightAttack,
        bool hitStun,
        bool pierceCloaking = false,
        EStatusEffects statusEffect = EStatusEffects.Default)
    {
        if(isInvincible){return;}

        if(!SuperArmor || hitStun ){
            myAnimator.Play(EMegamanAnimations.Megaman_Hurt.ToString());
        }
        
        if(damageAmount * DefenseMultiplier >= playerHP)
        {
            isAlive = false;
            playerHP = 0;
            return;
        }

        playerHP = playerHP - (int)(damageAmount * DefenseMultiplier);
        healthText.text = playerHP.ToString();

        if(!lightAttack){
        StartCoroutine(InvincibilityFrames(1f));
        }
        StartCoroutine(ChangeAnimState(EMegamanAnimations.Megaman_Idle.ToString(), EMegamanAnimations.Megaman_Hurt.ToString()));
        
        return;
    }

    IEnumerator ChangeAnimState(string stateName, string transitionState)
    {
        yield return new WaitForSeconds(GetAnimationLength(transitionState));
        myAnimator.Play(stateName);
    }

    float GetAnimationLength(string stateName)
    {
        int index = myAnimator.GetLayerIndex("Base Layer");
        animationLength = myAnimator.GetCurrentAnimatorStateInfo(index).length;
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


    public int getHealth()
    {
        return playerHP;
    }

    public int basicShotDamage()
    {
        return shotDamage;
    }

    public void setHealthText(int number)
    {
        number = playerHP;
        healthText.text = number.ToString();
    }

    public Vector3Int getCellPosition()
    {
        return currentCellPos;
    }

    public void setCellPosition(int x, int y)
    {
        currentCellPos.Set(x, y, currentCellPos.z);
        parentTransform.transform.localPosition = stageHandler.stageTilemap.
                                                    GetCellCenterWorld(currentCellPos);
    }

    public void setHealth(int value)
    {
        playerHP = value;
    }

    public IEnumerator setStatusEffect(EStatusEffects status)
    {

       switch (status) 
       {
        case EStatusEffects.Paralyzed: 
            myAnimator.speed = 0f;
            yield return new WaitForSecondsRealtime(1f);
            myAnimator.speed = 1f;
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
