using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDescriptionPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDescription;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] Image cardImage;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] Image elementIcon;
    [SerializeField] Image typeIcon;
    

    void Start()
    {
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Take in a card slot and update the panel based on the cardSO stored within the card slot.
    public void UpdateDescription(CardSlot cardSlot)
    {
        if(cardSlot.IsEmpty()){return;}

        ChipSO cardSO = cardSlot.cardObjectReference.chipSO;

        textDescription.text = cardSO.GetChipDescription();
        cardName.text = cardSO.ChipName;
        cardImage.sprite = cardSO.GetChipImage();
        damageText.text = cardSO.GetChipDamage().ToString();
        gameObject.SetActive(true);
    }
    public void UpdateDescription(ChipSO cardSO)
    {
    
        textDescription.text = cardSO.GetChipDescription();
        cardName.text = cardSO.ChipName;
        cardImage.sprite = cardSO.GetChipImage();
        damageText.text = cardSO.GetChipDamage().ToString();
        gameObject.SetActive(true);
    }


    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }


}
