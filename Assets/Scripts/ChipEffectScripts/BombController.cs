using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;

public class BombController : ObjectSummonAttributes
{

    PlayerMovement player;
    Animator animator;
    [SerializeField] Transform worldTransform;
    BoxCollider2D boxCollider2D;
    [SerializeField] AnimationCurve yPosCurve;
    [SerializeField] EventReference ExplosionSoundEffect;
    Vector3 InitialPosition;


  

    void Awake()
    {
        InitialPosition = transform.localPosition;

        animator = GetComponent<Animator>();

        player = PlayerMovement.Instance;
        //worldTransform.localPosition.Set(player.worldTransform.localPosition.x, player.worldTransform.localPosition.y, 0);
        //worldTransform = transform.parent.transform;
        boxCollider2D = GetComponent<BoxCollider2D>();   
        boxCollider2D.enabled = false;

        

    }

    void Start()
    {
        
    }

    void OnEnable()
    {
        //worldTransform.localPosition.Set(player.worldTransform.localPosition.x, player.worldTransform.localPosition.y, 0);

        if(AddStatusEffect != EStatusEffects.Default)
        {
            StatusEffect = AddStatusEffect;
        }else
        {
            StatusEffect = InheritedChip.GetStatusEffect();
        }



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
        FMODUnity.RuntimeManager.PlayOneShotAttached(ExplosionSoundEffect, this.gameObject);
        
        boxCollider2D.enabled = true;
        transform.DOLocalMoveY(transform.localPosition.y + 0.2f, 0.25f).SetEase(Ease.Linear).SetUpdate(true);        
        yield return new WaitForSecondsRealtime(0.25f);
        boxCollider2D.enabled = false;

        ResetObjectToInitialState();

  
    }

   void OnTriggerEnter2D(Collider2D other)
    {

        if(other.GetComponent<BStageEntity>())
        {
            BStageEntity victim = other.GetComponent<BStageEntity>();

            victim.hurtEntity((int)((InheritedChip.GetChipDamage() + AddDamage)*player.AttackMultiplier),
            false, true, player, statusEffect: StatusEffect);

        }

    }
    
    public void ResetObjectToInitialState()
    {
        ResetAttributesToInitialState();

        transform.localPosition = InitialPosition;
        worldTransform.localPosition = new Vector3(player.worldTransform.position.x, player.worldTransform.position.y, 0);
        boxCollider2D.enabled = false;
        transform.parent.gameObject.SetActive(false);



    }



    void Update()
    {
        if(worldTransform.position.x > 12)
        {
            transform.parent.gameObject.SetActive(false);
            ResetObjectToInitialState();

        }
    }
}
