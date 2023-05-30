using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public List<string> panelNames;
    public TabButton selectedTab;
    public GetGameData gameData;

    private int buttonsArrayIndex;

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
        buttonsArrayIndex = 0;
    }

    private void Update()
    {
        if (selectedTab == null) OnTabSelected(tabButtons[buttonsArrayIndex]);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (buttonsArrayIndex != 0)
            {
                buttonsArrayIndex--;
                SelectTab(buttonsArrayIndex);
            }
            else if (buttonsArrayIndex == 0)
            {
                buttonsArrayIndex = tabButtons.Count - 1;
                SelectTab(buttonsArrayIndex);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (buttonsArrayIndex == tabButtons.Count - 1)
            {
                buttonsArrayIndex = 0;
                SelectTab(buttonsArrayIndex);
            }
            else if (buttonsArrayIndex != tabButtons.Count - 1)
            {
                buttonsArrayIndex++;
                SelectTab(buttonsArrayIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //save settings
            gameData.sliderValues.Clear();
            gameData.dropdownOptions.Clear();
            gameData.toggleValues.Clear();
            SaveSettings();
            gameData.SaveSettings();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            //load values
            gameData.LoadSettings();
            LoadSettings();
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
                if (menuPanels[i].activeSelf == false) //check if menupanel is already open
                {
                    buttonsArrayIndex = i;
                    menuPanels[i].transform.localScale = new Vector3(0, 0, 0);
                    menuPanels[i].SetActive(true);
                    LeanTween.scale(menuPanels[i], new Vector3(1f, 1f, 1f), 0.1f).setDelay(0.1f).setEase(LeanTweenType.easeInSine);
                }
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
            panelNames.Add(gridLayout.name);
            gridLayout.GetComponent<ScrollRect>().viewport = viewPanel.GetComponent<RectTransform>();
            gridLayout.SetActive(false);
            menuPanels.Add(gridLayout);
        }
        else
        {
            GameObject panel = Instantiate(menuPanelObj, viewPanel.transform);
            panel.transform.parent = viewPanel.transform;
            panel.name = menuOption.tabOption + " panel";
            panelNames.Add(panel.name);
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

    public void SelectTab(int index)
    {
        OnTabSelected(tabButtons[index]);
        tabButtons[index].Tween();
        tabButtons[index].source.PlayOneShot(tabButtons[index].clip);
    }

    public void SaveDragAndDropMenus()
    {
        //search for inventory slot
        //check if child in game object(item)
        //save transform of parent object
        //respawn item at inventory slot with same transform.
    }

    public void SaveSettings()
    {
        List<GameObject> contentPanels = new List<GameObject>();
        for (int i = 0; i < menuPanels.Count; i++)
        {
            if (panelNames[i] != null)
            {
                if (menuPanels[i].name == panelNames[i])
                {
                    for (int j = 0; j < menuPanels[i].transform.childCount; j++)
                    {
                        if (menuPanels[i].transform.GetChild(j).gameObject.name == "Content(Clone)")
                        {
                            contentPanels.Add(menuPanels[i].transform.GetChild(j).gameObject);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < contentPanels.Count; i++)
        {
            for (int j = 0; j < contentPanels[i].transform.childCount; j++)
            {
                if (contentPanels[i].transform.GetChild(j).gameObject.name == "Slider")
                {
                    GameObject slider = contentPanels[i].transform.Find("Slider").gameObject;
                    if (slider.activeSelf == true)
                    {
                        gameData.sliderValues.Add(slider.GetComponent<Slider>().value);
                    }
                }

                if (contentPanels[i].transform.GetChild(j).gameObject.name == "Dropdown")
                {
                    GameObject dropdown = contentPanels[i].transform.Find("Dropdown").gameObject;
                    if (dropdown.activeSelf == true)
                    {
                        gameData.dropdownOptions.Add(dropdown.GetComponent<TMPro.TMP_Dropdown>().value);
                    }
                }

                if (contentPanels[i].transform.GetChild(j).gameObject.name == "Toggle")
                {
                    GameObject toggle = contentPanels[i].transform.Find("Toggle").gameObject;
                    if (toggle.activeSelf == true)
                    {
                        gameData.toggleValues.Add(toggle.GetComponent<Toggle>().isOn);
                    }
                }
            }
        }
    }

    public void LoadSettings()
    {
        List<GameObject> contentPanels = new List<GameObject>();
        for (int i = 0; i < menuPanels.Count; i++)
        {
            if (panelNames[i] != null)
            {
                if (menuPanels[i].name == panelNames[i])
                {
                    for (int j = 0; j < menuPanels[i].transform.childCount; j++)
                    {
                        if (menuPanels[i].transform.GetChild(j).gameObject.name == "Content(Clone)")
                        {
                            contentPanels.Add(menuPanels[i].transform.GetChild(j).gameObject);

                        }
                    }
                }
            }
        }

        List<GameObject> sliders = new List<GameObject>();
        List<GameObject> toggles = new List<GameObject>();
        List<GameObject> dropdowns = new List<GameObject>();
        
        for (int i = 0; i < contentPanels.Count; i++)
        {
            
            for (int j = 0; j < contentPanels[i].transform.childCount; j++)
            {
                if (contentPanels[i].transform.GetChild(j).gameObject.name == "Slider")
                {
                    GameObject slider = contentPanels[i].transform.Find("Slider").gameObject;
                    if (slider.activeSelf == true)
                    {
                        sliders.Add(slider);
                    }
                }

                if (contentPanels[i].transform.GetChild(j).gameObject.name == "Dropdown")
                {
                    GameObject dropdown = contentPanels[i].transform.Find("Dropdown").gameObject;
                    if (dropdown.activeSelf == true)
                    {
                        dropdowns.Add(dropdown);
                    }
                }

                if (contentPanels[i].transform.GetChild(j).gameObject.name == "Toggle")
                {
                    GameObject toggle = contentPanels[i].transform.Find("Toggle").gameObject;
                    if (toggle.activeSelf == true)
                    {
                        toggles.Add(toggle);
                    }
                }
            }
        }

        for (int i = 0; i < sliders.Count; i++)
        {
            sliders[i].GetComponent<Slider>().value = gameData.sliderValues[i];
        }

        for (int i = 0; i < toggles.Count; i++)
        {
            toggles[i].GetComponent<Toggle>().isOn = gameData.toggleValues[i];
        }

        for (int i = 0; i < dropdowns.Count; i++)
        {
            dropdowns[i].GetComponent<TMPro.TMP_Dropdown>().value = gameData.dropdownOptions[i];
        }
    }
}

    
