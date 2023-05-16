using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using static MenuOption;

public class TabGroup : MonoBehaviour
{
    public UIScriptableObject menu;
    public GameObject tabButtonObj;
    public GameObject headerText;
    public GameObject contentText;
    public GameObject controlsText;

    public List<GameObject> tabMenuHeaders;
    public List<GameObject> contentOptions;
    
    public List<TabButton> tabButtons;
    public SettingsValues settings;

    public TabButton selectedTab;
    public Transform panelLocation;

    public void Awake()
    {
        for (int i = 0; i < menu.menuOptions.Count; i++)
        {
            GameObject button = Instantiate(tabButtonObj, this.transform);
            button.GetComponent<TabButton>().tabText.text = menu.menuOptions[i].tabOption;
        }

        settings = this.gameObject.GetComponent<SettingsValues>();
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

        SpawnMenuContents(button);
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

        //clear contents of array
        tabMenuHeaders.Clear();
        contentOptions.Clear();
    }

    public void SpawnMenuContents(TabButton button)
    {
        //spawn menu options for each the individual tab selected
        for (int i = 0; i < menu.menuOptions.Count; i++)
        {
            if (button.GetComponent<TabButton>().tabText.text == menu.menuOptions[i].tabOption)
            {
                for (int j = 0; j < menu.menuOptions[i].panelCreation.Count; j++)
                {
                    GameObject header = Instantiate(headerText, panelLocation);
                    tabMenuHeaders.Add(header);
                    header.GetComponent<TextMeshProUGUI>().text = menu.menuOptions[i].panelCreation[j].header;

                    if (header.GetComponent<TextMeshProUGUI>().text == menu.menuOptions[i].panelCreation[j].header)
                    {
                        for (int k = 0; k < menu.menuOptions[i].panelCreation[j].panelContent.Count; k++)
                        {
                            GameObject content = Instantiate(contentText, panelLocation);
                            contentOptions.Add(content);
                            GameObject contentTextField = content.transform.GetChild(0).gameObject;
                            contentTextField.GetComponent<TextMeshProUGUI>().text = menu.menuOptions[i].panelCreation[j].panelContent[k].contentText;

                            if (menu.menuOptions[i].menuType.ToString() == "Settings")
                            {
                                DisplaySettingsUIElement(menu.menuOptions[i].panelCreation[j].panelContent[k], content);
                            }

                            if (menu.menuOptions[i].menuType.ToString() == "Controls")
                            {
                                DisplayControlsUIElement(menu.menuOptions[i].panelCreation[j].panelContent[k], content);
                            }
                        }
                    }
                }
            }
        }
    }

    public void DisplaySettingsUIElement(ContentField uiElement, GameObject content)
    {
        GameObject contentSlider = content.transform.GetChild(1).gameObject;
        GameObject contentDropDown = content.transform.GetChild(2).gameObject;
        GameObject contentToggle = content.transform.GetChild(3).gameObject;

        if (uiElement.slider == true) contentSlider.gameObject.SetActive(true);
        else contentSlider.gameObject.SetActive(false);
        if (uiElement.dropDownMenu == true) contentDropDown.gameObject.SetActive(true);
        else contentDropDown.gameObject.SetActive(false);
        if (uiElement.toggle == true) contentToggle.gameObject.SetActive(true);
        else contentToggle.gameObject.SetActive(false);

        if (uiElement.contentText == "Volume") settings.volumeSlider = contentSlider.gameObject.GetComponent<Slider>();
    }

    public void DisplayControlsUIElement(ContentField uiElement, GameObject content)
    {
        GameObject controlText = content.transform.GetChild(4).gameObject;

        controlText.gameObject.SetActive(true);

        controlText.GetComponent<TextMeshProUGUI>().text = uiElement.controlKey;
    }
}

    
