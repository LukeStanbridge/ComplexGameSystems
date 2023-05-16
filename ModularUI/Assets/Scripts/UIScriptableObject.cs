using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Menu", menuName = "CustomUI/Settings")]
public class UIScriptableObject : ScriptableObject
{
    public List<MenuOption> menuOptions;
}

[System.Serializable]
public struct MenuOption
{
    public enum MenuType { Settings, Controls, DragAndDrop, Battlepass }
    [SerializeField] public MenuType menuType;

    public string tabOption;
    public List<HeaderAndContent> panelCreation;
}

[System.Serializable]
public struct HeaderAndContent
{
    public string header;
    public List<ContentField> panelContent;
}

[System.Serializable]
public class ContentField
{
    public string contentText;

    [Space(5)]
    [Header("Settings options")]
    public bool slider;
    public bool dropDownMenu;
    public bool toggle;

    [Space(5)]
    [Header("Control Options")]
    public string controlKey;
}
