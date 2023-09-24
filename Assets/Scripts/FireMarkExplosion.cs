using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMarkExplosion : MonoBehaviour
{

    [SerializeField] Collider2D primaryExplosion;
    [SerializeField] AnimationClip explosionAnimation;
    [SerializeField] AttackPayload attackPayload;

    public int BaseDamage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() 
    {
        StartCoroutine(DisableObject());    
    }



    private void OnCollisionEnter2D(Collision2D other) 
    {
        Collider2D explosionCollider = other.GetContact(0).otherCollider;
        BStageEntity target = other.gameObject.GetComponent<BStageEntity>();
        attackPayload.damage = BaseDamage;
        if(target != null)
        {
            if(explosionCollider != primaryExplosion)
            {
                print("used secondary explosion");
                AttackPayload secondaryPayload = attackPayload;
                secondaryPayload.damage /= 2;
                target.HurtEntity(secondaryPayload);
            }else
            {
                print("used primary explosion");
                target.HurtEntity(attackPayload);
            }



        }
        
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSecondsRealtime(explosionAnimation.length);
        gameObject.SetActive(false);


    }


}
