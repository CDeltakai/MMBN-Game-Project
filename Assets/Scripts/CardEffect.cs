using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class containing common functionality and variables between all EffectPrefabs for cards
public abstract class CardEffect : MonoBehaviour
{
    public delegate void TriggeredEffectEvent();
    public event TriggeredEffectEvent triggeredEffect;


    public PlayerMovement player;
    public ChipSO chipSO;
    public List<GameObject> ObjectSummonList = new List<GameObject>();
    public List<QuantifiableEffect> quantifiableEffects = new List<QuantifiableEffect>(); 
    private AttackPayload defaultAttackPayload = new AttackPayload();
    protected Transform firePoint;


    //Temporary modifiers that should be cleared after the card is used up
    public int damageModifier;
    public List<EStatusEffects> statusModifiers = new List<EStatusEffects>();
    public AttackElement elementModifier = AttackElement.Normal;


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


    //Calculates the final payload used by any damage calculations by applying any valid modifiers
    public virtual AttackPayload CalculateFinalPayload()
    {
        AttackPayload finalPayload = defaultAttackPayload;
        finalPayload.damage += damageModifier;
        finalPayload.additionalStatusEffects = statusModifiers;
        
        //If the attack element of the original card is not Normal, then its element cannot be modified.
        //Card element cannot be modified with a Normal element.
        if(finalPayload.attackElement != AttackElement.Normal && elementModifier != AttackElement.Normal)
        {
            finalPayload.attackElement = elementModifier;
        }


        return finalPayload;
    }

    //Generic method to apply a payload to a BStageEntity
    public virtual void ApplyPayloadToTarget(BStageEntity entity, AttackPayload payload)
    {

        entity.HurtEntity(payload);

    }

    //Clear all temporary modifiers and sets them back to default values
    public void ClearModifiers()
    {
        damageModifier = 0;
        statusModifiers.Clear();
        elementModifier = AttackElement.Normal;
    }


    //Method to call when it is time to disable the effect prefab
    protected abstract IEnumerator DisableEffectPrefab();


}