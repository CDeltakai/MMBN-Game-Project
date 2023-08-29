using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockElement : MonoBehaviour
{

    [SerializeField] CustomizerBlockSO selectedBlock;
    [SerializeField] List<Image> blockList;

    
    //private List<Vector2Int> blockSize;
    private BlockProperties blockProperties;
    private RectTransform rectTransform;

    private void Awake()
    {
        //blockSize = selectedBlock.blockSize.GetBlockSizeCoords(); 
        blockProperties = selectedBlock.blockSize.GetBlockProperties();   
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        foreach(Vector2Int coord in blockProperties.blockCoordinates) 
        {
            print(coord.ToString());    
        }

        print("Block Dimensions: " + blockProperties.blockDimensions.ToString());

        UpdateTransform();

    }


    void Update()
    {
        
    }

    public void UpdateTransform()
    {
        Vector2 size = new Vector2(128 * blockProperties.blockDimensions.x, 128 * blockProperties.blockDimensions.y);

        rectTransform.sizeDelta = size;
    }

    public void UpdateBlocks()
    {
        
    }


}
