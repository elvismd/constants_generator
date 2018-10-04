using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using UnityEditor.Callbacks;
using UnityEngine.SceneManagement;

public class ConstantsGenerator : Editor
{
    static string ConstantsFileName = "ConstantsReference";

    [MenuItem("Constants Generator/Generate Constants")]
    static void GenerateConstants()
    {
        string FileContents = "using System;\nusing System.Collections;\nusing UnityEngine;\nusing UnityEngine.SceneManagement;\n\n";
        FileContents += "namespace ConstantsRef\n{\n";

        FileContents += GenerateTags();
        FileContents += GenerateLayers();
        FileContents += GenerateSortingLayers();
        FileContents += GenerateSceneConstants();

        FileContents += "}";

        File.WriteAllText(Application.dataPath + "/" + ConstantsFileName + ".cs", FileContents);
    }

    [PostProcessScene, DidReloadScripts]
    static void OnPostProcessSceneOrScripts()
    {
        EditorApplication.hierarchyChanged -= GenerateConstants;
        EditorApplication.hierarchyChanged += GenerateConstants;

        GenerateConstants();
    }

    static string TagsFileName = "Tags";
    static string GenerateTags()
    {
        string[] Tags = InternalEditorUtility.tags;
        int TagsCount = Tags.Length;

        string FileContents = "";

        FileContents += "\tpublic static class " + TagsFileName + "\n\t{\n";
        for (int i = 0; i < TagsCount; i++)
        {
            FileContents += "\t\tpublic static readonly string " + Tags[i] + " = \"" + Tags[i] + "\";\n";
        }

        FileContents += "\t}\n\n";

        return FileContents;
    }

    static string SortingLayersFileName = "SortingLayers";
    static string GenerateSortingLayers()
    {
        SortingLayer[] SortingLayers = SortingLayer.layers;

        int TagsCount = SortingLayers.Length;

        string FileContents = "";

        FileContents += "\tpublic static class " + SortingLayersFileName + "\n\t{\n";

        for (int i = 0; i < TagsCount; i++)
        {
            FileContents += "\t\tpublic static readonly SortingLayer " + SortingLayers[i].name + " = SortingLayer.layers[" + i + "];\n";
        }

        FileContents += "\n";
        FileContents += "\t\tpublic static class ByValue\n\t\t{\n";

        for (int i = 0; i < TagsCount; i++)
        {
            FileContents += "\t\t\tpublic static readonly int " + SortingLayers[i].name + " = " + SortingLayers[i].value + ";\n";
        }
        FileContents += "\t\t}\n\n";

        FileContents += "\t\tpublic static class ByName\n\t\t{\n";

        for (int i = 0; i < TagsCount; i++)
        {
            FileContents += "\t\t\tpublic static readonly string " + SortingLayers[i].name + " = \"" + SortingLayers[i].name + "\";\n";
        }
        FileContents += "\t\t}\n";

        FileContents += "\t}\n\n";

        return FileContents;
    }

    static string ScenesFileName = "Scenes";
    static string GenerateSceneConstants()
    {
        int SceneCount = SceneManager.sceneCountInBuildSettings;

        string FileContents = "";

        FileContents += "\tpublic static class " + ScenesFileName + "\n\t{\n";
        for (int i = 0; i < SceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneByBuildIndex(i);
            scene.GetRootGameObjects();
            FileContents += "\t\tpublic static class " + scene.name + "\n\t\t{\n";

            FileContents += "\t\t\tpublic static readonly int index = " + i + ";\n";
            FileContents += "\t\t\tpublic static readonly string name = \"" + scene.name + "\";\n";
            FileContents += "\t\t\tpublic static readonly bool isLoaded = " + scene.isLoaded.ToString().ToLower() + ";\n";
            FileContents += "\t\t\tpublic static readonly int rootCount = " + scene.rootCount + ";\n";
            FileContents += "\t\t\tpublic static readonly GameObject[] root = SceneManager.GetSceneByBuildIndex(" + i + ").GetRootGameObjects();\n";

            FileContents += "\t\t}\n";
        }

        FileContents += "\t}\n\n";

        return FileContents;
    }

    static string LayersFileName = "Layers";
    static string GenerateLayers()
    {    
        string[] Layers = InternalEditorUtility.layers;
        int LayersCount = Layers.Length;

        string FileContents = "";

        FileContents += "\tpublic static class " + LayersFileName + "\n\t{\n";

        for (int i = 0; i < LayersCount; i++)
        {
            int LayerIndex = LayerMask.NameToLayer(Layers[i]);
            FileContents += "\t\tpublic static readonly int " + Layers[i].Replace(" ", "") + " = " + LayerIndex + ";\n";
        }
        FileContents += "\n";

        FileContents += "\t\tpublic static class ByName\n\t\t{\n";
        for (int i = 0; i < LayersCount; i++)
        {
            FileContents += "\t\t\tpublic static readonly string " + Layers[i].Replace(" ", "") + " = \"" + Layers[i] + "\";\n";
        }

        FileContents += "\t\t}\n\n";
        
        FileContents +=
        "\t\tpublic static bool IsValueOnMask(int layer, LayerMask mask)\n" +
        "\t\t{\n" +
            "\t\t\treturn (mask == (mask | (1 << layer)));\n" +
        "\t\t}\n";

        FileContents += "\t}\n\n";

        return FileContents;
    }
}

public class ConstantsWindow : EditorWindow
{
    [MenuItem("Constants Generator/Constants Table")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(ConstantsWindow));
        var titleContent = window.titleContent;
        titleContent.text = "Constants Table";
    }

    List<Object> folderObjects = new List<Object>();

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Folders references used to generate Prefabs Constants, the folders dragged here may contains prefabs", EditorStyles.wordWrappedLabel);

        EditorGUILayout.Space();
        DropAreaGUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < folderObjects.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField((i + 1).ToString(), folderObjects[i], typeof(DefaultAsset), false);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                folderObjects.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    public void DropAreaGUI()
    {
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.normal.textColor = Color.white;
        GUI.Box(drop_area, "Drag and drop folders here", boxStyle);

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object dragged_object in DragAndDrop.objectReferences)
                    {
                        // Do On Drag Stuff here
                        folderObjects.Add(dragged_object);
                    }

                    evt.Use();
                }
            break;
        }
    }
}