using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip;

    public void Awake()
    {
        current = this;
    }

    public static void Show(List<string> weaponStats, List<float> statValue, string content, string header = "")
    {
        current.tooltip.SetText(weaponStats, statValue, content, header);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide() 
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
