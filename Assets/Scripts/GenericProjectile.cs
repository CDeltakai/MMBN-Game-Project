using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class GenericProjectile : MonoBehaviour
{
    public CardEffect parentCard;
    public AttackPayload attackPayload;
    public int pierceCount = 0;


    public PlayerMovement player;
    BoxCollider2D boxCollider2D;
    Rigidbody2D rigbody;

    [SerializeField] SpriteRenderer sprite;//Needs to be set in inspector
    [SerializeField] TrailRenderer trail;//May or may not be set in inspector

    [SerializeField] public Vector2 velocity = new Vector2(25, 0);


    void Awake()
    {
        rigbody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

 
    }

    void Start()
    {



    }





    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Obstacle")
        {
            BStageEntity target = other.gameObject.GetComponent<BStageEntity>();

            //parentChip.OnActivationEffect(target);            
            pierceCount--;
            if(pierceCount < 0)
            {
                DestroyObject();    
            }
        }

    }


    


    void DestroyObject()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);        
    }

    void Update()
    {

        if(TimeManager.isCurrentlySlowedDown)
        {
            rigbody.MovePosition(rigbody.position + velocity*0.5f * Time.deltaTime);
        }else
        {
            rigbody.MovePosition(rigbody.position + velocity * Time.deltaTime);
        }


    }
}
