using System.Collections;
using System.Collections.Generic;
using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(InspectorArrayLayout))]
public class CustomizerBlockSizeDrawer : PropertyDrawer
{
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        EditorGUI.PrefixLabel(position, label);
        Rect newPosition = position;

        newPosition.y += 18f;

        SerializedProperty rows = property.FindPropertyRelative("rows");

        for(int j = 0; j < InspectorArrayLayout.rowSize; j++)
        {
            SerializedProperty row = rows.GetArrayElementAtIndex(j).FindPropertyRelative("row");
            newPosition.height = 18f;

            if(row.arraySize != InspectorArrayLayout.rowSize)
            {
                row.arraySize = InspectorArrayLayout.rowSize;
            }

            newPosition.width = (position.width/InspectorArrayLayout.rowSize)/2;

            for(int i = 0; i < InspectorArrayLayout.rowSize; i++)
            {
                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(i), GUIContent.none);
                newPosition.x += newPosition.width;    
            }

            newPosition.x = position.x;
            newPosition.y += 18f;
        }

    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 18 * 10f;
    }


}
#endif