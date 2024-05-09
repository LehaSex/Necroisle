using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    public class MapTerrain : MonoBehaviour
    {
        private List<GameObject> planes;
        [SerializeField] private Material mat;
        private static int mapSize = 200;
        private float chunkSize = 10f;
        private Vector2 playerChunk;

        MapGenerator map = new MapGenerator(777777777, mapSize, mapSize);

        [SerializeField]
        private GameObject mapBlock;
        public PlayerController player;

        private int loadedChunksRadius = 5; // Number of chunks to load around the player
        private Dictionary<Vector2, GameObject> loadedChunks = new Dictionary<Vector2, GameObject>();
        private Vector2? previousPlayerChunk;

        void Awake()
        {
            planes = new List<GameObject>();
            chunkSize = GameManager.Instance.getChunkSize();
        }

        void Start()
        {   
            /// <summary>
            /// Chunk's size in unity units
            /// </summary>
            map.InitChunks();
            for (int i = 0; i < 6; i++)
            {
                map.Learn(100, 0.33);
            }
            map.recolorMap();
        }

        /// <summary>
        /// Creates a chunk at the given position
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        void CreateChunk(int i, int j)
        {
            if (i < 0 || j < 0)
            {
                // Ignore negative chunks
                return;
            }
            RGB chunk = map.GetChunk(i, j);
            GameObject plane = Instantiate(mapBlock);

            plane.transform.position = new Vector3(i * chunkSize, 0, j * chunkSize);
            plane.transform.localScale = new Vector3(chunkSize / 10, 1, chunkSize / 10);
            plane.GetComponent<MeshRenderer>().material = new Material(mat);

            // Set Coefficients to material
            plane.GetComponent<MeshRenderer>().material.SetFloat("_BlendWeight1", (float)chunk.r / 255);
            plane.GetComponent<MeshRenderer>().material.SetFloat("_BlendWeight2", (float)chunk.g / 255);
            plane.GetComponent<MeshRenderer>().material.SetFloat("_BlendWeight4", (float)chunk.b / 255);
            plane.GetComponent<MeshRenderer>().material.SetFloat("_BlendWeight3", ((float)chunk.b / 255 + (float)chunk.g / 255 + (float)chunk.r / 255) / 3);
            plane.GetComponent<MeshRenderer>().material.SetFloat("_ObjectXCoordinate", i);
            plane.GetComponent<MeshRenderer>().material.SetFloat("_ObjectYCoordinate", j);

            // Make static
            plane.isStatic = true;
            // Add plane to the dictionary
            loadedChunks[new Vector2(i, j)] = plane;

            Debug.Log("Chunk " + i + " " + j + " " + chunk.r + " " + chunk.g + " " + chunk.b);
        }

        void FixedUpdate()
        {
            playerChunk = player.GetPlayerChunk();

            // Check if the previous player chunk position is not null and is equal to the current position
            if (previousPlayerChunk.HasValue && previousPlayerChunk == playerChunk)
            {
                // Player chunk position has not changed, skip further processing
                return;
            }

            // Update the previous player chunk position
            previousPlayerChunk = playerChunk;

            // Unload chunks outside the loadedChunksRadius
            List<Vector2> chunksToRemove = new List<Vector2>();
            foreach (var chunkPos in loadedChunks.Keys)
            {
                float distance = Vector2.Distance(chunkPos, playerChunk);
                if (distance > loadedChunksRadius)
                {
                    chunksToRemove.Add(chunkPos);
                }
            }

            foreach (var chunkPosToRemove in chunksToRemove)
            {
                GameObject chunkToRemove;
                if (loadedChunks.TryGetValue(chunkPosToRemove, out chunkToRemove))
                {
                    loadedChunks.Remove(chunkPosToRemove);
                    Destroy(chunkToRemove);
                }
            }

            // Load new chunks around the player
            for (int i = (int)playerChunk.x - loadedChunksRadius; i <= (int)playerChunk.x + loadedChunksRadius; i++)
            {
                for (int j = (int)playerChunk.y - loadedChunksRadius; j <= (int)playerChunk.y + loadedChunksRadius; j++)
                {
                    Vector2 chunkPos = new Vector2(i, j);
                    if (!loadedChunks.ContainsKey(chunkPos))
                    {
                        CreateChunk(i, j);
                    }
                }
            }
        }
    }
}
