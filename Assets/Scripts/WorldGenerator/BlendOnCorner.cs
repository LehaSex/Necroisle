using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle.WorldGen
{
    public class BlendOnCorner : MonoBehaviour
    {
        public Transform[] points;
        public LayerMask floorLayer = 1 << 8;
        public float edgeOffset = 0.1f; // Величина смещения краев по осям x и z

        void Awake()
        {
            points = GetComponent<BiomeZone>().points;
        }

        void Start()
        {
            foreach (Transform point in points)
            {
                Vector3 cornerPoint = point.position;

                Collider[] overlappingBiomes = Physics.OverlapSphere(cornerPoint, 1.0f, floorLayer);
                foreach (Collider collider in overlappingBiomes)
                {
                    BlendOnCorner otherZone = collider.GetComponentInParent<BlendOnCorner>();

                    if (otherZone != null && otherZone.HasSameCornerPoints(cornerPoint, GetNextCornerPoint(point)))
                    {
                        // Получаем координаты текущей и следующей точек
                        Vector3 startPoint = cornerPoint;
                        Vector3 endPoint = GetNextCornerPoint(point);

                        // Изменяем положение вершин у граничных точек
                        AdjustEdgeVertices(startPoint, endPoint);

                        // Выполняем смешивание материалов
                        //BlendMaterials(startPoint, endPoint, otherZone);
                    }
                    else
                    {
                        Debug.Log("No matching corner points found");
                    }
                }
            }
        }

        public bool HasSameCornerPoints(Vector3 point1, Vector3 point2)
        {
            bool containsPoint1 = false;
            bool containsPoint2 = false;

            foreach (Transform cornerPoint in points)
            {
                if (cornerPoint.position == point1)
                {
                    containsPoint1 = true;
                }
                else if (cornerPoint.position == point2)
                {
                    containsPoint2 = true;
                }
            }

            return containsPoint1 && containsPoint2;
        }

        Vector3 GetNextCornerPoint(Transform currentPoint)
        {
            int index = (Array.IndexOf(points, currentPoint) + 1) % points.Length;
            return points[index].position;
        }

        void AdjustEdgeVertices(Vector3 startPoint, Vector3 endPoint)
        {
            // Находим меш объекта
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
            {
                Debug.LogWarning("MeshFilter or mesh is missing.");
                return;
            }

            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;

            // Находим индексы вершин, ближайших к startPoint и endPoint
            int startIndex = FindNearestVertexIndex(vertices, startPoint);
            int endIndex = FindNearestVertexIndex(vertices, endPoint);

            // Изменяем положение вершин по осям x и z
            vertices[startIndex].x += edgeOffset;
            vertices[startIndex].z += edgeOffset;

            vertices[endIndex].x += edgeOffset;
            vertices[endIndex].z += edgeOffset;

            // Обновляем меш
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }

        int FindNearestVertexIndex(Vector3[] vertices, Vector3 point)
        {
            int index = 0;
            float minDistance = Mathf.Infinity;

            for (int i = 0; i < vertices.Length; i++)
            {
                float distance = Vector3.Distance(vertices[i], point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    index = i;
                }
            }

            return index;
        }

        void Update()
        {
            // По желанию можно добавить логику обновления
        }
    }
}
