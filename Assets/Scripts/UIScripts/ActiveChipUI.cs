using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActiveChipUI : MonoBehaviour
{

    //[SerializeField] Sprite[] chipQeueImages = new Sprite[5];
    [SerializeField] List<ChipSlot> ActiveChipSlots = new List<ChipSlot>();

    PlayerMovement player;
    ChipLoadManager chipLoadManager;
    Image chipImage;
    bool hasLoadedChips;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        chipLoadManager = FindObjectOfType<ChipLoadManager>();

    }

    void Update()
    {
    


    }


    public void LoadChipImages()
    {
        for (int i = 0; i < player.PlayerChipQueue.Count; i++)
        {
            chipImage = ActiveChipSlots[i].GetComponent<Image>();
            //chipQeueImages[i] = player.PlayerChipQueue[i].GetChipImage();
            ActiveChipSlots[i].GetComponent<Image>().sprite = player.PlayerChipQueue[i].GetChipImage();
            chipImage.color = new Color(chipImage.color.r, chipImage.color.g, chipImage.color.b, 1f);

        }


        


    }


}
