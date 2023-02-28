using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class DeckContentManager : MonoBehaviour
{
    int DeckCapacity;
    public DeckEditMenu deckEditMenu;
    public InventoryContentManager inventoryContentManager;

    [SerializeField] GameObject deckElementPrefab;
    [SerializeField] PlayerAttributeSO playerData;
    [SerializeField] TextMeshProUGUI deckCounterText;

    

    [SerializeField] public List<ChipInventoryReference> temporaryChipDeck = new List<ChipInventoryReference>();
    [SerializeField] List<DeckChipSlot> internalElementList = new List<DeckChipSlot>();


    private void Awake() 
    {
        playerData = deckEditMenu.playerData;

        InitializeTemporaryChipDeck();



        DeckCapacity = playerData.AdjustOrGetCurrentDeckCapacity();
        //deckCounterText.text = (DeckCapacity + "/" +)    
    }

    private void InitializeTemporaryChipDeck()
    {
        foreach(ChipInventoryReference chipInvRef in playerData.CurrentChipDeck)
        {
            ChipSO chip = chipInvRef.chip;
            int chipCount = chipInvRef.chipCount;
            temporaryChipDeck.Add(new ChipInventoryReference(chip, chipCount));
        }

    }




    void Start()
    {
        AddContentFromDeck(temporaryChipDeck);        


    }

    public void ResetDeckView()
    {
        temporaryChipDeck.Clear();
        InitializeTemporaryChipDeck();
        
        foreach(DeckChipSlot deckElement in internalElementList)
        {
            Destroy(deckElement.gameObject);
        }
        internalElementList.Clear();

        AddContentFromDeck(temporaryChipDeck);

    }


    public void AddContentFromDeck(List<ChipInventoryReference> chipInvRefList)
    {
        foreach(ChipInventoryReference chipInvRef in chipInvRefList)
        {

            for(int i = 0; i < chipInvRef.chipCount ; i++) 
            {
                DeckChipSlot deckElement = Instantiate(deckElementPrefab, gameObject.transform).GetComponent<DeckChipSlot>();
                internalElementList.Add(deckElement);
                deckElement.chip = chipInvRef.chip;
                deckElement.gameObject.name = (deckElement.chip.GetChipName() + "_DeckElement");
                deckElement.contentManager = this;
                deckElement.deckEditMenu = deckEditMenu;
                deckElement.RefreshElement();

            }

        }

    }


    public void AddElementToDeck(ChipSO chip)
    {
        //First, search for this chip within the temporary chip deck
        //If the chip exists within the deck, increment the chip counter on that chip.
        //If it doesn't, add a new ChipInventoryReference in the temporary deck with a counter of 1.
        //In the both cases, create a new deck element which will be displayed on screen.
        if(temporaryChipDeck.Any(x => x.chip == chip))
        {
            temporaryChipDeck.Find(x => x.chip == chip).AddChipCount(1);
        }
        else
        {
            temporaryChipDeck.Add(new ChipInventoryReference(chip, 1));
        }

        DeckChipSlot deckElement = Instantiate(deckElementPrefab, gameObject.transform).GetComponent<DeckChipSlot>();
        internalElementList.Add(deckElement);
                deckElement.chip = chip;
                deckElement.gameObject.name = (deckElement.chip.GetChipName() + "_DeckElement");
                deckElement.contentManager = this;
                deckElement.deckEditMenu = deckEditMenu;
                deckElement.RefreshElement();

    }





    public void RemoveSpecifiChipFromDeck(ChipSO chip)
    {

    }

    public void RemoveDeckChipSlotFromDeck(DeckChipSlot deckElement)
    {
        ChipSO chip = deckElement.chip;
        ChipInventoryReference chipToFind = temporaryChipDeck.Find(x => x.chip == chip);

        chipToFind.AddChipCount(-1);
        if(chipToFind.chipCount <= 0)
        {
            temporaryChipDeck.Remove(chipToFind);
        }
        internalElementList.Remove(deckElement);
        Destroy(deckElement.gameObject);


    }


    public void TransferElementToInventory(DeckChipSlot deckElement)
    {
        inventoryContentManager.AddElementToInventory(new ChipInventoryReference(deckElement.chip, 1));
        RemoveDeckChipSlotFromDeck(deckElement);        



    }



    //Logic for clicking on a deck element to transfer to inventory
    public void ClickDeckElement(DeckChipSlot deckElement)
    {

        TransferElementToInventory(deckElement);

    }



}
