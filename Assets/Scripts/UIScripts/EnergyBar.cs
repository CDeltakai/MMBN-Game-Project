using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using TMPro;
public class EnergyBar : MonoBehaviour
{

    private static EnergyBar _instance;
    public static EnergyBar Instance {get {return _instance;} }
    private void InitializeSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
        }else
        {
            _instance = this;
        }
    }


    [SerializeField] PlayerAttributeSO PlayerAttributes;
    [SerializeField] TextMeshProUGUI EnergyText;
    PlayerMovement player;

    Image BarImage;

    public int MaxEnergy;
    public int currentEnergy;
    
    private void Awake()
    {
        InitializeSingleton();
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
