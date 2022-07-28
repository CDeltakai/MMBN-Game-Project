using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EChipTypes
{
//Passive chips apply an additional effect on the closest Active or OffensiveSpecial chip
//Active chips deal damage and apply their effects in real-time
//Status chips do not deal damage and apply their effects in real-time
//Special chips freeze time before applying their effects
//OffensiveSpecial chips freeze time before dealing damage and applying effects

//Skill chips are abilities that do not need energy to cast. They are different from regular chips
//in that they do not require a data blueprint to cast and are more akin to abilities a character
//can do on their own.


    Passive = 0,
    Active = 1,
    Status = 2,
    Special = 3,
    OffensiveSpecial = 4,
    Skill = 5


}
