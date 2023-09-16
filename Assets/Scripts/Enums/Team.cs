using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Team enum is used to dictate what attacks or effects can affect who.
//Player: Negative effects can affect Enemy/Neutral. Positive effects can affect Player
//Enemy: Negative effects can affect Player/Neutral. Positive effects can affect Enemy/Neutral
//Neutral: both Positive and Negative effects can affect all teams including its own team.
//Note: Neutral is usually used for environmental hazards/elements that do not care about friend or foe
public enum Team
{
    Player,
    Enemy,
    Neutral

}
