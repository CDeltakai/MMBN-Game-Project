using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_VFX_Slash_Controller : MonoBehaviour
{

    BoxCollider2D boxCollider;
    Generic_Sword sword;
    PlayerMovement player;

    // Start is called before the first frame update
    
    void Awake() {
        player = FindObjectOfType<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
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
            IBattleStageEntity script = other.GetComponent<IBattleStageEntity>();
            script.hurtEntity((int)((sword.BaseDamage + sword.AdditionalDamage)*player.AttackMultiplier), false, true);
        }

    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSecondsRealtime(0.025f);
        boxCollider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);

    }


}
