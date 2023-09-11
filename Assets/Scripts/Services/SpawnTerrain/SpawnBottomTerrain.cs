using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using Unity.Mathematics;
using Aquarium.Utility;

namespace Aquarium.Terrain.SpawnTerrain
{
    public enum TerrainType
    {
        Mound,
        Pit,
        Column,
        Pool,
        Terrace,
    }

    [System.Serializable]
    public class SpawnBottomParameter : SpawnPitAndMountParameter
    {
        [Range(0.5f, 5)]
        public float pitIntensity = 1;
        [Range(0.5f, 5)]
        public float moundIntensity = 1;

        [Range(0.5f, 20)]
        public float minMoundIntensity = 1;
        [Range(0.5f, 20)]
        public float maxMoundIntensity = 3;

        [Range(0.5f, 20)]
        public float minPitIntensity = 1;
        [Range(0.5f, 20)]
        public float maxPitIntensity = 3;

        [Range(0.5f, 4)]
        public float minMoundPowIntensity = 1;
        [Range(0.5f, 4)]
        public float maxMoundPowIntensity = 2;

        [Range(0.5f, 4)]
        public float minPitPowIntensity = 1;
        [Range(0.5f, 4)]
        public float maxPitPowIntensity = 2;

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

        [Range(0f, 20)]
        public float maxPoolBlock = 3;
        [Range(0f, 20)]
        public float minPoolBlock = 1;

        [HideInInspector]
        public Vector2[] pits;
        [HideInInspector]
        public Vector2[] mounds;
        [HideInInspector]
        public Vector4[] columns;
        [HideInInspector]
        public Vector3[] pools;
        [HideInInspector]
        public Vector3[] terraces;
    }

/*    public struct Index 
    {
        public int x;
        public int z;
    }*/

    public class BlockInfo
    {
        public Dictionary<Vector2Int, List<GameObject>> blocks = new Dictionary<Vector2Int, List<GameObject>>();

        public int[,] ij_y;

        public Vector2Int[,] idxs;

        public int[,] ij_deep;
    }

    public class SpawnBottomTerrain : SpawnTerrain<SpawnBottomParameter>
    {
        public new SpawnBottomParameter sp { get; set; }

        private int height;

        private int[,] ij_deep;

        private BlockInfo bi;

        private SpawnBottomTerrain()
        {
        }


        public SpawnBottomTerrain(SpawnBottomParameter sp) 
        {
            this.sp = sp;
        }

        private void GenerateRim( int y,int i ,int j, int[,] ij_y, Vector2Int[,] idxs) 
        {
            ij_deep[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)] = 0;
            int l_y = y;
            int b_y = y;
            if (i > -(int)(sp.xArea / 2))
            {
                l_y = ij_y[i + (int)(sp.xArea / 2) - 1, j + (int)(sp.zArea / 2)];
                /*l_dis = GenerateTerrainDis(l_dis, pits, mounds, i-1, j);
                l_y = GenerateTerrainY(l_dis, pits.Length, mounds.Length);*/
            }
            if (j > -(int)(sp.zArea / 2))
            {
                b_y = ij_y[i + (int)(sp.xArea / 2) , j + (int)(sp.zArea / 2) - 1];
                /*                b_dis = GenerateTerrainDis(b_dis, pits, mounds, i, j -1);
                                b_y = GenerateTerrainY(b_dis, pits.Length, mounds.Length);*/
            }

            int u_l_y = l_y + height;
            int u_b_y = b_y + height;
            int u_y = y + height;
            int g_l = u_l_y - u_y;
            int g_b = u_b_y - u_y;
            //如果当前区块大于左边下边则添加当前区块下的block 并且获取深度值 如果当前区块小于左右两边则在左右的根据深度下面添加block
            if (abs(u_l_y - u_y) > 1 || abs(u_b_y - u_y) > 1)
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
                                bi.blocks[idx].Add(Instantiate(new Vector3(i - 1, l_y - k, j) + sp.parent.position, sp.gameObject, sp.parent));
                            }
                        }
                        else 
                        {
                            for (int k = 1; k <= g_l; k++)
                            {
                                Vector2Int idx = idxs[i + (int)(sp.xArea / 2) - 1, j + (int)(sp.zArea / 2)];
                                bi.blocks[idx].Add(Instantiate(new Vector3(i - 1, l_y - k, j) + sp.parent.position, sp.gameObject, sp.parent));
                            }
                        }
                        ij_deep[i + (int)(sp.xArea / 2) - 1, j + (int)(sp.zArea / 2) ] = g_l;
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
                                bi.blocks[idx].Add(Instantiate(new Vector3(i, b_y - k, j - 1) + sp.parent.position, sp.gameObject, sp.parent));
                            }
                        }
                        else 
                        {
                            for (int k = 1; k <= g_b; k++)
                            {
                                Vector2Int idx = idxs[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2) - 1];
                                bi.blocks[idx].Add(Instantiate(new Vector3(i, b_y - k, j - 1) + sp.parent.position, sp.gameObject, sp.parent));
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
                /*    for (int k = 0; k < t; k++)
                    {
                        Instantiate(new Vector3(i, y + k + 1, j) + sp.parent.position, sp.gameObject, sp.parent);
                    }*/
                b = abs(b);
                for (int k = 1; k < b; k++)
                {
                    Vector2Int idx = idxs[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)];
                    bi.blocks[idx].Add(Instantiate(new Vector3(i, y - k, j) + sp.parent.position, sp.gameObject, sp.parent));
                }
                ij_deep[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)]  = b;
            }
            else
            {
                ij_deep[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)] = max((g_l == -1 ? 1 : 0), (g_b == -1 ? 1 : 0));
            }
        }

       private bool isColumn = false;
       private bool isPool= false;
       private bool isTerrace = false;
        private int GenerateColumn(int x,int y,int z) 
        {
            isColumn = false;
            for (int i = 0;i < sp.columns.Length;i++) 
            {
                float block = lerp(sp.minColumnBlock, sp.maxColumnBlock + 0.999f, Tool.random(sp.seed + i + 182.23f));
                if (block > 0f)
                {
                    Vector2 v = new Vector2(x, z) - new Vector2(sp.columns[i].x, sp.columns[i].w);
                    if (block >= v.magnitude)
                    {
                        isColumn = true;
                        y += (int)(pow((1 - v.magnitude / block), lerp(sp.minColumnIntensity, sp.maxColumnIntensity, Tool.random(sp.seed + i + 182.35f))) * (int)sp.columns[i].z);
                        if (Tool.filpACoin(sp.seed + 1.53f) == true && v.magnitude < 1)
                        {
                            y++;
                        }
                    }
                }
            }
            return min(y, (int)(height + height * 0.5f));
        }

        private int GeneratePoolBottom(int x, int y, int z) 
        {
            isPool = false;
            for (int i = 0; i < sp.pools.Length; i++)
            {
                float block = lerp(sp.minPoolBlock, sp.maxPoolBlock, Tool.random(sp.seed + i + 539.127f));
                Vector2 v = new Vector2(x, z) - new Vector2(sp.pools[i].x, sp.pools[i].z);
                if (block >= v.magnitude && isColumn == false) 
                {
                    isPool = true;
                    if (block >= v.magnitude * 1.2f)
                    {
                        y -= (int)sp.pools[i].y;
                    }
                    else 
                    {
                        y -= (int)(sp.pools[i].y * 0.7f);
                    }
                    //if (i == 2)
                    //Debug.Log("sb pools:" + i + " x:" + x + " y:" + z);
                }
            }
            return max(y, -height - 1);
        }

        private int GenerateTerraceBottom() 
        {
            return 0;
        }

        private BlockInfo GenerateTerrains(Vector2[] pits, Vector2[] mounds) 
        {
            float[] pitIntensitys = new float[pits.Length];
            float[] moundIntensitys = new float[mounds.Length];
            float[] pitPowIntensitys = new float[pits.Length];
            float[] moundPowIntensitys = new float[mounds.Length];
            for (int i = 0;i < pits.Length;i++) 
            {
                pitIntensitys[i] = lerp(sp.minPitIntensity, sp.maxPitIntensity, Tool.random(i + 12379.92f)) ;
                pitPowIntensitys[i] = lerp(sp.minPitPowIntensity, sp.maxPitPowIntensity, Tool.random(i + 1379.32f));
            }
            for (int i = 0; i < mounds.Length; i++)
            {
                moundIntensitys[i] = lerp(sp.minMoundIntensity, sp.maxMoundIntensity, Tool.random(i + 31873.22f));
                moundPowIntensitys[i] = lerp(sp.minMoundPowIntensity, sp.maxMoundPowIntensity, Tool.random(i + 998.129f));
            }

            int[,] ij_y = new int[(int)(sp.xArea / 2) * 2, (int)(sp.zArea / 2) * 2];
            ij_deep = new int[(int)(sp.xArea / 2) * 2, (int)(sp.zArea / 2) * 2];
            Vector2Int[,] idxs = new Vector2Int[(int)(sp.xArea / 2) * 2, (int)(sp.zArea / 2) * 2];

            bi = new BlockInfo();
            for (int i = -(int)(sp.xArea / 2); i < (int)(sp.xArea / 2); i++)
            {
                for (int j = -(int)(sp.zArea / 2); j < (int)(sp.zArea / 2); j++)
                {
                    float dis = 0.0f;
                    dis = GenerateTerrainDis(dis, pits, mounds, i, j,moundIntensitys, pitIntensitys, sp.contrastIntensity, sp.xArea, sp.zArea, sp.moundIntensity, sp.pitIntensity, pitPowIntensitys, moundPowIntensitys);
                    int y = GenerateTerrainY(dis, pits.Length, mounds.Length, height, sp.xArea,sp.zArea,i,j);
                    y = GenerateColumn(i ,y,j );
                    y = GeneratePoolBottom(i, y, j);
                    //todo GenerateTerraceBottom

                    Vector2Int idx = new Vector2Int();
                    idx.x = i;
                    idx.y = j;
                    idxs[i + (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)] = idx;
                    List<GameObject> gl = new List<GameObject>();
                    gl.Add(Instantiate(new Vector3(i, y, j) + sp.parent.position, sp.gameObject, sp.parent));
                    bi.blocks.Add(idx, gl);
                    ij_y[i+ (int)(sp.xArea / 2), j + (int)(sp.zArea / 2)] = y;
                    GenerateRim( y, i, j,ij_y, idxs);
                }
            }
            bi.ij_deep = ij_deep;
            bi.ij_y = ij_y;
            bi.idxs = idxs;
            return bi;
        }

        public new BlockInfo Spawn()
        {
            height = (int)(sp.height * 0.5f);
            return this.GenerateTerrains(sp.pits, sp.mounds);
        }
    }
}