using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Longsword_slash : MonoBehaviour
{

    BoxCollider2D boxCollider;
    Longsword longsword;
    PlayerMovement player;
    // Start is called before the first frame update
    
    void Awake() {
        player = FindObjectOfType<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
        longsword = FindObjectOfType<Longsword>();
    }
    
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other) {
        print("Longsword hit collider");
        if(other.tag == "Enemy")
        {

            print("Longsword hit enemy");
            IBattleStageEntity script = other.GetComponent<IBattleStageEntity>();
            script.hurtEntity((int)((longsword.BaseDamage + longsword.AdditionalDamage)*player.AttackMultiplier), false, true);
        }

    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(0.375f);

    }


}
