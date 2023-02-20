using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipDescriptionView : MonoBehaviour
{

    public ChipSO chipToView;

    [SerializeField] UnityEngine.UI.Image chipImage;
    [SerializeField] TextMeshProUGUI chipNameTxt;
    [SerializeField] TextMeshProUGUI chipDamageTxt;
    [SerializeField] TextMeshProUGUI chipDescriptionTxt;
 


    public void RefreshView(ChipSO chip)
    {
        //print("Attempting to refresh chip description view");
        chipToView = chip;
        //print("ChipDamage: " + chip.GetChipDamage().ToString());
        chipImage.sprite = chip.GetChipImage();
        chipNameTxt.text = chip.GetChipName();
        chipDamageTxt.text = chip.GetChipDamage().ToString();
        chipDescriptionTxt.text = chip.GetChipDescription();

    }

}
