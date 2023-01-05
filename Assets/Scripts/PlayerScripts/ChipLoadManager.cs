using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipLoadManager : MonoBehaviour
{

public delegate void LoadChipsEvent();
public event LoadChipsEvent loadChipsEvent;

[SerializeField] public List<ChipObjectReference> nextChipRefLoad = new List<ChipObjectReference>();


[SerializeField] public List<ChipObjectReference> chipRefQueue = new List<ChipObjectReference>();


List<ChipObjectReference> chipRefsToRemove = new List<ChipObjectReference>();



PlayerMovement player;


private static ChipLoadManager _instance;
public static ChipLoadManager Instance {get {return _instance;} }
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


    private void Awake() 
    {    
        InitializeSingleton();
    }

    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        
    }

    public void calcNextChipRefLoad()
    {
        if(chipRefQueue.Count == 0)
        {
            print("ChipRef Qeue Empty " + "Class: ChipLoadManager");
            return;
        }

        nextChipRefLoad.Add(chipRefQueue[0]);
        chipRefQueue.RemoveAt(0);

        foreach(ChipObjectReference chipRef in chipRefQueue)
        {
            if(chipRef.chipSORef.GetChipType() == EChipTypes.Passive)
            {
                nextChipRefLoad.Add(chipRef);
                chipRefsToRemove.Add(chipRef);

            }else
            {
                break;
            }
        }



        foreach(ChipObjectReference chipRef in chipRefsToRemove)
        {
            chipRefQueue.Remove(chipRef);
        }

        if(loadChipsEvent != null)
        {print("Attempted loadChipsEvent");
            loadChipsEvent();} 


    }


}
