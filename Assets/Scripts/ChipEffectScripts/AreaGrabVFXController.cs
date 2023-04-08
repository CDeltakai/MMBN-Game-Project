using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AreaGrabVFXController : MonoBehaviour
{

    [SerializeField] GameObject[] AreaGrabEffects;
    [SerializeField] GameObject SpriteGroup;
    [SerializeField] float duration = 1;
    public Vector3 InitialPosition = new Vector3(6.4f, 10f, 0);

    void Awake()
    {
        gameObject.SetActive(false);
    }


    private void OnEnable() 
    {
        transform.position = InitialPosition;        
    }

    public void TriggerVFX() 
    {
        StartCoroutine(InitializeVFX());        
    }

    IEnumerator InitializeVFX()
    {
        //yield return new WaitForSeconds(2);
        transform.DOMoveY(3f, duration).SetUpdate(true).SetEase(Ease.Linear);

        yield return new WaitForSeconds(duration);
        SpriteGroup.transform.DOLocalMoveY(-0.5f, 0.1f);

        foreach(GameObject aregrabvfx in AreaGrabEffects)
        {
            aregrabvfx.GetComponent<Animator>().Play("AreaGrabImpact");
        }



    }

    private void OnDisable() 
    {
        transform.position = InitialPosition;        
    }
    
}
