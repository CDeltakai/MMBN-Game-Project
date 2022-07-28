using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ChipInterface
{

    public int GetChipID(string ChipName);
    public int GetChipType(int chipID);
    public int GetChipDamage(int chipID);


}
