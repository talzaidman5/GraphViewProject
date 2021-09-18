using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue graph");
    }
    
    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView
        {
            name = "Dialouge Graph"
        };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueManager>();

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        dialogueContainer._graphView = _graphView;
        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{Constants.DIALOGUE_MANAGER}.asset");
        AssetDatabase.SaveAssets();
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();
        toolbar.Add(new Button(() => _graphView.CreateNode("DialogueNodeA", Color.red, Color.magenta))
        { text = "Create Node A" });
        toolbar.Add(new Button(() => _graphView.CreateNode("DialogueNodeB", Color.yellow, Color.red))
        { text = "Create Node B" });
        toolbar.Add(new Button(() => _graphView.CreateNode("DialogueNodeC", Color.magenta, Color.yellow))
        { text = "Create Node C" });
        toolbar.Add(new Button(() => Clear()) { text = "Clear" });
        rootVisualElement.Add(toolbar);
    }

    private void Clear()
    {
        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        saveUtility.ClearGraph();
    }

   
}
