using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Longsword_slash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.tag == "Enemy")
        {
            
        }

    }

    IEnumerator SelfDestruct()
    {

    }


}
