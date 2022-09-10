using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChipEffectRef
{
    [SerializeField] internal ChipSO chipSOToReference;
    [SerializeField] internal GameObject effectPrefab;
    [SerializeField] internal GameObject ObjectSummon;

}


public class ObjectPoolManager : MonoBehaviour
{

	[SerializeField] List<GameObject> ChipObjectPool = new List<GameObject>();
    [SerializeField] GameObject ChipObjectPoolParent;
    [SerializeField] public List<ChipEffectRef> ChipRefList = new List<ChipEffectRef>();
	
    void Awake()
    {
        PoolChipObjects();
    }

	
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


void PoolChipObjects()
{
      ChipSO[] chipLoad = Resources.LoadAll<ChipSO>("Chips");

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



            var chipEffectRef = new ChipEffectRef
            {
                chipSOToReference = chip,
                effectPrefab = prefab,
                ObjectSummon = objectSummon
            };

            prefab.SetActive(false);

            if(chip.GetObjectSummon() != null)
            {
                objectSummon.SetActive(false);
            }

            ChipRefList.Add(chipEffectRef);


        }else
        {
            Debug.LogWarning("Chip: "+ chip.GetChipName() + " does not have an effect prefab, chip will be non-functional.");
        }






    }
}


}
