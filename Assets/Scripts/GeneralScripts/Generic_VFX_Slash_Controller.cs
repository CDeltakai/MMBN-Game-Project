using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Generic_VFX_Slash_Controller : ObjectSummonAttributes
{

    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    PlayerMovement player;

    
    void Awake() {

        player = FindObjectOfType<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;

    }
    



    void OnEnable()
    {
        if(AddStatusEffect != EStatusEffects.Default)
        {
            StatusEffect = AddStatusEffect;
        }else
        {
            StatusEffect = InheritedChip.GetStatusEffect();
        }

        boxCollider.enabled = true;
        spriteRenderer.enabled = true;


        StartCoroutine(SelfDisable());

    }

    void OnTriggerEnter2D(Collider2D other) {
        //print("Sword slash hit collider");
        if(other.tag == "Enemy")
        {

            if(other.GetComponent<BStageEntity>()){
                BStageEntity entity = other.GetComponent<BStageEntity>();
                applyDamage(entity);

          

                return;
            }
            


        }

    }
    

    IEnumerator SelfDisable()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
        yield return new WaitForSecondsRealtime(0.25f);

        AddDamage = 0;
        StatusEffect = InheritedChip.GetStatusEffect();
        AddStatusEffect = EStatusEffects.Default;
        AddObjectSummon = null;

        transform.parent.gameObject.SetActive(false);
  

    }


}
