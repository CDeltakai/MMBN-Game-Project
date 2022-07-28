using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using testExtensions;

public class Cannon : MonoBehaviour, IChip
{
    public Transform firePoint;
    PlayerMovement player;
    public int BaseDamage {get;} = 40;
    public int AdditionalDamage{get; set;} = 0;
    public EChipTypes ChipType => EChipTypes.Active;

    public EChipElements chipElement => EChipElements.Normal;

    public EStatusEffects statusEffect {get;set;} = EStatusEffects.Default;


    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default)
    {

        
        AdditionalDamage += AddDamage;
        player = GetComponent<PlayerMovement>();
        Debug.Log("Attempted cannon effect");
        firePoint = FindObjectOfType<ChipEffects>().firePoint;

        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies"));
        if(hitInfo)
        {

            

            IBattleStageEntity script = hitInfo.transform.gameObject.GetComponent<IBattleStageEntity>();
            script.hurtEntity((int)((BaseDamage + AdditionalDamage) * player.AttackMultiplier), false, true);
        }
        
        AdditionalDamage = 0;



    }

}
