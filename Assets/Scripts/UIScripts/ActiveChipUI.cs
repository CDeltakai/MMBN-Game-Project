using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//Chip Payload Indicator
public class ActiveChipUI : MonoBehaviour
{

    [SerializeField] Vector3 InViewPosition = new Vector3();
    [SerializeField] Vector3 OutOfViewPosition = new Vector3();
    [SerializeField] List<ChipSlot> ActiveChipSlots = new List<ChipSlot>();

    RectTransform rect;
    PlayerMovement player;
    ChipLoadManager chipLoadManager;
    ChipSelectScreenMovement chipSelectScreen;
    Image chipImage;
    bool hasLoadedChips;

    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        chipLoadManager = FindObjectOfType<ChipLoadManager>();
        rect = GetComponent<RectTransform>();
        chipSelectScreen = FindObjectOfType<ChipSelectScreenMovement>();
        chipSelectScreen.triggerMenuEvent += MoveUIPosition;
        chipLoadManager.loadChipsEvent += LoadChipImages;
        player.usedChipEvent += LoadChipImages;

    }


    ///<summary>
    ///true = move UI to InViewPosition, false = move UI to OutOfViewPosition
    ///</summary> 
    void MoveUIPosition(bool condition)
    {
        if(condition)
        {
            //rect.DOMoveX(333, 0.2f).SetUpdate(true);
            rect.DOMoveX(-5, 0.2f).SetUpdate(true);
        }else
        {
            //rect.DOMoveX(-333, 0.2f).SetUpdate(true);
            rect.DOLocalMoveX(-2.4f, 0.2f).SetUpdate(true);

        }

    }

    public void LoadChipImages()
    {
        foreach(ChipSlot chipSlot in ActiveChipSlots)
        {
            chipSlot.clearChip();
        }        

        if(chipLoadManager.chipRefQueue.Count == 0 && chipLoadManager.nextChipRefLoad.Count == 0)
        {
            foreach(ChipSlot chipSlot in ActiveChipSlots)
            {
                chipSlot.clearChip();
            }
            return;
                

        }        

        ActiveChipSlots[0].changeImage(chipLoadManager.nextChipRefLoad[0].chipSORef);


        for (int i = 1; i < chipLoadManager.chipRefQueue.Count + 1; i++)
        {

            chipImage = ActiveChipSlots[i].GetComponent<Image>();
            
            ActiveChipSlots[i].changeImage(chipLoadManager.chipRefQueue[i-1].chipSORef);
            chipImage.color = new Color(chipImage.color.r, chipImage.color.g, chipImage.color.b, 0.65f);

        }


        


    }


}
