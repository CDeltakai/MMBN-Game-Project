using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DamageFunctions
{
    void hurtEntity(int damageAmount);
    int getHealth();

    //this should be a player function - TODO: create a new interface for player-specific functions
    void setHealthText(int number);

}
