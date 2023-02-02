using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class ChipSlot : MonoBehaviour
{


[SerializeField] ChipSO SelectedChip;
[SerializeField] ChipObjectReference CurrentChipReference;
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
    activeImage.enabled = true;
}

public void changeChip(ChipSO chip)
{
    
    SelectedChip = chip;
    changeImage(SelectedChip);
    activeImage.enabled = true;
}



public void changeChipReference(ChipObjectReference chipRef)
{
    CurrentChipReference = chipRef;
    chipImage = chipRef.chipSORef.GetChipImage();
    activeImage.sprite = chipImage;
    activeImage.enabled = true;
}

public ChipObjectReference getChipObjRef()
{
    return CurrentChipReference;
}

public void clearChip()
{
    SelectedChip = null;
    CurrentChipReference = null;
    chipImage = null;
    activeImage.enabled = false;
}

public ChipSO getChip()
{
    return SelectedChip;
}






}
