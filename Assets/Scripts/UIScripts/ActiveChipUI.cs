using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActiveChipUI : MonoBehaviour
{

    [SerializeField] Sprite[] chipQeueImages = new Sprite[5];
    [SerializeField] GameObject[] ActiveChipSlots = new GameObject[5];

    public PlayerMovement player;
    public Image chipImage;
    bool hasLoadedChips;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
    
        if(player.PlayerChipQueue.Count > 0)
        {
            LoadChipImages();
        }

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
