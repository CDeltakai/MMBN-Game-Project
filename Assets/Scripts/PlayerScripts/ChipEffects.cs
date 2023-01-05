using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ChipEffects : MonoBehaviour
{

    Dictionary<int, Func<object, object>> chipFunctionDictionary = new Dictionary<int, Func<object, object>>();

    ChipLoadManager chipLoadManager;

    public Transform firePoint;
    PlayerMovement player;
    BoxCollider2D playerCollider;
    [SerializeField] public GameObject ParryCollider;
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




  
    public void ApplyChipEffectV2()
    {

        chip = player.activeChip;
        Debug.Log("Chip used:" + chip.GetChipName());
        Type CurrentChipScript = Type.GetType(chip.GetEffectScript());
        //ChipScript = chip.GetEffectScript();
        

        IChip chipEffect = gameObject.AddComponent(CurrentChipScript) as IChip;

        chipEffect.Effect();
        Destroy(GetComponent(CurrentChipScript));
        //ChipScript = null;
    }


    public void ApplyChipEffectRef()
    {

        chipRefList = chipLoadManager.nextChipRefLoad;

        if(chipLoadManager.nextChipRefLoad.Count() == 1)
        {
            var chip = chipLoadManager.nextChipRefLoad[0];
            ChipEffectBlueprint chipEffectScript = chip.effectPrefab.GetComponent<ChipEffectBlueprint>();
            chipEffectScript.Effect();

            StartCoroutine(disableEffectPrefab(chip.chipSORef.GetAnimationDuration(), chip.effectPrefab));

        }else
        {

            var chip = chipLoadManager.nextChipRefLoad[0];
            ChipEffectBlueprint chipEffectScript = chip.effectPrefab.GetComponent<ChipEffectBlueprint>();
            chipEffectScript.Effect();
            StartCoroutine(disableEffectPrefab(chip.chipSORef.GetAnimationDuration(), chip.effectPrefab));

        }



    }



    IEnumerator disableEffectPrefab(float duration, GameObject prefab)
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
