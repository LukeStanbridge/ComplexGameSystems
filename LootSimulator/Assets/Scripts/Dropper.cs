using System.Collections;
using System.Collections.Generic;
using TinyScript;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    public LootDrops Loot;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Loot.SpawnDrop(this.transform, RandomDropCount, DropRange);
        }
    }
}
