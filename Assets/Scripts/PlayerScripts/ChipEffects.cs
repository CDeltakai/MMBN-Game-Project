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

    public void ApplyChipEffect(int id)
    {
        chipFunctionDictionary[id](null);
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

    public void UseChipEffect(string GivenAddressableKey = null)
    {
        chipList = chipLoadManager.nextChipLoad;

        if(chipLoadManager.nextChipLoad.Count() == 1)
        {
            Type chipScript = Type.GetType(chipLoadManager.nextChipLoad[0].GetEffectScript());
            StartCoroutine(removeChipFromLoad(chipLoadManager.nextChipLoad[0].GetAnimationDuration(), chipScript));
            print("Used ApplyChipEffectV3, with chip count at 1");
            return;
        }
    }


    public void ApplyChipEffectV3(string GivenAddressableKey = null)
    {

        chipList = chipLoadManager.nextChipLoad;

        if(chipLoadManager.nextChipLoad.Count() == 1)
        {
            Type chipScript = Type.GetType(chipLoadManager.nextChipLoad[0].GetEffectScript());
            IChip chipEffect = gameObject.AddComponent(chipScript) as IChip;
            chipEffect.Effect(AddressableKey: GivenAddressableKey);
            StartCoroutine(removeChipFromLoad(chipLoadManager.nextChipLoad[0].GetAnimationDuration(), chipScript));
            //Destroy(GetComponent(chipScript));
            print("Used ApplyChipEffectV3, with chip count at 1");
            return;
        }

        foreach(ChipSO chip in chipLoadManager.nextChipLoad)
        {
            scriptList.Add(Type.GetType(chip.GetEffectScript()));
        }
        
        foreach(Type script in scriptList)
        {
            IChip chipEffect = gameObject.AddComponent(script) as IChip;
        }

        chipObjectList = GetComponents<IChip>().ToList();


        foreach(IChip chip in chipObjectList)
        {
            if(chip.ChipType == EChipTypes.Passive)
            {
                print("Attempted passive chip effect");
                chip.Effect();
                
                Destroy(GetComponent(chip.GetType()));
                
            } else if (chip.ChipType != EChipTypes.Passive)
            {
                //nonPassiveChipFound = true;
                continue;
            }

        }

        //Finds the non-passive chip within the chipObjectList and attempts its effect
        // 
        if(GivenAddressableKey == null){
            
            var chip = chipObjectList.Find(chip => chip.ChipType != EChipTypes.Passive);
            chip.Effect();


        }else{
            chipObjectList.Find(chip => chip.ChipType != EChipTypes.Passive).Effect(AddressableKey: GivenAddressableKey);

        }
        StartCoroutine(removeChipFromLoad(chipLoadManager.nextChipLoad[0].GetAnimationDuration(), chipObjectList.Find(x => x.ChipType != EChipTypes.Passive).GetType()));


    
    }


    public void ApplyChipEffectRef()
    {

        chipRefList = chipLoadManager.nextChipRefLoad;

        if(chipLoadManager.nextChipRefLoad.Count() == 1)
        {
            var chip = chipLoadManager.nextChipRefLoad[0];
            chip.effectPrefab.SetActive(true);
            ChipEffectBlueprint chipEffectScript = chip.effectPrefab.GetComponent<ChipEffectBlueprint>();
            chipEffectScript.Effect();

            //chip.chipEffectScript.Effect();
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
