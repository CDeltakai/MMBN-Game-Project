using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipInventory : MonoBehaviour
{
ObjectPoolManager objectPoolManager;


private Dictionary<int, ChipSO> chipDictionary = new Dictionary<int, ChipSO>();

[SerializeField] public List<ChipSO> chipInventory = new List<ChipSO>();
[SerializeField] public List<ChipSO> chipRefInventory = new List<ChipSO>();
[SerializeField] public List<ChipSO> chipDeck = new List<ChipSO>();
private ChipSO[] chipLoad;
ChipSO newChip;

private void Awake() {
    objectPoolManager = FindObjectOfType<ObjectPoolManager>();
    FillChipInventory();
}


void FillChipRefInventory()
{
    foreach(ChipObjectReference chipRef in objectPoolManager.ChipRefList)
    {
        


    }

}

void FillChipInventory()
{
    chipLoad = Resources.LoadAll<ChipSO>("Chips");

    foreach(ChipSO chip in chipLoad)
    {
        chipInventory.Add(chip);
        chipDictionary.Add(chip.GetChipID(), chip);
    }
}




public List<ChipSO> getChipInventory()
{
    return chipInventory;
}

}



