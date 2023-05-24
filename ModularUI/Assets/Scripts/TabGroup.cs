using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using UnityEngine.UIElements;
using static MenuOption;

public class TabGroup : MonoBehaviour
{
    public UIScriptableObject menu;

    public GameObject tabButtonObj;
    public GameObject dragAndDropLayoutObj;
    public GameObject menuPanelObj;
    public GameObject inventorySlotObj;
    public GameObject headerText;
    public GameObject contentText;

    public GameObject viewPanel;
    public List<GameObject> menuPanels;
    public List<TabButton> tabButtons;
    public TabButton selectedTab;

    public void Awake()
    {
        for (int i = 0; i < menu.menuOptions.Count; i++)
        {
            //Instantiate menu buttons
            GameObject button = Instantiate(tabButtonObj, this.transform);
            button.GetComponent<TabButton>().tabText.text = menu.menuOptions[i].tabOption;

            //Instantiate menu panels
            CreateMenuPanels(menu.menuOptions[i]);
            
            //Spawn panel data in correct panel
            SpawnMenuContents(button.GetComponent<TabButton>(), menuPanels[i]);
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

        ShowMenuPanel(button);
    }

    public void ResetTabs()
    {
        //reset button colour
        foreach (TabButton button in tabButtons)
        {
            button.background.color = button.inactiveColour;
        }
    }

    public void SpawnMenuContents(TabButton button, GameObject menuPanel)
    {
        //spawn menu options for each the individual tab selected
        for (int i = 0; i < menu.menuOptions.Count; i++)
        {
            if (button.GetComponent<TabButton>().tabText.text == menu.menuOptions[i].tabOption)
            {
                if (menu.menuOptions[i].menuType.ToString() == "Inventory")
                {
                    DisplayInventoryUIElement(menu.menuOptions[i], menuPanel);
                }
                else if (menu.menuOptions[i].menuType.ToString() == "Settings")
                {
                    DisplaySettingsUIElement(menu.menuOptions[i], menuPanel);
                }
                else if (menu.menuOptions[i].menuType.ToString() == "Controls")
                {
                    DisplayControlsUIElement(menu.menuOptions[i], menuPanel);
                }
            }
        }
    }

    //Switch between menu panels 
    public void ShowMenuPanel(TabButton button)
    {
        for (int i = 0; i < tabButtons.Count; i++)
        {
            if (button == tabButtons[i])
            {
                menuPanels[i].SetActive(true);
            }
            else menuPanels[i].SetActive(false);
        }
    }

    public void CreateMenuPanels(MenuOption menuOption)
    {
        if (menuOption.menuType.ToString() == "Inventory")
        {
            GameObject gridLayout = Instantiate(dragAndDropLayoutObj, viewPanel.transform);
            gridLayout.transform.parent = viewPanel.transform;
            gridLayout.name = menuOption.tabOption + " panel";
            gridLayout.GetComponent<ScrollRect>().viewport = viewPanel.GetComponent<RectTransform>();
            gridLayout.SetActive(false);
            menuPanels.Add(gridLayout);
        }
        else
        {
            GameObject panel = Instantiate(menuPanelObj, viewPanel.transform);
            panel.transform.parent = viewPanel.transform;
            panel.name = menuOption.tabOption + " panel";
            panel.GetComponent<ScrollRect>().viewport = viewPanel.GetComponent<RectTransform>();
            panel.SetActive(false);
            menuPanels.Add(panel);
        }
    }

    //Display settings
    public void DisplaySettingsUIElement(MenuOption menuOption, GameObject menuPanel)
    {
        GameObject content;
        
        for (int i = 0; i < menuOption.settingsPanelCreation.Count; i++)
        {
            GameObject header = Instantiate(headerText, menuPanel.transform);
            header.GetComponent<TextMeshProUGUI>().text = menuOption.settingsPanelCreation[i].header;
            
            if (header.GetComponent<TextMeshProUGUI>().text == menuOption.settingsPanelCreation[i].header)
            {
                for (int j = 0; j < menuOption.settingsPanelCreation[i].panelContent.Count; j++)
                {
                    SettingsContent uiElement = menuOption.settingsPanelCreation[i].panelContent[j];
                    content = Instantiate(contentText, menuPanel.transform);
                    GameObject contentTextField = content.transform.GetChild(0).gameObject;
                    contentTextField.GetComponent<TextMeshProUGUI>().text = menuOption.settingsPanelCreation[i].panelContent[j].settingsContentText;

                    GameObject contentSlider = content.transform.GetChild(1).gameObject;
                    GameObject contentDropDown = content.transform.GetChild(2).gameObject;
                    GameObject contentToggle = content.transform.GetChild(3).gameObject;

                    if (uiElement.slider == true) contentSlider.gameObject.SetActive(true);
                    else contentSlider.gameObject.SetActive(false);
                    if (uiElement.dropDownMenu == true) contentDropDown.gameObject.SetActive(true);
                    else contentDropDown.gameObject.SetActive(false);
                    if (uiElement.toggle == true) contentToggle.gameObject.SetActive(true);
                    else contentToggle.gameObject.SetActive(false);
                }
            }
        }
    }

    //Display Controls
    public void DisplayControlsUIElement(MenuOption menuOption, GameObject menuPanel)
    {
        GameObject content;
        
        for (int i = 0; i < menuOption.controlsPanelCreation.Count; i++)
        {
            GameObject header = Instantiate(headerText, menuPanel.transform);
            header.GetComponent<TextMeshProUGUI>().text = menuOption.controlsPanelCreation[i].header;

            if (header.GetComponent<TextMeshProUGUI>().text == menuOption.controlsPanelCreation[i].header)
            {
                for (int j = 0; j < menuOption.controlsPanelCreation[i].panelContent.Count; j++)
                {
                    ControlsContent uiElement = menuOption.controlsPanelCreation[i].panelContent[j];
                    content = Instantiate(contentText, menuPanel.transform);
                    GameObject contentTextField = content.transform.GetChild(0).gameObject;
                    contentTextField.GetComponent<TextMeshProUGUI>().text = menuOption.controlsPanelCreation[i].panelContent[j].controlsContentText;

                    GameObject controlText = content.transform.GetChild(4).gameObject;
                    controlText.gameObject.SetActive(true);
                    controlText.GetComponent<TextMeshProUGUI>().text = uiElement.controlKey;
                }
            }
        }
    }

    //Display Inventory
    public void DisplayInventoryUIElement(MenuOption menuOption, GameObject gridLayout)
    {
        for (int i = 0; i < menuOption.inventorySlots; i++)
        {
            GameObject inventorySlot = Instantiate(inventorySlotObj, gridLayout.transform);
            inventorySlot.transform.parent = gridLayout.transform;

            //spawn drag and drop items
            if (i < menuOption.starterItems.Count)
            {
                GameObject starterItem = Instantiate(menuOption.starterItems[i], inventorySlot.transform);
                starterItem.transform.parent = inventorySlot.transform;
            }
        }
    }
}

    
