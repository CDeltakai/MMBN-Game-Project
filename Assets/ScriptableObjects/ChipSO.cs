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
[Header("Combat Attributes")]
    [SerializeField] int BaseDamage;
    [field:SerializeField] public int PierceCount {get; private set;}
    [field:SerializeField] public AttackElement ChipElement{get; private set;}
    [SerializeField] int ChipSize;
    [field:SerializeField] public int EnergyCost{get; private set;}
    // Chip Types: 2 = Freeze-time ability(Special chips that freeze time before applying effect)
    // 1 = Active(Real-time usable chip), 0 = Passive(Applies effect to succeeding chip)
    [SerializeField] EChipTypes ChipType;
    [SerializeField] EStatusEffects BaseStatusEffect;
    ///<summary>
    ///This list of Vector2Ints indicates what tiles in relation to the player's position
    ///this chip has influence over (eg. what range of tiles an attack chip can hit). This
    ///variable may be used by a chip's effect in order to modify a chip's effective range.
    ///This variable is also used by the reticle indicator which will aid the player in aiming
    ///the chip. Can be empty depending how the chip works.
    ///</summary>
    [field:SerializeField] public List<Vector2Int> RangeOfInfluence{get; private set;}
    ///<summary>
    ///This list indicates what tiles on the world the chip has influence over. This list is not affected
    ///by the player's position and is always static.
    ///</summary>
    [field:SerializeField] public List<Vector2Int> RangeOfInfluenceWorld{get; private set;}


[TextArea(10,20)]
    [SerializeField] string ChipDescription;
    [SerializeField] Sprite ChipImage;
    [SerializeField] float AnimationDuration;
    [SerializeField] String EffectScript;
    [SerializeField] EventReference SFX;
    [SerializeField] List<EventReference> AdditionalSFX;
    [SerializeField] AnimationClip AnimationClipToUse;
    [SerializeField] UnityEngine.GameObject ObjectSummon;
    [SerializeField] UnityEngine.GameObject EffectPrefab;
    [SerializeField] EffectMechanism effectMechanism;
    [SerializeField] bool pierceUntargetable;
    [SerializeField] bool lightAttack;
    [SerializeField] bool hitFlinch;
    [SerializeField] bool UseAnimationEvent;
    ///<summary>
    ///If this is set to true, this chip will not automatically disable itself after its initial
    ///casting period. The chip will need have its own function to disable itself after some
    ///condition.
    ///</summary>
    [field:SerializeField] public bool isPersistent {get; private set;} = false;
    ///<summary>
    ///This condition dictates if this chip allows for additional objects to be summoned
    ///through certain passive chips or other modifiers.
    ///</summary>
    [field:SerializeField] public bool AllowSummonObjectMod{get; private set;}
    [HideInInspector] UnityEngine.GameObject TempEffectPrefabRef;


    public void ResetID()
    {
        Id = null;
    }

    public void ResetEffectPrefabRef()
    {
        TempEffectPrefabRef = null;
    }

    public UnityEngine.GameObject GetEffectPrefab()
    {
        if(EffectPrefab == null)
        {
            return null;
        }
        return EffectPrefab;
    }

    public AnimationClip GetAnimationClip()
    {
        return AnimationClipToUse;
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

    public UnityEngine.GameObject GetObjectSummon()
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
        return ChipName;
    }

    public int GetChipDamage()
    {
        return BaseDamage;
    }

    public string GetChipDescription()
    {
        return ChipDescription;
    }

    public AttackElement GetChipElement()
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

