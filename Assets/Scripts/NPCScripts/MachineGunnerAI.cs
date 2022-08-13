using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum Anims
{
    Gunner_Search,
    Gunner_Target,
    Gunner_Shoot

}
public class MachineGunnerAI : MonoBehaviour
{



    private MachineGunner machineGunner;
    private Animator animator;
    private bool isAttacking = false;
    private bool foundTarget = false;


    void Awake()
    {
        machineGunner = GetComponent<MachineGunner>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.Play(Anims.Gunner_Search.ToString(), 0);

    }

    // Update is called once per frame
    void Update()
    {
        




    }


    IEnumerator SearchForTarget()
    {
        if(!isAttacking && !foundTarget)
        {
        RaycastHit2D hitInfo = Physics2D.Raycast (machineGunner.worldTransform.position, new Vector2(-1, 0),
                                                 Mathf.Infinity, LayerMask.GetMask("Player", "Player_Ally"));

            if(hitInfo)
            {
                foundTarget = true;
                animator.Play(Anims.Gunner_Target.ToString(), 0);
                

            }

        }
        yield return null;
    }

    IEnumerator FoundTarget()
    {
        yield return null;
    }

    IEnumerator Fire()
    {
        yield return null;
    }


}
