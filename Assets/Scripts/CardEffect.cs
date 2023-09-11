using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class containing common functionality and variables between all EffectPrefabs for cards
public abstract class CardEffect : MonoBehaviour
{

    public PlayerMovement player;
    public ChipSO chipSO;
    public List<GameObject> ObjectSummonList = new List<GameObject>();
    public List<QuantifiableEffect> quantifiableEffects = new List<QuantifiableEffect>(); 
    private AttackPayload defaultAttackPayload = new AttackPayload();
    protected Transform firePoint;




    protected void InitializeAwakeVariables()
    {
        //If cardSO does not have its ObjectSummons pooled, will set the ObjectSummonList to be the same as that defined within the cardSO.
        if(!chipSO.ObjectSummonsArePooled)
        {
            ObjectSummonList = chipSO.ObjectSummonList;
        }
        quantifiableEffects = chipSO.QuantifiableEffects;

        //Setting the default attack payload based on the ChipSO stats
        defaultAttackPayload.attacker = player;
        defaultAttackPayload.damage = chipSO.GetChipDamage();
        defaultAttackPayload.lightAttack = chipSO.LightAttack;
        defaultAttackPayload.hitFlinch = chipSO.HitFlinch;
        defaultAttackPayload.pierceUntargetable = chipSO.PierceConcealment;
        defaultAttackPayload.statusEffect = chipSO.GetStatusEffect();
        defaultAttackPayload.attackElement = chipSO.ChipElement;
    }

    public AttackPayload GetDefaultPayload()
    {
        return defaultAttackPayload;
    }

    protected virtual void Awake()
    {
        InitializeAwakeVariables();
    }

    //All card effects should begin anchored on the world transform of the player
    protected void InitializeStartingStates()
    {
        transform.position = player.worldTransform.position;
    }

    private void OnEnable() 
    {
        if(player != null)
        {
            transform.position = player.worldTransform.position;   
        }
    }

    protected virtual void Start()
    {
        InitializeStartingStates();
    }

    //The main method that gets called whenever the card is used. Should contain all necessary statements and calls
    //to make the card work.
    public abstract void ActivateCardEffect();

    //Generic method to apply a payload to a BStageEntity
    public void ApplyPayloadToTarget(BStageEntity entity, AttackPayload payload)
    {





        //entity.hurtEntity(payload);

    }



    //Method to call when it is time to disable the effect prefab
    protected abstract IEnumerator DisableEffectPrefab();


}