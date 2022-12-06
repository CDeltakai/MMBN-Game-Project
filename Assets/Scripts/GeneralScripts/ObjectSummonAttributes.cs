using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSummonAttributes : MonoBehaviour
{
    public int BaseDamage;
    public int AddDamage = 0;

    public EStatusEffects BaseStatusEffect;
    public EStatusEffects AddStatusEffect = EStatusEffects.Default;

    public bool lightAttack;
    public bool hitFlinch;
    public bool pierceUntargetable;




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
