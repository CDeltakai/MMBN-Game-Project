using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum ChampyAnims
{
    Champy_Idle,
    Champy_Attack
}


public class Champy_RF : BStageEntity
{

    public override bool isGrounded { get; set; } = false;
    public override bool isStationary => false;
    public override bool isStunnable => true;
    public override int maxHP => 60;
    public override ETileTeam team{get;set;} = ETileTeam.Enemy;

    BoxCollider2D champyCollider;


    // Start is called before the first frame update
    void Start()
    {
        champyCollider = GetComponent<BoxCollider2D>();
        currentCellPos.Set((int)(Math.Round((worldTransform.position.x/1.6f), MidpointRounding.AwayFromZero)),
                            (int)transform.parent.position.y, 0);
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);
        currentHP = maxHP;
        healthText.text = currentHP.ToString();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
