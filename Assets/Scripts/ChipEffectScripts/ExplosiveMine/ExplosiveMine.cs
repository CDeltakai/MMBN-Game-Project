using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : MonoBehaviour
{
    [SerializeField] GameObject MineObject;
    [SerializeField] GameObject PrimaryExplosion;

    [SerializeField] List<GameObject> SecondaryExplosions;

    Animator MineObjectAnimator;
    Animator SecondaryExplosionAnimator;

    SpriteRenderer MineObjectSprRenderer;
    SpriteRenderer SecondaryExplosionSprRenderer;

    BoxCollider2D MineObjectBoxCollider2D;
    BoxCollider2D SecondaryExplosionBoxCollider2D;

    private void Awake() 
    {
        MineObjectAnimator = MineObject.GetComponent<Animator>();
        MineObjectSprRenderer = MineObject.GetComponent<SpriteRenderer>();
        MineObjectBoxCollider2D = MineObject.GetComponent<BoxCollider2D>();


        SecondaryExplosionAnimator = PrimaryExplosion.GetComponent<Animator>();
        SecondaryExplosionSprRenderer = PrimaryExplosion.GetComponent<SpriteRenderer>();
        SecondaryExplosionBoxCollider2D = PrimaryExplosion.GetComponent<BoxCollider2D>();

        


    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
