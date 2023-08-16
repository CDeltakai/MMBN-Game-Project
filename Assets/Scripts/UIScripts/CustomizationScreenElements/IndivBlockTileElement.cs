using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IndivBlockTileElement : MonoBehaviour
{
    public Sprite tileSprite;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = tileSprite;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
