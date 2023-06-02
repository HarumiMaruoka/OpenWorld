using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// BuildSettingsに登録されているSceneをドロップダウンリストで選択できるようにした。
/// </summary>
[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    int _sceneIndex = -1;
    GUIContent[] _sceneNames;
    readonly string[] _scenePathSplitters = { "/", ".unity" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (EditorBuildSettings.scenes.Length == 0) return;
        if (_sceneIndex == -1)
            Setup(property);

        int oldIndex = _sceneIndex;
        _sceneIndex = EditorGUI.Popup(position, label, _sceneIndex, _sceneNames);

        if (oldIndex != _sceneIndex)
            property.stringValue = _sceneNames[_sceneIndex].text;
    }

    void Setup(SerializedProperty property)
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        _sceneNames = new GUIContent[scenes.Length];

        for (int i = 0; i < _sceneNames.Length; i++)
        {
            string path = scenes[i].path;
            if (string.IsNullOrEmpty(path))
            {
                _sceneNames[i] = new GUIContent("INVALID. SCENE WAS DELETED. OPEN BUILD SETTINGS");
            }
            else
            {
                string[] splitPath = path.Split(_scenePathSplitters, StringSplitOptions.RemoveEmptyEntries);
                _sceneNames[i] = new GUIContent(splitPath[splitPath.Length - 1]);
            }
        }

        if (_sceneNames.Length == 0)
            _sceneNames = new[] { new GUIContent("[No Scenes In Build Settings]") };

        if (!string.IsNullOrEmpty(property.stringValue))
        {
            bool sceneNameFound = false;
            for (int i = 0; i < _sceneNames.Length; i++)
            {
                if (_sceneNames[i].text == property.stringValue)
                {
                    _sceneIndex = i;
                    sceneNameFound = true;
                    break;
                }
            }
            if (!sceneNameFound)
                _sceneIndex = 0;
        }
        else _sceneIndex = 0;

        property.stringValue = _sceneNames[_sceneIndex].text;
    }
}