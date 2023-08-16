using System.Collections;
using System.Collections.Generic;
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
    

}
