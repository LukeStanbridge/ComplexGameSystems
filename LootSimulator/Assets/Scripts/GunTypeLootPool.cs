using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;

public class GunTypeLootPool : MonoBehaviour
{
    public TooltipTrigger weaponTypeField;
    
    private WeaponType weapon;
    [HideInInspector] public string weaponRarity;
    [SerializeField] private float rarityWeaponModifier = 1.1f;
    [SerializeField] private Stat[] weaponStatsArray; //add tooltip here

    [System.Serializable]
    public struct Stat
    {
        public string name;
        public float value;
        public bool rarityModifiable;
        public bool weaponTypeModifiable;
    }

    public enum WeaponType
    {
        [Description("Hand Cannon")]
        HandCannon,
        [Description("Scout Rifle")]
        ScoutRifle,
        [Description("Auto Rifle")]
        AutoRifle,
        [Description("Pulse Rifle")]
        PulseRifle,
        [Description("Sidearm")]
        Sidearm,
        [Description("Submachine Gun")]
        SMG,
        [Description("Combat Bow")]
        Bow,
    }

    private void Awake()
    {
        weaponTypeField.weaponType = "Weapon Type: " + GenerateRandomWeaponType(); // populate text field with weapon data once they spawn
    }

    private void Start()
    {
        //modify weapon stats according to rarity
        switch (weaponRarity)
        {
            case "Common":
                WeaponRarityStatModifications(rarityWeaponModifier);
                break;

            case "Uncommon":
                WeaponRarityStatModifications(rarityWeaponModifier);
                break;

            case "Rare":
                WeaponRarityStatModifications(rarityWeaponModifier);
                break;

            case "Legendary":
                WeaponRarityStatModifications(rarityWeaponModifier);
                break;

            case "Exotic":
                WeaponRarityStatModifications(rarityWeaponModifier);
                break;

            default:
                Debug.Log("nothing happening");
                break;
        }
    }

    //generate a random weapon type and store it using Description data in enum
    public string GenerateRandomWeaponType()
    {
        weapon = (WeaponType)UnityEngine.Random.Range(0, (float)Enum.GetValues(typeof(WeaponType)).Cast<WeaponType>().Max()+ 1);
        DescriptionAttribute attribute = weapon.GetType().GetField(weapon.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault() as DescriptionAttribute;
        return attribute == null ? weapon.ToString() : attribute.Description;
    }

    //adjust stats with rarity
    public void WeaponRarityStatModifications(float rarityModifier)
    {
        for(int i = 0; i < weaponStatsArray.Length; i++) 
        {
            //modify stat values
            if (weaponStatsArray[i].rarityModifiable)
            {
                weaponStatsArray[i].value = weaponStatsArray[i].value * rarityModifier;
            }

            //add name and value to tooltip text field
            weaponTypeField.weaponStat.Add(weaponStatsArray[i].name); 
            weaponTypeField.statValue.Add(weaponStatsArray[i].value);
        }
    }
}


