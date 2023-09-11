using System.Collections;
using System.Collections.Generic;
using static Unity.Mathematics.math;
using UnityEngine;
using Aquarium.Utility;

namespace Aquarium.Terrain.SpawnTerrain
{
    [System.Serializable]
    public class SpawnTopParameter : SpawnPitAndMountParameter
    {
        [Range(0.5f, 5)]
        public float pitIntensity = 1;
        [Range(0.5f, 5)]
        public float moundIntensity = 1;

        [Range(0.5f, 20)]
        public float minMoundIntensity = 1;
        [Range(0.5f, 20)]
        public float maxMoundIntensity = 1;

        [Range(0.5f, 20)]
        public float minPitIntensity = 1;
        [Range(0.5f, 20)]
        public float maxPitIntensity = 1;

        [Range(0.5f, 4)]
        public float minMoundPowIntensity = 1;
        [Range(0.5f, 4)]
        public float maxMoundPowIntensity = 2;

        [Range(0.5f, 4)]
        public float minPitPowIntensity = 1;
        [Range(0.5f, 4)]
        public float maxPitPowIntensity = 3;

        [Range(0.5f, 5)]
        public float contrastIntensity = 1;

        [Range(0f, 20)]
        public float maxColumnBlock = 3;
        [Range(0f, 20)]
        public float minColumnBlock = 1;


        [Range(0.5f, 3.0f)]
        public float maxColumnIntensity = 3;
        [Range(0.5f, 3.0f)]
        public float minColumnIntensity = 1;

        [HideInInspector]
        public Vector2[] pits;
        [HideInInspector]
        public Vector2[] mounds;
        [HideInInspector]
        public Vector4[] columns;
        [HideInInspector]
        public int[,] b_y;
    }

    public class SpawnTopTerrain : SpawnTerrain<SpawnParameter>
    {

        private int height;

        int[,] ij_deep;

        public new SpawnTopParameter sp { get ; set; }

        private SpawnTopTerrain() { }

        private BlockInfo bi;

        public SpawnTopTerrain(SpawnTopParameter sp) 
        {
            this.sp = sp;
        }


        private void GenerateRim(int y, int i, int j, int[,] ij_y, Vector2Int[,] idxs)
        {
            ij_deep[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)] = 0;
            y = -y;
            int l_y = y;
            int b_y = y;
            if (i > -(int)(sp.xArea / 2))
            {
                l_y = -ij_y[i + (int)(sp.xArea / 2) - 1, j + (int)(sp.zArea / 2)];
            }
            if (j > -(int)(sp.zArea / 2))
            {
                b_y = -ij_y[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2) - 1];
            }

            int u_l_y = l_y + 2;
            int u_b_y = b_y + 2;
            int u_y = y + 2;

            int g_l = u_l_y - u_y;
            int g_b = u_b_y - u_y;

            if (abs(l_y - y) > 1 || abs(b_y - y) > 1)
            {
                int b = 0;

                if (g_l > 1)
                {
                    int deep = ij_deep[i + (int)(sp.xArea / 2) - 1, j + (int)(sp.zArea / 2)];
                    if (deep < g_l)
                    {
                        if (deep >= 1)
                        {
                            for (int k = deep; k < g_l; k++)
                            {
                                Vector2Int idx = idxs[i + (int)(sp.xArea / 2) - 1, j + (int)(sp.zArea / 2)];
                                bi.blocks[idx].Add(Instantiate(new Vector3(i - 1, -l_y + k, j) + sp.parent.position, sp.gameObject, sp.parent));
                            }
                        }
                        else 
                        {
                            for (int k = 1; k < g_l; k++)
                            {
                                Vector2Int idx = idxs[i + (int)(sp.xArea / 2) - 1, j + (int)(sp.zArea / 2)];
                                bi.blocks[idx].Add(Instantiate(new Vector3(i - 1, -l_y + k, j) + sp.parent.position, sp.gameObject, sp.parent));
                            }
                        }
                    }
                }
                if (g_b > 1)
                {
                    int deep = ij_deep[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2) - 1];
                    if (deep < g_b)
                    {
                        if (deep >= 1)
                        {
                            for (int k = deep; k < g_b; k++)
                            {
                                Vector2Int idx = idxs[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2) - 1];
                                bi.blocks[idx].Add(Instantiate(new Vector3(i, -b_y + k, j - 1) + sp.parent.position, sp.gameObject, sp.parent));
                            }
                        }
                        else 
                        {
                            for (int k = 1; k < g_b; k++)
                            {
                                Vector2Int idx = idxs[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2) - 1];
                                bi.blocks[idx].Add(Instantiate(new Vector3(i, -b_y + k, j - 1) + sp.parent.position, sp.gameObject, sp.parent));
                            }
                        }
                        ij_deep[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2) - 1] = g_b;
                    }
                }

                if (g_l < -1 && g_b < -1)
                {
                    b = min(g_l, g_b);
                }
                else if (g_l < -1)
                {
                    b = g_l;
                }
                else if (g_b < -1)
                {
                    b = g_b;
                }
                b = abs(b);
                for (int k = 1; k < b; k++)
                {
                    Vector2Int idx = idxs[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)];
                    bi.blocks[idx].Add(Instantiate(new Vector3(i, -y + k, j) + sp.parent.position, sp.gameObject, sp.parent));
                }
                ij_deep[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)] = b;
            }
            else 
            {
                ij_deep[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)] = max((g_l == -1 ? 1 : 0), (g_b == -1 ? 1 : 0));
            }
        }

        protected new int GenerateTerrainY(float dis, int p_l, int m_l, int height)
        {
            int y = (int)lerp(0f, height + 0.999f, dis / (p_l + m_l));
            y = -min(max(-2, y), height);
            return y;
        }

        private int GenerateColumn(int x, int y, int z)
        {
            for (int i = 0; i < sp.columns.Length; i++)
            {
                float block = lerp(sp.minColumnBlock, sp.maxColumnBlock + 0.999f, Tool.random(sp.seed + i + 182.23f));
                if (block > 0f)
                {
                    Vector2 v = new Vector2(x, z) - new Vector2(sp.columns[i].x, sp.columns[i].w);
                    if (block >= v.magnitude)
                    {
                        y -= (int)(pow((1 - v.magnitude / block), lerp(sp.minColumnIntensity, sp.maxColumnIntensity, Tool.random(sp.seed + i + 282.25f))) * (int)sp.columns[i].y);
                        if (Tool.filpACoin(sp.seed + 1.53f) == true && v.magnitude < 1)
                        {
                            y--;
                        }
                    }
                }
            }
            return max(y, -((sp.height * 4 ) - 3 - 10 -(sp.b_y[x + (int)(sp.xArea / 2), z + (int)(sp.zArea / 2)]))); //(int)-(sp.height * 4));
        }

        private BlockInfo GenerateTerrains(Vector2[] mounds, Vector2[] pits)
        {
            int[,] ij_y = new int[(int)(sp.xArea / 2) * 2, (int)(sp.zArea / 2) * 2];
            float[] pitIntensitys = new float[pits.Length];
            float[] moundIntensitys = new float[mounds.Length];
            float[] pitPowIntensitys = new float[pits.Length];
            float[] moundPowIntensitys = new float[mounds.Length];
           
            ij_deep = new int[(int)(sp.xArea / 2) * 2, (int)(sp.zArea / 2) * 2];
            for (int i = 0; i < pits.Length; i++)
            {
                pitIntensitys[i] = lerp(sp.minPitIntensity, sp.maxPitIntensity, Tool.random(i + 1279.92f));
                pitPowIntensitys[i] = lerp(sp.minPitPowIntensity, sp.maxPitPowIntensity, Tool.random(i + 679.32f));
            }
            for (int i = 0; i < mounds.Length; i++)
            {
                moundIntensitys[i] = lerp(sp.minMoundIntensity, sp.maxMoundIntensity, Tool.random(i + 3873.22f));
                moundPowIntensitys[i] = lerp(sp.minMoundIntensity, sp.maxMoundIntensity, Tool.random(i + 2875.1233f));
            }

            bi = new BlockInfo();
            Vector2Int[,] idxs = new Vector2Int[(int)(sp.xArea / 2) * 2, (int)(sp.zArea / 2) * 2];

            for (int i = -(int)(sp.xArea / 2); i < (int)(sp.xArea / 2); i++)
            {
                for (int j = -(int)(sp.zArea / 2); j < (int)(sp.zArea / 2); j++)
                {
                    float dis = 0.0f;
                    dis = GenerateTerrainDis(dis, pits, mounds, i, j, moundIntensitys, pitIntensitys, sp.contrastIntensity,
                        sp.xArea, sp.zArea, sp.moundIntensity, sp.pitIntensity, pitPowIntensitys, moundPowIntensitys);
                    int y = GenerateTerrainY(dis, pits.Length, mounds.Length, height);
                    y = GenerateColumn(i, y, j);

                    Vector2Int idx = new Vector2Int();
                    idx.x = i;
                    idx.y = j;
                    idxs[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)] = idx;
                    List<GameObject> gl = new List<GameObject>();
                    gl.Add(Instantiate(new Vector3(i, y - 0.5f, j) + sp.parent.position, sp.gameObject, sp.parent));
                    bi.blocks.Add(idx, gl);
                    ij_y[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)] = y;
                    GenerateRim(y, i, j, ij_y, idxs);
                }
            }
            bi.ij_deep = ij_deep;
            bi.ij_y = ij_y;
            bi.idxs = idxs;
            return bi;
        }

        public new BlockInfo Spawn() 
        {
            height = sp.height - 2;
            return GenerateTerrains(sp.pits, sp.mounds);
        }
    }
}
