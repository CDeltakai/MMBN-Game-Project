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


[Serializable]
public class QuantifiableEffect
{
    [field:SerializeField] public string EffectName{get; private set;}
    [field:SerializeField] public int IntegerQuantity{get; private set;}
    [field:SerializeField] public float FloatQuantity{get; private set;}
    [field:SerializeField] public bool CanBeModified{get; private set;} = false;


}



[CreateAssetMenu(fileName = "Chip Data", menuName = "New Chip", order = 0)]
public class ChipSO : ChipScriptableObject 
{

    [SerializeField] int ChipID;
    [SerializeField] EChips Chip;
    [field:SerializeField] public string ChipName{get; private set;}
[Header("Combat Attributes")]
    [SerializeField] int BaseDamage;
    [field:SerializeField] public List<QuantifiableEffect> QuantifiableEffects{get;private set;}
    [field:SerializeField] public int PierceCount {get; private set;}
    [field:SerializeField] public AttackElement ChipElement{get; private set;}
    [SerializeField] int ChipSize;
    [field:SerializeField] public int EnergyCost{get; private set;}

    [field:SerializeField] public EChipTypes ChipType{get; private set;}
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
    ///This list indicates what tiles on the stage the chip has influence over. This list is not affected
    ///by the player's position and is always static.
    ///</summary>
    [field:SerializeField] public List<Vector2Int> RangeOfInfluenceWorld{get; private set;}


[TextArea(10,20)]
    [SerializeField] string ChipDescription;
    [SerializeField] Sprite ChipImage;
    [SerializeField] float AnimationDuration;
    [SerializeField] EventReference SFX;
    [SerializeField] List<EventReference> AdditionalSFX;
    //What animation will the player trigger when they use this chip? Can be left empty, in which case the chip effect will
    //trigger instantly on use.
    [SerializeField] AnimationClip AnimationClipToUse;
    [SerializeField] GameObject ObjectSummon;
    [field:SerializeField] public List<GameObject> ObjectSummonList {get; private set;}
    [field:SerializeField] public bool ObjectSummonsArePooled {get; private set;}
    [SerializeField] GameObject EffectPrefab;
    [SerializeField] EffectMechanism effectMechanism;
    [field:SerializeField] public bool PierceConcealment {get; private set;}
    [field:SerializeField] public bool LightAttack {get; private set;}
    [field:SerializeField] public bool HitFlinch {get; private set;}
    //If true, the chip will attempt to trigger an animation where an event is called on said animation which activates
    //this chip's effect. This means that this chip can be interrupted by flinching if the player does not have super armor. 
    [field:SerializeField] public bool UseAnimationEvent{get; private set;}
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
   


    public void ResetID()
    {
        Id = null;
    }



    public GameObject GetEffectPrefab()
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
        return LightAttack;
    }
    public bool IsHitFlinch()
    {
        return HitFlinch;
    }
    public int GetChipID()
    {
        return (int)Chip;
    }

    public bool IsPierceUntargetable()
    {
        return PierceConcealment;
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

