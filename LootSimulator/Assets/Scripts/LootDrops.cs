using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LootDrops : MonoBehaviour
{
    [System.Serializable]
    public class DropItem
    {
        public float Weight;
        public GameObject Drop;
        public int MinCountItem;
        public int MaxCountItem;
    }

    public DropItem[] DropItems = new DropItem[0];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDrop(Transform position)
    {
        for (int i = 0; i < DropItems.Length; i++)
        {
            Instantiate(randomLoot[i], position, Quaternion.identity);
        }
    }
}
