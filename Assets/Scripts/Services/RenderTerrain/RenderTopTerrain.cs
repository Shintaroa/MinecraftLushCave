using Aquarium.Terrain.SpawnTerrain;
using Aquarium.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
namespace Aquarium.Terrain.RenderTerrain
{
    [System.Serializable]
    public class RenderTopParameter
    {
        public float seed;

        public Material clay;

        public Material deepslate;

        public Material diorite;

        public Material granite;

        public Material gravel;

        public Material moss_block;

        public Material stone;

        [Range(0,1)]
        public float clay_weight = 0.33f;

        [Range(0, 1)]
        public float moss_weight = 0.33f;
        
        [Range(0, 1)]
        public float stone_weight = 0.33f;

        [HideInInspector]
        public int zArea = 10;
        [HideInInspector]
        public int xArea = 10;
        [HideInInspector]
        public BlockInfo b_i;
        [HideInInspector]
        public Vector4[] columns;
        [Range(0f, 20)]
        public float maxColumnBlock = 3;
        [Range(0f, 20)]
        public float minColumnBlock = 1;
    }

    public class RenderTopTerrain
    {
        public RenderTopParameter rp;

        private RenderTopTerrain() { }

        public RenderTopTerrain(RenderTopParameter rp) 
        {
            this.rp = rp;
        }

        public Vector2[] RandomMaterial(uint count, float seed) 
        {
            Vector2[] a = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                float x = lerp(-(int)(rp.xArea / 2), (int)(rp.xArea / 2), Tool.random(i + seed + seed));
                float y = lerp(-(int)(rp.zArea / 2), (int)(rp.zArea / 2), Tool.random(i + seed + seed + seed));
                a[i] = new Vector2(x, y);
            }
            return a;
        }

        private bool isColumn(int x,  int z)
        {
            for (int i = 0; i < rp.columns.Length; i++)
            {
                float block = lerp(rp.minColumnBlock, rp.maxColumnBlock + 0.999f, Tool.random(rp.seed + i + 182.23f));
                if (block > 0f)
                {
                    Vector2 v = new Vector2(x, z) - new Vector2(rp.columns[i].x, rp.columns[i].w);
                    if (block >= v.magnitude)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public RenderInfo Render() 
        {
            float weight = max(0.001f, rp.clay_weight) + max(0.001f, rp.moss_weight) + max(0.001f, rp.stone_weight);
            float clay_weight = rp.clay_weight / weight;
            float stone_weight =rp.stone_weight / weight;
            uint clay_count = (uint)lerp(1, (float)(rp.zArea + rp.xArea) / 20+ 0.9999f, Tool.random(rp.seed + 102.13f));
            uint stone_count = (uint)lerp(1, (float)(rp.zArea + rp.xArea) / 20 + 0.9999f, Tool.random(rp.seed + 453.12f));

            Vector2[] stones = RandomMaterial(stone_count, rp.seed + 111.111f);
            Vector2[] clays = RandomMaterial(clay_count, rp.seed + 222.222f);

            List<Vector2Int> moss_indexs = new List<Vector2Int>();
            List<Vector2Int> stone_indexs = new List<Vector2Int>();
            List<Vector2Int> clay_indexs = new List<Vector2Int>();

            RenderInfo r_i = new RenderInfo();

            for (int i = -(int)(rp.xArea / 2); i < (int)(rp.xArea / 2); i++) 
            {
                for (int j = -(int)(rp.zArea / 2); j < (int)(rp.zArea / 2); j++)
                {
                    {
                        //stones
                        for (int k = 0; k < stones.Length; k++)
                        {
                            float il = i - stones[k].x;
                            float jl = j - stones[k].y;
                            float dis = sqrt(il * il + jl * jl) / sqrt(rp.xArea * 0.5f * rp.xArea * 0.5f + rp.zArea * 0.5f * rp.zArea * 0.5f);
                            if (dis < stone_weight ||  isColumn(i, j) == true)
                            {
                                Vector2Int idx = rp.b_i.idxs[i + (int)(rp.xArea / 2), j + (int)(rp.zArea / 2)];
                                List<GameObject> gbs = rp.b_i.blocks[idx];
                                foreach (GameObject gb in gbs)
                                {
                                    gb.GetComponent<Renderer>().material = rp.stone;
                                }
                                stone_indexs.Add(idx);
                                r_i.stone_indexs = stone_indexs;
                            }
                            else if (dis < stone_weight + stone_weight * 0.2f)
                            {
                                Vector2Int idx = rp.b_i.idxs[i + (int)(rp.xArea / 2), j + (int)(rp.zArea / 2)];
                                List<GameObject> gbs = rp.b_i.blocks[idx];
                                if (isColumn(i, j) == false && Tool.filpACoin(131.742f + rp.seed + k + j *332.983f+ i * 100.022f) == true)
                                {
                                    foreach (GameObject gb in gbs)
                                    {
                                        gb.GetComponent<Renderer>().material = rp.stone;
                                    }
                                    stone_indexs.Add(idx);
                                    r_i.stone_indexs = stone_indexs;
                                }
                            }
                        }
                    }
                    {
                        //moss
                        for (int k = 0; k < clays.Length; k++)
                        {
                            float il = i - clays[k].x;
                            float jl = j - clays[k].y;
                            float dis = sqrt(il * il + jl * jl) / sqrt(rp.xArea * 0.5f * rp.xArea * 0.5f + rp.zArea * 0.5f * rp.zArea * 0.5f);
                            if (dis < clay_weight)
                            {
                                Vector2Int idx = rp.b_i.idxs[i + (int)(rp.xArea / 2), j + (int)(rp.zArea / 2)];
                                List<GameObject> gbs = rp.b_i.blocks[idx];
                                if (isColumn(i, j) == false)
                                {
                                    foreach (GameObject gb in gbs)
                                    {
                                        gb.GetComponent<Renderer>().material = rp.clay;
                                    }
                                    if (stone_indexs.Contains(idx))
                                    {
                                        stone_indexs.Remove(idx);
                                    }
                                    clay_indexs.Add(idx);
                                }
                                r_i.clay_indexs = clay_indexs;
                            }
                            else if (dis < clay_weight + clay_weight * 0.2f) 
                            {
                                Vector2Int idx = rp.b_i.idxs[i + (int)(rp.xArea / 2), j + (int)(rp.zArea / 2)];
                                List<GameObject> gbs = rp.b_i.blocks[idx];
                                if (isColumn(i, j) == false && Tool.filpACoin(231.123f + rp.seed + k + j * 901.12f + i *300.233f) == true)
                                {
                                    foreach (GameObject gb in gbs)
                                    {
                                        gb.GetComponent<Renderer>().material = rp.clay;
                                    }
                                    if (stone_indexs.Contains(idx))
                                    {
                                        stone_indexs.Remove(idx);
                                    }
                                    clay_indexs.Add(idx);
                                }
                                r_i.clay_indexs = clay_indexs;
                            }
                        }
                    }
                    {
                        //moss
                        Vector2Int idx = rp.b_i.idxs[i + (int)(rp.xArea / 2), j + (int)(rp.zArea / 2)];
                        if (!stone_indexs.Contains(idx) && !clay_indexs.Contains(idx)) 
                        {
                            List<GameObject> gbs = rp.b_i.blocks[idx];
                            foreach (GameObject gb in gbs)
                            {
                                gb.GetComponent<Renderer>().material = rp.moss_block;
                            }
                            moss_indexs.Add(idx);
                            r_i.moss_indexs = moss_indexs;
                        }
                    }
                }
             }
            return r_i;
        }
    }
}
