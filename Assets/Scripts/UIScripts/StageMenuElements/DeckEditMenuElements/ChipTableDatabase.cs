using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class ChipCounterReference
{
    public int InventoryCount = 0;

    public ChipCounterReference(int inventoryCount = 0)
    {
        this.InventoryCount = inventoryCount;
    }


}


public class ChipTableDatabase : MonoBehaviour
{

    [SerializeField] PlayerAttributeSO playerData;
    Dictionary<ChipSO, ChipCounterReference> ChipInventoryCountDict = new Dictionary<ChipSO, ChipCounterReference>();



    void InitializeChipDictionary()
    {
        ChipSO[] chipResources = Resources.LoadAll<ChipSO>("ChipScriptableObjects");

        foreach(ChipSO chip in chipResources)
        {
            ChipInventoryCountDict.Add(chip, new ChipCounterReference(0));
        }


        foreach(ChipInventoryReference chipInvRef in playerData.CurrentChipInventory)
        {
            ChipInventoryCountDict[chipInvRef.chip].InventoryCount = chipInvRef.chipCount;
        }
    }

    private void Awake() {
        DontDestroyOnLoad(this);
        InitializeChipDictionary();
    }

    public void RefreshDatabase()
    {
        foreach(KeyValuePair<ChipSO, ChipCounterReference> keyValuePair in ChipInventoryCountDict)
        {
            keyValuePair.Value.InventoryCount = 0;
        }

        foreach(ChipInventoryReference chipInvRef in playerData.CurrentChipInventory)
        {
            ChipInventoryCountDict[chipInvRef.chip].InventoryCount = chipInvRef.chipCount;
        }        
    }

    public bool CheckChipExistence(ChipSO chip)
    {
        if(ChipInventoryCountDict[chip].InventoryCount > 0)
        {
            return true;
        }


        return false;
    }

    public void SetInventoryChipCount(ChipSO chip, int count)
    {
        ChipInventoryCountDict[chip].InventoryCount = count;

    }

    public void AddInventoryChipCount(ChipSO chip, int count)
    {
        ChipInventoryCountDict[chip].InventoryCount += count;
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
