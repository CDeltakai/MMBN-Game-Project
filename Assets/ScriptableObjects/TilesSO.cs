using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BStageTile Data", menuName = "New BStageTile", order = 0)]
public class TilesSO : ScriptableObject
{

[SerializeField] string TileName;
[SerializeField] Tile tile;
[SerializeField] String TileScriptName;


}
