using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class CardSelectionMenu : MonoBehaviour
{
    //Event for when the menu is activated
    public delegate void ActivateMenuEvent();
    public event ActivateMenuEvent MenuActivatedEvent;

    //Event for when the menu is disabled
    public delegate void DisableMenuEvent();
    public event DisableMenuEvent MenuDisabledEvent;


    [SerializeField] CardPoolManager cardPoolManager; //Should be set in inspector to improve performance
    [SerializeField] PlayerCardManager playerCardManager; //Should be set in inspector to improve performance

    [SerializeField] GameObject cardSelectPanel; //Must be set in inspector
    [SerializeField] GameObject cardLoadPanel; //Must be set in inspector

    [SerializeField] List<Image> connectorIndicators;

    //List of references to all the card slots within the cardSelectPanel and cardLoad panel
    [SerializeField] List<CardSlot> selectableCardSlots; // Needs to be set in inspector with the corresponding cardslots
    [SerializeField] List<CardSlot> cardLoadSlots; // Needs to be set in inspector with the corresponding card slots

    //List of run-time references to all the cardObjectReferences within the card slots within the cardSelectPanel and cardLoadPanel
    [SerializeField] List<CardObjectReference> cardObjectReferencesInSelectPanel;
    [SerializeField] List<CardObjectReference> cardObjectReferencesInLoadPanel;

    [SerializeField] List<CardObjectReference> currentDeckReference = new List<CardObjectReference>();

    RectTransform rectTransform;
    public bool isActive{get; private set;} = false;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        if(cardPoolManager == null)
        {
            cardPoolManager = FindObjectOfType<CardPoolManager>();
        }

        if(playerCardManager == null)
        {
            playerCardManager = FindObjectOfType<PlayerCardManager>();
        }

    }

    void Start()
    {
        InitializeMenu();
        PopulateCardSelect();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeMenu()
    {


        for(int i = 0; i < selectableCardSlots.Count ; i++)
        {
            if(!selectableCardSlots[i].IsEmpty())
            {
                cardObjectReferencesInSelectPanel.Add(selectableCardSlots[i].cardObjectReference);
            }

            selectableCardSlots[i].slotIndex = i;

        }

        for(int i = 0; i < cardLoadSlots.Count ; i++) 
        {
            cardLoadSlots[i].slotIndex = i;
        }

    }

    public void OpenMenuInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            ActivateMenu();
        }

    }    

    //Method for activating the menu and moving it into view. Will repopulate the card select with fresh cards on activation.
    public void ActivateMenu()
    {
        PopulateCardSelect();
        rectTransform.DOLocalMoveX(0, 0.25f).SetUpdate(true);
        for(int i = 0; i < selectableCardSlots.Count ; i++)
        {
            if(!selectableCardSlots[i].IsEmpty())
            {
                cardObjectReferencesInSelectPanel.Add(selectableCardSlots[i].cardObjectReference);
            }

        }

        isActive = true;
        MenuActivatedEvent?.Invoke();

        GameManager.PauseGame();
    }

    //Moves the menu out of view so it cannot be interacted with
    public void DisableMenu()
    {
        rectTransform.DOLocalMoveX(-600, 0.25f).SetUpdate(true);   

        isActive = false;
        MenuDisabledEvent?.Invoke();

        GameManager.UnpauseGame();
    }

    //Populates the Card Selection Panel with CardObjectReferences from the CardPoolManager. Uses a random index to grab a random card
    //from the CardObjectReferences and updates each of the card slots with said CardObjectReference
    private void PopulateCardSelect()
    {
        currentDeckReference.Clear();

        //Create a deep copy of references of the CardObjectReferences from the CardPoolManager that we can modify freely
        foreach(CardObjectReference card in cardPoolManager.CardObjectReferences)
        {
            currentDeckReference.Add(card);
        }

        //Wipe the panels clean of any leftover card objects and clear all card slots
        cardObjectReferencesInSelectPanel.Clear();
        cardObjectReferencesInLoadPanel.Clear();
        foreach(CardSlot cardSlot in selectableCardSlots)
        {cardSlot.ClearCardSlot();}
        foreach(CardSlot cardSlot in cardLoadSlots)
        {cardSlot.ClearCardSlot();}

        
        foreach(CardSlot cardSlot in selectableCardSlots)
        {
            if(currentDeckReference.Count <= 0)
            {
                break;
            }
            int randomIndex = Random.Range(0, currentDeckReference.Count - 1);
            cardSlot.ChangeCard(currentDeckReference[randomIndex]);
            currentDeckReference.RemoveAt(randomIndex);


        }
    }


    //Checks the cardload to see if passive chips are correctly ordered in the card load to be attached, then attach them to valid active cards. 
    public void ValidateCardLoad()
    {
        CardObjectReference currentCard;
        CardObjectReference mostRecentActiveCard = null;   
        for(int i = 0; i < cardObjectReferencesInLoadPanel.Count; i++) 
        {
            currentCard = cardLoadSlots[i].cardObjectReference;
            print("Index: " + i + " card: " + currentCard.chipSO.ChipName);
            
            if(currentCard.chipSO.ChipType == EChipTypes.Active)
            {
                mostRecentActiveCard = currentCard;
                print("MostRecentActiveCard: " + mostRecentActiveCard.chipSO.ChipName);
            }else
            if(currentCard.chipSO.ChipType == EChipTypes.Passive)
            {
                //If the first card is a passive card, continue - passive cards must be placed before the most recent active card, not after
                if(i == 0)
                {
                    continue;
                }

                if(mostRecentActiveCard != null || !mostRecentActiveCard.IsEmpty())
                {
                    mostRecentActiveCard.AttachPassiveCard(currentCard);
                }
            }



        }


    }

    



    //Logic for what happens when clicking on a card slot in the cardSelectPanel
    //Clicking on a slot in the load panel should return the card within the card slot to the previous select slot that it was originally taken from.
    //The card load panel should then update by moving all cards forward in the list to fill the gap left by the clicked slot.
    public void OnClickCardSlotLoadPanel(GameObject cardSlot)
    {
        CardSlot loadSlot = cardSlot.GetComponent<CardSlot>();

        if(loadSlot.IsEmpty())
        {
            Debug.LogWarning("loadSlot is empty but still interactable, which shouldn't be happening - something may have gone wrong.", loadSlot);
            return;
        }

        cardObjectReferencesInLoadPanel.Remove(loadSlot.cardObjectReference);

        loadSlot.TransferCardToSlot(loadSlot.previousTransfererSlot);
        cardObjectReferencesInSelectPanel.Add(loadSlot.previousTransfererSlot.cardObjectReference);

        //Update other loadSlots by moving forward all cards to fill in gaps
        for(int i = loadSlot.slotIndex + 1; i < cardLoadSlots.Count ; i++) 
        {
            if(cardLoadSlots[i].IsEmpty())
            {break;}

            cardLoadSlots[i].ReplaceCardSlot(cardLoadSlots[i - 1]);            

        }

        //ValidateCardLoad();

    }

    //Logic for what happens when clicking on a card slot in the cardLoadPanel
    //Clicking on a slot in the select panel should transfer the card object reference stored within to the bottom of card load panel list
    public void OnClickCardSlotSelectPanel(GameObject cardSlot)
    {
        CardSlot selectSlot = cardSlot.GetComponent<CardSlot>();

        if(selectSlot.IsEmpty())
        {
            Debug.LogWarning("loadSlot is empty but still interactable, which shouldn't be happening - something may have gone wrong.", selectSlot);
            return;
        }

        print("Clicked card: " + selectSlot.cardObjectReference.chipSO.GetChipName());


        if(cardObjectReferencesInLoadPanel.Count < cardLoadSlots.Count)//Prevent user from selecting more cards than there is capacity in the load panel
        {
            CardSlot loadSlot = cardLoadSlots[cardObjectReferencesInLoadPanel.Count];

            cardObjectReferencesInSelectPanel.Remove(selectSlot.cardObjectReference);
            selectSlot.TransferCardToSlot(loadSlot);

            cardObjectReferencesInLoadPanel.Add(loadSlot.cardObjectReference);
              
        }

        //ValidateCardLoad();


    }

    //Logic for what happens when clicking the OK button
    public void OnClickOKButton()
    {
        ValidateCardLoad();
        playerCardManager.LoadCardMagazine(cardObjectReferencesInLoadPanel);
        DisableMenu();
    }

}
