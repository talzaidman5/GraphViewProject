using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class GraphLoader: EditorWindow
{
     string _name;
    [MenuItem("Window/Graph Loader")]
    static void Init()
    {
        GraphLoader window = (GraphLoader)EditorWindow.GetWindow(typeof(GraphLoader));
        window.Show();
    }


    void OnGUI()
    {
        GUILayout.Space(20);
        _name = EditorGUILayout.TextField(Constants.FILE_NAME,_name);
        GUILayout.Space(20);
        var save = GUILayout.Button("Save");
        var load = GUILayout.Button("Load");

        var dialogueManager = Resources.Load<DialogueManager>(Constants.DIALOGUE_MANAGER);
        if (dialogueManager != null)
        {
            var saveUtility = GraphSaveUtility.GetInstance(dialogueManager._graphView);
            if (save)
                saveUtility.SaveGraph(_name);            
            if (load)
                saveUtility.LoadGraph(_name);
            GUIUtility.ExitGUI();
        }

    }
}