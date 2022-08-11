using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BStageEntity : MonoBehaviour
{
    BattleStageHandler stageHandler;
    public Transform worldTransform;
    SpriteRenderer spriteRenderer;
    public Vector3Int currentCellPos;

    [SerializeField] public int hitPoints{get;set;}
    [SerializeField] public int shieldPoints{get;set;}
    [SerializeField] public float DefenseMultiplier{get;set;} = 1;
    [SerializeField] public float AttackMultiplier {get;set;} = 1;


    public void Start()
    {
        stageHandler = FindObjectOfType<BattleStageHandler>();
        worldTransform = transform.parent.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public abstract void hurtEntity();

    public void setCellPosition(int x, int y)
    {
            stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
            currentCellPos.Set(x, y, currentCellPos.z);
            stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);

            worldTransform.transform.localPosition = stageHandler.stageTilemap.
                                        GetCellCenterWorld(currentCellPos);
    }


}
