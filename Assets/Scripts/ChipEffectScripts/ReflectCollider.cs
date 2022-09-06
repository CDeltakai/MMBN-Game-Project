using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectCollider : MonoBehaviour
{
    float parryDuration = 0.417f;
    ChipEffects chipEffects;
    Reflect reflect;
    BoxCollider2D boxCollider2D;
    PlayerMovement player;
    int count = 0;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        reflect = FindObjectOfType<Reflect>();
        chipEffects = FindObjectOfType<ChipEffects>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        Collider2D[] detectedColliders = new Collider2D[10];
        ContactFilter2D filter2D = new ContactFilter2D().NoFilter();
        boxCollider2D.offset = new Vector2(player.worldTransform.localPosition.x, player.worldTransform.localPosition.y - 0.5f);

        StartCoroutine(SelfDestruct());

    }

    // Update is called once per frame
    void Update()
    {
        

    }


 

    public void HitByRay()
    {
        if(count == 0){

            count += 1;
            Debug.Log("Ray reflected using HitByRay");
            RaycastHit2D hitInfo = Physics2D.Raycast (chipEffects.firePoint.position, chipEffects.firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies"));
            if(hitInfo)
            {
                BStageEntity script = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
                script.hurtEntity((int)((reflect.BaseDamage + reflect.AdditionalDamage)*player.AttackMultiplier), false, true, player);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        

        if(other.tag == "Enemy_Attack_Reflectable" && count == 0 )
        {
            count += 1;
            Debug.Log("Projectile reflected using OnTriggerEnter2D");
            RaycastHit2D hitInfo = Physics2D.Raycast (chipEffects.firePoint.position, chipEffects.firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies"));
            if(hitInfo)
            {
                BStageEntity script = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
                script.hurtEntity((int)((reflect.BaseDamage + reflect.AdditionalDamage)*player.AttackMultiplier), false, true, player);
                
            }
        }

    reflect.AdditionalDamage = 0;

    }
    

    IEnumerator SelfDestruct()
    {

        yield return new WaitForSeconds(parryDuration);
        count = 0;
        Destroy(gameObject);
    }
}
