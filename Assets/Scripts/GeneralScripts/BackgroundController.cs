using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundController : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void FadeToBlack(float duration)
    {

        spriteRenderer.DOColor(Color.black, duration).SetUpdate(true);


    }

    public void FadeToNormal(float duration)
    {
        spriteRenderer.DOColor(new Color(0.7f, 0.7f, 0.7f, 1), duration).SetUpdate(true);

    }

    
}
