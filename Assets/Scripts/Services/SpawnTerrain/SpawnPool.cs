using Aquarium.Utility;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
namespace Aquarium.Terrain.SpawnTerrain
{
    [System.Serializable]
    public class SpawnPoolParameter : SpawnParameter
    {
        [Range(0, 8)]
        public int minPoolCount = 1;
        [Range(0, 8)]
        public int maxPoolCount = 3;
        [Range(1, 10)]
        public int maxDepth = 10;
        [Range(1, 10)]
        public int minDepth = 1;
    }

    public class SpawnPool : SpawnTerrain<SpawnPoolParameter>
    {

        private SpawnPool() { }

        public SpawnPool(SpawnPoolParameter sp)
        {
            this.sp = sp;
        }
        public new SpawnPoolParameter sp { get; set; }

        public Dictionary<TerrainType, Vector3[]> SpawnDetail()
        {
            Dictionary<TerrainType, Vector3[]> tp_d = new Dictionary<TerrainType, Vector3[]>();
            uint pool_count = (uint)lerp(sp.minPoolCount, (float)sp.maxPoolCount + 0.9999f, Tool.random(sp.seed + 194.198f));
            Vector3[] pools = new Vector3[pool_count];
            for (int i = 0; i < pool_count; i++)
            {
                float x = Tool.random(i + sp.seed + 1621.01f) * (sp.xArea + 2.0f) - ((sp.xArea + 2.0f) * 0.5f);
                x = max((-sp.xArea / 2.0f), min((sp.xArea / 2.0f), x));
                float z = Tool.random(i + sp.seed + 833.033f) * (sp.zArea + 2.0f) - ((sp.zArea + 2.0f) * 0.5f);
                z = max((-sp.zArea / 2.0f), min((sp.zArea / 2.0f), z));

                int depth = (int)lerp(sp.minDepth, sp.maxDepth + 0.999f, Tool.random(i + sp.seed + 621.01f));

                pools[i] = new Vector4(x, depth, z);
            }
            tp_d.Add(TerrainType.Pool, pools);
            return tp_d;
        }
     }
}