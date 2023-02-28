using System.Collections;
using System.Collections.Generic;
using UnityEngine;



///<summary>
/// <para>
///      Passive chips apply an additional effect on the Active or OffensiveSpecial chip they are attached to.
/// </para>
/// <para>
///      Active chips deal damage and apply their effects in real-time and are tied to an animation on Megaman
/// </para>
/// <para>
///      Status chips do not deal modifiable damage and apply their effects in real-time and are tied to an animation on Megaman.
/// </para>
/// <para>
///      Special chips do not deal modifiable damage and apply their effects in real-time and are not tied to an animation on Megaman.
/// </para>
/// <para>
///      OffensiveSpecial chips deal modifiable damage and apply their effects in real-time and are not tied to an animation on Megaman.
/// </para>
/// <para>
///      Skill chips are abilities that do not need energy to cast. They are different from regular chips
///      in that they do not require a data blueprint to cast and are more akin to abilities a character can do on their own.
/// </para>
///</summary>
public enum EChipTypes
{



    Passive = 0,
    Attack = 1,
    Status = 2,
    Special = 3,
    OffensiveSpecial = 4,
    Skill = 5


}
