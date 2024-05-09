using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Necroisle.WorldGen.EditorTool
{
    [CustomEditor(typeof(WorldGenerator))]
    public class WorldGeneratorEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            WorldGenerator myScript = target as WorldGenerator;

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

            GUIStyle text_style = new GUIStyle();
            text_style.fontSize = 12;
            text_style.fontStyle = FontStyle.Normal;

            if (myScript.mode == WorldGeneratorMode.Runtime)
            {
                if (GUILayout.Button("Clear World"))
                {
                    myScript.ClearWorld();
                    EditorUtility.SetDirty(myScript);
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
                return;
            }

            EditorGUILayout.LabelField("World Zones Generator", title_style);

            if (GUILayout.Button("Clear World"))
            {
                myScript.ClearWorld();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            if (GUILayout.Button("Generate Zones"))
            {
                myScript.GenerateZones();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Terrain Generator", title_style);

            if (GUILayout.Button("Clear All Terrain"))
            {
                myScript.ClearTerrain();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            if (GUILayout.Button("Generate Noise"))
            {
                myScript.NoisyEdges();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            if (GUILayout.Button("Generate All Terrain"))
            {
                myScript.GenerateAllTerrain();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            if (GUILayout.Button("Generate Walls"))
            {
                myScript.GenerateWalls();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Debug Overlap Fixer", title_style);        

            // fix overlapping points
            if (GUILayout.Button("Fix Overlapping Points"))
            {
                myScript.FixOverlappingPoints();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Biomes Objects Generator", title_style);

            if (GUILayout.Button("Clear All Biome Objects"))
            {
                myScript.ClearAllBiomes();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            if (GUILayout.Button("Generate All Biome Objects"))
            {
                myScript.GenerateAllBiomesObjects();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Finalizing", title_style);

            if (GUILayout.Button("Generate All UIDs"))
            {
                myScript.GenerateAllUID();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            if (GUILayout.Button("Generate Navmesh"))
            {
                myScript.GenerateNavmesh();
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

    }

}
