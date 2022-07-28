using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class ChipSlot : MonoBehaviour
{


[SerializeField] ChipSO SelectedChip;
[SerializeField] Sprite chipImage;
Image activeImage;
    

private void Awake() {
    activeImage = GetComponent<Image>();
    activeImage.enabled = false;
    
}



public void changeImage(ChipSO chip)
{
    chipImage = chip.GetChipImage();
    activeImage.sprite = chipImage;
}

public void changeChip(ChipSO chip)
{
    
    SelectedChip = chip;
    changeImage(SelectedChip);
    activeImage.enabled = true;
}

public void clearChip()
{
    SelectedChip = null;
    chipImage = null;
    activeImage.enabled = false;
}

public ChipSO getChip()
{
    return SelectedChip;
}






}
