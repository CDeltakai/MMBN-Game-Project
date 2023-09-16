using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class GenericProjectile : MonoBehaviour
{
    public CardEffect parentEffectPrefab;
    public AttackPayload attackPayload;
    public int pierceCount = 0;
    public Team team; //The team of a projectile dictates what kinds of entities it can hit


    public PlayerMovement player;
    BoxCollider2D boxCollider2D;
    Rigidbody2D rigbody;

    [SerializeField] SpriteRenderer sprite;//Needs to be set in inspector
    [SerializeField] Animator animator;//May or may not be set in inspector
    [SerializeField] TrailRenderer trail;//May or may not be set in inspector

    [SerializeField] public Vector2 velocity = new Vector2(25, 0);

    protected virtual void InitializeAwakeVariables()
    {

    }

    protected virtual void Awake()
    {
        rigbody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        InitializeAwakeVariables();
 
    }

    protected virtual void InitializeStartingStates()
    {

    }

    void Start()
    {
        InitializeStartingStates();


    }





    protected virtual void OnCollisionEnter2D(Collision2D other) 
    {


    }





    protected void DestroyProjectile()
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
