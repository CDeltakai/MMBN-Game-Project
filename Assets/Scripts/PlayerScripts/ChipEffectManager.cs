using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectMechanism
{

	Raycast = 0,
	SummonObject = 1,
	TimeFreezeStageEffect = 2,
	PassiveChipBuff = 3,
	PlayerBuff = 4,



}


public class ChipEffectManager : MonoBehaviour
{

	PlayerMovement player;
	[SerializeField] Transform firePoint;
	[SerializeField] List<UnityEngine.GameObject> ChipObjectRefList = new List<UnityEngine.GameObject>();
	

	void Awake()
	{
		player = GetComponent<PlayerMovement>();
		
	}


    void Start()
    {
        
    }

	

	public void ActivateEffect(int mechanismID)
	{

		EffectMechanism effectMechanism = (EffectMechanism)mechanismID;


	}



    public void RaycastEffect(ChipSO chip)
    {

		int BaseDamage = chip.GetChipDamage();
		int AddDamage = 0;
		

        
        Debug.Log("Attempted cannon effect");

        RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies"));
        if(hitInfo)
        {
            BStageEntity target = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
            target.hurtEntity((int)((BaseDamage + AddDamage) * player.AttackMultiplier), false, true, player);
        }
        

    }








}
