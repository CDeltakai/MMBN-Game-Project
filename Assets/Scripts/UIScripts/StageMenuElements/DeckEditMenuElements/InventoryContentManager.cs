using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryContentManager : MonoBehaviour
{

    public DeckEditMenu deckEditMenu;
    public DeckContentManager deckContentManager;


    [SerializeField] UnityEngine.GameObject inventoryElementPrefab;
    [SerializeField] PlayerAttributeSO playerData;


    [SerializeField] public List<ChipInventoryReference> temporaryChipInventory = new List<ChipInventoryReference>();
    public Dictionary<ChipSO, ChipInventoryReference> temporaryChipDictionary = new Dictionary<ChipSO, ChipInventoryReference>();

    [SerializeField] List<InventoryChipSlot> internalElementList = new List<InventoryChipSlot>();
    Dictionary<ChipSO, InventoryChipSlot> internalElementDictionary = new Dictionary<ChipSO, InventoryChipSlot>();



    private void Awake() 
    {
        playerData = deckEditMenu.playerData;
        InitializeTemporaryChipInventory();

        
    }

    private void InitializeTemporaryChipInventory()
    {
        foreach(ChipInventoryReference chipInvRef in playerData.CurrentChipInventory)
        {
            temporaryChipDictionary.Add(chipInvRef.chip, new ChipInventoryReference(chipInvRef.chip, chipInvRef.chipCount));
            temporaryChipInventory.Add(new ChipInventoryReference(chipInvRef.chip, chipInvRef.chipCount));
        }
    }


    void Start()
    {
        AddContentFromInventory(temporaryChipInventory);


    }


    public void AddContentFromInventory(List<ChipInventoryReference> chipInvRefList)
    {
        foreach(ChipInventoryReference chipInvRef in chipInvRefList)
        {

            InventoryChipSlot inventoryElement = Instantiate(inventoryElementPrefab, gameObject.transform).GetComponent<InventoryChipSlot>();
            inventoryElement.chipInvRef = chipInvRef;
            inventoryElement.gameObject.name = (chipInvRef.chipName + "_InvElement");
            inventoryElement.contentManager = this;
            inventoryElement.deckEditMenu = deckEditMenu;
            inventoryElement.RefreshElement();

            internalElementDictionary.Add(chipInvRef.chip, inventoryElement);

        }


    }



    public void AddElementToInventory(ChipInventoryReference chipInvRef)
    {
        if(temporaryChipDictionary.ContainsKey(chipInvRef.chip))
        {
            temporaryChipDictionary[chipInvRef.chip].AddChipCount(chipInvRef.chipCount);

            internalElementDictionary[chipInvRef.chip].chipInvRef.SetChipCount(temporaryChipDictionary[chipInvRef.chip].chipCount);
            internalElementDictionary[chipInvRef.chip].RefreshChipCount();

        }else
        {

            if(chipInvRef.chipCount <= 0)
            {
                Debug.LogWarning("Given ChipInventoryReference: " + chipInvRef.chip.GetChipName() +
                "has a chip count of less than or equal to 0 which should not happen. Aborted adding element to inventory." + 
                "Make sure that the given ChipInventoryReference has a positive chip count.");
                return;
            }

            temporaryChipDictionary.Add(chipInvRef.chip, chipInvRef);

            CreateNewInventoryElement(chipInvRef);

        }

    }



    ///<summary>
    ///This method instantiates a new inventory element prefab and sets the values
    ///of that prefab to the corresponding values of the given ChipInventoryReference
    ///</summary>
    void CreateNewInventoryElement(ChipInventoryReference chipInvRef)
    {
        InventoryChipSlot inventoryElement = Instantiate(inventoryElementPrefab, gameObject.transform).GetComponent<InventoryChipSlot>();
        inventoryElement.chipInvRef = chipInvRef;
        inventoryElement.gameObject.name = (chipInvRef.chipName + "_InvElement");
        inventoryElement.contentManager = this;
        inventoryElement.deckEditMenu = deckEditMenu;
        inventoryElement.RefreshElement();

        internalElementDictionary.Add(chipInvRef.chip, inventoryElement);
        
    }





    public void TransferElementToDeck(InventoryChipSlot inventoryElement)
    {

        deckContentManager.AddElementToDeck(inventoryElement.chipInvRef.chip);
        RemoveChipFromInventory(inventoryElement.chipInvRef.chip);


    }

    public void RemoveChipFromInventory(ChipSO chip)
    {
        if(temporaryChipDictionary.ContainsKey(chip))
        {
            if(temporaryChipDictionary[chip].chipCount <= 0)
            {
                Debug.LogError("Chip: " + chip.GetChipName() + " chipCount somehow has a less than or equal to 0 value." +
                "This should not be happening and means something has gone wrong. Aborted removing chip from inventory.");
                return;
            }

            if(temporaryChipDictionary[chip].chipCount == 1)
            {
                temporaryChipDictionary.Remove(chip);
                Destroy(internalElementDictionary[chip].gameObject);
                internalElementDictionary.Remove(chip);
            }else
            {
                temporaryChipDictionary[chip].AddChipCount(-1);
                internalElementDictionary[chip].chipInvRef.AddChipCount(-1);
                internalElementDictionary[chip].RefreshChipCount();
            }

        }
    }

    //Logic for clicking on an inventory chip to transfer it to the current deck
    public void ClickInventoryElement(InventoryChipSlot inventoryElement)
    {
        TransferElementToDeck(inventoryElement);


    }

    public void SortElementsByName()
    {

        

    }


    public void ResetInventoryView()
    {
        temporaryChipInventory.Clear();
        temporaryChipDictionary.Clear();
        InitializeTemporaryChipInventory();
        
        foreach(KeyValuePair<ChipSO, InventoryChipSlot> keyValuePair in internalElementDictionary)
        {
            Destroy(keyValuePair.Value.gameObject);
        }
        internalElementDictionary.Clear();

        AddContentFromInventory(temporaryChipInventory);


    }
    


}
