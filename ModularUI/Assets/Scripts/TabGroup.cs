using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public GameObject tabButtonObj;
    public GameObject headerText;
    public GameObject contentText;

    public List<GameObject> tabMenuHeaders;
    public List<GameObject> contentOptions;

    public List<MenuOption> menuOptions;
    public List<TabButton> tabButtons;

    public TabButton selectedTab;
    public Transform panelLocation;

    public void Awake()
    {
        for (int i = 0; i < menuOptions.Count; i++)
        {
            GameObject button = Instantiate(tabButtonObj, this.transform);
            button.GetComponent<TabButton>().tabText.text = menuOptions[i].tabOption;
        }
    }

    // Adds tabs to list
    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        button.background.color = button.activeColour;
    }

    public void OnTabExit(TabButton button)
    {
        //update button colours on exiting button
        if (selectedTab == null && button != selectedTab)
        {
            button.background.color = button.inactiveColour;
        }

        if (selectedTab != null && button == selectedTab)
        {
            button.background.color = button.selectedColour;
        }

        if (selectedTab != null && button != selectedTab)
        {
            button.background.color = button.inactiveColour;
        }
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = button.selectedColour;

        //spawn menu options for each the individual tab selected
        for (int i = 0; i < menuOptions.Count; i++)
        {
            if (button.GetComponent<TabButton>().tabText.text == menuOptions[i].tabOption)
            {
                for (int j = 0; j < menuOptions[i].headers.Count; j++)
                {
                    GameObject header = Instantiate(headerText, panelLocation);
                    tabMenuHeaders.Add(header);
                    header.GetComponent<TextMeshProUGUI>().text = menuOptions[i].headers[j].header;

                    if (header.GetComponent<TextMeshProUGUI>().text == menuOptions[i].headers[j].header)
                    {
                        for (int k = 0; k < menuOptions[i].headers[j].content.Count; k++)
                        {
                            GameObject content = Instantiate(contentText, panelLocation);
                            contentOptions.Add(content);
                            GameObject contentTextField = content.transform.GetChild(0).gameObject;
                            contentTextField.GetComponent<TextMeshProUGUI>().text = menuOptions[i].headers[j].content[k].contentText;

                            GameObject contentSlider = content.transform.GetChild(1).gameObject;
                            GameObject contentDropDown = content.transform.GetChild(2).gameObject;
                            GameObject contentToggle = content.transform.GetChild(3).gameObject;
                            
                            if (menuOptions[i].headers[j].content[k].slider == true) contentSlider.gameObject.SetActive(true);
                            else contentSlider.gameObject.SetActive(false);
                            if (menuOptions[i].headers[j].content[k].dropDownMenu == true) contentDropDown.gameObject.SetActive(true);
                            else contentDropDown.gameObject.SetActive(false);
                            if (menuOptions[i].headers[j].content[k].toggle == true) contentToggle.gameObject.SetActive(true);
                            else contentToggle.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    public void ResetTabs()
    {
        //reset button colour
        foreach (TabButton button in tabButtons)
        {
            button.background.color = button.inactiveColour;
        }

        //reset menu options in panel
        foreach (GameObject header in tabMenuHeaders)
        {
            Destroy(header);
        }

        foreach (GameObject content in contentOptions)
        {
            Destroy(content);
        }

        tabMenuHeaders.Clear();
        contentOptions.Clear();
    }
}

[System.Serializable]
public struct MenuOption
{
    public string tabOption;
    public List<HeaderAndContent> headers;
}

[System.Serializable]
public struct HeaderAndContent
{
    public string header;
    public List<ContentField> content;
}

[System.Serializable]
public struct ContentField
{
    public string contentText;
    public bool slider;
    public bool dropDownMenu;
    public bool toggle;
}

    
