using Aquarium.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
namespace Aquarium.Terrain.SpawnTerrain
{
    [System.Serializable]
    public class SpawnTerraceParameter : SpawnParameter
    {
        [Range(0, 8)]
        public int minTerraceCount = 1;
        [Range(0, 8)]
        public int maxTerraceCount = 3;
    }

    public class SpawnTerrace : SpawnTerrain<SpawnTerraceParameter>
    {

        private SpawnTerrace() { }

        public SpawnTerrace(SpawnTerraceParameter sp)
        {
            this.sp = sp;
        }
        public new SpawnTerraceParameter sp { get; set; }

        public Dictionary<TerrainType, Vector3[]> SpawnDetail()
        {
            Dictionary<TerrainType, Vector3[]> tp_d = new Dictionary<TerrainType, Vector3[]>();
            uint terrace_count = (uint)lerp(sp.minTerraceCount, (float)sp.maxTerraceCount + 0.9999f, Tool.random(sp.seed + 345.366f));
            Vector3[] terraces = new Vector3[terrace_count];
            for (int i = 0; i < terrace_count; i++)
            {
                float x = Tool.random(i + sp.seed + 1221.11f) * (sp.xArea + 2.0f) - ((sp.xArea + 2.0f) * 0.5f);
                x = max((-sp.xArea / 2.0f), min((sp.xArea / 2.0f), x));
                float z = Tool.random(i + sp.seed + 134.43f) * (sp.zArea + 2.0f) - ((sp.zArea + 2.0f) * 0.5f);
                z = max((-sp.zArea / 2.0f), min((sp.zArea / 2.0f), z));
                terraces[i] = new Vector4(x, 1, z);
            }
            tp_d.Add(TerrainType.Terrace, terraces);
            return tp_d;
        }
     }
}