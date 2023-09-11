using Aquarium.Terrain.RenderTerrain;
using Aquarium.Terrain.SpawnTerrain;
using Aquarium.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
namespace Aquarium.Terrain.SpawnDecoration
{
    [System.Serializable]
    public class SpawnBottomDecorationParameter
    {
        public float seed;

        [HideInInspector]
        public int zArea = 10;
        [HideInInspector]
        public int xArea = 10;

        [HideInInspector]
        public int height = 10;

        public GameObject azaleavox;

        public GameObject brown_mushroom;

        public GameObject floweringazaleavox;

        public GameObject grass;

        public GameObject red_mushroom;

        public GameObject sea_grass;

        public GameObject tall_grass;

        public GameObject tall_seagrass;

        public GameObject small_dripleaf;

        public GameObject big_dripleaf_stem;

        public GameObject big_dripleaf_leaf;

        public GameObject moss_carpet;

        public Transform parent;

        [HideInInspector]
        public RenderInfo r_i;

        [HideInInspector]
        public BlockInfo b_b_i;

        [HideInInspector]
        public BlockInfo t_b_i;

        [HideInInspector]
        public WaterInfo w_i;
    }

    public class SpawnBottomDecoration
    {
        public SpawnBottomDecorationParameter sp;

        private SpawnBottomDecoration() { }

        public SpawnBottomDecoration(SpawnBottomDecorationParameter sp) 
        {
            this.sp = sp;
        }

        public bool IsSeabed(Vector2Int idx) 
        {
            if (sp.r_i.water_indexs.ContainsKey(idx)) 
            {
               return true;
            }
            return false;
        }

        public int GetSeabedDistance(Vector3Int idx)
        {
            if (sp.r_i.water_indexs.ContainsKey(new Vector2Int(idx.x,idx.z)))
            {
                return sp.r_i.water_indexs[new Vector2Int(idx.x, idx.z)] - idx.y;
            }
            return 0;
        }

        public int GetHeight(Vector3Int idx)
        {
            if (sp.r_i.water_indexs.ContainsKey(new Vector2Int(idx.x, idx.z)))
            {
                return sp.r_i.water_indexs[new Vector2Int(idx.x, idx.z)];
            }
            return idx.y;
        }

        public int GetDistance(Vector2Int idx) 
        {
            int t = sp.t_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)];
            int b = sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)];//GetHeight(new Vector3Int(idx.x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)], idx.y));
            t = t + sp.height - 2 + (int)(sp.height * 0.5f);
            return t-b ;
        }

        public void Spawn() 
        {
            {
                //moss
                foreach (Vector2Int idx in sp.r_i.moss_indexs) 
                {
                    int d = GetDistance(idx);
                    int s_d = GetSeabedDistance(new Vector3Int(idx.x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)], idx.y));
                    if (Tool.filpACoin(sp.seed + 899.742f + idx.x * 22.98f + idx.y * idx.y * 323.1f, 0.9f) && d > 1)
                    {
                        float x = lerp(-0.1f, 0.1f, Tool.random(2.233f * idx.x + 2332.23f));
                        float y = lerp(-0.1f, 0.1f, Tool.random(99.22f * idx.y + 1232.23f));
                        if (s_d >= 1)
                        {
                            //偏移

                            GameObject.Instantiate(sp.sea_grass, new Vector3(idx.x + x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + 1 + sp.parent.position.y, idx.y + y),
                            Quaternion.identity, sp.parent);
                        }
                        else
                        {
                            GameObject.Instantiate(sp.grass, new Vector3(idx.x + x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + 1 + sp.parent.position.y, idx.y + y),
                                Quaternion.identity, sp.parent);
                        }
                    }
                    else if (Tool.filpACoin(sp.seed + 99.742f + idx.x * 121.98f + idx.y * idx.y * 633.12f, 0.9f) && d > 2)
                    {
                        float x = lerp(-0.1f, 0.1f, Tool.random(2.233f * idx.x + 2332.23f));
                        float y = lerp(-0.1f, 0.1f, Tool.random(99.22f * idx.y + 1232.23f));
                        if (s_d >= 2)
                        {
                            GameObject.Instantiate(sp.tall_seagrass, new Vector3(idx.x + x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + 1 + sp.parent.position.y, idx.y + y),
                            Quaternion.identity, sp.parent);
                        }
                        else if (s_d >= 1)
                        {
                            //偏移

                            GameObject.Instantiate(sp.sea_grass, new Vector3(idx.x + x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + 1 + sp.parent.position.y, idx.y + y),
                            Quaternion.identity, sp.parent);
                        }
                        else
                        {

                            GameObject.Instantiate(sp.tall_grass, new Vector3(idx.x + x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + 1 + sp.parent.position.y, idx.y + y),
                                Quaternion.identity, sp.parent);
                        }
                    }
                    else if (Tool.filpACoin(sp.seed + 992.742f + idx.x * 1222.98f + idx.y * idx.y * 633.322f, 0.8f) && d > 1)
                    {
                        GameObject.Instantiate(sp.moss_carpet, new Vector3(idx.x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + 1 + sp.parent.position.y, idx.y),
                        Quaternion.identity, sp.parent);
                    } else if (Tool.filpACoin(sp.seed + 992.742f + idx.x * 522.98f + idx.y  * 299.32f, 0.9f) && d > 1 && s_d == 0) 
                    {
                        if (Tool.filpACoin(sp.seed + 192.742f + idx.x * 222.98f + idx.y * 599.32f,0.1f))
                        {
                            GameObject.Instantiate(sp.azaleavox, new Vector3(idx.x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + 1 + sp.parent.position.y, idx.y),
                           Quaternion.identity, sp.parent);
                        }
                        else 
                        {
                            GameObject.Instantiate(sp.floweringazaleavox, new Vector3(idx.x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + 1 + sp.parent.position.y, idx.y),
                         Quaternion.identity, sp.parent);
                        }
                    }
                }
            }
            {
                //clay
                foreach (Vector2Int idx in sp.r_i.clay_indexs)
                {
                        int y = sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)];
                        int s_d = GetSeabedDistance(new Vector3Int(idx.x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)], idx.y));
                        int d = GetDistance(idx);
                        if (s_d == 1 && d > 1)
                        {
                        if (Tool.filpACoin(sp.seed + 899.742f + idx.x * 22.98f + idx.y * 323.1f, 0.8f))
                        {
                            GameObject.Instantiate(sp.sea_grass, new Vector3(idx.x , sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + 1.5f + sp.parent.position.y, idx.y  ),
                           Quaternion.identity, sp.parent);
                        }
                         else  if (Tool.filpACoin(sp.seed + 299.342f + idx.x * 22.938f + idx.y * 98.132f))
                            {
                                GameObject.Instantiate(sp.small_dripleaf, new Vector3(idx.x,
                                    y + 1 + sp.parent.position.y, idx.y),
                                Tool.filpACoin(sp.seed + 199.242f + idx.x * 52.938f + idx.y * 91.12f, 0.3f) ? Quaternion.identity : Quaternion.Euler(0f,180f,0f), sp.parent);
                            }
                        }
                        else if(d > 1)
                        {
                        Quaternion q = Tool.filpACoin(sp.seed + 199.242f + idx.x * 52.938f + idx.y * 91.12f, 0.3f) ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);
                        if (s_d > 1)
                        {
                            if (Tool.filpACoin(sp.seed + 899.742f + idx.x * 22.98f + idx.y * 323.1f, 0.8f))
                            {
                                GameObject.Instantiate(sp.sea_grass, new Vector3(idx.x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + +1.5f + sp.parent.position.y, idx.y),
                               Quaternion.identity, sp.parent);
                            }
                            else if (Tool.filpACoin(sp.seed + 99.742f + idx.x * 121.98f + idx.y * idx.y * 633.12f, 0.9f) && d > 2) 
                            {
                                GameObject.Instantiate(sp.tall_seagrass, new Vector3(idx.x, sp.b_b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] + +1.5f + sp.parent.position.y, idx.y ),
                                Quaternion.identity, sp.parent);
                            }
                            else if (Tool.filpACoin(sp.seed + 299.342f + idx.x * 22.938f + idx.y * 98.132f,0.8f))
                            {
                                //todo:加上水面高度
                                int t = (int)lerp(s_d, s_d+3 + 0.9999f, Tool.random(sp.seed + 299.342f + idx.x * 22.938f + idx.y * 933.132f));
                                t = min(t, d);
                                for (int i = 1; i <= t; i++)
                                {
                                    GameObject.Instantiate(sp.big_dripleaf_stem, new Vector3(idx.x,
                                   y + i + sp.parent.position.y + 0.5f, idx.y),
                                    q, sp.parent);
                                    if (i == t)
                                    {
                                        GameObject.Instantiate(sp.big_dripleaf_leaf, new Vector3(idx.x,
                                       y + i + sp.parent.position.y + 0.5f, idx.y),
                                        q, sp.parent);
                                    }
                                }
                            }
                        }
                        else 
                        {
                            //地面上的荷叶 越是高越少
                            float rate = ((float)y + (float)sp.height * 0.5f)/ (float)sp.height * 0.1f;
                            if (Tool.filpACoin(sp.seed+299.342f + idx.x * 22.938f + idx.y * 933.132f, 0.9f + rate))
                            {
                                int t = (int)lerp(1, 5, Tool.random(sp.seed + 299.342f + idx.x * 22.938f + idx.y * 98.132f));
                                t = min(t, d);
                                for (int i = 1; i <= t; i++)
                                {
                                    GameObject.Instantiate(sp.big_dripleaf_stem, new Vector3(idx.x,
                                    y + i + sp.parent.position.y + 0.5f, idx.y),
                                    q, sp.parent);
                                    if (i == t)
                                    {
                                        GameObject.Instantiate(sp.big_dripleaf_leaf, new Vector3(idx.x,
                                       y + i + sp.parent.position.y + 0.5f, idx.y),
                                        q, sp.parent);
                                    }
                                }
                            }
                            }
                        }
                }
            }
        }
    }
}
