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
    public class SpawnTopDecorationParameter
    {
        public float seed;

        [HideInInspector]
        public int zArea = 10;
        [HideInInspector]
        public int xArea = 10;

        public GameObject cave_vines;

        public GameObject cave_vines_lit;

        public GameObject cave_vines_plant;

        public GameObject cave_vines_plant_lit;

        public GameObject chain;

        public GameObject spore_blossom;

        public Transform parent;

        [HideInInspector]
        public RenderInfo r_i;

        [HideInInspector]
        public BlockInfo b_i;
    }

    public class SpawnTopDecoration
    {
        public SpawnTopDecorationParameter sp;

        private SpawnTopDecoration() { }

        public SpawnTopDecoration(SpawnTopDecorationParameter sp) 
        {
            this.sp = sp;
        }

        public bool isSeabed(Vector2Int idx) 
        {
            if (sp.r_i.water_indexs.ContainsKey(idx))
            {
                return true;
            }
            return false;
        }

        public void Spawn() 
        {
            {
                //moss
                foreach (Vector2Int idx in sp.b_i.idxs) 
                {

                    if (Tool.filpACoin(899.742f + idx.x * 22.98f + idx.y * 323.1f, 0.9f))
                    {
                        int h = (int)lerp(1.0f, 10.0f + 0.9999f, Tool.random(899.742f + idx.x * 122.22f + idx.y * 762.1f));
                        for (int y = 1; y <= h; y++) {
                            if (y < h)
                            {
                                if (Tool.filpACoin(899.742f + idx.x * 22.98f + idx.y * 323.1f + y * 235.398f))
                                {
                                    GameObject.Instantiate(sp.cave_vines_plant_lit, new Vector3(idx.x, sp.b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] - y + sp.parent.position.y, idx.y),
                                     Quaternion.identity, sp.parent);
                                }
                                else 
                                {
                                    GameObject.Instantiate(sp.cave_vines_plant, new Vector3(idx.x, sp.b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] - y + sp.parent.position.y, idx.y),
                                        Quaternion.identity, sp.parent);
                                }
                            }
                            else
                            {
                                if (Tool.filpACoin(899.742f + idx.x * 232.98f + idx.y * 223.1f + y * 935.398f))
                                {
                                    GameObject.Instantiate(sp.cave_vines_lit, new Vector3(idx.x, sp.b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] - y + sp.parent.position.y, idx.y),
                                     Quaternion.identity, sp.parent);
                                }
                                else
                                {
                                    GameObject.Instantiate(sp.cave_vines, new Vector3(idx.x, sp.b_i.ij_y[idx.x + (int)(sp.xArea / 2), idx.y + (int)(sp.zArea / 2)] - y + sp.parent.position.y, idx.y),
                                        Quaternion.identity, sp.parent);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
