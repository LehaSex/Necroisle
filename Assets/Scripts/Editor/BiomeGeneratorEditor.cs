using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Necroisle.WorldGen.EditorTool
{

    [CustomEditor(typeof(BiomeGenerator)), CanEditMultipleObjects]
    public class BiomeGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BiomeGenerator myScript = target as BiomeGenerator;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            GUIStyle title_style = new GUIStyle();
            title_style.fontSize = 14;
            title_style.fontStyle = FontStyle.Bold;
            title_style.normal.textColor = Color.white;
            title_style.hover.textColor = Color.white;
            title_style.active.textColor = Color.white;
            title_style.focused.textColor = Color.white;
            title_style.onNormal.textColor = Color.white;
            title_style.onHover.textColor = Color.white;
            title_style.onActive.textColor = Color.white;
            title_style.onFocused.textColor = Color.white;

            if (myScript.mode == WorldGeneratorMode.Runtime)
            {
                if (GUILayout.Button("Clear World"))
                {
                    myScript.ClearBiomeObjects();
                    EditorUtility.SetDirty(myScript);
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
                return;
            }

            EditorGUILayout.LabelField("Objects Generator", title_style);

            if (GUILayout.Button("Clear Biome Objects"))
            {
                myScript.ClearBiomeObjects();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            if (GUILayout.Button("Generate Biome Objects"))
            {
                myScript.GenerateBiomeObjects();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            EditorGUILayout.LabelField("Finalizing", title_style);

            if (GUILayout.Button("Generate Biome UIDs"))
            {
                myScript.GenerateBiomeUID();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

    }

}
