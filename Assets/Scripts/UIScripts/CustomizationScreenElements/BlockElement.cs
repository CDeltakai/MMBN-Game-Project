using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockElement : MonoBehaviour
{

    [SerializeField] CustomizerBlockSO selectedBlock;

    void Start()
    {
        foreach(var coord in selectedBlock.blockSize.GetBlockSizeCoords()) 
        {
            print(coord.ToString());    
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
