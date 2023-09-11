using Aquarium.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
namespace Aquarium.Terrain.SpawnTerrain
{
    [System.Serializable]
    public class SpawnColumnParameter : SpawnParameter
    {
        [Range(0, 8)]
        public int minColumnCount = 1;
        [Range(0, 8)]
        public int maxColumnCount = 3;
        [Range(0, 25)]
        public int maxTopHeight = 10;
        [Range(0, 25)]
        public int minTopHeight = 0;
        [Range(0, 25)]
        public int maxBottomHeight = 10;
        [Range(0, 25)]
        public int minBottomHeight = 0;
        [Range(0, 1.0f)]
        public float topScale = 1;
        [Range(0, 1.0f)]
        public float bottomScale = 1;
    }

    public class SpawnColumn : SpawnTerrain<SpawnColumnParameter>
    {

        private SpawnColumn() { }

        public SpawnColumn(SpawnColumnParameter sp)
        {
            this.sp = sp;
        }
        public new SpawnColumnParameter sp { get; set; }

        public Dictionary<TerrainType, Vector4[]> SpawnDetail()
        {
            int maxTopHeight = min((int)(sp.xArea / 2), sp.maxTopHeight);
            int minTopHeight = min(maxTopHeight, sp.minTopHeight);
            int maxBottomHeight = min((int)(sp.xArea / 2), sp.maxBottomHeight);
            int minBottomHeight = min(maxTopHeight, sp.minBottomHeight);

            Dictionary<TerrainType, Vector4[]> tc_d = new Dictionary<TerrainType, Vector4[]>();
            uint column_count = (uint)lerp(sp.minColumnCount, (float)sp.maxColumnCount + 0.9999f, Tool.random(sp.seed + 64.98f));
            Vector4[] column = new Vector4[column_count];
            for (int i = 0; i < column_count; i++) 
            {
                float x = Tool.random(i + sp.seed + 321.021f) * (sp.xArea + 2.0f) - ((sp.xArea + 2.0f) * 0.5f);
                x = max((-sp.xArea / 2.0f), min((sp.xArea / 2.0f), x));
                float z = Tool.random(i + sp.seed + 133.233f) * (sp.zArea + 2.0f) - ((sp.zArea + 2.0f) * 0.5f);
                z = max((-sp.zArea / 2.0f), min((sp.zArea / 2.0f), z));

                int y_t = (int)(lerp(minTopHeight, maxTopHeight+0.999f, Tool.random(i + sp.seed + 2631.01f)) * sp.topScale);
                int y_b = (int)(lerp(maxBottomHeight, minBottomHeight + 0.999f, Tool.random(i + sp.seed + 8748.344f)) * sp.bottomScale);

                column[i] = new Vector4(x, y_t, y_b, z);
            }
            tc_d.Add(TerrainType.Column, column);
            return tc_d;
        }
     }
}