using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lark : BStageEntity
{
    public override bool isGrounded { get; set; } = false;

    public override bool isStationary => false;

    public override bool isStunnable => true;

    public override int maxHP => 80;

    public override ETileTeam tileTeam { get; set;} = ETileTeam.Enemy;






    // Update is called once per frame
    void Update()
    {
        
    }
}
