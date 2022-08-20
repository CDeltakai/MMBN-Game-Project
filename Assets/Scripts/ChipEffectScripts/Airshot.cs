using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airshot : MonoBehaviour, IChip
{
    public Transform firePoint;
    PlayerMovement player;
    public int BaseDamage {get;} = 20;
    public int AdditionalDamage{get; set;} = 0;
    public EChipTypes ChipType => EChipTypes.Active;

    public EChipElements chipElement => EChipElements.Air;

    public EStatusEffects chipStatusEffect {get;set;} = EStatusEffects.Default;

    public void Effect(int AddDamage = 0, EStatusEffects status = EStatusEffects.Default, string AddressableKey = null)
    {

        AdditionalDamage += AddDamage;
        player = GetComponent<PlayerMovement>();
        Debug.Log("Attempted airshot effect");
        firePoint = FindObjectOfType<ChipEffects>().firePoint;

        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies", "Obstacle"));
        if(hitInfo)
        {

            

            BStageEntity target = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
            if(target == null)
            {return;}

            target.hurtEntity((int)((BaseDamage + AdditionalDamage) * player.AttackMultiplier), false, true, statusEffect:chipStatusEffect);
            StartCoroutine(target.Shove(1, 0));            
        }
        
        AdditionalDamage = 0;



    }




}
