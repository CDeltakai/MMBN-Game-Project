using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem.Interactions;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Base StageTile", menuName = "New Base StageTile", order = 0)]

public class BaseTile : Tile
{

[SerializeField] string tileName;
[SerializeField] ETiles tileEnum;
[SerializeField] public ETileTeam tileTeam;

//DO NOT CHANGE THE NAME OF THE CurrentTile OR ELSE THE WHOLE TILEMAP WILL BREAK!!!
[SerializeField] Tile CurrentTile;
[SerializeField] Animation tileAnimation;
[SerializeField] string tileScriptName;
[SerializeField] public bool isPassable = true;


public Tile GetTile()
{
    return CurrentTile;

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




    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {

        tileData.sprite = GetTile().sprite;
        tileData.transform = transform;
        tileData.gameObject = gameObject;
        tileData.flags = flags;
        tileData.colliderType = colliderType;
    }




}
