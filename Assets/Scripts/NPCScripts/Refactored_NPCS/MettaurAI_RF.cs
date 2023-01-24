using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MettaurAI_RF : MonoBehaviour
{

    private PlayerMovement player;
    private Mettaur_RF mettaur;
    private Vector3Int targetPosition;
    private Vector3Int mettaurPosition;
    private float movementCooldownTimer;
    private float movementCooldown = 1f;

    void Awake()
    {
        mettaur = GetComponent<Mettaur_RF>();
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        movementCooldownTimer = movementCooldown;
        

    }

    // Update is called once per frame
    void Update()
    {
        if(mettaur.currentHP <= 0)
        {return;}
        if(mettaur.isAttacking){return;}
        targetPosition = player.getCurrentCellPos();
        mettaurPosition = mettaur.getCurrentCellPos();


        if(movementCooldownTimer > 0)
        {
            movementCooldownTimer -= Time.deltaTime;
        }

        if(movementCooldownTimer <= 0){

            if(mettaurPosition.y == targetPosition.y)
            {
                StartCoroutine(mettaur.AttackAnimation());
                movementCooldownTimer = movementCooldown + UnityEngine.Random.Range(0.1f, 0.2f);
            }
            else
            if(mettaurPosition.y < targetPosition.y)
            {
                //mettaur.cellMoveUp();
                mettaur.cellMoveVerified(0, 1);
                movementCooldownTimer = movementCooldown + UnityEngine.Random.Range(0.1f, 0.2f);;
            }else
            if(mettaurPosition.y > targetPosition.y)
            {
                //mettaur.cellMoveDown();
                mettaur.cellMoveVerified(0, -1);
                movementCooldownTimer = movementCooldown + UnityEngine.Random.Range(0.1f, 0.2f);;
            }

        }



    }
}
