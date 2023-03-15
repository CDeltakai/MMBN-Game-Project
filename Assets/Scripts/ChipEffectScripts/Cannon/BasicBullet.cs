using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{

    public ChipEffectBlueprint parentChip;
    public EffectPropertyContainer effectPropertyContainer;

    public PlayerMovement player;
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TrailRenderer trail;
    [SerializeField] float projectileDotTweenSpeed = 0.1f;
    
    Rigidbody2D rigbody;

    Sequence movement;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public RaycastHit2D hitPosition;
    public int Damage;
    [SerializeField] Vector2 velocityInSlowMotion = new Vector2(5, 0);

    public bool InitializeSlowBullet = false;

    // public int DamageModifier = 0;
    // public EStatusEffects StatusEffectModifier = EStatusEffects.Default;
    // protected bool lightAttack;
    // protected bool hitFlinch;
    // protected bool pierceUntargetable;


    void Awake()
    {
        rigbody = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        boxCollider2D.enabled = false;
        //movement = DOTween.Sequence(this);
 
    }

    void Start()
    {

        // StatusEffectModifier = parentChip.StatusEffectModifier;
        // DamageModifier = parentChip.DamageModifier;
        // lightAttack = parentChip.chip.IsLightAttack();
        // hitFlinch = parentChip.chip.IsHitFlinch();
        // pierceUntargetable = parentChip.chip.IsPierceUntargetable();

    }


    public IEnumerator MoveBullet()
    {
        if(TimeManager.isCurrentlySlowedDown)
        {

            //StartCoroutine(TimedDestruction(0.35f));
            boxCollider2D.enabled = true;
            InitializeSlowBullet = true;
            //transform.DOMoveX(endPosition.x+0.4f, 0.3f);

            yield break;


        }

        transform.DOMoveX(endPosition.x+0.3f, projectileDotTweenSpeed).SetUpdate(true);
        yield return new WaitForSecondsRealtime(projectileDotTweenSpeed);
        DestroyObject();




    }


    private void OnTriggerEnter2D(Collider2D other) 
    {

        if(other.tag == "Enemy" || other.tag == "Obstacle")
        {
            print("projectile hit a target");
            BStageEntity target = other.GetComponent<BStageEntity>();

            parentChip.OnActivationEffect(target);            

            //parentChip.OnActivationEffect(target);
            DestroyObject();
        }




    }

    void DestroyObject()
    {
        gameObject.SetActive(false);
        Destroy(gameObject.transform.parent.gameObject);
        Destroy(gameObject);        
    }

    IEnumerator TimedDestruction(float duration)
    {
        yield return new WaitForSeconds(duration);
        DestroyObject();
    }

    IEnumerator DestroyBullet()
    {




        gameObject.SetActive(false);
        Destroy(gameObject.transform.parent.gameObject);
        yield break;

    }

    void Update()
    {
        if(InitializeSlowBullet)
        {
            if(!TimeManager.isCurrentlySlowedDown)
            {
                rigbody.MovePosition(rigbody.position + (velocityInSlowMotion*2) * Time.deltaTime);
                return;
            }

            rigbody.MovePosition(rigbody.position + velocityInSlowMotion * Time.deltaTime);
        }


    }

    




}
