using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;

public class ChipSelectScreenMovement : MonoBehaviour
{



    private Vector3 endPosition = new Vector3 (-690, 170, 0);
    private Vector3 startPosition = new Vector3 (-1229, 170, 0);
    ChipInventory chipInventory;
    ObjectPoolManager objectPoolManager;
    int chipHandCapacity = 8;
    int maxSelectableChips = 8;
    [SerializeField] float desiredDuration = 1f;

    private float elapsedTime = 0;
    private RectTransform rectTransform;
    private float journeyLength;

    [SerializeField] List<ChipSO> activeChips = new List<ChipSO>();
    [SerializeField] List<ChipObjectReference> activeChipRefs = new List<ChipObjectReference>();

    [SerializeField] List<ChipSO> selectableChips;
    [SerializeField] List<ChipObjectReference> selectableChipRefs;
    [SerializeField] GameObject[] chipButtons;
    [SerializeField] GameObject[] ActiveChipSlots;
    PlayerMovement playerMovement;
    ChipLoadManager chipLoadManager;

    int ActiveChipSlotAccumulator = 0;
    


    private bool isTriggered = false;
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
        //populateChipSelect();
        populateChipSelectRefType();
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
            isTriggered = true;
            elapsedTime = 0;
            //Debug.Log("Trigger True");
        }
        else if(isTriggered)
        {
            unPause();
            isTriggered = false;
            elapsedTime = 0;
            //Debug.Log("Trigger False");
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

    void populateChipSelectRefType()
    {
        //int randomIndex = 0;
        //var random = new System.Random();
        
        selectableChipRefs.Clear();
        for (int i = 0; i < 8; i++)
        {
            chipButtons[i].GetComponent<ChipSlot>().clearChip();

        }

            foreach(GameObject button in chipButtons)
            {
                button.SetActive(true);
            }




            for (int i = 0; i < 2; i++)
            {
                selectableChipRefs.Add(objectPoolManager.ChipRefList[i]);
            }

            for (int i = 0; i < (2); i++)
            {
                chipButtons[i].GetComponent<ChipSlot>().changeChipReference(selectableChipRefs[i]);

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


    public void OnChipHover()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(ChipHoverVFX, this.gameObject);
    }

    //OK Button Functionality
    public void LoadIntoChipQueue()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(OKButtonVFX, this.gameObject);

        foreach(ChipSO chip in activeChips)
        {
            chipLoadManager.chipQueue.Add(chip);
        }

            activeChips.Clear();
    

        for (int i = 0; i < ActiveChipSlotAccumulator; i++)
        {
        ActiveChipSlots[i].GetComponent<ChipSlot>().clearChip();
        }

        ActiveChipSlotAccumulator = 0;
        chipLoadManager.calcNextChipLoad();


        print("Class: ChipSelectScreenMovement, attempted calcNextChipLoad()");
        ToggleChipMenu();       
    }

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
