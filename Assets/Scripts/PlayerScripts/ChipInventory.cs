using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipInventory : MonoBehaviour
{

private Dictionary<int, ChipSO> chipDictionary = new Dictionary<int, ChipSO>();

[SerializeField] public List<ChipSO> chipInventory = new List<ChipSO>();
private ChipSO[] chipLoad;
ChipSO newChip;

private void Awake() {
    AddChip();
}


void AddChip()
{
    chipLoad = Resources.LoadAll<ChipSO>("Chips");

    foreach(var chip in chipLoad)
    {
        chipInventory.Add(chip);
        chipDictionary.Add(chip.GetChipID(), chip);
    }
}

public ChipSO GetChip(int id)
    {
        return chipDictionary[id];
    }

public List<ChipSO> getChipInventory()
{
    return chipInventory;
}

}



