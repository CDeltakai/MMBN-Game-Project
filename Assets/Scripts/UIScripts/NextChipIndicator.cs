using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextChipIndicator : MonoBehaviour
{

    PlayerMovement player;
    ChipSlot chipSlot;
    ChipLoadManager chipLoadManager;
    ChipSelectScreenMovement chipSelectScreen;

    void Start()
    {
        chipSlot = GetComponent<ChipSlot>();
        player = FindObjectOfType<PlayerMovement>();
        chipLoadManager = FindObjectOfType<ChipLoadManager>();

        player.usedChip += UpdateChipImage;
        chipLoadManager.loadChipsEvent += UpdateChipImage;



    }


    void Update()
    {
        
    }


    void UpdateChipImage()
    {
        if(chipLoadManager.nextChipRefLoad.Count > 0)
        {
            chipSlot.changeChipReference(chipLoadManager.nextChipRefLoad[0]);
        }else
        {
            chipSlot.clearChip();
        }
        


    }

}
