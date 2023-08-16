using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Controls ability to interact with the grid
[RequireComponent(typeof(CustomizationGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    CustomizationController customizationController;
    CustomizationGrid customizationGrid;


    public void OnPointerEnter(PointerEventData eventData)
    {
        customizationController.customizationGrid = customizationGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        customizationController.customizationGrid = null;
    }

    private void Awake() {
        customizationController = FindObjectOfType<CustomizationController>();
        customizationGrid = GetComponent<CustomizationGrid>();
    }


}
