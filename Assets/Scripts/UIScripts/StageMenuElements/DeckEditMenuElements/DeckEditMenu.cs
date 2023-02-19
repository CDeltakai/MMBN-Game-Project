using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckEditMenu : MonoBehaviour
{

    [SerializeField] GameObject chipDescriptionView;
    [SerializeField] GameObject InventoryView;
    [SerializeField] GameObject DeckView;

    [HideInInspector]
    public  ChipDescriptionView chipDescViewScript;

    private void Awake() {
        chipDescViewScript = chipDescriptionView.GetComponent<ChipDescriptionView>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void ConfirmChanges()
    {

    }

    public void ResetChanges()
    {
        
    }





}
