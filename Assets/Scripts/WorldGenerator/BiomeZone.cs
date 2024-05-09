using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle.WorldGen
{
    [ExecuteInEditMode]
    public class BiomeZone : MonoBehaviour
    {
        public BiomeData data;

        [Header("Biome Generator")]
        public int seed;
        public int iterations = 1000;

        [Header("Saved Values")]
        public Transform[] points;
        public List<AdjacentAndPoints> adjp = new List<AdjacentAndPoints>();
        public GameObject floor;
        
        private List<GameObject> spawned_items = new List<GameObject>();
        private List<GameObject> spawned_items_group = new List<GameObject>();
        private Dictionary<GameObject, float> group_size = new Dictionary<GameObject, float>();
        private Dictionary<GameObject, float> collider_size = new Dictionary<GameObject, float>();

        private void Start()
        {
            //Add code to do at start

        }

        public void ClearTerrain()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
            floor = null;
        }

        public void GenerateTerrain()
        {
            if (AreObjectsGenerated())
                return;

            ClearTerrain();

            gameObject.name = data.id;
            floor = new GameObject("floor");
            floor.isStatic = true;
            floor.transform.SetParent(transform);
            floor.transform.position = transform.position;

            MeshRenderer render = floor.AddComponent<MeshRenderer>();
            MeshFilter mesh = floor.AddComponent<MeshFilter>();
            render.material = data.floor_material;
            floor.layer = 9; //Floor layer

            mesh.sharedMesh = new Mesh();
            CreateFloorMesh(mesh.sharedMesh);
            MeshCollider collide = floor.AddComponent<MeshCollider>();
            collide.convex = true;
            if (data.is_water)
            {
                GenerateWater();
            }
        }

        private void GenerateWater()
        {
            floor.layer = 4; //Water layer

            //Water collider
            GameObject fcollider = new GameObject("water-collider");
            fcollider.transform.SetParent(floor.transform);
            fcollider.transform.position = floor.transform.position;
            fcollider.isStatic = true;
            fcollider.layer = 14; //Water wall layer

            MeshRenderer crender = fcollider.AddComponent<MeshRenderer>();
            MeshFilter cmesh = fcollider.AddComponent<MeshFilter>();
            crender.enabled = false;

            cmesh.sharedMesh = new Mesh();
            CreateFloorMesh(cmesh.sharedMesh, 5f);
            MeshCollider ccollide = fcollider.AddComponent<MeshCollider>();
            ccollide.convex = true;

            //Drink Selectable
            GameObject fselect = new GameObject("water-drink");
            fselect.transform.SetParent(floor.transform);
            fselect.transform.position = floor.transform.position;
            fselect.layer = floor.layer;

            MeshRenderer srender = fselect.AddComponent<MeshRenderer>();
            MeshFilter smesh = fselect.AddComponent<MeshFilter>();
            srender.enabled = false;

            smesh.sharedMesh = new Mesh();
            CreateFloorMesh(smesh.sharedMesh, 1f, -1f);
            MeshCollider scollide = fselect.AddComponent<MeshCollider>();
            scollide.convex = true;
            scollide.isTrigger = true;

            Selectable selectable = fselect.AddComponent<Selectable>();
            selectable.type = SelectableType.InteractSurface;

            if (WorldGenerator.Get())
            {
                SAction action = WorldGenerator.Get().water_action;
                selectable.actions = new SAction[] { action };
                GroupData group = WorldGenerator.Get().water_group;
                selectable.groups = new GroupData[] { group };
            }
        }

        public void ClearBiomeObjects()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if(child.gameObject != floor)
                    DestroyImmediate(child.gameObject);
            }

            spawned_items.Clear();
            spawned_items_group.Clear();
            group_size.Clear();
            collider_size.Clear();
        }

        public void GenerateBiomeObjects()
        {
            if (!IsTerrainGenerated())
                return;

            ClearBiomeObjects();

            int index = 0;
            foreach (BiomeSpawnData group in data.spawns)
            {
                SpawnBiomeGroup(group, index);
                index++;
            }
        }

        public void SpawnBiomeGroup(BiomeSpawnData data, int index)
        {
            Random.InitState(seed + index); //Each group should have its own seed, so if one group change the other is not affected
            spawned_items_group.Clear();

            float area_size = WorldGenTool.AreaSizePolygon(points);
            float density_dist = (150f - data.variance) / data.density; //Density determine minimum distance between each object of same group
            int spawn_max = Mathf.RoundToInt(data.density * area_size / (10f * data.variance)); //Determine max number of objects

            GameObject parent = new GameObject(data.name);
            parent.transform.SetParent(transform);
            parent.transform.localPosition = Vector3.zero;

            Vector3 min = WorldGenTool.GetPolygonMin(points);
            Vector3 max = WorldGenTool.GetPolygonMax(points);

            for (int i=0; i < iterations; i++){

                if (spawned_items_group.Count > spawn_max)
                    return;

                Vector3 pos = new Vector3(Random.Range(min.x, max.x), this.data.elevation, Random.Range(min.z, max.z));

                if (IsInsideZone(pos))
                {
                    GameObject prefab = data.PickRandomPrefab();
                    if (prefab != null) {

                        WorldGenObject properties = prefab.GetComponent<WorldGenObject>();
                        float gsize = (properties != null) ? properties.size_group : 0.25f; //Group size
                        float csize = (properties != null) ? properties.size : 0.25f; //Colliding size

                        if (!IsNearOther(pos, csize) && IsFitDensity(pos, density_dist, gsize)) {

                            bool is_valid;
                            if (properties != null && properties.type == WorldGenObjectType.AvoidEdge)
                                is_valid = !IsNearPolyEdge(pos, properties.edge_dist);
                            else if (properties != null && properties.type == WorldGenObjectType.NearEdge)
                                is_valid = IsNearPolyEdge(pos, properties.edge_dist) && !IsNearPolyEdge(pos, csize);
                            else
                                is_valid = !IsNearPolyEdge(pos, csize);

                            if (is_valid)
                            {
                                GameObject nobj = InstantiatePrefab(prefab, parent.transform);
                                nobj.transform.position = pos;
                                if(data.random_rotation)
                                    nobj.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                                if (data.random_scale > 0.01f)
                                    nobj.transform.localScale = Vector3.one * (1f + Random.Range(-data.random_scale, data.random_scale));

                                if (properties == null)
                                {
                                    Collider collide = nobj.GetComponentInChildren<Collider>();
                                    csize = collide != null ? collide.bounds.extents.magnitude : 0.25f;
                                }

                                spawned_items.Add(nobj);
                                spawned_items_group.Add(nobj);
                                group_size[nobj] = gsize;
                                collider_size[nobj] = csize;
                            }
                        }
                    }
                }
            }
        }

        private GameObject InstantiatePrefab(GameObject prefab, Transform parent)
        {
            GameObject nobj;
#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
                nobj = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent);
            else
#endif
                nobj = Instantiate(prefab, parent);
            return nobj;
        }

        public void GenerateBiomeUID()
        {
            UniqueID[] all_uids = GetComponentsInChildren<UniqueID>();
            UniqueID.ClearAll(all_uids);

            Random.InitState(seed);
            UniqueID.GenerateAll(all_uids);
        }

        public bool IsTerrainGenerated()
        {
            return floor != null;
        }

        public bool AdjacentsFound()
        {
            return adjp.Count > 0;
        }

        public bool AreObjectsGenerated()
        {
            return transform.childCount > 1;
        }

        //In world space
        private bool IsInsideZone(Vector3 pos)
        {
            return WorldGenTool.IsPointInPolygon(pos, points);
        }

        public void FindAdjacents()
        {
            if (!WorldGenerator.Get().AreZonesGenerated() && !IsTerrainGenerated())
                return;
            FindAdjacentWithOnePoint();
            MergePointsInAdjacents();
            ClearAdjacents();
            BiomeZone[] biomeZones = WorldGenerator.Get().zones;
            foreach (BiomeZone zone in biomeZones)
            {
                // find zones adjacent to this zone where two same points in BiomeZone
                if (zone != this)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        Transform[] points_adj = GetAdjacentPoints(i);
                        foreach (Transform point in points_adj)
                        {
                            if (zone.ContainsPoint(points[i]) && zone.ContainsPoint(point) && !adjp.Exists(x => x.adjacent == zone.transform))
                            {
                                adjp.Add(new AdjacentAndPoints(zone.transform, points[i], point));
                                break;
                            }
                        }
                    }
                }
            }
        }

        private bool ContainsPoint(Transform point)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == point)
                    return true;
            }
            return false;
        }

        public void FindAdjacentWithOnePoint()
        {
            if (!WorldGenerator.Get().AreZonesGenerated() && !IsTerrainGenerated())
                return;
            ClearAdjacents();
            BiomeZone[] biomeZones = WorldGenerator.Get().zones;
            foreach (BiomeZone zone in biomeZones)
            {
                // find zones adjacent to this zone where two same points in BiomeZone
                if (zone != this)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        if (zone.ContainsPoint(points[i]))
                        {
                            adjp.Add(new AdjacentAndPoints(zone.transform, points[i], null));
                            break;
                        }
                    }
                }
            }
        }

        public void MergePointsInAdjacents(float distance = 0.1f)
        {
            List<Transform> merged_points = new List<Transform>();
            if (!WorldGenerator.Get().AreZonesGenerated() && !IsTerrainGenerated())
                return;
            foreach (Transform mergepoint in points)
            {
                for (int i = 0; i < adjp.Count; i++)
                {
                    // получаем BiomeZone из adjp
                    BiomeZone adjacent = adjp[i].adjacent.GetComponentInParent<BiomeZone>();
                    // проходимся по точкам adjacent.points
                    for (int j = 0; j < adjacent.points.Length; j++)
                    {
                        if ((adjacent.points[j].position - mergepoint.position).magnitude < distance)
                        {
                            // заменить точку mergepoint на adjacent.points[j]
                            ReplacePoint(mergepoint, adjacent.points[j]);
                            // add point to merged_points
                            merged_points.Add(mergepoint);
                        }
                    }
                }
            }
        }


        private void ReplacePoint(Transform old_point, Transform new_point)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == old_point)
                {
                    points[i] = new_point;
                }
            }
        }

        private void InsertBetweenPoints(Transform point1, Transform point2, Transform new_point)
        {
            // insert new_point between point1 and point2 or point2 and point1
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == point1 || points[i] == point2)
                {
                    int next = i + 1;
                    if (next >= points.Length)
                        next = 0;
                    if (points[next] == point1 || points[next] == point2)
                    {
                        // insert new_point between point1 and point2
                        Transform[] new_points = new Transform[points.Length + 1];
                        for (int j = 0; j < i + 1; j++)
                        {
                            new_points[j] = points[j];
                        }
                        new_points[i + 1] = new_point;
                        for (int j = i + 2; j < new_points.Length; j++)
                        {
                            new_points[j] = points[j - 1];
                        }
                        points = new_points;
                        break;
                    }
                }
            }
        }

        public void GenerateNoise(float levels = 5f, float amplitude = 0.5f, float treshold = 3f)
        {
            if (!WorldGenerator.Get().AreZonesGenerated() && !IsTerrainGenerated() && AdjacentsFound())
                return;    
                      
            Vector3 centerThisZone = FindZoneCenter();

            foreach (AdjacentAndPoints ap in adjp)
            {
                if (ap.adjacent == null)
                    continue;

                BiomeZone adjacent = ap.adjacent.GetComponentInParent<BiomeZone>();
                if (adjacent == null || adjacent.adjp == null)
                    continue;

                if (ap.p1 == null || ap.p2 == null)
                    continue;

                AdjacentAndPoints adjacentAP = adjacent.adjp.Find(x => x.adjacent == transform);
                if (adjacentAP == null || adjacentAP.noise)
                    continue;

                float distance = (ap.p1.position - ap.p2.position).magnitude;
                if (distance <= treshold)
                    continue;

                adjacentAP.noise = true;
                ap.noise = true;
                
                Vector3 centerAdjacentZone = adjacent.FindZoneCenter();
                MidpointDisplacement(ap, ap.p1, ap.p2, centerThisZone, centerAdjacentZone, levels, amplitude);
            } 
        }

        private void MidpointDisplacement(AdjacentAndPoints ap, Transform p1, Transform p2, Vector3 center_p1, Vector3 center_p2, float levels = 5f, float amplitude = 0.5f)
        {
            if (levels <= 0)
                return;

            // Находим среднюю точку между p1 и p2
            Vector3 midpoint = (p1.position + p2.position) / 2f;
            // Проводим 2 отрезка от midpoint до центров зон и используем их как ось для смещения по амплитуде
            Vector3 axis1 = (center_p1 - midpoint).normalized;
            Vector3 axis2 = (center_p2 - midpoint).normalized;
            // нужно чтобы работало 
            Random.InitState(seed * (int)levels);
/*             float offset = Random.Range(-amplitude, amplitude);
            Vector3 offset_vec = Vector3.zero;
            if (offset < 0)
                offset_vec = axis2 * offset;
            else
                offset_vec = axis1 * offset; */
            // шум Перлина
            float offset = Mathf.PerlinNoise(midpoint.x, midpoint.z) * amplitude;
            Vector3 offset_vec = Vector3.zero;
            if (offset < 0)
                offset_vec = axis2 * offset;
            else
                offset_vec = axis1 * offset;

            // Смещаем midpoint на offset
            midpoint += offset_vec;
            // Добавляем midpoint в массив точек
            Transform new_point = new GameObject("midpoint").transform;
            new_point.SetParent(WorldGenerator.Get().transform);
            new_point.position = midpoint;
            // Вставляем новую точку между p1 и p2
            InsertBetweenPoints(p1, p2, new_point);
            // Вставляем новую точку между p1 и p2 в соседнюю зону
            ap.adjacent.GetComponentInParent<BiomeZone>().InsertBetweenPoints(p1, p2, new_point);
            // В результате получаем 2 отрезка: p1 -> midpoint и midpoint -> p2
            // Рекурсивно вызываем функцию для каждого отрезка levels - 1 раз

            MidpointDisplacement(ap, p1, new_point, (p1.position + center_p1) /2f, (p1.position + center_p2) /2f, levels - 1, amplitude / 2f);
            MidpointDisplacement(ap, new_point, p2, (p2.position + center_p2) /2f, (p2.position + center_p1) /2f, levels - 1, amplitude / 2f);
        }

        public void FixOverlappingTerrain()
        {
            if (!IsTerrainGenerated())
                return;

            Mesh mesh = floor.GetComponent<MeshFilter>().sharedMesh;
            Vector3[] vertices = mesh.vertices;
            List<Vector3> new_vertices = new List<Vector3>(vertices);
            for (int i = 0; i < new_vertices.Count; i++)
            {
                Vector3 vertex = new_vertices[i];
                Vector3 pos = floor.transform.TransformPoint(vertex);
                if (IsInsideZone(pos))
                {
                    Vector3 normal = mesh.normals[i];
                    Vector3 offset = normal * 0.1f;
                    pos += offset;
                    new_vertices[i] = floor.transform.InverseTransformPoint(pos);
                }
            }
            mesh.vertices = new_vertices.ToArray();
            mesh.RecalculateBounds();
        }


        private void ClearAdjacents()
        {
            adjp.Clear();
        }

        private Transform GetNextPoint(int id)
        {
            int next = id + 1;
            if (next >= points.Length)
                next = 0;
            return points[next];
        }

        private Transform GetLastPoint()
        {
            return points[points.Length - 1];
        }

        private Transform[] GetAdjacentPoints(int id)
        {
            // получить соседние точки, если id = 0, то вернуть последнюю и первую точку, если id = points.Length - 1, то вернуть предпоследнюю и первую точку
            Transform[] adj_points = new Transform[2];
            if (id == 0)
            {
                adj_points[0] = points[points.Length - 1];
                adj_points[1] = points[1];
            }
            else if (id == points.Length - 1)
            {
                adj_points[0] = points[points.Length - 2];
                adj_points[1] = points[0];
            }
            else
            {
                adj_points[0] = points[id - 1];
                adj_points[1] = points[id + 1];
            }
            return adj_points;
        }

        private Transform GetFirstPoint()
        {
            return points[0];
        }

        private bool IsNearPolyEdge(Vector3 pos, float size)
        {
            if (size < 0.01f)
                return false;

            for (int i=0; i<points.Length-1; i++)
            {
                if (WorldGenTool.GetEdgeDist(pos, points[i].position, points[i + 1].position) < size)
                    return true;
            }

            if (WorldGenTool.GetEdgeDist(pos, points[points.Length - 1].position, points[0].position) < size)
                return true;

            return false;
        }

        private bool IsNearOther(Vector3 pos, float size)
        {
            bool too_close = false;
            foreach (GameObject item in spawned_items)
            {
                float dist = (item.transform.position - pos).magnitude;
                float other_size = collider_size[item];
                too_close = dist < (other_size + size);
                if (too_close)
                    return too_close;
            }
            return too_close;
        }

        private bool IsFitDensity(Vector3 pos, float density_dist, float size)
        {
            bool fit_density;
            foreach (GameObject item in spawned_items_group)
            {
                float dist = (item.transform.position - pos).magnitude;
                float other_size = group_size[item];
                fit_density = dist > density_dist && dist > (other_size + size);
                if (!fit_density)
                    return false;
            }
            return true;
        }

        private void CreateFloorMesh(Mesh aMesh, float offset=0f, float bottom=-10f)
        {
            AddMeshFace(aMesh, Vector3.up, data.elevation + offset, true);
            AddMeshFace(aMesh, Vector3.down, bottom, true);
            AddMeshEdge(aMesh);
            aMesh.RecalculateBounds();
        }

        private void AddMeshEdge(Mesh aMesh)
        {
            List<Vector3> vertices = new List<Vector3>(aMesh.vertices);
            List<Vector3> normals = new List<Vector3>(aMesh.normals);
            List<Vector2> uvs = new List<Vector2>(aMesh.uv);
            List<int> triangles = new List<int>(aMesh.triangles);
            Vector3 center = FindZoneCenter();
            int nb_vertices = aMesh.vertices.Length;
            int half = nb_vertices / 2 - 1;

            for (int j = 0; j < points.Length; j++)
            {
                int itop = j + 1;
                int ibottom = j + half + 2;
                int itopnext = j + 2;
                int ibottomnext = j + half + 3;

                if (j == points.Length - 1)
                {
                    itopnext = 1;
                    ibottomnext = half + 2;
                }

                Vector3 v1 = aMesh.vertices[itop];
                Vector3 v2 = aMesh.vertices[ibottom];
                Vector3 v3 = aMesh.vertices[itopnext];
                Vector3 v4 = aMesh.vertices[ibottomnext];
                Vector3 normal1 = (v1 - center);
                Vector3 normal2 = (v3 - center);
                normal1.y = 0f; normal2.y = 0f;
                Vector3 normal = (normal1.normalized + normal2.normalized).normalized;

                vertices.Add(v1); //Top
                vertices.Add(v2); //Bottom
                vertices.Add(v3); //Top next
                vertices.Add(v4); //bottom next
                normals.Add(normal);
                normals.Add(normal);
                normals.Add(normal);
                normals.Add(normal);
                uvs.Add(FindUVEdge(v1));
                uvs.Add(FindUVEdge(v2));
                uvs.Add(FindUVEdge(v3));
                uvs.Add(FindUVEdge(v4));

                triangles.Add(vertices.Count - 4); //Top
                triangles.Add(vertices.Count - 3); //Bottom
                triangles.Add(vertices.Count - 1); //Bottom next
                triangles.Add(vertices.Count - 4); //Top
                triangles.Add(vertices.Count - 1); //Bottom next
                triangles.Add(vertices.Count - 2); //Top next
            }

            aMesh.vertices = vertices.ToArray();
            aMesh.triangles = triangles.ToArray();
            aMesh.normals = normals.ToArray();
            aMesh.uv = uvs.ToArray();
        }

        private void AddMeshFace(Mesh aMesh, Vector3 normal, float elevation, bool local)
        {
            List<Vector3> vertices = new List<Vector3>(aMesh.vertices);
            List<Vector3> normals = new List<Vector3>(aMesh.normals);
            List<Vector2> uvs = new List<Vector2>(aMesh.uv);
            List<int> triangles = new List<int>(aMesh.triangles);
            int nb_vertices = vertices.Count;

            Vector3 center = FindZoneCenter();
            if(local)
                center -= transform.position;
            center.y = elevation;

            vertices.Add(center);
            normals.Add(normal);
            uvs.Add(FindUV(center));

            for (int j = 0; j < points.Length; j++)
            {
                Vector3 point = points[j].position;
                if (local)
                    point -= transform.position;
                point.y = elevation;

                vertices.Add(point);
                normals.Add(normal);
                uvs.Add(FindUV(point));

                if (normal.y > 0f)
                {
                    if (j == points.Length - 1)
                    {
                        triangles.Add(nb_vertices);
                        triangles.Add(vertices.Count - 1);
                        triangles.Add(nb_vertices + 1);
                        
                    }
                    else
                    {
                        triangles.Add(nb_vertices);
                        triangles.Add(vertices.Count - 1);
                        triangles.Add(vertices.Count);
                    }
                }
                else
                {
                    if (j == points.Length - 1)
                    {
                        triangles.Add(nb_vertices);
                        triangles.Add(nb_vertices + 1);
                        triangles.Add(vertices.Count - 1);
                        
                    }
                    else
                    {
                        triangles.Add(nb_vertices);
                        triangles.Add(vertices.Count);
                        triangles.Add(vertices.Count - 1);
                    }
                }
            }

            aMesh.vertices = vertices.ToArray();
            aMesh.triangles = triangles.ToArray();
            aMesh.normals =  normals.ToArray();
            aMesh.uv =  uvs.ToArray();

            //aMesh.RecalculateNormals();
        }

        private Vector2 FindUV(Vector3 pos)
        {
            return new Vector2(pos.x, pos.z);
        }

        private Vector2 FindUVEdge(Vector3 pos)
        {
            return new Vector2((pos.x + pos.z), pos.y);
        }

        private Vector3 FindZoneCenter()
        {
            Vector3 center = Vector3.zero;
            for (int j = 0; j < points.Length; j++)
            {
                center += points[j].position;
            }
            center = center / points.Length;
            return center;
        }

        private void OnDrawGizmos()
        {
            if (IsTerrainGenerated())
                return;

            //Display the voronoi diagram
            Color random_color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            Gizmos.color = MaterialTool.HasColor(data.floor_material) ? data.floor_material.color : random_color;

            Mesh triangleMesh = new Mesh();
            AddMeshFace(triangleMesh, Vector3.up, 0f, false);

            Gizmos.DrawMesh(triangleMesh);

            //Display the sites
            //Gizmos.color = Color.white;
            //Gizmos.DrawSphere(center.transform.position, 0.2f);
        }
        
        [System.Serializable]
        public class AdjacentAndPoints
        {
            public Transform adjacent;
            public Transform p1;
            public Transform p2;
            public bool noise;

            public AdjacentAndPoints(Transform adjacent, Transform p1, Transform p2, bool noise = false)
            {
                this.adjacent = adjacent;
                this.p1 = p1;
                this.p2 = p2;
                this.noise = noise;
            }
        }
    }

}