using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A data container struct that contains all the information an "attack" will have for the purposes of damage/effect calculations
[System.Serializable]
public struct AttackPayload
{

    public int damage;
    public bool lightAttack; //Does the attack trigger invincibility frames?
    public bool hitFlinch; //Does the attack cause the enemy to flinch if they can flinch?
    public bool pierceUntargetable; //Can this attack hit enemies that are concealed/untargetable?
    public BStageEntity attacker;
    public EStatusEffects statusEffect;
    public List<EStatusEffects> additionalStatusEffects;
    public AttackElement attackElement;

    public AttackPayload(int damage,
                            bool lightAttack,
                            bool hitFlinch, 
                            bool pierceUntargetable, 
                            BStageEntity attacker, 
                            EStatusEffects statusEffect, 
                            List<EStatusEffects> additionalStatusEffects,  
                            AttackElement attackElement)
    {
        this.damage = damage;
        this.lightAttack = lightAttack;
        this.hitFlinch = hitFlinch;
        this.pierceUntargetable = pierceUntargetable;
        this.attacker = attacker;
        this.statusEffect = statusEffect;
        this.additionalStatusEffects = additionalStatusEffects;
        this.attackElement = attackElement;

    }


}
