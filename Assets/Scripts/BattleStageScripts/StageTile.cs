using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageTile
{


    internal Vector3Int localCoords{get;set;}
    internal Vector3 worldPosition{get;set;}
    internal CustomTile custTile {get;set;}

    internal ETileTeam tileTeam{get;set;}

    internal string tileName{get;set;}

    //TilemapMember property that defines which Tilemap the tile belongs to
    internal Tilemap TilemapMember{get;set;}
    internal bool isOccupied{get;set;} = false;
    internal BStageEntity entity{get;set;}


}
