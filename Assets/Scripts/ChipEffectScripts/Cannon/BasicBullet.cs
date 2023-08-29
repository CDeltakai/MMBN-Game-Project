using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{

    public ChipEffectBlueprint parentChip;
    public EffectProperties effectPropertyContainer;
    public int pierceCount = 0;
    public bool IsProjectileAlways = false;

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
    [SerializeField] public Vector2 velocityInSlowMotion = new Vector2(5, 0);

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

    public void StartBullet()
    {
        StartCoroutine(MoveBullet());
    }

    public IEnumerator MoveBullet()
    {
        if(TimeManager.isCurrentlySlowedDown || IsProjectileAlways)
        {

            //StartCoroutine(TimedDestruction(0.35f));
            boxCollider2D.enabled = true;
            InitializeSlowBullet = true;
            //transform.DOMoveX(endPosition.x+0.4f, 0.3f);
            yield return new WaitForSeconds(2f);
            DestroyObject();
            yield break;


        }

        transform.DOMoveX(endPosition.x+0.3f, projectileDotTweenSpeed).SetUpdate(true);
        yield return new WaitForSecondsRealtime(projectileDotTweenSpeed);
        DestroyObject();




    }




    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Obstacle")
        {
            print("projectile hit a target");
            BStageEntity target = other.gameObject.GetComponent<BStageEntity>();

            parentChip.OnActivationEffect(target);            
            pierceCount--;
            if(pierceCount < 0)
            {
                DestroyObject();    
            }
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
