using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackPayload
{

    public int damage;
    public bool lightAttack;
    public bool hitFlinch;
    public bool pierceUntargetable;
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
