using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static LTDescr delay;
    public string rarity;
    [Multiline()]
    public string weaponType;
    [Multiline()]
    public List<string> weaponStat;
    [Multiline()]
    public List<float> statValue;

    public void OnPointerEnter(PointerEventData eventData)
    { 
        delay = LeanTween.delayedCall(0.5f, () =>
        {
            TooltipSystem.Show(weaponStat, statValue, weaponType, rarity);
        });
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }
}
