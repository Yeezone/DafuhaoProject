using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using com.QH.QPGame.GameUtils;

public class DebugWindow : EditorWindow
{
    [MenuItem("Tools/DebugConsole")]
    private static void DebugConsole()
    {
        DebugWindow window = (DebugWindow)EditorWindow.GetWindow(typeof(DebugWindow));
        window.title = "DebugConsole";
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }


    public Dictionary<string,bool> LoggerDict = new Dictionary<string, bool>(); 

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        foreach (var item in Logger.Loggers)
        {
            bool value = EditorGUILayout.Toggle(item.Key, item.Value.DebugOut);
            item.Value.DebugOut = value;
        }
        
        EditorGUILayout.EndVertical();
    }
}