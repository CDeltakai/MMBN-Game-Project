using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BStageTile Data", menuName = "New BStageTile", order = 0)]
public class TilesSO : ScriptableObject
{

[SerializeField] string tileName;
[SerializeField] ETiles tileEnum;
[SerializeField] Tile tile;
[SerializeField] string tileScriptName;



public Tile GetTile()
{
    return tile;
}
public ETiles GetTileEnum()
{
    return tileEnum;
}
public string GetName()
{
    return tileName;
}

public string getTileScriptName()
{
    return tileScriptName;
}
public Type getTileScript()
{
    return Type.GetType(tileScriptName);
}



}
