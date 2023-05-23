using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuGeneratorEditorWindow : ExtendedEditorWindow
{
    public static void Open(UIScriptableObject uIScriptableObject)
    {
        MenuGeneratorEditorWindow window = GetWindow<MenuGeneratorEditorWindow>("Menu Generator Editor");
        window.serializedObject = new SerializedObject(uIScriptableObject);
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("menuOptions");
        //DrawProperties(currentProperty, true);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawSidebar(currentProperty);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if (selectedProperty != null)
        {
            //DrawProperties(selectedProperty, true);
            DrawSelectedPropertiesPanel();
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

        if(GUILayout.Button("Settings Options", EditorStyles.toolbarButton))
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
            DrawField("panelCreation", true);
            DrawField("header", true);
            DrawField("panelContent", true);
            DrawField("contentText", true);
            DrawField("slider", true);
            DrawField("dropDownMenu", true);
            DrawField("toggle", true);
            EditorGUILayout.EndVertical();

        }

        if (controls)
        {
            EditorGUILayout.BeginVertical();
            DrawField("panelCreation", true);
            DrawField("header", true);
            DrawField("panelContent", true);
            DrawField("contentText", true);
            DrawField("controlKey", true);
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
            DrawField("panelCreation", true);
            DrawField("header", true);
            DrawField("panelContent", true);
            DrawField("contentText", true);
            //Add battlepass funtionality with image
            EditorGUILayout.EndVertical();
        }
    }
}
