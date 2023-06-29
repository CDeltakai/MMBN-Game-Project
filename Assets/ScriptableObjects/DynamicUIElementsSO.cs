using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "Dynamic UI Elements", menuName = "New Dynamic UI Elements", order = 0)]
public class DynamicUIElementsSO : ScriptableObject
{

[Header("Chip Element Icons")]
[field:SerializeField] public Sprite Air; 
[field:SerializeField] public Sprite Blade; 
[field:SerializeField] public Sprite Breaking;
[field:SerializeField] public Sprite Buff;  
[field:SerializeField] public Sprite Cursor; 
[field:SerializeField] public Sprite Electric; 
[field:SerializeField] public Sprite Fire; 
[field:SerializeField] public Sprite Grass; 
[field:SerializeField] public Sprite Healing; 
[field:SerializeField] public Sprite Multiplier; 
[field:SerializeField] public Sprite Normal; 

public Sprite GetChipElementIcon(AttackElement element)
{
    switch (element) {
        case AttackElement.Air:
            if(Air == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Air;

        case AttackElement.Blade:
            if(Blade == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Blade;

        case AttackElement.Breaking:
            if(Breaking == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Breaking;

        case AttackElement.Buff:
            if(Blade == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Buff;

        case AttackElement.Cursor:
            if(Cursor == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Cursor;

        case AttackElement.Electric:
            if(Electric == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Electric;

        case AttackElement.Fire:
            if(Fire == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Fire;            

        case AttackElement.Grass:
            if(Grass == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Grass;

        case AttackElement.Multiplier:
            if(Multiplier == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Multiplier;

        case AttackElement.Normal:
            if(Normal == null)
            {
                Debug.LogWarning("Icon for element " + element.ToString() + " is undefined, returned Normal");
                return Normal;
            }
            return Normal;            

        default :
            Debug.LogWarning("Gave invalid attack element for icon, returned Normal");
            return Normal;
            
    }
}







}
