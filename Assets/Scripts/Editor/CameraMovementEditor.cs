using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraMovement))]
public class CameraMovementEditor : Editor
{
    private CameraMovement cameraMovement;
    private SerializedProperty interpolationMethodProp;
    private SerializedProperty smoothnessProp;
    private SerializedProperty movementMode;
    private SerializedProperty minX;
    private SerializedProperty maxX;
    private SerializedProperty minZ;
    private SerializedProperty maxZ;
    private SerializedProperty speed;


    private void OnEnable()
    {
        cameraMovement = (CameraMovement)target;
        interpolationMethodProp = serializedObject.FindProperty("interpolationMethod");
        smoothnessProp = serializedObject.FindProperty("smoothness");
        movementMode = serializedObject.FindProperty("movementMode");
        // add XZ properties
        minX = serializedObject.FindProperty("minX");
        maxX = serializedObject.FindProperty("maxX");
        minZ = serializedObject.FindProperty("minZ");
        maxZ = serializedObject.FindProperty("maxZ");
        speed = serializedObject.FindProperty("speed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        
        
        EditorGUILayout.PropertyField(movementMode);
        EditorGUILayout.PropertyField(minX);
        EditorGUILayout.PropertyField(maxX);
        EditorGUILayout.PropertyField(minZ);
        EditorGUILayout.PropertyField(maxZ);
        EditorGUILayout.PropertyField(speed);
        EditorGUILayout.PropertyField(smoothnessProp);
        EditorGUILayout.PropertyField(interpolationMethodProp);


        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(); // Добавить немного пространства между стандартными элементами и графиком

        // Создать новый график метода интерполяции
        Necroisle.EditorTool.GraphTool graph = new Necroisle.EditorTool.GraphTool(0, -1, 5, 1, "Interpolation Graph", 100);

        // В зависимости от выбранного метода интерполяции добавить соответствующую функцию на график
        switch (cameraMovement.interpolationMethod)
        {
            case InterpolationMethod.Linear:
                graph.AddFunction(x => x); // Функция y = x
                break;
            case InterpolationMethod.Bezier:
                graph.AddFunction(x => x * x); // Функция y = x^2
                break;
            case InterpolationMethod.SmoothStep:
                graph.AddFunction(x => Mathf.SmoothStep(0, 1, x)); // Функция SmoothStep
                break;
            case InterpolationMethod.EaseIn:
                graph.AddFunction(x => 1 - Mathf.Cos(x * Mathf.PI * 0.5f)); // Функция EaseIn
                break;
            case InterpolationMethod.EaseOut:
                graph.AddFunction(x => Mathf.Sin(x * Mathf.PI * 0.5f)); // Функция EaseOut
                break;
        }

        graph.Draw(); // Нарисовать график метода интерполяции
    }
}
