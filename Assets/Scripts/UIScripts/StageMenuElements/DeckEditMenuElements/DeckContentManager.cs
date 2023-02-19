using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckContentManager : MonoBehaviour
{
    int DeckCapacity;
    public DeckEditMenu deckEditMenu;
    public InventoryContentManager inventoryContentManager;

    [SerializeField] GameObject deckElementPrefab;
    [SerializeField] PlayerAttributeSO playerStats;
    [SerializeField] TextMeshProUGUI deckCounterText;

    

    [SerializeField] public List<ChipInventoryReference> temporaryChipDeck = new List<ChipInventoryReference>();
    [SerializeField] List<DeckChipSlot> internalElementList = new List<DeckChipSlot>();


    private void Awake() 
    {
        temporaryChipDeck = playerStats.CurrentChipDeck;

        DeckCapacity = playerStats.AdjustOrGetCurrentDeckCapacity();
        //deckCounterText.text = (DeckCapacity + "/" +)    
    }






    void Start()
    {
        AddContentFromPlayerDeck(playerStats.CurrentChipDeck);        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddContentFromPlayerDeck(List<ChipInventoryReference> chipInvRefList)
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


    public void AddElementToDeck(ChipInventoryReference chipInvRef)
    {






    }


    //Logic for clicking on a deck element to transfer to inventory
    public void ClickDeckElement(DeckChipSlot deckElement)
    {


    }



}
