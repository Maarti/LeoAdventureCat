using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameUIController))]
public class GameUIControllerEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        GameUIController gui = (GameUIController)target;
        if (GUILayout.Button("Finish level"))
            gui.EndGame();
        if (GUILayout.Button("Game Over"))
            gui.GameOver();
    }
}
