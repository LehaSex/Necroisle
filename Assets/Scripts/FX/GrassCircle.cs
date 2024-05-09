using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{

    /// <summary>
    /// Generates a grass mesh in a circle shape
    /// </summary>

    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class GrassCircle : MonoBehaviour
    {
        public int widthSegments = 10;
        public int lengthSegments = 10;
        public float width = 10f;
        public float length = 10f;
        public float maxOffset = 1f; // Максимальное смещение вершин

        //public Transform[] paths;

        private MeshRenderer render;
        private MeshFilter mesh;

        void Awake()
        {
            mesh = GetComponent<MeshFilter>();
            render = gameObject.GetComponent<MeshRenderer>();
            RefreshMesh();
        }

        Mesh CreateMesh()
        {
            System.Random random = new System.Random((int)System.DateTime.Now.Ticks); // Используем текущее время как seed
            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            int numVertices = (widthSegments + 1) * (lengthSegments + 1);
            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uv = new Vector2[numVertices];

            for (int i = 0, z = 0; z <= lengthSegments; z++)
            {
                for (int x = 0; x <= widthSegments; x++, i++)
                {
                    float xPos = (float)x / widthSegments * width;
                    float zPos = (float)z / lengthSegments * length;
                    float yOffset = (float)random.NextDouble() * 2f * maxOffset - maxOffset; // Генерируем случайное число в диапазоне [-maxOffset, maxOffset]
                    vertices[i] = new Vector3(xPos, 0f, zPos) + new Vector3((float)random.NextDouble() * 2f * maxOffset - maxOffset, 0f, (float)random.NextDouble() * 2f * maxOffset - maxOffset);
                    uv[i] = new Vector2((float)x / widthSegments, (float)z / lengthSegments);
                }
            }

            int[] triangles = new int[widthSegments * lengthSegments * 6];
            int vert = 0;
            int tris = 0;
            for (int z = 0; z < lengthSegments; z++)
            {
                for (int x = 0; x < widthSegments; x++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + widthSegments + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + widthSegments + 1;
                    triangles[tris + 5] = vert + widthSegments + 2;

                    vert++;
                    tris += 6;
                }
                vert++;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;

            mesh.RecalculateNormals();
            return mesh;
        }

        public void RefreshMesh()
        {
            mesh.mesh = CreateMesh();
        }
    }

}