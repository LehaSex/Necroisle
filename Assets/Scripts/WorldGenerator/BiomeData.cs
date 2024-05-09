using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle.WorldGen
{

    [CreateAssetMenu(fileName = "Biome", menuName = "Necroisle/WorldGen/Biome", order = 100)]
    public class BiomeData : ScriptableObject
    {
        public string id;
        public float probability;
        public bool starting_zone;

        [Header("Terrain")]
        public Material floor_material;
        public float elevation = 0f;
        public bool is_water;

        [Header("Spawns")]
        public BiomeSpawnData[] spawns;


    }

}