using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class InventoryChipSlot : MonoBehaviour
{

    public ChipInventoryReference chipInvRef;

    [HideInInspector]
    public InventoryContentManager contentManager;
    [HideInInspector]
    public DeckEditMenu deckEditMenu;

    [SerializeField] UnityEngine.UI.Image chipImage;
    [SerializeField] TextMeshProUGUI chipCountText;
    [SerializeField] TextMeshProUGUI chipNameText;


    private void Awake() {
        gameObject.SetActive(true);
    }


    void Start()
    {
        

    }


    public void RefreshElement()
    {
        chipImage.sprite = chipInvRef.chip.GetChipImage();
        chipCountText.text = chipInvRef.chipCount.ToString();
        chipNameText.text = chipInvRef.chipName;
        gameObject.SetActive(true);
    }

    public void RefreshChipCount()
    {
        chipCountText.text = chipInvRef.chipCount.ToString();
    }


    public void RefreshElement(ChipInventoryReference specificChipInvRef)
    {
        chipImage.sprite = specificChipInvRef.chip.GetChipImage();
        chipCountText.text = specificChipInvRef.chipCount.ToString();
        chipNameText.text = specificChipInvRef.chipName;
        gameObject.SetActive(true);        

    }

    public void OnClick()
    {
        contentManager.ClickInventoryElement(this);


    }

    public void OnHover(InventoryChipSlot inventoryChipSlot)
    {
        //print("Hovered over: " + inventoryChipSlot.chipNameText.text);
        //deckEditMenu.RefreshChipDescriptionView(inventoryChipSlot);
        deckEditMenu.chipDescViewScript.RefreshView(chipInvRef.chip);

    }


}
