using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MettaurAI : MonoBehaviour
{

    [SerializeField] GameObject target;
    PlayerMovement player;
    Mettaur mettaur;
    float movementCooldown = 1;

    Vector3Int targetPosition;
    Vector3Int mettaurPosition;


    void Start()
    {
        player = target.GetComponent<PlayerMovement>();
        //mettaurPosition = GetComponent<Mettaur>().currentCellPos;
        mettaur = GetComponent<Mettaur>();

    }



    void Update()
    {
        //Check target position every frame
        targetPosition = player.getCurrentCellPos();
        //Debug.Log("MettaurAI Target Position: " + targetPosition.ToString());
        mettaurPosition = GetComponent<Mettaur>().getCellPosition();


        if(movementCooldown > 0)
        {
            movementCooldown -= Time.deltaTime;
        }

        if(movementCooldown <= 0){

        if(mettaurPosition.y == targetPosition.y)
        {
            mettaurAttack();
            movementCooldown = 1;

        }
        else
        if(mettaurPosition.y < targetPosition.y)
        {
            mettaurMoveUp();
            movementCooldown = 1;
        }else
        if(mettaurPosition.y > targetPosition.y)
        {
            mettaurMoveDown();
            movementCooldown = 1;

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
