using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyManager : MonoBehaviour
{

    PlayerMovement player;
    public int MaxEnergy;

    private void Awake() 
    {
        player = GetComponent<PlayerMovement>();
        MaxEnergy = player.playerAttributes.AdjustOrGetMaxEnergy();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
