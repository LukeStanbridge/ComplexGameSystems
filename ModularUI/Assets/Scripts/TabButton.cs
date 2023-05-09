using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public float tweenTime;
    public float tweenSize;
    RectTransform rectTransform;

    // Initiates button control when dragging mouse over tab or selecting tab 
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
        Tween();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
        Tween();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        tabGroup.Subscribe(this);
        tweenTime = 0.5f;
        tweenSize = 1.3f;
        rectTransform = GetComponent<RectTransform>();
    }

    public void Tween()
    {
        if (gameObject.transform.parent.name == "Tabs") rectTransform.SetAsLastSibling(); // pop tab to front if parent has no flexible grid layout script

        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.one * tweenSize, tweenTime).setEasePunch().setIgnoreTimeScale(true);

    }
}
