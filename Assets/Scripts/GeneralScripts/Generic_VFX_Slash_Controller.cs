using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Generic_VFX_Slash_Controller : ObjectSummonAttributes
{

    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    [SerializeField] ChipSO inheritedChip;
    PlayerMovement player;

    
    void Awake() {

        player = PlayerMovement.Instance;
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
            StatusEffect = inheritedChip.GetStatusEffect();
        }

        boxCollider.enabled = true;
        spriteRenderer.enabled = true;


        StartCoroutine(SelfDestruct());

    }

    void OnTriggerEnter2D(Collider2D other) {
        print("Sword slash hit collider");
        if(other.tag == "Enemy")
        {

            print("Sword slash hit enemy");

            if(other.GetComponent<BStageEntity>()){
            BStageEntity entity = other.GetComponent<BStageEntity>();
            entity.hurtEntity((int)((inheritedChip.GetChipDamage() + AddDamage)*player.AttackMultiplier),
            inheritedChip.IsLightAttack(), inheritedChip.IsHitFlinch(), player, StatusEffect);
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

        AddDamage = 0;
        StatusEffect = inheritedChip.GetStatusEffect();
        AddStatusEffect = EStatusEffects.Default;

        transform.parent.gameObject.SetActive(false);
  

    }


}
