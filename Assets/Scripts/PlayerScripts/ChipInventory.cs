using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipInventory : MonoBehaviour
{

private static ChipInventory _instance;
public static ChipInventory Instance {get {return _instance;} }

ObjectPoolManager objectPoolManager;


private Dictionary<int, ChipSO> chipDictionary = new Dictionary<int, ChipSO>();

[SerializeField] public List<ChipSO> chipInventory = new List<ChipSO>();
[SerializeField] public List<ChipSO> chipRefInventory = new List<ChipSO>();
[SerializeField] public List<ChipSO> chipDeck = new List<ChipSO>();
private ChipSO[] chipLoad;
ChipSO newChip;

    private void InitializeSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
        }else
        {
            _instance = this;
        }
    }


private void Awake() {
    InitializeSingleton();
    objectPoolManager = FindObjectOfType<ObjectPoolManager>();
    FillChipInventory();
    FillChipDeck();
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

//Debug method; this will just fill the deck with the same chip as inventory
void FillChipDeck()
{
    foreach(ChipSO chip in chipInventory)
    {
        chipDeck.Add(chip);
    }


}


public List<ChipSO> getChipInventory()
{
    return chipInventory;
}

}



