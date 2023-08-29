using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationController : MonoBehaviour
{
    [HideInInspector]
    public CustomizationGrid customizationGrid;


    void Update()
    {
        if(customizationGrid == null) {return;}

        if(Input.GetMouseButtonDown(0))
        {
            print(customizationGrid.GetTileGridPosition(Input.mousePosition));
        }


    }
}
