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

///<summary>
///The tileOwner is the original team that owns this tile. This should be set only once at run time and should not be changed under normal circumstances.
///The purpose of the tileOwner is to enable the ability to revert stolen tiles back to their original owner so that a tile is
///not permanently taken over by an opponent.
///</summary>
[SerializeField] ETileTeam tileOwnerTeam;
[SerializeField] public ETileTeam tileTeam;

//DO NOT CHANGE THE NAME OF THE IDENTIFIER OF CurrentTile OR ELSE THE WHOLE TILEMAP WILL BREAK!!!
[SerializeField] Tile CurrentTile;
[SerializeField] Animation tileAnimation;
[SerializeField] string tileScriptName;
[SerializeField] public bool isPassable = true;
[SerializeField] public TilesSO tileSO;

[SerializeField] Tile PlayerTile;
[SerializeField] Tile EnemyTile;
[SerializeField] CustomTile CustomPlayerTile;
[SerializeField] CustomTile CustomEnemyTile;


public Tile GetTile()
{
    if(tileTeam == ETileTeam.Player)
    {
        return PlayerTile;
    }else
    {
        return EnemyTile;
    }
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

public Tile getPlayerTile()
{
    return PlayerTile;
}
public CustomTile getCustomPlayerTile()
{
    return CustomPlayerTile;
}
public CustomTile getCustomEnemyTile()
{
    return CustomEnemyTile;
}
public Tile getEnemyTile()
{
    return EnemyTile;
}

public void setTileTeamOwner(ETileTeam team)
{
    tileOwnerTeam = team;
}


public void switchToTileTeam(ETileTeam team)
{   

    if(getPlayerTile() == null || getEnemyTile() == null)
    {
        Debug.Log("Error: player or enemy tile variant not set");
        return;
    }

    if(team == ETileTeam.Player)
    {
        CurrentTile = getPlayerTile();
    }else
    {
        CurrentTile = getEnemyTile();
    }
}

public IEnumerator revertToTileOwner(float duration)
{
    if(tileTeam != tileOwnerTeam)
    {
        yield return new WaitForSeconds(duration);
        switchToTileTeam(tileOwnerTeam);
    }

    yield break;
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
