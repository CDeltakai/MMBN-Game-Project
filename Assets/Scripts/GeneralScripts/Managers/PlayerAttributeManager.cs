using System.Linq;
using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerAttributeManager : MonoBehaviour
{

private static PlayerAttributeManager _instance;
public static PlayerAttributeManager Instance {get {return _instance;} }

[SerializeField] public PlayerAttributeSO CurrentPlayerAttributes;

public int PlayerMaxHP;
public int ShieldMaxHP;


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

    void Awake()
    {
        InitializeSingleton();
        //Debug_FillChipDeck();
        DontDestroyOnLoad(this.gameObject);
    }


    ///<summary>
    ///Debug method: will fill chip deck with all chips from ChipScriptableObjects folder and arbitrary chip count.
    ///</summary>
    private void Debug_FillChipDeck()
    {
        CurrentPlayerAttributes.CurrentChipDeck.Clear();
        ChipSO[] chipList = Resources.LoadAll<ChipSO>("ChipScriptableObjects");
        foreach(ChipSO chip in chipList)
        {
            CurrentPlayerAttributes.CurrentChipDeck.Add(new ChipInventoryReference(chip, 2));
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
