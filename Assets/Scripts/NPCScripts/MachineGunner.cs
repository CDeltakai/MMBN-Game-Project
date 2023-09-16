using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MachineGunner : BStageEntity
{
    public override bool isGrounded { get; set; } = true;
    
    public override bool isStationary => true;
    public override bool isStunnable => true;
    public override int maxHP => 60;
    public override ETileTeam tileTeam{get;set;} = ETileTeam.Enemy;

    [SerializeField] MachineGunner_Reticle reticle;
    BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    public override void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        currentCellPos.Set((int)(Math.Round((worldTransform.position.x/1.6f), MidpointRounding.AwayFromZero)),
                            (int)transform.parent.position.y, 0);
        stageHandler.setCellEntity(currentCellPos.x, currentCellPos.y, this, true);
        healthText.text = currentHP.ToString();
        

    }

    // Update is called once per frame
    void Update()
    {
        


    }


    







}
