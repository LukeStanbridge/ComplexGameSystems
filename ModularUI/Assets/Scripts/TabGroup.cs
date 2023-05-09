using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public PanelGroup panelGroup;
    public MenuNavigation option;
    public int panelNumber;

    // Adds tabs to list
    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }
    // Dispaly panel
    public void OnTabEnter(TabButton button)
    {
        if (panelGroup.name == "Panels" && !RetainMenu())
        {
            if (panelGroup != null)
            {
                panelGroup.SetPageIndex(tabButtons.IndexOf(button));
            }
        }
    }

    // Remove panel when not hovering over tab
    public void OnTabExit(TabButton button)
    {
        if (panelGroup.name == "Panels" && !RetainMenu())
        {
            if (panelGroup != null)
            {
                panelGroup.SetPageIndex(0);
            }
        }
    }

    // Displays body panels
    public void OnTabSelected(TabButton button)
    {
        if (panelGroup != null)
        {
            // shows the menu panels section
            if (panelGroup.name == "Panels")
            {
                option.MainMenuOption(button.name);
                panelGroup.SetPageIndex(panelNumber);
            }
            // controls thh panel display for the to tab bar
            else
            {
                panelGroup.SetPageIndex(tabButtons.IndexOf(button));
            }
        }
    }

    // Used to reset tab/button panels when opening and cloasing pause menu
    public void OnClosePauseMenu()
    {
        if (panelGroup != null)
        {
            if (panelGroup.name == "Panels")
            {
                panelGroup.SetPageIndex(0);
            }
        }
    }

    // Keeps certain panels displayed when tab is selected
    public bool RetainMenu()
    {
        if (panelNumber == 6 || panelNumber == 7 || panelNumber == 8) return true;
        else return false;
    }
}
