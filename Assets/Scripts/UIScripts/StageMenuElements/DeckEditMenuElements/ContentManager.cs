using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentManager : MonoBehaviour
{

    [SerializeField] PlayerAttributeSO currentPlayerStats;

    public List<ChipInventoryReference> chipInventory;
    public List<ChipInventoryReference> chipDeck;


    private void Awake() 
    {
        chipInventory = currentPlayerStats.CurrentChipInventory;
        
    }



    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
