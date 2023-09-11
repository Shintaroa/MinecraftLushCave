using Aquarium.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
namespace Aquarium.Terrain.SpawnTerrain
{
    public class SpawnWaterParameter : SpawnParameter
    {
       public BlockInfo b_i;

        [HideInInspector]
        public Vector3[] pools;
        [HideInInspector]
        public Vector4[] columns;

        [Range(0f, 20)]
        public float maxColumnBlock = 3;
        [Range(0f, 20)]
        public float minColumnBlock = 1;

        [Range(0f, 20)]
        public float maxPoolBlock = 3;
        [Range(0f, 20)]
        public float minPoolBlock = 1;

    }

    public class WaterInfo
    {
        public  List<GameObject> blocks = new List<GameObject>();

        public  Vector4[] water_l_r_h; //location ratius height
    }

    public class SpawnWater : SpawnTerrain<SpawnWaterParameter>
    {

        private SpawnWater() { }

        public SpawnWater(SpawnWaterParameter sp)
        {
            this.sp = sp;
        }

  /*      private bool isColumn(int x, int z)
        {
            for (int i = 0; i < sp.columns.Length; i++)
            {
                float block = lerp(sp.minColumnBlock, sp.maxColumnBlock + 0.999f, Tool.random(sp.seed + i + 182.23f));
                if (block > 0f)
                {
                    Vector2 v = new Vector2(x, z) - new Vector2(sp.columns[i].x, sp.columns[i].w);
                    if (block >= v.magnitude)
                    {
                        return true;
                    }
                }
            }
            return false;
        }*/

        List<Vector2Int>[] pools;
        List<Vector2Int>[] shores;

        private void GetPoolAndShore()
        {
            pools = new List<Vector2Int>[sp.pools.Length];
            shores = new List<Vector2Int>[sp.pools.Length];
            for (int i = 0; i < sp.pools.Length; i++)
            {
                float block = lerp(sp.minPoolBlock, sp.maxPoolBlock, Tool.random(sp.seed + i + 539.127f));
                pools[i] = new List<Vector2Int>();
                shores[i] = new List<Vector2Int>();
                for (int x = (int)(-block + sp.pools[i].x - 1); x <= block + sp.pools[i].x + 1; x++)
                {
                    for (int y = (int)(-block + sp.pools[i].z - 1); y <=block + sp.pools[i].z + 1; y++)
                    {
                        Vector2 v = new Vector2(x, y) - new Vector2(sp.pools[i].x, sp.pools[i].z);
                        if (v.magnitude <= block )// isColumn(x,y) == false)
                        {
                            pools[i].Add(new Vector2Int(x, y));
                            //Instantiate(new Vector3((int)(x), 20, (int)(y)) + sp.parent.position, sp.gameObject, null);
                            /*if (i == 2)
                            {
                                Instantiate(new Vector3((int)(x), 20, (int)(y)) + sp.parent.position, sp.gameObject, null);
                                //Debug.Log("sw pools:" + i + " x:" + (int)(x + v.x) + " y:" + (int)(y + v.y));
                            }*/
                        }
                        else if(v.magnitude <= block + sqrt(2))// && isColumn(x, y) == false)
                        {
                            //Instantiate(new Vector3((int)(x), 19, (int)(y)) + sp.parent.position, sp.gameObject, null);
                                /*f (i == 2)
                                     Instantiate(new Vector3((int)(x), 19, (int)(y)) + sp.parent.position, sp.gameObject, null);*/
                            shores[i].Add(new Vector2Int(x, y));
                        }
                    }
                }
            }
        }

        List<Vector2Int> connectedPools;

        private void GetConnected()
        {
            /*  Vector3[] c = new Vector3[pools.Length];
              for (int i = 0; i < pools.Length; i++)
              {
                  float block = lerp(sp.minPoolBlock, sp.maxPoolBlock, Tool.random(sp.seed + i + 539.127f));
                  Vector2 v = new Vector2(sp.pools[i].x, sp.pools[i].z);
                  c[i] = new Vector3(v.x, v.y, block);
              }
              for (int i = 0; i < c.Length; i++)
              {
                  for (int j = i + 1; j < c.Length; j++)
                  {
                      float dis = (c[i] - c[j]).magnitude;
                      if (dis > (c[i].z + c[i].z))
                      {
                          connect_pool_idx.Add(new Vector2Int(i, j));
                      }
                  }
              }*/

            connectedPools = new List<Vector2Int>();
            for (int i = 0; i < pools.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    bool isConnected = false;
                    for (int ki = 0; ki < pools[i].Count; ki++)
                    {
                        for (int kj = 0; kj < pools[j].Count; kj++)
                        {
                            if (pools[i][ki].Equals(pools[j][kj]))
                            {
                                isConnected = true;
                                connectedPools.Add(new Vector2Int(i,j));
                                break;
                            }
                            if (isConnected) 
                            {
                                break;
                            }
                        }
                    }

                    if (isConnected) 
                    {
                     
                       for (int kj = 0; kj < pools[j].Count; kj++)
                       {
                            for (int ki = 0; ki < shores[i].Count; ki++)
                            {
                                if (pools[j][kj].Equals(shores[i][ki])) 
                                {
                                    shores[i].Remove(shores[i][ki]);
                                    ki--;
                                    break;
                                }
                            }
                        }

                       for (int ki = 0; ki < pools[i].Count; ki++)
                        {
                            for (int kj = 0; kj < shores[j].Count; kj++)
                            {
                                if (shores[j][kj].Equals(pools[i][ki]))
                                {
                                    shores[j].Remove(shores[j][kj]);
                                    kj--;
                                    break;
                                 }
                            }
                        }

                        for (int ki = 0; ki < shores[i].Count; ki++)
                        {
                            bool isEqu = false;
                            for (int kj = 0; kj < shores[j].Count; kj++)
                            {
                                if (shores[j][kj].Equals(shores[i][ki]))
                                {
                                    shores[j].Remove(shores[j][kj]);
                                    kj--;
                                    isEqu = true;
                                    break;
                                }
                            }
                            if (isEqu) 
                            {
                                shores[i].Remove(shores[i][ki]);
                                ki--;
                            }
                        }

                        for (int ki = 0; ki < pools[i].Count; ki++)
                        {
                            bool isEqu = false;
                            for (int kj = 0; kj < pools[j].Count; kj++)
                            {
                                if (pools[i][ki].Equals(pools[j][kj]))
                                {
                                    //REMOVE
                                    pools[j].Remove(pools[j][kj]);
                                    kj--;
                                }
                            }
                            if (isEqu) 
                            {
                                pools[i].Remove(pools[i][ki]);
                                ki--;
                            }
                        }
                        /*shores[i].AddRange(shores[j]);
                        shores[j] = shores[i];*/
                    }
                }
            }
        }

        public new SpawnWaterParameter sp { get; set; }
        public new WaterInfo Spawn()
        {
            WaterInfo w_i = new WaterInfo();
            List<GameObject> blocks = new List<GameObject>();
            Vector4[] water_l_r_h = new Vector4[sp.pools.Length];
            GetPoolAndShore();
            GetConnected();
            int[] surface_hs = new int[pools.Length];
            for (int i = 0; i < shores.Length; i++) 
            {
                int surface_h = sp.height;
                foreach (Vector2Int v in shores[i])
                {
                    if (v.x >= -(int)(sp.xArea / 2) && v.y >= -(int)(sp.zArea / 2)&& v.x < (int)(sp.xArea / 2) && v.y < (int)(sp.zArea / 2)) 
                    {
                        int h = sp.b_i.ij_y[v.x + (int)(sp.xArea / 2), v.y + (int)(sp.zArea / 2)];
                        surface_h = min(surface_h, h);
                    }
                }
                surface_hs[i] = surface_h;
            }

            foreach (Vector2Int c_p in connectedPools) 
            {
                for (int i = 0; i < surface_hs.Length; i++) 
                {
                    if (c_p.x == i || c_p.y == i) 
                    {
                        surface_hs[i] = min(surface_hs[c_p.x], surface_hs[c_p.y]);
                    }
                }
            }

            for (int i = 0; i < pools.Length; i++)
            {
                int surface_h = surface_hs[i];
                float block = lerp(sp.minPoolBlock, sp.maxPoolBlock, Tool.random(sp.seed + i + 539.127f));
                water_l_r_h[i] = new Vector4(sp.pools[i].x, sp.pools[i].z, block, surface_h);
                foreach (Vector2Int v in pools[i]) 
                {
                    if (v.x >= -(int)(sp.xArea / 2) && v.y >= -(int)(sp.zArea / 2) && v.x < (int)(sp.xArea / 2) && v.y < (int)(sp.zArea / 2))
                    {
                        int h = sp.b_i.ij_y[v.x + (int)(sp.xArea / 2), v.y + (int)(sp.zArea / 2)];
                        if (h < surface_h)
                        {
                            for (int j = h + 1;j <= surface_h;j++) 
                            {
                                GameObject gb = Instantiate(new Vector3(v.x, j, v.y) + sp.parent.position, sp.gameObject, sp.parent);
                                blocks.Add(gb);
                            }
                        }
                    }
                }
            }
            w_i.blocks = blocks;
            return w_i;
        }
     }
}