using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public GameObject tabButtonObj;
    public List<string> menuOptions;
    public List<TabButton> tabButtons;
    public TabButton selectedTab;

    public void Awake()
    {
        for (int i = 0; i < menuOptions.Count; i++)
        {
            GameObject button = Instantiate(tabButtonObj, this.transform);
           button.GetComponent<TabButton>().tabText.text = menuOptions[i];
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

    // Remove panel when not hovering over tab
    public void OnTabExit(TabButton button)
    {
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
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            button.background.color = button.inactiveColour;
        }
    }
}
