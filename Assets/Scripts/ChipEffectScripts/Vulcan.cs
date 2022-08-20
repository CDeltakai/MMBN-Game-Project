using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulcan : MonoBehaviour, IChip
{
    public Transform firePoint;

    PlayerMovement player;

    public int BaseDamage {get;set;} = 10;

    public int AdditionalDamage{get; set;} = 0;

    public EChipTypes ChipType => EChipTypes.Active;
    public EStatusEffects chipStatusEffect {get;set;} = EStatusEffects.Default;
    public EChipElements chipElement => EChipElements.Normal;


    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default, string AddressableKey = null)
    {

        AdditionalDamage += AddDamage;

        player = GetComponent<PlayerMovement>();
        firePoint = FindObjectOfType<ChipEffects>().firePoint;

        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies"));
        if(hitInfo)
        {
            IBattleStageEntity script = hitInfo.transform.gameObject.GetComponent<IBattleStageEntity>();
            script.hurtEntity((int)((BaseDamage + AdditionalDamage) * player.AttackMultiplier), true, false);


            Debug.Log(hitInfo.transform.name + "HP:" + script.getHealth());
        }

        AdditionalDamage = 0;
    }



}
