using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunner_Reticle : MonoBehaviour
{
    MachineGunner gunner;
    MachineGunnerAI gunnerAI;
    SpriteRenderer spriteRenderer;
    Animator animator;
    BoxCollider2D boxCollider2D;
    BattleStageHandler stageHandler;
    [SerializeField] AnimationCurve xCurve, yCurve;
    float timeElapsed;
    Transform worldTransform;
    Rigidbody2D body;
    [SerializeField] float speed = 1f;
    [SerializeField] BoxCollider2D attackVFXCollider;
    [SerializeField] Animator attackVFXAnimator;
    [SerializeField] SpriteRenderer attackVFXSpriteRenderer;
    bool started = false;
    bool lockedOn = false;


    void Awake()
    {
        gunner = FindObjectOfType<MachineGunner>();
        gunnerAI = FindObjectOfType<MachineGunnerAI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        stageHandler = FindObjectOfType<BattleStageHandler>();
        worldTransform = transform.parent.transform;


    }

    void Start()
    {
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        //attackVFXCollider.enabled = false;
        attackVFXSpriteRenderer.enabled = false;
        attackVFXAnimator.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(gunner.getHealth() <= 0)
        {
            //StopCoroutine(FireAtTarget());
            StopAllCoroutines();
            boxCollider2D.enabled = false;
            attackVFXAnimator.enabled = false;
            attackVFXSpriteRenderer.enabled = false;
            attackVFXAnimator.speed = 0;
            animator.speed = 0;
        }
        if(gunnerAI.foundTarget && !lockedOn)
        {

           if(!started)
            {
                transform.position = stageHandler.stageTilemap.GetCellCenterWorld
                (stageHandler.playerBoundsList.Find(cell => cell.y == gunner.currentCellPos.y));
                started = true;
            }

            spriteRenderer.enabled = true;
            boxCollider2D.enabled = true;

            moveReticle(new Vector2(-1, 0), speed);

            if(transform.position.x <= 0)
            {
                gunnerAI.foundTarget = false;
                spriteRenderer.enabled = false;
                boxCollider2D.enabled = false;
                transform.position = stageHandler.stageTilemap.GetCellCenterWorld
                (stageHandler.playerBoundsList.Find(cell => cell.y == gunner.currentCellPos.y));
                started = false;
                gunnerAI.animator.Play(GunnerAnims.Gunner_Search.ToString(), 0);
                
            }
        }


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Player_Ally")
        {
            lockedOn = true;
            transform.position = other.transform.position;
            StartCoroutine(FireAtTarget());
        }
        print("Reticle found: " + other.gameObject.name);
    }

    IEnumerator FireAtTarget()
    {
        animator.Play("Reticle_Found");
        yield return new WaitForSeconds(0.4f);

        gunnerAI.animator.Play(GunnerAnims.Gunner_Shoot.ToString());
        attackVFXSpriteRenderer.enabled = true;
        attackVFXAnimator.enabled = true;

        attackVFXAnimator.Play("Gunner_Attack_VFX");
        animator.Play("Reticle_Search");

        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;        
        yield return new WaitForSeconds(1.04f);
        gunnerAI.animator.Play(GunnerAnims.Gunner_Target.ToString());
        attackVFXSpriteRenderer.enabled = false;
        attackVFXAnimator.enabled = false;
        yield return new WaitForSeconds(0.5f);
        gunnerAI.animator.Play(GunnerAnims.Gunner_Search.ToString());

        lockedOn = false;
        gunnerAI.foundTarget = false;
        

        transform.position = stageHandler.stageTilemap.GetCellCenterWorld
        (stageHandler.playerBoundsList.Find(cell => cell.y == gunner.currentCellPos.y));
        

    }




    void moveReticle(Vector2 direction, float speed)
    {
        transform.Translate(direction * speed * Time.deltaTime);

    }




}
