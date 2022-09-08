using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BombController : MonoBehaviour
{

    Bomb bomb;
    PlayerMovement player;
    Animator animator;
    public Transform worldTransform;
    BoxCollider2D boxCollider2D;
    [SerializeField] AnimationCurve yPosCurve;
    float movementTime;
    Vector3 destination;
    int BaseDamage;
    int AdditionalDamage;
    EStatusEffects chipStatusEffect;

    void Awake()
    {

        bomb = FindObjectOfType<Bomb>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
        worldTransform = transform.parent.transform;
        boxCollider2D = GetComponent<BoxCollider2D>();   
        boxCollider2D.enabled = false;
    }

    void Start()
    {
        BaseDamage = bomb.BaseDamage;
        AdditionalDamage = bomb.AdditionalDamage;
        chipStatusEffect = bomb.chipStatusEffect;        
        StartCoroutine(MoveBomb());
    }

    [SerializeField] float MoveYValue = 3;
    IEnumerator MoveBomb()
    {
        worldTransform.DOMoveX(worldTransform.position.x + 6.7f, 0.75f).SetUpdate(true);//.SetLoops(-1, LoopType.Restart);
        worldTransform.DOMoveY(worldTransform.position.y + MoveYValue, 0.75f).SetEase(yPosCurve).SetUpdate(true);//.SetLoops(-1, LoopType.Restart);
        transform.DOLocalRotate(new Vector3(0, 0, 360), 0.25f, RotateMode.FastBeyond360).SetLoops(2, LoopType.Restart)
        .SetEase(Ease.Linear).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.75f);
        transform.rotation.Set(0, 0, 0, 0);
        animator.Play("BombExplosionVFX");
        
        boxCollider2D.enabled = true;
        transform.DOLocalMoveY(transform.localPosition.y + 0.2f, 0.25f).SetEase(Ease.Linear).SetUpdate(true);        
        yield return new WaitForSecondsRealtime(0.25f);
        boxCollider2D.enabled = false;


        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }

   void OnTriggerEnter2D(Collider2D other)
    {

        if(other.GetComponent<BStageEntity>())
        {
            BStageEntity victim = other.GetComponent<BStageEntity>();

            victim.hurtEntity((int)((BaseDamage + AdditionalDamage)*player.AttackMultiplier),
            false, true, player, statusEffect: chipStatusEffect);

        }

    }
    

    void Update()
    {
        if(worldTransform.position.x > 12)
        {
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
}
