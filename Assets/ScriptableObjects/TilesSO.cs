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
[SerializeField] ETileTeam tileTeam;
[SerializeField] Tile tile;
[SerializeField] Animation tileAnimation;
[SerializeField] string tileScriptName;
[SerializeField] bool isPassable = true;



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

public ETileTeam GetTileTeam()
{
    return tileTeam;
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
