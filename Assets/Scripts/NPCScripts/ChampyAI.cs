using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampyAI : MonoBehaviour
{

    PlayerMovement player;
    BattleStageHandler stageHandler;
    Champy champy;
    const string CHAMPY_ATTACK = "Champy_Attack";
    const string CHAMPY_IDLE = "ChampyIdle";


    Vector3Int targetPosition;
    Vector3Int champyCellPosition;
    Transform champyTransform;
    Vector3 champyWorldPosition;

    Animator animator;

    bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
        champy = GetComponent<Champy>();
        champyTransform = champy.worldTransform;
        champyWorldPosition = champyTransform.localPosition;
        stageHandler = GetComponent<BattleStageHandler>();
        isAttacking = false;

        
    }

    // Update is called once per frame
    void Update()
    {


        //Check target position every frame
        targetPosition = player.getCurrentCellPos();
        //Debug.Log("MettaurAI Target Position: " + targetPosition.ToString());
        champyWorldPosition = champy.worldTransform.localPosition;
        champyCellPosition = GetComponent<Champy>().getCellPosition();
        


        if(!isAttacking){
        RaycastHit2D hitInfo = Physics2D.Raycast (champyWorldPosition, new Vector2(-1, 0), Mathf.Infinity, LayerMask.GetMask("Player", "Player_Ally"));

            if(hitInfo)
            {
                isAttacking = true;
                StartCoroutine(AttackAnimation());

            }

        }


    }


    public IEnumerator AttackAnimation()
    {
        Vector3Int previousCellPosition = champyCellPosition;
        champy.setCellPosition(player.getCellPosition().x + 1, champyCellPosition.y);
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(champyCellPosition)].isOccupied = true;


        animator.Play(CHAMPY_ATTACK);
        float delay = 0.417f;
        isAttacking = true;
        yield return new WaitForSeconds(delay + 0.3f);
        animator.Play(CHAMPY_IDLE);
        yield return new WaitForSeconds(1);

        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(champyCellPosition)].isOccupied = false;
        champy.setCellPosition(previousCellPosition.x, previousCellPosition.y);
        previousCellPosition = champyCellPosition;

        yield return new WaitForSeconds(1.5f);
        isAttacking = false;

        


    }

    public void straightHitRegister(int damage)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast (champy.getWorldPosition().localPosition, new Vector2(-1, 0), 1, LayerMask.GetMask("Player", "Player_Ally"));
        if(hitInfo)
        {

            hitInfo.collider.gameObject.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);
          print("Champy Attacked (Straight)");
          IBattleStageEntity script = hitInfo.transform.gameObject.GetComponent<IBattleStageEntity>();
          if(script != null){
          script.hurtEntity(damage, true, true);
          }
        }
    }

    public void uppercutHitRegister(int damage)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast (champy.getWorldPosition().localPosition, new Vector2(-1, 0), 1, LayerMask.GetMask("Player", "Player_Ally"));
        if(hitInfo)
        {
            hitInfo.collider.gameObject.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);
          print("Champy Attacked (Uppercut)");
          IBattleStageEntity script = hitInfo.transform.gameObject.GetComponent<IBattleStageEntity>();
          if(script != null){
          script.hurtEntity(damage, false, true);
          }
        }


    }




}
