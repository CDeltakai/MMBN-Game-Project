using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckChipSlot : MonoBehaviour
{

    public ChipSO chip;
    [HideInInspector]
    public DeckContentManager contentManager;

    [HideInInspector]
    public DeckEditMenu deckEditMenu;
    [SerializeField] UnityEngine.UI.Image chipThumbnail;
    [SerializeField] TextMeshProUGUI chipName;


    void Awake()
    {

    }

    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshElement()
    {
        chipThumbnail.sprite = chip.GetChipImage();
        chipName.text = chip.GetChipName();
    }

    public void OnClick()
    {
        contentManager.ClickDeckElement(this);


    }

    public void OnHover(DeckChipSlot deckChipSlot)
    {
        //print("Hovered over: " + deckChipSlot.chip.GetChipName());
        deckEditMenu.chipDescViewScript.RefreshView(chip);

        //deckEditMenu.RefreshChipDescriptionView(deckChipSlot);

    }



}
