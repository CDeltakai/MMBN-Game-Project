using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum GunnerAnims
{

    Gunner_Search,
    Gunner_Target,
    Gunner_Shoot

}


public class MachineGunner : BStageEntity
{
    public override bool isGrounded { get; set; } = true;
    public override bool isStationary => true;
    public override bool isStunnable => true;
    public override int maxHP => 60;
    Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void hurtEntity(int damage,
                           bool lightAttack,
                           bool hitStun,
                           bool pierceCloaking = false,
                           EStatusEffects statusEffect = EStatusEffects.Default)
    {
        if(damage >= currentHP)
        {
            currentHP = 0;
            healthText.text = currentHP.ToString();
            stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = false;
            healthText.enabled = false;
            StartCoroutine(DestroyEntity());
            return;
        }
        currentHP = currentHP - (int)(damage * DefenseMultiplier);
        healthText.text = currentHP.ToString();
        return;    
    }
}
