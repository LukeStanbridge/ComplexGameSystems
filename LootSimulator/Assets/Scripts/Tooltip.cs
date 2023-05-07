using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    
    public TextMeshProUGUI rarityField;
    public TextMeshProUGUI weaponTypeField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI[] statTextField;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(List<string> weaponStat, List<float> statValue, string content, string header = "")
    {
        for (int i = 0; i < statTextField.Length; i++)
        {
            statTextField[i].text = weaponStat[i] + ": " + statValue[i].ToString();
        }
        if (string.IsNullOrEmpty(header))
        {
            rarityField.gameObject.SetActive(false);
        }
        else
        {
            rarityField.gameObject.SetActive(true);
            rarityField.text = header;
        }

        weaponTypeField.text = content;

        //check if text is longer than char limit
        int headerLength = rarityField.text.Length;
        int contentLength = weaponTypeField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    private void Update()
    {
        if(Application.isEditor)
        { 
            //check if text is longer than char limit
            int headerLength = rarityField.text.Length;
            int contentLength = weaponTypeField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
        }

        //position tootip offset from mouse location
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = position;
    }
}
