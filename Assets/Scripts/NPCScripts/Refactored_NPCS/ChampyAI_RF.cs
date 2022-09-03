using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampyAI_RF : MonoBehaviour
{
    Champy_RF champy;



    void Start()
    {
        champy = GetComponent<Champy_RF>();
    }

    // Update is called once per frame
    void Update()
    {
        if(champy.currentHP <= 0)
        {
            return;
        }

        if(!champy.isAttacking){
        RaycastHit2D hitInfo = Physics2D.Raycast (champy.worldTransform.position, new Vector2(-1, 0), Mathf.Infinity, LayerMask.GetMask("Player", "Player_Ally"));

            if(hitInfo)
            {
                champy.isAttacking = true;
                StartCoroutine(champy.AttackAnimation());

            }

        }
    }
}
