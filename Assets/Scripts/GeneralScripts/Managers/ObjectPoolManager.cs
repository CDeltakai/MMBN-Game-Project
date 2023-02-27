using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChipObjectReference
{
    [SerializeField] internal ChipSO chipSORef;
    [SerializeField] internal GameObject effectPrefab;
    [SerializeField] internal GameObject ObjectSummon;

}


public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    ChipInventory chipInventory;
    PlayerAttributeManager playerAttributeManager;

	[SerializeField] List<GameObject> ChipObjectPool = new List<GameObject>();
    [SerializeField] GameObject ChipObjectPoolParent;
    [SerializeField] public List<ChipObjectReference> ChipRefList = new List<ChipObjectReference>();
	
    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        chipInventory = FindObjectOfType<ChipInventory>();
        playerAttributeManager = PlayerAttributeManager.Instance;
        playerAttributeManager = FindObjectOfType<PlayerAttributeManager>();
        PoolChipsFromAttributesDeck();


    }

	
	void Start()
    {
        //PoolChipsFromDeck();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void PoolChipObjects()
    {
        ChipSO[] chipResources = Resources.LoadAll<ChipSO>("Chips");
        List<ChipSO> chipDeckLoad = chipInventory.chipDeck;

        foreach(ChipSO chip in chipResources)
        {
            if(chip.GetEffectPrefab() != null)
            {

                GameObject prefab = Instantiate(chip.GetEffectPrefab(), transform.position, Quaternion.identity, ChipObjectPoolParent.transform);
                prefab.GetComponent<ChipEffectBlueprint>().player = player;

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
                    prefab.GetComponent<GenericObjectSummonEffect>().PooledSummonObject = objectSummon;
                    objectSummon.SetActive(false);
                }

                ChipRefList.Add(chipObjRef);


            }else
            {
                Debug.LogWarning("Chip: "+ chip.GetChipName() + " does not have an effect prefab, chip will be non-functional.");
            }

        }
    }

    ///<summary>
    ///Takes chips from the current chip deck defined within the Current Player Attributes scriptable object
    ///and instantiates their pre-defined effect prefabs and object summons (if applicable) within the StageObjectPool
    ///game object.
    ///</summary>
    void PoolChipsFromAttributesDeck()
    {
        //List<ChipSO> chipDeckLoad = chipInventory.chipDeck;
        //print(playerAttributeManager.ToString());
        List<ChipInventoryReference> currentPlayerDeck = playerAttributeManager.CurrentPlayerAttributes.GetCurrentChipDeck();

        foreach(ChipInventoryReference chipInvRef in currentPlayerDeck)
        {
            for(int i = 1; i <= chipInvRef.chipCount; i++)
            {

                if(chipInvRef.chip.GetEffectPrefab() != null)
                {
                    GameObject prefab = Instantiate(chipInvRef.chip.GetEffectPrefab(), transform.position, Quaternion.identity, ChipObjectPoolParent.transform);
                    GameObject objectSummon = null;
                    prefab.GetComponent<ChipEffectBlueprint>().player = player;


                    if(chipInvRef.chip.GetObjectSummon() != null)
                    {
                        objectSummon = Instantiate(chipInvRef.chip.GetObjectSummon(), transform.position, Quaternion.identity, ChipObjectPoolParent.transform);
                    }

                    var chipObjRef = new ChipObjectReference
                    {
                        chipSORef = chipInvRef.chip,
                        effectPrefab = prefab,
                        ObjectSummon = objectSummon
                    };

                    prefab.SetActive(false);
                

                    if(chipInvRef.chip.GetObjectSummon() != null)
                    {
                        prefab.GetComponent<GenericObjectSummonEffect>().PooledSummonObject = objectSummon;
                        objectSummon.SetActive(false);
                    }

                    ChipRefList.Add(chipObjRef);


                }else
                {
                    Debug.LogWarning("Chip: "+ chipInvRef.chip.GetChipName() + " does not have an effect prefab, chip will be non-functional.");
                }

        }

        }



    }


}
