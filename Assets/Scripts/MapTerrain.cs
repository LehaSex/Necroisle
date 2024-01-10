using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// Generates map
    /// </summary>
    public class MapTerrain : MonoBehaviour
    {
        private List<GameObject> planes;

        [SerializeField]
        private Material mat;

        private static int mapSize = 200;
        private float chunkSize = 10f;

        MapGenerator map = new MapGenerator(777777777, mapSize, mapSize);

        [SerializeField]
        private GameObject mapBlock;


        void Awake()
        {
            planes = new List<GameObject>();
        }

        void Start()
        {
            
            /// <summary>
            /// Chunk's size in unity units
            /// </summary>
            
            
            map.InitChunks();
            map.Learn(100, 0.33);
            map.recolorMap();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    RGB chunk = map.GetChunk(i, j);
/*                     GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    plane.AddComponent<BoxCollider>();
                    plane.layer = LayerMask.NameToLayer("Floor"); */
                    GameObject plane = Instantiate(mapBlock);


                    plane.transform.position = new Vector3(i * chunkSize, 0, j * chunkSize);
                    plane.transform.localScale = new Vector3(chunkSize / 10, 1, chunkSize / 10);
                    plane.GetComponent<MeshRenderer>().material = new Material(mat); 

                    // Set Coefficients to material
                    plane.GetComponent<MeshRenderer>().material.SetFloat("_BlendWeight1", (float)chunk.r/255);
                    plane.GetComponent<MeshRenderer>().material.SetFloat("_BlendWeight2", (float)chunk.g/255);
                    plane.GetComponent<MeshRenderer>().material.SetFloat("_BlendWeight4", (float)chunk.b/255);
                    plane.GetComponent<MeshRenderer>().material.SetFloat("_BlendWeight3", ((float)chunk.b/255 + (float)chunk.g/255 + (float)chunk.r/255)/3);

                    //plane.GetComponent<Renderer>().material.color = new Color((float)chunk.r/255, (float)chunk.g/255, (float)chunk.b/255);    
                    // Add collider
                    // make static 
                    plane.isStatic = true;
                    // Add plane to list
                    planes.Add(plane);
                    // occlusion culling
/*                     plane.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; */


                    Debug.Log("Chunk " + i + " " + j + " " + chunk.r + " " + chunk.g + " " + chunk.b);
                }
            }
/* 
            // place chunks in 20f radius around player
            int playerx = Mathf.RoundToInt(player.x);
            int playerz = Mathf.RoundToInt(player.z);
            int radius = 20;
            int startx = playerx - radius;
            int startz = playerz - radius;
            int endx = playerx + radius;
            int endz = playerz + radius;
            Debug.Log("Player position: " + playerx + " " + playerz);
            Debug.Log("Start position: " + startx + " " + startz);
            for (int i = playerx; i < endx; i++)
            {
                for (int j = playerz; j < endz; j++)
                {
                    RGB chunk = map.GetChunk(i, j);
                    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    plane.AddComponent<BoxCollider>();
                    plane.layer = LayerMask.NameToLayer("Floor");
                    plane.transform.position = new Vector3(i * chunkSize, 0, j * chunkSize);
                    plane.transform.localScale = new Vector3(chunkSize / 10, 1, chunkSize / 10);
                    plane.GetComponent<Renderer>().material.color = new Color((float)chunk.r/255, (float)chunk.g/255, (float)chunk.b/255);    
                    // Add collider
                    // make static 
                    plane.isStatic = true;
                    // Add plane to list
                    planes.Add(plane);
                    Debug.Log("Chunk " + i + " " + j + " " + chunk.r + " " + chunk.g + " " + chunk.b);
                }
            } */

        }

        public void setChunkSize(float size)
        {
            chunkSize = size;
        }

        // Update is called once per frame
    }
}
