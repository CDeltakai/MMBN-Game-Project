using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MettaurProjectile : MonoBehaviour
{

    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] Transform currentPosition;
    [SerializeField] int damage = 10;
    [SerializeField] float interval = 1f;
    float time = 0f;
    Mettaur mettaur;
    Rigidbody2D projectileBody;
    Rigidbody2D parentBody;
    PlayerMovement player;
    DamageFunctions damageFunctions;
    BoxCollider2D boxCollider2D;
    bool isTriggered = false;
    bool isMoving = false;
    void Start()
    {
        projectileBody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        parentBody = transform.GetComponentInParent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        
    }

    void Update()
    {

        //variable to give projectile a constant velocity
        //currentPosition.localPosition = new Vector3 (currentPosition.localPosition.x - projectileSpeed, currentPosition.localPosition.y, 0f);

        time += Time.deltaTime;
        while(time >= interval)
        {
            move();
            time -= interval;
        }

        if(currentPosition.position.x < 0)
        {
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }



    void move()
    {
        currentPosition.localPosition = new Vector2 (currentPosition.localPosition.x - 1.6f, currentPosition.localPosition.y);
    }



    void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.tag == "Obstacle")
        {
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
            return; 
        }

        if(other.tag == "Player" && !isTriggered)
        {
            player.hurtEntity(damage, false, true);
            player.healthText.text = player.playerHP.ToString();
            Debug.Log("Player damaged:" + player.playerHP.ToString());
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        //isTriggered = false;    
    }


}
