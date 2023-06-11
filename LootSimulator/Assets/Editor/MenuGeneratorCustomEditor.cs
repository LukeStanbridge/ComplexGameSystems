using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceID, int line)
    {
        UIScriptableObject obj = EditorUtility.InstanceIDToObject(instanceID) as UIScriptableObject;
        if (obj != null)
        {
            MenuGeneratorEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(UIScriptableObject))]
public class MenuGeneratorCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            MenuGeneratorEditorWindow.Open((UIScriptableObject)target);
        }
    }
}
