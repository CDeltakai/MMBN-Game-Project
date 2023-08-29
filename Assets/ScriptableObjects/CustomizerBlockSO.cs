using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BlockProperties
{
    [field:SerializeField] public int blockCount {get; private set;}
    [field:SerializeField] public List<Vector2Int> blockCoordinates {get; private set;}
    [field:SerializeField] public Vector2Int blockDimensions {get; private set;}

    public BlockProperties(int blockCount, List<Vector2Int> blockCoordinates, Vector2Int blockDimensions)
    {
        this.blockCount = blockCount;
        this.blockCoordinates = blockCoordinates;
        this.blockDimensions = blockDimensions;
    }

}



[CreateAssetMenu(fileName = "Customizer Block", menuName = "New Customizer Block", order = 0)]
public class CustomizerBlockSO : ScriptableObject
{

    [field:SerializeField] public Sprite BlockSprite {get; private set;}
    public InspectorArrayLayout blockSize;






}
