using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Necroisle.EditorTool
{

    /// <summary>
    /// Just the text on the UniqueID component in Unity inspector
    /// </summary>

    [CustomEditor(typeof(UniqueID)), CanEditMultipleObjects]
    public class UIDEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            UniqueID myScript = target as UniqueID;

            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Кнопка ниже генерирует UID только для этого объекта.", GUILayout.Height(50));

            if (GUILayout.Button("Generate UID"))
            {
                Undo.RecordObject(myScript, "Generate UID");
                myScript.GenerateUIDEditor();
                EditorUtility.SetDirty(myScript);
            }

            EditorGUILayout.Space();
        }

    }

}
