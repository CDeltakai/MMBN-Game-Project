using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageTile
{


    public Vector3Int localCoords{get;set;}
    public Vector3 worldPosition{get;set;}
    public TileBase tileBase {get;set;}

    public ETileTeam tileTeam{get;set;}

    public string tileName{get;set;}

    //TilemapMember property that defines which Tilemap the tile belongs to
    public Tilemap TilemapMember{get;set;}
    public bool isOccupied{get;set;} = false;


}
