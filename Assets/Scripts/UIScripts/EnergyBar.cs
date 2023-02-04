using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using TMPro;
public class EnergyBar : MonoBehaviour
{





    [SerializeField] PlayerAttributeSO PlayerAttributes;
    [SerializeField] TextMeshProUGUI EnergyText;
    PlayerMovement player;

    Image BarImage;

    public int MaxEnergy;
    public int currentEnergy;
    
    private void Awake()
    {
        BarImage = GetComponent<Image>();
        BarImage.fillAmount = 1f;

    }


    void Start()
    {
        if(PlayerMovement.Instance != null)
        {
            player = PlayerMovement.Instance;
        }else
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        MaxEnergy = PlayerAttributes.AdjustOrGetMaxEnergy();
        currentEnergy = player.currentEnergy;
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustEnergy(int amount)
    {
        float fillAdjustAmount = (float)amount/(float)MaxEnergy;
        
        BarImage.DOFillAmount(BarImage.fillAmount += fillAdjustAmount, 0.2f);

    }

}
