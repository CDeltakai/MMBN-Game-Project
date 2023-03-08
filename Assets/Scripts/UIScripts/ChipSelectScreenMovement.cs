using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;
using TMPro;

public class ChipSelectScreenMovement : MonoBehaviour
{

    public delegate void TriggeredMenuEvent(bool condition);
    public event TriggeredMenuEvent triggerMenuEvent;

    private Vector3 endPosition = new Vector3 (-690, 170, 0);
    private Vector3 startPosition = new Vector3 (-1229, 170, 0);
    ChipInventory chipInventory;
    ObjectPoolManager objectPoolManager;
    int chipHandCapacity = 8;
    int maxSelectableChips = 10;
    [SerializeField] float desiredDuration = 1f;

    private float elapsedTime = 0;
    private RectTransform rectTransform;
    private float journeyLength;

    [SerializeField] List<ChipSO> activeChips = new List<ChipSO>();
    [SerializeField] List<ChipObjectReference> activeChipRefs = new List<ChipObjectReference>();

    [SerializeField] List<ChipSO> selectableChips;
    [SerializeField] List<ChipObjectReference> selectableChipRefs;
    [SerializeField] List<ChipObjectReference> chipRefsDeck = new List<ChipObjectReference>();
    [SerializeField] List<ChipObjectReference> discardedChipRefsDeck = new List<ChipObjectReference>();
    [SerializeField] UnityEngine.GameObject[] chipButtons;
    [SerializeField] UnityEngine.GameObject[] ActiveChipSlots;

    [SerializeField] UnityEngine.GameObject ChipDescriptor;
    [SerializeField] UnityEngine.GameObject ChipNameField;
    [SerializeField] UnityEngine.GameObject ChipModifiersField;
    [SerializeField] UnityEngine.GameObject ChipElementField;
    [SerializeField] UnityEngine.GameObject ChipAttributeField;
    [SerializeField] UnityEngine.GameObject ChipDamageField;
    PlayerMovement playerMovement;
    ChipLoadManager chipLoadManager;

    int ActiveChipSlotAccumulator = 0;
    


    public bool isTriggered = false;
    private bool isActive = false;

    public static bool GameIsPaused = false;

    [SerializeField] EventReference ScreenOpenVFX;
    [SerializeField] EventReference OKButtonVFX;
    [SerializeField] EventReference ChipSelectVFX;
    [SerializeField] EventReference ChipHoverVFX;

    void Awake()
    {
        
        objectPoolManager = FindObjectOfType<ObjectPoolManager>();


    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        journeyLength = Vector3.Distance(startPosition, endPosition);
        rectTransform.anchoredPosition = startPosition;
        chipInventory = FindObjectOfType<ChipInventory>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        chipLoadManager = FindObjectOfType<ChipLoadManager>();
        populateChipRefDeck();

    }

    void Update()
    {
        if (!isActive){return;}

        if(isTriggered)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float percentageComplete = elapsedTime/desiredDuration;

            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, percentageComplete);
        }

        if(!isTriggered)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float percentageComplete = elapsedTime/desiredDuration;
            rectTransform.anchoredPosition = Vector3.Lerp(endPosition, startPosition, percentageComplete);
        }


    }

//Tied to the OpenDeck keybinding within the PlayerControl InputActionAsset
    public void ToggleChipMenu()
    {
        Debug.Log("Triggered Menu");
        isActive = true;

        if(!isTriggered)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(ScreenOpenVFX, this.gameObject);
            //populateChipSelect();
            populateChipSelectRefType();
            Pause();
            if(triggerMenuEvent != null)
            {
                triggerMenuEvent(true);
            }
            isTriggered = true;
            elapsedTime = 0;
        }
        else if(isTriggered)
        {
            unPause();
            if(triggerMenuEvent !=null)
            {
                triggerMenuEvent(false);
            }

            isTriggered = false;
            elapsedTime = 0;
        }
    }


    void Pause(){
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    void unPause(){Time.timeScale = 1; GameIsPaused = false;}



    public void ResetChipDeck()
    {
        chipRefsDeck.Clear();
        discardedChipRefsDeck.Clear();
        
    }


//Looks at the ChipRefList from the ObjectPoolManager and adds all the ChipReferences within that list
//to an internal chip deck (chipRefsDeck) in the ChipSelectScreen.
    void populateChipRefDeck()
    {
        print("Attempting populate chiprefdeck");

        for(int i = 0; i < objectPoolManager.ChipObjectList.Count; i++)
        {
            chipRefsDeck.Add(objectPoolManager.ChipObjectList[i]);
            print("Attempted to add "+ objectPoolManager.ChipObjectList[i].chipSORef.GetChipName());
        }

    }

//Populates the chip select screen with selectable chips drawn from the chipRefsDeck. Uses Unity's
//random number generator to select a random index within the chipRefsDeck and adds that to the 
//list of selectable chips (selectableChipRefs) that is then applied to the buttons on the UI.
//
//After each chip is randomly selected, that chip also gets placed into a discard deck (discardedChipRefsDeck)
//before being removed from the current deck using the same index generated by the random number generator to select it.
    void populateChipSelectRefType()
    {
        
        selectableChipRefs.Clear();
        if(chipRefsDeck.Count <= 0)
        {
            chipRefsDeck.AddRange(discardedChipRefsDeck);
            discardedChipRefsDeck.Clear();            
        }

        foreach(UnityEngine.GameObject button in chipButtons)
        {
            button.GetComponent<ChipSlot>().clearChip();
            button.SetActive(true);
        }


    //Randomly selecting from chipRefsDeck. If there are less chips within the deck than
    //the current maxSelectableChips then the operation breaks and moves on.
        for (int i = 0; i < maxSelectableChips; i++)
        {
            if(chipRefsDeck.Count <= 0)
            {
                break;
            }
            int randomInt = UnityEngine.Random.Range(0, (chipRefsDeck.Count-1));
            selectableChipRefs.Add(chipRefsDeck[randomInt]);
            discardedChipRefsDeck.Add(chipRefsDeck[randomInt]);
            chipRefsDeck.RemoveAt(randomInt);
        }

        for (int i = 0; i < (maxSelectableChips); i++)
        {
            if(i >= selectableChipRefs.Count)
            {
                break;
            }
            chipButtons[i].GetComponent<ChipSlot>().changeChipReference
            (selectableChipRefs[i]);
        }

    }


//UI Events


    public void OnChipSelect(int buttonIndex)
    {
        if(ActiveChipSlotAccumulator == 5)
        {
            return;
        }
        FMODUnity.RuntimeManager.PlayOneShotAttached(ChipSelectVFX, this.gameObject);
        ActiveChipSlots[ActiveChipSlotAccumulator].GetComponent<ChipSlot>().changeChip(selectableChips[buttonIndex]);
        chipButtons[buttonIndex].SetActive(false);
        activeChips.Add(selectableChips[buttonIndex]);
        ActiveChipSlotAccumulator++;
    }

    public void OnChipSelectRefType(int buttonIndex)
    {
        if(ActiveChipSlotAccumulator == 5)
        {
            return;
        }
        FMODUnity.RuntimeManager.PlayOneShotAttached(ChipSelectVFX, this.gameObject);
        ActiveChipSlots[ActiveChipSlotAccumulator].GetComponent<ChipSlot>().changeChipReference(selectableChipRefs[buttonIndex]);
        chipButtons[buttonIndex].SetActive(false);
        activeChipRefs.Add(selectableChipRefs[buttonIndex]);
        ActiveChipSlotAccumulator++;



    }


    public void OnChipHover(ChipSlot chipSlot)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(ChipHoverVFX, this.gameObject);
        //print(chipSlot.getChipObjRef().chipSORef.GetChipDescription());
        if(chipSlot.getChipObjRef() != null)
        {
            ChipNameField.GetComponent<TextMeshProUGUI>().text = chipSlot.getChipObjRef().chipSORef.GetChipName();            
            ChipDescriptor.GetComponent<TextMeshProUGUI>().text = chipSlot.getChipObjRef().chipSORef.GetChipDescription();

            if(chipSlot.getChipObjRef().chipSORef.GetChipDamage() == 0)
            {
                ChipDamageField.GetComponent<TextMeshProUGUI>().text = "N/A";
            }else
            {
                ChipDamageField.GetComponent<TextMeshProUGUI>().text = chipSlot.getChipObjRef().chipSORef.GetChipDamage().ToString();
            }

        }

    }

    //OK Button Functionality
    public void LoadRefsIntoQueue()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(OKButtonVFX, this.gameObject);

        foreach(ChipObjectReference chipRef in activeChipRefs)
        {
            chipLoadManager.chipRefQueue.Add(chipRef);
        }

            activeChipRefs.Clear();
    

        for (int i = 0; i < ActiveChipSlotAccumulator; i++)
        {
            ActiveChipSlots[i].GetComponent<ChipSlot>().clearChip();
        }

        ActiveChipSlotAccumulator = 0;
        chipLoadManager.calcNextChipRefLoad();


        print("Class: ChipSelectScreenMovement, attempted calcNextChipRefLoad()");
        ToggleChipMenu(); 

    }



}
