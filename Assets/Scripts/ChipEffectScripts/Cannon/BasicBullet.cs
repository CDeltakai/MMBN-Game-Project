using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{

    public ChipEffectBlueprint parentChip;
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TrailRenderer trail;
    Rigidbody2D rigbody;

    Sequence movement;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public RaycastHit2D hitPosition;
    public int Damage;
    [SerializeField] Vector2 velocityInSlowMotion = new Vector2(5, 0);

    public bool InitializeBullet = false;


    void Awake()
    {
        rigbody = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        boxCollider2D.enabled = false;
        //movement = DOTween.Sequence(this);
 
    }

    void Start()
    {
        //StartCoroutine(MoveBullet());


    }


    public IEnumerator MoveBullet()
    {
        if(TimeManager.isCurrentlySlowedDown)
        {

            //StartCoroutine(TimedDestruction(0.35f));
            boxCollider2D.enabled = true;
            InitializeBullet = true;
            //transform.DOMoveX(endPosition.x+0.4f, 0.3f);

            yield break;


        }

        transform.DOMoveX(endPosition.x+0.3f, 0.1f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.1f);
        DestroyObject();




    }


    private void OnTriggerEnter2D(Collider2D other) 
    {

        if(other.GetComponent<BStageEntity>())
        {
            print("projectile hit a target");
            BStageEntity target = other.GetComponent<BStageEntity>();
            parentChip.applyChipDamage(target);
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
        if(InitializeBullet)
        {
            rigbody.MovePosition(rigbody.position + velocityInSlowMotion * Time.deltaTime);
        }


    }

    




}
