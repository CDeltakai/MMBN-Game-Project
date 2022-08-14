using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MettaurAI : MonoBehaviour
{

    //[SerializeField] GameObject target;
    PlayerMovement player;
    Mettaur mettaur;
    float movementCooldownTimer;
    float movementCooldown = 0.9f;

    Vector3Int targetPosition;
    Vector3Int mettaurPosition;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        //mettaurPosition = GetComponent<Mettaur>().currentCellPos;
        mettaur = GetComponent<Mettaur>();
        movementCooldownTimer = movementCooldown;

    }



    void Update()
    {
        //Check target position every frame
        targetPosition = player.getCurrentCellPos();
        //Debug.Log("MettaurAI Target Position: " + targetPosition.ToString());
        mettaurPosition = GetComponent<Mettaur>().getCellPosition();


        if(movementCooldownTimer > 0)
        {
            movementCooldownTimer -= Time.deltaTime;
        }

        if(movementCooldownTimer <= 0){

        if(mettaurPosition.y == targetPosition.y)
        {
            mettaurAttack();
            movementCooldownTimer = movementCooldown;

        }
        else
        if(mettaurPosition.y < targetPosition.y)
        {
            mettaurMoveUp();
            movementCooldownTimer = movementCooldown;
        }else
        if(mettaurPosition.y > targetPosition.y)
        {
            mettaurMoveDown();
            movementCooldownTimer = movementCooldown;

        }
        }

    void mettaurAttack()
    {
        mettaur.Attack();
        
        //yield return new WaitForSeconds(1);
    }

    void mettaurMoveUp()
    {
        mettaur.cellMoveUp();

        //yield return new WaitForSeconds(1);

    }

    void mettaurMoveDown()
    {
        mettaur.cellMoveDown();
    }



    }
}
