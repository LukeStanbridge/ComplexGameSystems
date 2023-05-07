using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot System", menuName = "Weapon Rarity")]
public class RarityLootPool : ScriptableObject
{
    public DropItem[] GuaranteedLootTable = new DropItem[0];
    public DropItem[] OneItemFromList = new DropItem[1];
    public float WeightToNoDrop = 100;
    public GunTypeLootPool item;

    // Return List of Guaranteed Drop 
    public List<GameObject> GetGuaranteeedLoot()
    {
        List<GameObject> lootList = new List<GameObject>();

        for (int i = 0; i < GuaranteedLootTable.Length; i++)
        {
            // Adds the drawn number of items to drop
            int count = Random.Range(GuaranteedLootTable[i].MinCountItem, GuaranteedLootTable[i].MaxCountItem);
            for (int j = 0; j < count; j++)
            {
                lootList.Add(GuaranteedLootTable[i].Drop);
            }
        }

        return lootList;
    }

    // Return List of Optional Drop 
    public List<GameObject> GetRandomLoot(int ChangeCount)
    {
        List<GameObject> lootList = new List<GameObject>();
        float totalWeight = WeightToNoDrop;

        // Executes a function a specified number of times
        for (int j = 0; j < ChangeCount; j++)
        {
            // They add up the entire weight of the items
            for (int i = 0; i < OneItemFromList.Length; i++)
            {
                totalWeight += OneItemFromList[i].Weight;
            }

            float value = Random.Range(0, totalWeight);
            float timed_value = 0;

            for (int i = 0; i < OneItemFromList.Length; i++)
            {
                // If timed_value is greater than value, it means this item has been drawn
                timed_value += OneItemFromList[i].Weight;
                if (timed_value > value)
                {
                    int count = Random.Range(OneItemFromList[i].MinCountItem, OneItemFromList[i].MaxCountItem + 1);
                    for (int c = 0; c < count; c++)
                    {
                        lootList.Add(OneItemFromList[i].Drop);
                    }
                    break;
                }
            }
        }

        return lootList;
    }



    public void SpawnDrop(Transform _position, int _count, float _range)
    {
        
        List<GameObject> guaranteed = GetGuaranteeedLoot();
        List<GameObject> randomLoot = GetRandomLoot(_count);

        for (int i = 0; i < guaranteed.Count; i++)
        {
            Instantiate(guaranteed[i], new Vector3(_position.position.x + Random.Range(-_range, _range), _position.position.y, _position.position.z + Random.Range(-_range, _range)), Quaternion.identity);
            item = FindObjectOfType<GunTypeLootPool>();
            item.weaponRarity =  guaranteed[i].name;
            item.weaponTypeField.rarity = guaranteed[i].name;
        }

        for (int i = 0; i < randomLoot.Count; i++)
        {
            Instantiate(randomLoot[i], new Vector3(_position.position.x + Random.Range(-_range, _range), _position.position.y, _position.position.z + Random.Range(-_range, _range)), Quaternion.identity);
            item = FindObjectOfType<GunTypeLootPool>();
            item.weaponRarity = randomLoot[i].name;
            item.weaponTypeField.rarity = randomLoot[i].name;
        }
    }
}

[System.Serializable]
public class DropItem
{
    public float Weight;
    public GameObject Drop;
    public int MinCountItem;
    public int MaxCountItem;
}
