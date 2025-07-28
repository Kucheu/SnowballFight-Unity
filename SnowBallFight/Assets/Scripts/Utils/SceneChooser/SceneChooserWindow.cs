using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Eflatun.SceneReference;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor.SceneManagement;

public class SceneChooserWindow : EditorWindow
{
    [SerializeField]
    private SceneChooserData scenesData;

    [MenuItem("Kucheu/SceneChooser")]
    public static void ShowWindow()
    { 
        GetWindow<SceneChooserWindow>("Scene Chooser");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        root.style.flexGrow = 1;
        root.style.flexDirection = FlexDirection.Row;
        root.style.flexWrap = Wrap.Wrap;
        for (int i = 0; i < scenesData.Scenes.Count; i++)
        {
            SceneReference scene = new SceneReference(scenesData.Scenes[i].Guid);
            Button sceneButton = new Button();
            sceneButton.text = scene.Name;
            sceneButton.style.width = Length.Percent(48);
            sceneButton.clicked += () => ChangeScene(scene);
            root.Add(sceneButton);

            Button additiveSceneButton = new Button();
            additiveSceneButton.text = "Additive " + scene.Name;
            additiveSceneButton.style.width = Length.Percent(48);
            additiveSceneButton.clicked += () => AddScene(scene);
            root.Add(additiveSceneButton);
        }
    }

    private void ChangeScene(SceneReference scene)
    {
        EditorSceneManager.OpenScene(scene.Path, OpenSceneMode.Single);
        Debug.Log("Open Scene Single " + scene.Name);
    }

    private void AddScene(SceneReference scene)
    {
        EditorSceneManager.OpenScene(scene.Path, OpenSceneMode.Additive);
        Debug.Log("Open Scene Additive " + scene.Name);
    }
}