using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationGrid : MonoBehaviour
{
    const float tileSizeWidth = 128;
    const float tileSizeHeight = 128;

    RectTransform rectTransform;

    Vector2 positionOnGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnGrid.x/tileSizeWidth);
        tileGridPosition.y = (int)(positionOnGrid.y/tileSizeHeight);

        return tileGridPosition;
    }


}
