using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Player Attributes Data", menuName = "New Player Attributes Data", order = 0)]
public class PlayerAttributeSO : ScriptableObject
{
    [SerializeField] int BaseHP;
    [SerializeField] int MaxHP;
    [SerializeField] int NumberOfHPUpgrades;

    [SerializeField] int BaseShieldHP;
    [SerializeField] int MaxShieldHP;

    [SerializeField] int BasicShotDamage;
    [SerializeField] float ChargeSpeed;


    [SerializeField] float GlobalOffenseMultiplier;
    [SerializeField] float GlobalDefenseMultiplier;

    [SerializeField] int MaxSelectableChips;
    [SerializeField] int MaxActiveChips;

    [SerializeField] bool HasSuperArmor;
    [SerializeField] bool IsGrounded;
    [SerializeField] bool IsStunnable;

    [SerializeField] int BaseDeckCapacity;
    [SerializeField] int CurrentDeckCapacity;

    [SerializeField] int MaxDeckLoadouts;


}
