using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

[CustomEditor(typeof(CustomTile))]
public class CustomTileEditor : Editor 
{

    CustomTile customTile;

    private void OnEnable() {
        customTile = target as CustomTile;
    }


    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        if(customTile.sprite == null) return;

        

        Texture2D texture = AssetPreview.GetAssetPreview(customTile.GetTile().sprite);

        
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(120));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        

    
        
    }



}
#endif



