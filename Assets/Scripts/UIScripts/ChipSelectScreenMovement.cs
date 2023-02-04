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
    [SerializeField] GameObject[] chipButtons;
    [SerializeField] GameObject[] ActiveChipSlots;

    [SerializeField] GameObject ChipDescriptor;
    [SerializeField] GameObject ChipNameField;
    [SerializeField] GameObject ChipModifiersField;
    [SerializeField] GameObject ChipElementField;
    [SerializeField] GameObject ChipAttributeField;
    [SerializeField] GameObject ChipDamageField;
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


    void populateChipSelect()
    {
        int randomIndex = 0;
        var random = new System.Random();
        
        selectableChips.Clear();
        for (int i = 0; i < 8; i++)
        {
            chipButtons[i].GetComponent<ChipSlot>().clearChip();

        }

            foreach(GameObject button in chipButtons)
            {
                button.SetActive(true);
            }


            for (int i = 0; i < maxSelectableChips; i++)
            {
                

                randomIndex = random.Next(0, chipInventory.getChipInventory().Count);

                if(selectableChips.Contains(chipInventory.getChipInventory()[randomIndex]))
                {
                    i--;
                    continue;
                }



                selectableChips.Add(chipInventory.getChipInventory()[randomIndex]);
            }

            for (int i = 0; i < selectableChips.Count; i++)
            {
                chipButtons[i].GetComponent<ChipSlot>().changeChip(selectableChips[i]);

            }
    }

    void populateChipRefDeck()
    {
        print("Attempting populate chiprefdeck");

        for(int i = 0; i < objectPoolManager.ChipRefList.Count; i++)
        {
            chipRefsDeck.Add(objectPoolManager.ChipRefList[i]);
            print("Attempted to add "+ objectPoolManager.ChipRefList[i].chipSORef.GetChipName());
        }

    }

    void populateChipSelectRefType()
    {
        
        selectableChipRefs.Clear();
        if(chipRefsDeck.Count <= 0)
        {
            chipRefsDeck.AddRange(discardedChipRefsDeck);
            discardedChipRefsDeck.Clear();            
        }

        foreach(GameObject button in chipButtons)
        {
            button.GetComponent<ChipSlot>().clearChip();
            button.SetActive(true);
        }


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
