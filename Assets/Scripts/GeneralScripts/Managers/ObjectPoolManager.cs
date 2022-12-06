using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChipObjectReference
{
    [SerializeField] internal ChipSO chipSORef;
    [SerializeField] internal GameObject effectPrefab;
    [SerializeField] internal GameObject ObjectSummon;
    internal ChipEffectBlueprint chipEffectScript;

}


public class ObjectPoolManager : MonoBehaviour
{
    ChipInventory chipInventory;

	[SerializeField] List<GameObject> ChipObjectPool = new List<GameObject>();
    [SerializeField] GameObject ChipObjectPoolParent;
    [SerializeField] public List<ChipObjectReference> ChipRefList = new List<ChipObjectReference>();
	
    void Awake()
    {
        chipInventory = FindObjectOfType<ChipInventory>();
    }

	
	void Start()
    {
        //PoolChipObjects();
        PoolChipsFromDeck();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void PoolChipObjects()
    {
        ChipSO[] chipLoad = Resources.LoadAll<ChipSO>("Chips");
        List<ChipSO> chipDeckLoad = chipInventory.chipDeck;

        foreach(ChipSO chip in chipLoad)
        {
            if(chip.GetEffectPrefab() != null)
            {

                GameObject prefab = Instantiate(chip.GetEffectPrefab(), transform.position, Quaternion.identity, ChipObjectPoolParent.transform);
                GameObject objectSummon = null;

                if(chip.GetObjectSummon() != null)
                {
                    objectSummon = Instantiate(chip.GetObjectSummon(), transform.position, Quaternion.identity, ChipObjectPoolParent.transform);
                }



                var chipObjRef = new ChipObjectReference
                {
                    chipSORef = chip,
                    effectPrefab = prefab,
                    ObjectSummon = objectSummon,
                    chipEffectScript = prefab.GetComponent<ChipEffectBlueprint>()
                };

                prefab.SetActive(false);

                if(chip.GetObjectSummon() != null)
                {
                    objectSummon.SetActive(false);
                }

                ChipRefList.Add(chipObjRef);


            }else
            {
                Debug.LogWarning("Chip: "+ chip.GetChipName() + " does not have an effect prefab, chip will be non-functional.");
            }

        }
    }


    void PoolChipsFromDeck()
    {
        List<ChipSO> chipDeckLoad = chipInventory.chipDeck;

        foreach(ChipSO chip in chipDeckLoad)
        {
            if(chip.GetEffectPrefab() != null)
            {

                GameObject prefab = Instantiate(chip.GetEffectPrefab(), transform.position, Quaternion.identity, ChipObjectPoolParent.transform);
                GameObject objectSummon = null;

                if(chip.GetObjectSummon() != null)
                {
                    objectSummon = Instantiate(chip.GetObjectSummon(), transform.position, Quaternion.identity, ChipObjectPoolParent.transform);
                }



                var chipObjRef = new ChipObjectReference
                {
                    chipSORef = chip,
                    effectPrefab = prefab,
                    ObjectSummon = objectSummon
                };

                prefab.SetActive(false);

                if(chip.GetObjectSummon() != null)
                {
                    objectSummon.SetActive(false);
                }

                ChipRefList.Add(chipObjRef);


            }else
            {
                Debug.LogWarning("Chip: "+ chip.GetChipName() + " does not have an effect prefab, chip will be non-functional.");
            }

        }



    }


}
