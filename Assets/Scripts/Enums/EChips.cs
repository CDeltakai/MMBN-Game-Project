using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// To create a new functional chip, here are the steps to follow:

// 1. Insert a new Enum value with the name of the chip in EChips (Do not change order of items)

// 2. Create a new ChipSO with the Chip value set to the corresponding chip name in EChips

// 3. If the chip is a Passive, create a new ChipEffectScript and input the name of the script
//    within the Effect Script field in ChipSO and the chip should work.

// 4. If the chip is an Active/Skill, create a new animation for it and drag it into the Megaman 
//    animation controller

// 5. Create a new Enum value within the EMegamanAnimations that corresponds to the name of the animation.
//    The Enum int value must equal to the corresponding EChip value. The animation must contain an animation
//    event that calls ApplyChipEffectV3.

// 6. Create a new ChipEffectScript the chip will use and type its name into the Effect Script field of the
//    ChipSO. The Animation Duration field of the ChipSO must be set or else the chip will not work.

// 7. The chip should now call its effect when used.
public enum EChips 
{

    AirShot = 100,
    AreaGrab,
    AttackPlus10,
    Cannon,
    Invisible,
    LongSword,
    WideSword,
    Reflect,
    Sword,
    Vulcan,
    WhiteCapsule,
    Bomb,



}
