using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
///  This class handles the activation of animation-based chips whenever the player uses them.
/// </summary>

public class ChipEffects : MonoBehaviour
{

    Dictionary<int, Func<object, object>> chipFunctionDictionary = new Dictionary<int, Func<object, object>>();

    ChipLoadManager chipLoadManager;

    public Transform firePoint;
    PlayerMovement player;
    BoxCollider2D playerCollider;
    [SerializeField] public UnityEngine.GameObject ParryCollider;
    string ChipScript;
    Time time;
    float timeElapsed = 0f;
    ChipSO chip;
    List<ChipSO> chipList;

    List<ChipObjectReference> chipRefList;

    List<Type> scriptList = new List<Type>();
    List<IChip> chipObjectList;
    ChipInventory chipInventory;

    
    private void Awake() {

    }

    private void Start() 
    {
        player = FindObjectOfType<PlayerMovement>();
        chipInventory = FindObjectOfType<ChipInventory>();
        chipLoadManager = FindObjectOfType<ChipLoadManager>();
        playerCollider = player.boxCollider2D;
        
    }


    /// <summary>
    /// Method used by chips which have an animation to apply their effects through an animation event.
    /// Normally this method is only called by an animation event on a Megaman chip-based animation and
    /// chips with an instantaneous or passive effect should use different method to apply their effect.
    /// </summary>
    public void ApplyChipEffectRef()
    {

        chipRefList = chipLoadManager.nextChipRefLoad;

        if(chipLoadManager.nextChipRefLoad.Count() == 1)
        {
            var chip = chipLoadManager.nextChipRefLoad[0];
            ChipEffectBlueprint chipEffectScript = chip.effectPrefab.GetComponent<ChipEffectBlueprint>();
            chipEffectScript.Effect();
            player.AdjustEnergy(-chip.chipSORef.EnergyCost);

            if(chip.chipSORef.GetAnimationClip() != null)
            {
                StartCoroutine(disableEffectPrefab(chip.chipSORef.GetAnimationClip().length, chip.effectPrefab));
            }else
            {
                Debug.LogWarning("Chip: " + chip.chipSORef.GetChipName() +
                "has no animation clip and thus may not function correctly."+
                " ApplyChipEffectRef may not be the correct method for this chip to use.");
                StartCoroutine(disableEffectPrefab(0.05f, chip.effectPrefab));
            }

        }else
        {

            var chip = chipLoadManager.nextChipRefLoad[0];
            ChipEffectBlueprint chipEffectScript = chip.effectPrefab.GetComponent<ChipEffectBlueprint>();
            chipEffectScript.Effect();
            player.AdjustEnergy(-chip.chipSORef.EnergyCost);


            if(chip.chipSORef.GetAnimationClip() != null)
            {
                StartCoroutine(disableEffectPrefab(chip.chipSORef.GetAnimationClip().length, chip.effectPrefab));
            }else
            {
                Debug.LogWarning("Chip: " + chip.chipSORef.GetChipName() +
                "has no animation clip. Chip may not function correctly. " +
                "ApplyChipEffectRef may not be the correct method for this chip to use.");
                StartCoroutine(disableEffectPrefab(0.05f, chip.effectPrefab));
            }

        }



    }



    IEnumerator disableEffectPrefab(float duration, UnityEngine.GameObject prefab)
    {
        yield return new WaitForSecondsRealtime(duration);
        prefab.SetActive(false);

    }

    IEnumerator removeChipFromLoad(float duration, Type chipToDestroy)
    {
        yield return new WaitForSecondsRealtime(duration);
        Destroy(GetComponent(chipToDestroy));
    }






}
