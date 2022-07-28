using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EChipElements
{

    Normal,
    Air,
    Blade,
    Fire,
    Water,
    Electric,
    Grass,
    Breaking,
    Cursor,
    Multiplier,
    Buff,
    Healing


}

namespace testExtensions
{
    
    public static class EChipElementsExtensions
    {

        public static string getNameOfElement(this EChipElements chipelement)
        {
            return chipelement.ToString();
        }

        

    }

}
