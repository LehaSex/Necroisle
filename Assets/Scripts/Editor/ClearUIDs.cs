using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace Necroisle.EditorTool
{

    /// <summary>
    /// Очистка всех уникальных идентификаторов в сцене
    /// Смена всех уникальных идентификаторов в сцене портит сохранения
    /// </summary>

    public class ClearUIDs : ScriptableWizard
    {

        [MenuItem("Necroisle/Clear UIDs", priority = 201)]
        static void ScriptableWizardMenu()
        {
            ScriptableWizard.DisplayWizard<ClearUIDs>("Clear Unique IDs", "Clear All UIDs");
        }

        void OnWizardCreate()
        {
            UniqueID.ClearAll(GameObject.FindObjectsOfType<UniqueID>());

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        void OnWizardUpdate()
        {
            helpString = "Очистка UID в сцене. Это действие нельзя отменить. ВНИМАНИЕ! ЭТО ПОРТИТ СТАРЫЕ СОХРАНЕНИЯ";
        }
    }

}