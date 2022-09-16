using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FMODUnity;

public class ScriptableObjectIdAttribute : PropertyAttribute {}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
public class ScriptableObjectIdDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        if(string.IsNullOrEmpty(property.stringValue))
        {
            property.stringValue = UnityEditor.GUID.Generate().ToString();

        }
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }

}
#endif

public class ChipScriptableObject :ScriptableObject
{
    [ScriptableObjectId] public string Id;

}




[CreateAssetMenu(fileName = "Chip Data", menuName = "New Chip", order = 0)]
public class ChipSO : ChipScriptableObject 
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
    [SerializeField] EStatusEffects BaseStatusEffect;
    [SerializeField] String EffectScript;
    [SerializeField] EventReference SFX;
    [SerializeField] List<EventReference> AdditionalSFX;
    [SerializeField] EMegamanAnimations AnimationToUse;
    [SerializeField] GameObject ObjectSummon;
    [SerializeField] GameObject EffectPrefab;
    [SerializeField] EffectMechanism effectMechanism;
    [SerializeField] bool pierceUntargetable;
    [SerializeField] bool lightAttack;
    [SerializeField] bool hitFlinch;
    [SerializeField] bool UseAnimationEvent;
    [HideInInspector] GameObject TempEffectPrefabRef;


    public void ResetID()
    {
        Id = null;
    }

    public void ResetEffectPrefabRef()
    {
        TempEffectPrefabRef = null;
    }

    public GameObject GetEffectPrefab()
    {
        if(EffectPrefab == null)
        {
            return null;
        }
        return EffectPrefab;
    }
    public EMegamanAnimations GetAnimation()
    {
        return AnimationToUse;
    }
    public bool IsLightAttack()
    {
        return lightAttack;
    }
    public bool IsHitFlinch()
    {
        return hitFlinch;
    }
    public int GetChipID()
    {
        return (int)Chip;
    }

    public bool IsPierceUntargetable()
    {
        return pierceUntargetable;
    }

    public EStatusEffects GetStatusEffect()
    {
        return BaseStatusEffect;
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

