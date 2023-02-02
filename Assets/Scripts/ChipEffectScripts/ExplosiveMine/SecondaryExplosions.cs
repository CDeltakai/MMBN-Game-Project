using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryExplosions : MonoBehaviour
{

    ExplosiveMine explosiveMineScript;
    [SerializeField] bool IsPrimaryExplosion;
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    PlayerMovement player;

    [HideInInspector]
    public EStatusEffects addStatus = EStatusEffects.Default;
    [HideInInspector]
    public int Damage = 10;


    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = PlayerMovement.Instance;
        explosiveMineScript = gameObject.transform.parent.transform.parent.gameObject.GetComponent<ExplosiveMine>();

        if(IsPrimaryExplosion)
        {
            Damage = explosiveMineScript.InheritedChip.GetChipDamage();
        }else
        {
            Damage = explosiveMineScript.InheritedChip.GetChipDamage()/2;
        }
        
        boxCollider2D.enabled = false;
        gameObject.SetActive(false);

    }


    void OnEnable()
    {
        if(IsPrimaryExplosion)
        {
            Damage = explosiveMineScript.InheritedChip.GetChipDamage();
        }else
        {
            Damage = explosiveMineScript.InheritedChip.GetChipDamage()/2;
        }
        
        StartCoroutine(InitiateExplosion());
    }



   void OnTriggerEnter2D(Collider2D other)
    {

        if(other.GetComponent<BStageEntity>())
        {
            BStageEntity victim = other.GetComponent<BStageEntity>();

            victim.hurtEntity((int)((Damage)),
            false, true, player, statusEffect: addStatus);

        }

    }



    IEnumerator InitiateExplosion()
    {
        boxCollider2D.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        boxCollider2D.enabled = false;
        yield return new WaitForSecondsRealtime(0.15f);
        gameObject.SetActive(false);
    }

}
