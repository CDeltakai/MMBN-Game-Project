using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChipID : IComparable
{

    public int ID;
    public string ChipName;
    public ChipSO chipSO;

    public ChipID(int id, string chipName, ChipSO chip)
    {
        this.ID = id;
        this.ChipName = chipName;
    }


    public int CompareTo(object incomingobject)
    {
      ChipID incomingChip = incomingobject as ChipID;
    
      return this.ID.CompareTo(incomingChip.ID);
    }
}




public class TupleList<T1, T2> : List<System.Tuple<T1, T2>> where T1 : IComparable
{

    public void Add(T1 item, T2 item2)
    {
        Add(new Tuple<T1, T2>(item, item2));
    }

    public new void Sort()
    {
        Comparison<Tuple<T1, T2>> c = (a, b) => a.Item1.CompareTo(b.Item1);
        base.Sort(c);
    }

}




public class PlayerAttributeManager : MonoBehaviour
{

private static PlayerAttributeManager _instance;
public static PlayerAttributeManager Instance {get {return _instance;} }


public int PlayerMaxHP;
public int ShieldMaxHP;
public List<ChipObjectReference> CurrentChipDeck;
public TupleList<Enum, ChipObjectReference> CurrentChipInventory;






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
        DontDestroyOnLoad(this.gameObject);
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
