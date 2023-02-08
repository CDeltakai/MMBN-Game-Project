using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


///<summary>
///The ChipInventoryReference is defined in the Player Attributes Scriptable Object.
///It is essentially a counter for the number of a 
///specific chip the player has. This struct is used when pooling chip objects
///in order to handle duplicate chips.
///</summary>
[System.Serializable]
public class ChipInventoryReference
{
    
    public string chipName;
    public ChipSO chip;
    public int chipCount;

    public ChipInventoryReference(ChipSO chip, int chipCount)
    {
        this.chip = chip;
        this.chipCount = chipCount;
        this.chipName = chip.GetChipName();
    }
}


//Wrapper class for List<ChipInventoryReference to allow for serialization of a list of list of ChipInventoryReferences.
[System.Serializable]
public class DeckLoadout
{
    public string DeckName;
    public List<ChipInventoryReference> Deck;
}

[CreateAssetMenu(fileName = "Player Attributes Data", menuName = "New Player Attributes Data", order = 0)]
public class PlayerAttributeSO : ScriptableObject
{

    int MinimumHP = 100;
    ///<summary>
    ///BaseHP is the HP of the player with only HP upgrades and no
    ///modifiers.
    ///</summary>
    [field:SerializeField] public int BaseHP{get; private set;} = 100;
    [field:SerializeField] public int CurrentMaxHP{get; set;} = 100;
    [SerializeField] int NumberOfHPUpgrades = 0;

    
    [field:SerializeField] public int BaseShieldHP{get; private set;} = 0;
    [SerializeField] int MaxShieldHP = 50;
    
    [field:SerializeField] public int BaseEnergy{get; private set;} = 5;
    [SerializeField] int MaxEnergy = 5;
    [field:SerializeField] float EnergyRegenRate{get; set;} = 1f;

    [SerializeField] int BasicShotDamage = 1;
    [field:SerializeField] public float MinimumChargeSpeed{get; private set;} = 0.75f;
    [SerializeField] float ChargeSpeed = 2.5f;


    [SerializeField] float GlobalOffenseMultiplier = 1;
    [SerializeField] float GlobalDefenseMultiplier = 1;

    int MinimumSelectableChips = 1;
    int HardMaxSelectableChips = 10;
    int MinActiveChips = 1;
    int HardMaxActiveChips = 7;
    [SerializeField] public int MaxSelectableChips = 5;
    [SerializeField] int MaxActiveChips = 5;

    [SerializeField] bool HasSuperArmor = false;
    [SerializeField] bool IsGrounded = true;
    [SerializeField] bool IsStunnable = true;

    int MinDeckCapacity = 15;
    int MaxDeckCapacity = 100;
    [SerializeField] int BaseDeckCapacity = 30;
    [SerializeField] int CurrentDeckCapacity = 30;

    int MinDeckLoadouts = 1;
    int MaxDeckLoadouts = 10;
    [SerializeField] int CurrentDeckLoadouts = 2;
    [SerializeField] public List<ChipInventoryReference> CurrentChipDeck = new List<ChipInventoryReference>();
    [SerializeField] public List<ChipInventoryReference> CurrentChipInventory = new List<ChipInventoryReference>();
    [SerializeField] public List<DeckLoadout> ChipDeckLoadouts = new List<DeckLoadout>();


    public void SaveToJson()
    {
        string saveData = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/playerSaveData.json", saveData);
        Debug.Log("Attempted saving player attributes to JSON" + Application.persistentDataPath);
    }


    public List<ChipInventoryReference> GetCurrentChipDeck()
    {
        return CurrentChipDeck;
        
    }


    public int GetBaseHP()
    {
        
        return BaseHP;
    }

    public int AddBaseHP(int value)
    {
        BaseHP += value;
        if(BaseHP < MinimumHP)
        {
            BaseHP = MinimumHP;
            Debug.LogWarning("Cannot set BaseHP below minimum HP of " + MinimumHP + ", defaulted to " + MinimumHP);
        }
        return BaseHP;
    }

    public int GetBaseShieldHP()
    {
        return BaseShieldHP;
    }

    public int GetBaseEnergy()
    {
        return BaseEnergy;
    }


    ///<summary>
    ///Use this method to adjust MaxHP by the given value (additive). Will always return the MaxHP.
    ///If no value is given or value is 0  then no adjustment will be made. You cannot reduce MaxHP
    ///below the BaseHP.
    ///</summary>
    public int AdjustOrGetCurrentMaxHP(int value = 0)
    {
        if(value != 0)
        {
            CurrentMaxHP += value;
            if(CurrentMaxHP < BaseHP)
            {
                CurrentMaxHP = BaseHP;
            }
        }
        return CurrentMaxHP;
    }

    public int AdjustOrGetHPUpgrades(int value = 0)
    {
        if(value != 0)
        {
            NumberOfHPUpgrades += value;
            if(NumberOfHPUpgrades < 0)
            {
                NumberOfHPUpgrades = 0;
            }
        }
        return NumberOfHPUpgrades;
    }

    public int AdjustOrGetMaxShieldHP(int value = 0)
    {
        if(value != 0)
        {
            MaxShieldHP += value;
            if(MaxShieldHP < BaseShieldHP)
            {
                MaxShieldHP = BaseShieldHP;
            }
        }
        return MaxShieldHP;
    }

    public int AdjustOrGetMaxEnergy(int value = 0)
    {
        if(value != 0)
        {
            MaxEnergy += value;
            if(MaxEnergy < BaseEnergy)
            {
                MaxEnergy = BaseEnergy;
            }
        }
        return BaseEnergy;
    }

    public int AdjustOrGetBasicShotDamage(int value = 0)
    {
        if(value != 0)
        {
            BasicShotDamage += value;
            if(BasicShotDamage < 1)
            {
                BasicShotDamage = 1;
            }
        }
        return BasicShotDamage;
    }

    public float AdjustOrGetChargeSpeed(float value = 0)
    {
        if(value != 0)
        {
            ChargeSpeed += value;
            if(ChargeSpeed < MinimumChargeSpeed)
            {
                ChargeSpeed = MinimumChargeSpeed;
            }
        }
        return ChargeSpeed;
    }

    public float AdjustOrGetGlobalOffenseMult(float value = float.Epsilon)
    {
        if(value != float.Epsilon)
        {
            GlobalOffenseMultiplier += value;
            if(GlobalOffenseMultiplier < float.Epsilon)
            {
                GlobalOffenseMultiplier = float.Epsilon;
            }
        }
        return GlobalOffenseMultiplier;
    }

    public float AdjustOrGetGlobalDefenseMult(float value = float.Epsilon)
    {
        if(value != float.Epsilon)
        {
            GlobalDefenseMultiplier += value;
            if(GlobalDefenseMultiplier < float.Epsilon)
            {
                GlobalDefenseMultiplier = float.Epsilon;
            }
        }
        return GlobalDefenseMultiplier;
    }

    public int AdjustOrGetMaxSelectableChips(int value = 0)
    {
        if(value != 0)
        {
            MaxSelectableChips += value;
            if(MaxSelectableChips < MinimumSelectableChips)
            {
                MaxSelectableChips = MinimumSelectableChips;
            }else
            if(MaxSelectableChips > HardMaxSelectableChips)
            {
                MaxSelectableChips = HardMaxSelectableChips;
            }
        }
        return MaxSelectableChips;
    }

    public int AdjustOrGetMaxActiveChips(int value = 0)
    {
        if(value != 0)
        {
            MaxActiveChips += value;
            if(MaxActiveChips < MinActiveChips)
            {
                MaxActiveChips = MinActiveChips;
            }else
            if(MaxActiveChips > HardMaxActiveChips)
            {
                MaxActiveChips = HardMaxActiveChips;
            }
        }
        return MaxActiveChips;
    }

    public bool GetSuperArmor()
    {
        return HasSuperArmor;
    }
    public bool SetAndReturnSuperArmor(bool condition)
    {
        if(condition)
        {
            HasSuperArmor = true;
        }else
        {
            HasSuperArmor = false;
        }
        return HasSuperArmor;
    }

    public bool GetIsGrounded()
    {
        return IsGrounded;
    }
    public bool SetReturnIsGrounded(bool condition)
    {
        if(condition)
        {
            IsGrounded = true;
        }else
        {
            IsGrounded = false;
        }
        return IsGrounded;
    }

    public bool GetIsStunnable()
    {
        return IsStunnable;
    }
    public bool SetReturnIsStunnable(bool condition)
    {
        if(condition)
        {
            IsStunnable = true;
        }else
        {
            IsStunnable = false;
        }
        return IsStunnable;
    }

    public int AdjustOrGetCurrentDeckCapacity(int value = 0)
    {
        if(value != 0)
        {
            CurrentDeckCapacity += value;
            if(CurrentDeckCapacity < MinDeckCapacity)
            {
                CurrentDeckCapacity = MinDeckCapacity;
            }else
            if(CurrentDeckCapacity > MaxDeckCapacity)
            {
                CurrentDeckCapacity = MaxDeckCapacity;
            }
        }
        return CurrentDeckCapacity;
    }

    public int AdjustOrGetCurrentDeckLoadouts(int value = 0)
    {
        if(value != 0)
        {
            CurrentDeckLoadouts += value;
            if(CurrentDeckLoadouts < MinDeckLoadouts)
            {
                CurrentDeckCapacity = MinDeckCapacity;
            }else
            if(CurrentDeckLoadouts > MaxDeckLoadouts)
            {
                CurrentDeckLoadouts = MaxDeckLoadouts;
            }
        }
        return CurrentDeckLoadouts;
    }


}
