using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Generic_VFX_Slash_Controller : MonoBehaviour
{

    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    Generic_Sword sword;
    PlayerMovement player;

    
    void Awake() {
        player = FindObjectOfType<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;
        sword = FindObjectOfType<Generic_Sword>();
    }
    
    void Start()
    {
        StartCoroutine(SelfDestruct());

    }

    void OnTriggerEnter2D(Collider2D other) {
        print("Sword slash hit collider");
        if(other.tag == "Enemy")
        {

            print("Sword slash hit enemy");

            if(other.GetComponent<BStageEntity>()){
            BStageEntity abstractScript = other.GetComponent<BStageEntity>();
            abstractScript.hurtEntity((int)((sword.BaseDamage + sword.AdditionalDamage)*player.AttackMultiplier),
            false, true, player, statusEffect: sword.chipStatusEffect);
            return;
            }
            


        }

    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
        yield return new WaitForSecondsRealtime(0.25f);


        Destroy(transform.parent.gameObject);
        Destroy(gameObject);

    }


}
