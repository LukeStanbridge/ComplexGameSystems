using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuGeneratorEditorWindow : ExtendedEditorWindow
{
    Vector2 scrollPosTabs = Vector2.zero;
    Vector2 scrollPosContent = Vector2.zero;
    public static void Open(UIScriptableObject uIScriptableObject)
    {
        MenuGeneratorEditorWindow window = GetWindow<MenuGeneratorEditorWindow>("Menu Generator Editor");
        window.serializedObject = new SerializedObject(uIScriptableObject);
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("menuOptions");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));

        scrollPosTabs = EditorGUILayout.BeginScrollView(scrollPosTabs);
        DrawSidebar(currentProperty);
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if (selectedProperty != null)
        {
            scrollPosContent = EditorGUILayout.BeginScrollView(scrollPosContent);
            DrawSelectedPropertiesPanel();
            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.LabelField("Select item from the list");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();
    }

    public bool settings;
    public bool controls;
    public bool inventory;
    public bool battlepass;

    void DrawSelectedPropertiesPanel()
    {
        currentProperty = selectedProperty;
        EditorGUILayout.BeginHorizontal("box");

        DrawField("menuType", true);
        DrawField("tabOption", true);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Settings Options", EditorStyles.toolbarButton))
        {
            settings = true;
            controls = false;
            inventory = false;
            battlepass = false;
        }

        if (GUILayout.Button("Controls Options", EditorStyles.toolbarButton))
        {
            settings = false;
            controls = true;
            inventory = false;
            battlepass = false;
        }

        if (GUILayout.Button("Inventory Options", EditorStyles.toolbarButton))
        {
            settings = false;
            controls = false;
            inventory = true;
            battlepass = false;
        }

        if (GUILayout.Button("Battlepass Options", EditorStyles.toolbarButton))
        {
            settings = false;
            controls = false;
            inventory = false;
            battlepass = true;
        }

        EditorGUILayout.EndHorizontal();

        if (settings)
        {
            EditorGUILayout.BeginVertical();
            DrawField("settingsPanelCreation", true);
            EditorGUILayout.EndVertical();
        }

        if (controls)
        {
            EditorGUILayout.BeginVertical();
            DrawField("controlsPanelCreation", true);
            EditorGUILayout.EndVertical();
        }

        if (inventory)
        {
            EditorGUILayout.BeginVertical();
            DrawField("inventorySlots", true);
            DrawField("starterItems", true);
            EditorGUILayout.EndVertical();
        }

        if (battlepass)
        {
            EditorGUILayout.BeginVertical();
            DrawField("battlepassPanelCreation", true);
            EditorGUILayout.EndVertical();
        }
    }
}
