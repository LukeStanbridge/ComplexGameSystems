using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Menu", menuName = "CustomUI/Settings")]
public class UIScriptableObject : ScriptableObject
{
    public List<MenuOption> menuOptions;
}

[System.Serializable]
public struct MenuOption
{
    public enum MenuType { Settings, Controls, Inventory, Battlepass }
    [SerializeField] public MenuType menuType;

    public string tabOption;

    //Inventory variables
    public int inventorySlots;
    public List<GameObject> starterItems;

    public List<SettingsLayout> settingsPanelCreation;
    public List<ControlsLayout> controlsPanelCreation;
    public List<BattlepassLayout> battlepassPanelCreation;
}

//Panel Layout options
[System.Serializable]
public struct SettingsLayout
{
    public string header;
    public List<SettingsContent> panelContent;
}
[System.Serializable]
public struct ControlsLayout
{
    public string header;
    public List<ControlsContent> panelContent;
}
[System.Serializable]

public struct BattlepassLayout
{
    public string header;
    public List<BattlepassContent> panelContent;
}


//Panel Content options
[System.Serializable]
public class SettingsContent
{
    public string settingsContentText;
    public bool slider;
    public bool dropDownMenu;
    public bool toggle;
}

[System.Serializable]
public class ControlsContent
{
    public string controlsContentText;
    public string controlKey;
}

[System.Serializable]
public class BattlepassContent
{
    public string battlepassContentText;
    public Image battlepassItemImage;
}


