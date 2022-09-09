using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "Chip Data", menuName = "New Chip", order = 0)]
public class ChipSO : ScriptableObject 
{

    [SerializeField] int ChipID;
    
    [SerializeField] EChips Chip;
    [SerializeField] string ChipName;
    [SerializeField] int BaseDamage;
    [SerializeField] Sprite ChipImage;
    [SerializeField] string ChipDescription;
    [SerializeField] EChipElements ChipElement;
    [SerializeField] int ChipSize;
    [SerializeField] float AnimationDuration;
    // Chip Types: 2 = Freeze-time ability(Special chips that freeze time before applying effect)
    // 1 = Active(Real-time usable chip), 0 = Passive(Applies effect to succeeding chip)
    [SerializeField] EChipTypes ChipType;
    [SerializeField] String EffectScript;
    [SerializeField] EventReference SFX;
    [SerializeField] List<EventReference> AdditionalSFX;
    [SerializeField] EMegamanAnimations AnimationToUse;
    [SerializeField] GameObject ObjectSummon;
    [SerializeField] EffectMechanism effectMechanism;
    [SerializeField] bool UseAnimationEvent;



    public int GetChipID()
    {
        return (int)Chip;
    }

    public GameObject GetObjectSummon()
    {
        if(ObjectSummon != null)
        {
            return ObjectSummon;
        }else
        {
            Debug.LogWarning("Chip does not have an ObjectSummon assigned, returned null");
            return null;
        }

    }

    public EffectMechanism GetEffectMechanism()
    {
        return effectMechanism;
    }


    public string GetChipName()
    {
        return Chip.ToString();
    }

    public int GetChipDamage()
    {
        return BaseDamage;
    }

    public string GetChipDescription()
    {
        return ChipDescription;
    }

    public EChipElements GetChipElement()
    {
        return ChipElement;
    }
    public EChips GetChipEnum()
    {
        return Chip;
    }

    public int GetChipSize()
    {
        return ChipSize;
    }

    public Sprite GetChipImage()
    {
        return ChipImage;
    }

    public float GetAnimationDuration()
    {
        return AnimationDuration;
    }

    public string GetEffectScript()
    {
        return EffectScript;
    }

    public EChipTypes GetChipType()
    {
        return ChipType;
    }

    public EventReference GetSFX()
    {
        if(SFX.IsNull)
        {
            Debug.LogWarning("Chip does not have an EventReference for SFX");
            
        }
        return SFX;
        
    }

 



} 

