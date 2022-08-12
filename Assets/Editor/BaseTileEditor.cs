using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

[CustomEditor(typeof(BaseTile))]
public class BaseTileEditor : Editor 
{

    BaseTile baseTile;

    private void OnEnable() {
        baseTile = target as BaseTile;
    }


    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        if(baseTile.GetTile().sprite == null) return;

        

        Texture2D texture = AssetPreview.GetAssetPreview(baseTile.GetTile().sprite);

        
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(120));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        

    
        
    }



}
#endif



