using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;

public class BasicBullet : MonoBehaviour
{

    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] SpriteRenderer sprite;

    Sequence movement = DOTween.Sequence();
    RaycastHit hitPosition;


    void Awake()
    {
        movement = DOTween.Sequence(this);
 
    }

    void Start()
    {
        StartCoroutine(MoveBullet());


    }


    IEnumerator MoveBullet()
    {
        
        movement.Append(transform.parent.transform.DOMoveX(15f, 0.2f));
        

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject.transform.parent.gameObject);

        yield break;

        // while(true)
        // {
        //     transform.parent.transform.DOMoveX(20f, 0.1f);
        //     yield return new WaitForSeconds(0.5f);
        //     transform.parent.transform.DOMoveX(3.2f, 0.1f).SetEase(Ease.Linear);
        //     yield return new WaitForSeconds(0.5f);


        // }


    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if(TimeManager.isCurrentlySlowedDown)
        // {

        // }
        print("projectile hit a target");

        StartCoroutine(DestroyBullet());



    }


    IEnumerator DestroyBullet()
    {

        //yield return new WaitForEndOfFrame();
        //yield return new WaitForEndOfFrame();
        int killedTweens = DOTween.Kill(this);
        print("killed tweens: "+ killedTweens);

        yield break;
        //gameObject.SetActive(false);
        //Destroy(gameObject.transform.parent.gameObject);

    }

    void Update()
    {
        
    }






}
