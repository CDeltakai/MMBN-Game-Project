using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Custom StageTile", menuName = "New Custom StageTile", order = 0)]

public class CustomTile : Tile
{

[SerializeField] string tileName;
[SerializeField] ETiles tileEnum;
[SerializeField] ETileTeam tileTeam;
[SerializeField] Tile tile;
[SerializeField] Animation tileAnimation;
[SerializeField] string tileScriptName;
[SerializeField] public bool isPassable = true;
[SerializeField] public TilesSO tileSO;


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


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Custom Tile")]
    public static void CreateCustomTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Custom Tile", "New Custom Tile", "Asset", "Save Custome Tile", "Assets");
        if (path == "") return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomTile>(), path);
    }
#endif


}
