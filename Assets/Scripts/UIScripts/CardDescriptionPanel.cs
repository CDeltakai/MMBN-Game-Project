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
    [SerializeField] string testValue;
    

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

        string cardDescription = cardSO.GetFormattedDescription();


        textDescription.text = cardDescription;
        cardName.text = cardSO.ChipName;
        //cardImage.sprite = cardSO.GetChipImage();
        if(cardSO.GetChipDamage() == 0)
        {
            damageText.text = "N/A";
        }else
        {
            damageText.text = cardSO.GetChipDamage().ToString();
        }
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

    string FormatDescription(ChipSO card, string description)
    {
        string formattedDescription = description;
        formattedDescription = formattedDescription.Replace("Q0", testValue);
        formattedDescription = formattedDescription.Replace("BD", card.GetChipDamage().ToString());



        return formattedDescription;
    }


    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }


}
