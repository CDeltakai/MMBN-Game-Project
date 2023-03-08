using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckEditMenu : MonoBehaviour
{
    [SerializeField] public PlayerAttributeSO playerData;
    [SerializeField] UnityEngine.GameObject chipDescriptionView;
    [SerializeField] UnityEngine.GameObject InventoryView;
    [SerializeField] UnityEngine.GameObject DeckView;
    [SerializeField] public ChipTableDatabase chipTableDatabase;

    [SerializeField] DeckContentManager deckContentManager;
    [SerializeField] InventoryContentManager inventoryContentManager;


    [HideInInspector]
    public  ChipDescriptionView chipDescViewScript;

    private void Awake() {
        chipDescViewScript = chipDescriptionView.GetComponent<ChipDescriptionView>();
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    ///<summary>
    ///Logic for Confirm button on Deck Edit Menu
    ///</summary>
    public void ConfirmChanges()
    {

        playerData.CurrentChipDeck.Clear();
        foreach(ChipInventoryReference chipInvRef in deckContentManager.temporaryChipDeck)
        {
            ChipSO chip = chipInvRef.chip;
            int chipCount = chipInvRef.chipCount;
            playerData.CurrentChipDeck.Add(new ChipInventoryReference(chip, chipCount));
        }





        List<ChipInventoryReference> updatedInventory = new List<ChipInventoryReference>();

        foreach(KeyValuePair<ChipSO, ChipInventoryReference> keyValuePair in inventoryContentManager.temporaryChipDictionary)   
        {
            updatedInventory.Add(keyValuePair.Value);
        } 
        playerData.CurrentChipInventory = updatedInventory;
        chipTableDatabase.RefreshDatabase();


    }


    ///<summary>
    ///Logic for Reset button on Deck Edit Menu
    ///</summary>
    public void ResetChanges()
    {
        deckContentManager.ResetDeckView();
        inventoryContentManager.ResetInventoryView();

    }





}
