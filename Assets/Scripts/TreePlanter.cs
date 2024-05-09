using UnityEngine;

public class TreePlanter : MonoBehaviour
{
    public GameObject[] treePrefabs; // Префабы деревьев
    public Transform groundPlane; // Ссылка на плоскость
    public float density = 0.1f; // Плотность деревьев
    public float maxOffset = 1.0f; // Максимальное смещение относительно каждой точки

    // Start is called before the first frame update
    void Start()
    {
        if (groundPlane == null)
        {
            Debug.LogWarning("Ground plane is not assigned.");
            return;
        }

        PlantTrees();
    }

    void PlantTrees()
    {
        if (treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogWarning("Tree prefabs are not assigned or empty.");
            return;
        }

        // Получаем размеры плоскости
        Renderer planeRenderer = groundPlane.GetComponent<Renderer>();
        if (planeRenderer == null)
        {
            Debug.LogWarning("Plane renderer not found.");
            return;
        }

        Bounds planeBounds = planeRenderer.bounds;

        // Определяем количество точек для размещения деревьев
        float totalArea = planeBounds.size.x * planeBounds.size.z;
        int treePointCount = Mathf.RoundToInt(totalArea * density);

        // Размещаем деревья равномерно по всей площади плоскости с случайным смещением и поворотом по оси Y
        for (int i = 0; i < treePointCount; i++)
        {
            // Генерируем случайные координаты на плоскости
            float randomX = Random.Range(planeBounds.min.x, planeBounds.max.x);
            float randomZ = Random.Range(planeBounds.min.z, planeBounds.max.z);

            // Генерируем случайное смещение в пределах заданных пределов
            float offsetX = Random.Range(-maxOffset, maxOffset);
            float offsetZ = Random.Range(-maxOffset, maxOffset);

            // Генерируем случайный поворот по оси Y
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // Создаем дерево с учетом смещения и поворота
            Vector3 position = new Vector3(randomX + offsetX, groundPlane.position.y, randomZ + offsetZ);
            GameObject selectedTreePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            Instantiate(selectedTreePrefab, position, rotation);
        }
    }
}
