using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
namespace Aquarium.Terrain.SpawnTerrain
{
    public class SpawnWallParameter : SpawnParameter
    {
        [HideInInspector]
        public BlockInfo b_i;
        public int bottom = 0;
        public Vector3 direction = Vector3.down;
    }

        public class SpawnWall : SpawnTerrain<SpawnWallParameter>
    {
        public new SpawnWallParameter sp { get; set; }

        private SpawnWall() { }

        public SpawnWall(SpawnWallParameter spawnParameter) { this.sp = spawnParameter; }

        private void Instantiate(int x, int y, int z)
        {
            GameObject.Instantiate(sp.gameObject, new Vector3(x, y, z) + sp.parent.position, Quaternion.identity, sp.parent);
        }

        public new int[,] Spawn()
        {
            //Debug.Log(sp.b_i.ij_deep[20-13,1]);
            //int height = (int)(spawnParameter.height * 0.5f);
            for (int i = -(int)(sp.xArea / 2); i < (int)(sp.xArea / 2); i++)
            {

                int y_f = 0;
                int y_b = 0; 
                if (sp.b_i.ij_deep[i + (int)(sp.xArea / 2), 0] <=1)
                {
                    y_f = sp.b_i.ij_y[i + (int)(sp.xArea / 2), 0];
                }
                else 
                {
                    Vector2Int idx = sp.b_i.idxs[i + (int)(sp.xArea / 2), 0];
                    List<GameObject> gos = sp.b_i.blocks[idx];
                    y_f = (int)gos[0].transform.localPosition.y;
                    foreach (GameObject go in gos) 
                    {
                        y_f = min((int)go.transform.localPosition.y, y_f);
                    }
                }

                if (sp.b_i.ij_deep[i + (int)(sp.xArea / 2), (int)(sp.zArea / 2) * 2 - 1] <= 1)
                {
                    y_b =  sp.b_i.ij_y[i + (int)(sp.xArea / 2), (int)(sp.zArea / 2) * 2 - 1];
                }
                else 
                {
                    Vector2Int idx = sp.b_i.idxs[i + (int)(sp.xArea / 2), (int)(sp.zArea / 2) * 2 - 1];
                    List<GameObject> gos = sp.b_i.blocks[idx];
                    y_b = (int)gos[0].transform.localPosition.y;
                    foreach (GameObject go in gos)
                    {
                        y_b = min((int)go.transform.localPosition.y, y_b);
                    }
                }

                if (sp.direction == Vector3.down)
                {
                    for (y_f = y_f - 1; y_f > sp.bottom; y_f--)
                    {
                        Instantiate(i, y_f, -(int)(sp.zArea / 2));
                    }
                    for (y_b = y_b - 1; y_b > sp.bottom - 1; y_b--)
                    {
                        Instantiate(i, y_b, (int)(sp.zArea / 2) - 1);
                    }
                } 
                else if (sp.direction == Vector3.up)
                {
                    for (y_f = y_f + 1; y_f < sp.bottom; y_f++)
                    {
                        Instantiate(i, y_f, -(int)(sp.zArea / 2));
                    }
                    for (y_b = y_b + 1; y_b < sp.bottom +1; y_b++)
                    {
                        Instantiate(i, y_b, (int)(sp.zArea / 2) - 1);
                    }
                }
            }
            for (int j = -(int)(sp.zArea / 2) + 1; j < (int)(sp.zArea / 2) ; j++)
            {
                int y_l = 0; // sp.in_ij[0, j + (int)(sp.xArea / 2);
                int y_r = 0; // sp.in_ij[ (int)(sp.zArea / 2) * 2 - 1, j + (int)(sp.zArea / 2)];
                if (sp.b_i.ij_deep[0, j + (int)(sp.zArea / 2)] <= 1)
                {
                    y_l = sp.b_i.ij_y[0, j + (int)(sp.zArea / 2)];
                }
                else
                {
                    Vector2Int idx = sp.b_i.idxs[0, j + (int)(sp.xArea / 2)];
                    List<GameObject> gos = sp.b_i.blocks[idx];
                    y_l = (int)gos[0].transform.localPosition.y;
                    foreach (GameObject go in gos)
                    {
                        y_l = min((int)go.transform.localPosition.y, y_l);
                    }
                }
                if (sp.b_i.ij_deep[(int)(sp.xArea / 2) * 2 - 1, j + (int)(sp.zArea / 2)] <= 1)
                {
                    y_r = sp.b_i.ij_y[(int)(sp.xArea / 2) * 2 - 1, j + (int)(sp.zArea / 2)];
                }
                else
                {
                    Vector2Int idx = sp.b_i.idxs[(int)(sp.zArea / 2) * 2 - 1, j + (int)(sp.zArea / 2)];
                    List<GameObject> gos = sp.b_i.blocks[idx];
                    y_r = (int)gos[0].transform.localPosition.y;
                    foreach (GameObject go in gos)
                    {
                        y_r = min((int)go.transform.localPosition.y, y_r);
                    }
                }

                if (sp.direction == Vector3.down)
                {
                    for (y_l = y_l - 1; y_l > sp.bottom; y_l--)
                    {
                        Instantiate(-(int)(sp.xArea / 2), y_l, j);
                    }
                    for (y_r = y_r - 1; y_r > sp.bottom; y_r--)
                    {
                        Instantiate((int)(sp.xArea / 2)-1, y_r, j);
                    }
                }
                else if (sp.direction == Vector3.up)
                {
                    for (y_l = y_l + 1; y_l < sp.bottom; y_l++)
                    {
                        Instantiate(-(int)(sp.xArea / 2), y_l, j);
                    }
                    for (y_r = y_r + 1; y_r < sp.bottom; y_r++)
                    {
                        Instantiate((int)(sp.xArea / 2)-1, y_r, j);
                    }
                }
            }
            return null;
        }
    }
}