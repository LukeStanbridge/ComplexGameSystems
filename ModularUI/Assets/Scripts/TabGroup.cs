using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    [SerializeField] private int inventoryIndex;
    [SerializeField] private int settingIndex;

    public void Awake()
    {
        settingIndex = 1;
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
        

        //load values
        gameData.LoadSettings();
        LoadSettings();
        LoadDragAndDropMenu();
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
            gameData.itemSlotID.Clear();
            gameData.itemIDValue.Clear();
            SaveSettings();
            SaveDragAndDropMenus();
            gameData.SaveSettings();
        }

        //reset 
        if (Input.GetKeyDown(KeyCode.R))
        {
            menuPanels.Clear();
            tabButtons.Clear();
            panelNames.Clear();

            foreach (Transform child in this.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (Transform child in viewPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

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

            gameData.sliderValues.Clear();
            gameData.dropdownOptions.Clear();
            gameData.toggleValues.Clear();
            gameData.itemSlotID.Clear();
            gameData.itemIDValue.Clear();
            SaveSettings();
            SaveDragAndDropMenus();
            gameData.SaveSettings();
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

                    if (uiElement.slider == true)
                    {
                        contentSlider.gameObject.SetActive(true);
                        content.GetComponent<SettingID>().settingID = settingIndex;
                        settingIndex++;
                    }
                    else contentSlider.gameObject.SetActive(false);
                    if (uiElement.dropDownMenu == true)
                    {
                        contentDropDown.gameObject.SetActive(true);
                        content.GetComponent<SettingID>().settingID = settingIndex;
                        settingIndex++;
                    }
                    else contentDropDown.gameObject.SetActive(false);
                    if (uiElement.toggle == true)
                    {
                        contentToggle.gameObject.SetActive(true);
                        content.GetComponent<SettingID>().settingID = settingIndex;
                        settingIndex++;
                    }
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
            inventorySlot.GetComponent<InventorySlot>().inventorySlotID = inventoryIndex;
            inventoryIndex++;

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
        List<GameObject> inventoryPanels = new List<GameObject>();
        for (int i = 0; i < menuPanels.Count; i++)
        {
            if (panelNames[i] != null)
            {
                if (menuPanels[i].name == panelNames[i])
                {
                    for (int j = 0; j < menuPanels[i].transform.childCount; j++)
                    {
                        if (menuPanels[i].transform.GetChild(j).gameObject.name == "InventorySlot(Clone)")
                        {
                            inventoryPanels.Add(menuPanels[i].transform.GetChild(j).gameObject);
                        }
                    }
                }
            }
        }

        //check and save psoiton if child in game object(item)
        for (int i = 0;i < inventoryPanels.Count; i++)
        {
            if (inventoryPanels[i].transform.childCount > 0)
            {
                gameData.itemSlotID.Add(inventoryPanels[i].GetComponent<InventorySlot>().inventorySlotID);
                gameData.itemIDValue.Add(inventoryPanels[i].GetComponentInChildren<DraggableItem>().ID);
            }
        }
    }

    public void LoadDragAndDropMenu()
    {
        //respawn item at inventory slot with same transform.
        for (int i = 0; i < menuPanels.Count; i++)
        {
            if (panelNames[i] != null)
            {
                if (menuPanels[i].name == panelNames[i])
                {
                    //collect inventory slots for tab option
                    List<GameObject> inventoryPanels = new List<GameObject>();
                    for (int j = 0; j < menuPanels[i].transform.childCount; j++)
                    {
                        if (menuPanels[i].transform.GetChild(j).gameObject.name == "InventorySlot(Clone)")
                        {
                            inventoryPanels.Add(menuPanels[i].transform.GetChild(j).gameObject);
                        }
                    }

                    //Obtain items in list
                    List<GameObject> items = new List<GameObject>();
                    for (int k = 0; k < inventoryPanels.Count; k++)
                    {
                        if (inventoryPanels[k].transform.childCount > 0)
                        {
                            items.Add(inventoryPanels[k].transform.GetChild(0).gameObject);
                        }
                    }

                    //sort list
                    List<GameObject> sorted = new List<GameObject>();
                    for (int l = 0; l < gameData.itemIDValue.Count; l++)
                    {
                        foreach (GameObject item in items)
                        {
                            if (item.GetComponent<DraggableItem>().ID == gameData.itemIDValue[l])
                            {
                                sorted.Add(item);
                            }
                        }
                    }

                    //Redistribute items to correct location
                    for (int m = 0; m < inventoryPanels.Count; m++)
                    {
                        for (int n = 0; n < sorted.Count; n++)
                        {
                            if (inventoryPanels[m].GetComponent<InventorySlot>().inventorySlotID == gameData.itemSlotID[n])
                            {
                                sorted[n].transform.SetParent(inventoryPanels[m].transform);
                            }
                        }
                    }
                }
            }
        }
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

                    for (int k = 0; k < contentPanels.Count; k++)
                    {
                        for (int l = 0; l < contentPanels[k].transform.childCount; l++)
                        {
                            if (contentPanels[k].transform.GetChild(l).gameObject.name == "Slider")
                            {
                                GameObject slider = contentPanels[k].transform.Find("Slider").gameObject;
                                if (slider.activeSelf == true)
                                {
                                    gameData.sliderValues.Add(slider.GetComponent<Slider>().value);
                                }
                            }

                            if (contentPanels[k].transform.GetChild(l).gameObject.name == "Dropdown")
                            {
                                GameObject dropdown = contentPanels[k].transform.Find("Dropdown").gameObject;
                                if (dropdown.activeSelf == true)
                                {
                                    gameData.dropdownOptions.Add(dropdown.GetComponent<TMPro.TMP_Dropdown>().value);
                                }
                            }

                            if (contentPanels[k].transform.GetChild(l).gameObject.name == "Toggle")
                            {
                                GameObject toggle = contentPanels[k].transform.Find("Toggle").gameObject;
                                if (toggle.activeSelf == true)
                                {
                                    gameData.toggleValues.Add(toggle.GetComponent<Toggle>().isOn);
                                }
                            }
                        }
                    }
                }
            }
        }

        //for (int i = 0; i < contentPanels.Count; i++)
        //{
        //    for (int j = 0; j < contentPanels[i].transform.childCount; j++)
        //    {
        //        if (contentPanels[i].transform.GetChild(j).gameObject.name == "Slider")
        //        {
        //            GameObject slider = contentPanels[i].transform.Find("Slider").gameObject;
        //            if (slider.activeSelf == true)
        //            {
        //                gameData.sliderValues.Add(slider.GetComponent<Slider>().value);
        //            }
        //        }

        //        if (contentPanels[i].transform.GetChild(j).gameObject.name == "Dropdown")
        //        {
        //            GameObject dropdown = contentPanels[i].transform.Find("Dropdown").gameObject;
        //            if (dropdown.activeSelf == true)
        //            {
        //                gameData.dropdownOptions.Add(dropdown.GetComponent<TMPro.TMP_Dropdown>().value);
        //            }
        //        }

        //        if (contentPanels[i].transform.GetChild(j).gameObject.name == "Toggle")
        //        {
        //            GameObject toggle = contentPanels[i].transform.Find("Toggle").gameObject;
        //            if (toggle.activeSelf == true)
        //            {
        //                gameData.toggleValues.Add(toggle.GetComponent<Toggle>().isOn);
        //            }
        //        }
        //    }
        //}
    }

    public void LoadSettings()
    {
        for (int i = 0; i < menuPanels.Count; i++)
        {
            if (panelNames[i] != null)
            {
                List<GameObject> contentPanels = new List<GameObject>();
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

                List<GameObject> sliders = new List<GameObject>();
                List<GameObject> toggles = new List<GameObject>();
                List<GameObject> dropdowns = new List<GameObject>();

                for (int k = 0; k < contentPanels.Count; k++)
                {
                    for (int l = 0; l < contentPanels[k].transform.childCount; l++)
                    {
                        if (contentPanels[k].transform.GetChild(l).gameObject.name == "Slider")
                        {
                            GameObject slider = contentPanels[k].transform.Find("Slider").gameObject;
                            if (slider.activeSelf == true)
                            {
                                sliders.Add(slider);
                            }
                        }

                        if (contentPanels[k].transform.GetChild(l).gameObject.name == "Dropdown")
                        {
                            GameObject dropdown = contentPanels[k].transform.Find("Dropdown").gameObject;
                            if (dropdown.activeSelf == true)
                            {
                                dropdowns.Add(dropdown);
                            }
                        }

                        if (contentPanels[k].transform.GetChild(l).gameObject.name == "Toggle")
                        {
                            GameObject toggle = contentPanels[k].transform.Find("Toggle").gameObject;
                            if (toggle.activeSelf == true)
                            {
                                toggles.Add(toggle);
                            }
                        }
                    }
                }

                for (int k = 0; k < sliders.Count; k++)
                {
                    sliders[k].GetComponent<Slider>().value = gameData.sliderValues[k];
                }

                for (int k = 0; k < toggles.Count; k++)
                {
                    toggles[k].GetComponent<Toggle>().isOn = gameData.toggleValues[k];
                }

                for (int k = 0; k < dropdowns.Count; k++)
                {
                    dropdowns[k].GetComponent<TMPro.TMP_Dropdown>().value = gameData.dropdownOptions[k];
                }
            }
        }

        //List<GameObject> sliders = new List<GameObject>();
        //List<GameObject> toggles = new List<GameObject>();
        //List<GameObject> dropdowns = new List<GameObject>();
        
        //for (int i = 0; i < contentPanels.Count; i++)
        //{
            
        //    for (int j = 0; j < contentPanels[i].transform.childCount; j++)
        //    {
        //        if (contentPanels[i].transform.GetChild(j).gameObject.name == "Slider")
        //        {
        //            GameObject slider = contentPanels[i].transform.Find("Slider").gameObject;
        //            if (slider.activeSelf == true)
        //            {
        //                sliders.Add(slider);
        //            }
        //        }

        //        if (contentPanels[i].transform.GetChild(j).gameObject.name == "Dropdown")
        //        {
        //            GameObject dropdown = contentPanels[i].transform.Find("Dropdown").gameObject;
        //            if (dropdown.activeSelf == true)
        //            {
        //                dropdowns.Add(dropdown);
        //            }
        //        }

        //        if (contentPanels[i].transform.GetChild(j).gameObject.name == "Toggle")
        //        {
        //            GameObject toggle = contentPanels[i].transform.Find("Toggle").gameObject;
        //            if (toggle.activeSelf == true)
        //            {
        //                toggles.Add(toggle);
        //            }
        //        }
        //    }
        //}

        //for (int i = 0; i < sliders.Count; i++)
        //{
        //    sliders[i].GetComponent<Slider>().value = gameData.sliderValues[i];
        //}

        //for (int i = 0; i < toggles.Count; i++)
        //{
        //    toggles[i].GetComponent<Toggle>().isOn = gameData.toggleValues[i];
        //}

        //for (int i = 0; i < dropdowns.Count; i++)
        //{
        //    dropdowns[i].GetComponent<TMPro.TMP_Dropdown>().value = gameData.dropdownOptions[i];
        //}
    }
}

    
