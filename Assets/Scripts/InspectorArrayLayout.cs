using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




[System.Serializable]
public class InspectorArrayLayout
{
    public static int rowSize = 7;


    [System.Serializable]
    public struct rowData
    {
        public bool[] row;
    }

    public rowData[] rows = new rowData[rowSize]; //Grid of 7x7

    public int GetNumberOfBlocks()
    {
        int count = 0;
        for(int i = 0; i < rowSize; i++)
        {
            for(int j = 0; j < rowSize; j++)
            {
                if(rows[i].row[j])
                {
                    count++;
                }
                

            }
        }    

        return count;    
    }


    //Get a list of coordinates based on the derived 2D-array grid of bools
    public List<Vector2Int> GetBlockSizeCoords()
    {
        List<Vector2Int> blockSizeList = new List<Vector2Int>();

        //j is the x axis and i is the y axis. 0,0 is in the top-left of the inspector grid
        for(int i = 0; i < rowSize; i++)
        {
            for(int j = 0; j < rowSize; j++)
            {
                if(rows[i].row[j])
                {
                    blockSizeList.Add(new Vector2Int(j, i));
                }
                

            }


        }
        return blockSizeList;
    }


    public BlockProperties GetBlockProperties()
    {
        int blockCount = 0;
        List<Vector2Int> blockCoordsList = new List<Vector2Int>();
        
        

        //j is the x axis and i is the y axis. 0,0 is in the top-left of the inspector grid
        for(int i = 0; i < rowSize; i++)
        {
            for(int j = 0; j < rowSize; j++)
            {
                if(rows[i].row[j])
                {
                    blockCoordsList.Add(new Vector2Int(j, i));
                    blockCount++;
                }
                
            }
        }

        //Find the maximum x and y value int the coordinate list to find dimensions of block
        Vector2Int blockDimensions = new Vector2Int(blockCoordsList.Max(t => t.x) + 1, blockCoordsList.Max(t => t.y) + 1);


        return new BlockProperties(0, blockCoordsList, blockDimensions);

    }



    

}
