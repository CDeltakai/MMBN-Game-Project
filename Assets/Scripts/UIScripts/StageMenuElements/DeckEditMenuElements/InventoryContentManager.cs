using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryContentManager : MonoBehaviour
{

    public DeckEditMenu deckEditMenu;
    public DeckContentManager deckContentManager;


    [SerializeField] GameObject inventoryElementPrefab;
    [SerializeField] PlayerAttributeSO playerStats;

    public List<ChipInventoryReference> chipInventory;
    public List<ChipInventoryReference> chipDeck;

    [SerializeField] public List<ChipInventoryReference> temporaryChipInventory = new List<ChipInventoryReference>();
    [SerializeField] List<InventoryChipSlot> internalElementList = new List<InventoryChipSlot>();


    public Dictionary<ChipInventoryReference, InventoryChipSlot> inventoryChipElementsDictionary = new Dictionary<ChipInventoryReference, InventoryChipSlot>();

    public Dictionary<InventoryChipSlot, ChipInventoryReference> inverseChipElementsDictionary = new Dictionary<InventoryChipSlot, ChipInventoryReference>();


    private void Awake() 
    {
        temporaryChipInventory = playerStats.CurrentChipInventory;

        chipInventory = playerStats.CurrentChipInventory;
        
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
            internalElementList.Add(inventoryElement);
            inventoryElement.chipInvRef = chipInvRef;
            inventoryElement.gameObject.name = (chipInvRef.chipName + "_InvElement");
            inventoryElement.contentManager = this;
            inventoryElement.deckEditMenu = deckEditMenu;
            inventoryElement.RefreshElement();

            //inventoryChipElementsDictionary.Add(chipInvRef, inventoryElement);
            //inverseChipElementsDictionary.Add(inventoryElement, chipInvRef);


        }


    }

    public void AddElementToInventory(ChipInventoryReference chipInvRef)
    {
        if(inventoryChipElementsDictionary.ContainsKey(chipInvRef))
        {
            //Update the chip count of the internal chip inventory reference that was originally taken from the player inventory.
            //This will later be converted into a list of chip inventory references that will replace the player inventory list on confirmation.
            //inverseChipElementsDictionary[inventoryChipElementsDictionary[chipInvRef]].chipCount += chipInvRef.chipCount;
            inverseChipElementsDictionary[inventoryChipElementsDictionary[chipInvRef]].SetChipCount(
                inverseChipElementsDictionary[inventoryChipElementsDictionary[chipInvRef]].chipCount + chipInvRef.chipCount
            );


            //Update the visible inventory element within the UI to show correct chip count
            //inventoryChipElementsDictionary[chipInvRef].chipInvRef.chipCount += chipInvRef.chipCount;

            inventoryChipElementsDictionary[chipInvRef].RefreshElement();     

            return;
        }


        temporaryChipInventory.Add(chipInvRef);

        InventoryChipSlot inventoryElement = Instantiate(inventoryElementPrefab, gameObject.transform).GetComponent<InventoryChipSlot>();
        internalElementList.Add(inventoryElement);

        inventoryElement.chipInvRef = chipInvRef;
        inventoryElement.gameObject.name = (chipInvRef.chipName + "_InvElement");
        inventoryElement.contentManager = this;
        inventoryElement.deckEditMenu = deckEditMenu;
        inventoryElement.RefreshElement();        

    }

    public void TransferElementToDeck(InventoryChipSlot inventoryElement)
    {
        ChipInventoryReference chipToTransfer = new ChipInventoryReference(inventoryElement.chipInvRef.chip, 1);


        if(inventoryElement.chipInvRef.chipCount == 1)
        {
            temporaryChipInventory.Remove(inventoryElement.chipInvRef);
            inverseChipElementsDictionary.Remove(inventoryElement);
            inventoryChipElementsDictionary.Remove(inverseChipElementsDictionary[inventoryElement]);

            deckContentManager.temporaryChipDeck.Add(inventoryElement.chipInvRef);

        }

        //inventoryElement.chipInvRef.chipCount--;


    }


    //Logic for clicking on an inventory chip to transfer it to the current deck
    public void ClickInventoryElement(InventoryChipSlot inventoryElement)
    {


    }



    


}
