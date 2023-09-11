using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using Unity.Mathematics;
namespace Aquarium.SpawnTerrain
{
    public class SpawnTerrainMonoBehaviour_Copy : MonoBehaviour
    {
        public int area = 40;

        public GameObject cube;

        public int pitCount = 3;

        public int moundCount = 3;

        public int contrastIntensity = 1;

        public float seed;

        public Transform terrainGround;

        [Range(0.5f, 20)]
        public float pitIntensity = 1;

        [Range(0.5f, 20)]
        public float mountIntensity = 1;

        public int height = 10;

        private float random(float st)
        {
            return math.frac(sin(st * 132.928f) * 437.5453f);
        }

        enum TerrainType
        {
            Mound,
            Pit
        }

        private float GeneratePitAndMount(float dis, Vector2[] pms, int pm_count, int i, int j, TerrainType tt)
        {
            float s = 0;
            for (int k = 0; k < pm_count; k++)
            {
                float il = i - pms[k].x;
                float jl = j - pms[k].y;
                if (tt == TerrainType.Mound)
                    s += max(0, (1 - (sqrt(il * il + jl * jl) / sqrt((400 + 400)))));
                else
                    s -= max(0, (1 - (sqrt(il * il + jl * jl) / sqrt((400 + 400)))));
            }
            if (tt == TerrainType.Mound)
                dis = dis + s * mountIntensity;
            else
                dis = dis + s * pitIntensity;

            return dis;
        }

        private void Start()
        {
            //float seed = UnityEngine.Random.Range(0.001f,1);
            int pit_count = (int)lerp(0, pitCount + 0.9999f, random(seed));
            Vector2[] pits = new Vector2[pit_count];
            Debug.Log(pit_count);
            for (int i = 0; i < pit_count; i++)
            {
                float x = lerp(-20, 20, random(i + seed));
                float y = lerp(-20, 20, random(i + seed + 100));
                pits[i] = new Vector2(x, y);
            }

            int mound_count = (int)lerp(0, moundCount + 0.9999f, random(seed + 112.32f));
            Debug.Log(mound_count);
            Vector2[] mounds = new Vector2[mound_count];
            for (int i = 0; i < mound_count; i++)
            {
                float x = lerp(-20, 20, random(i + seed + 23.090f));
                float y = lerp(-20, 20, random(i + seed + 7489.321f));
                mounds[i] = new Vector2(x, y);
            }

            for (int i = -(int)(area / 2); i < (int)(area / 2); i++)
            {
                for (int j = -(int)(area / 2); j < (int)(area / 2); j++)
                {
                    float dis = 0.0f;
                    dis = GeneratePitAndMount(dis, pits, pit_count, i, j, TerrainType.Pit);
                    dis = GeneratePitAndMount(dis, mounds, mound_count, i, j, TerrainType.Mound);
                    dis = contrastIntensity * dis;
                    int y = (int)math.lerp(-height - 0.999f, height + 0.999f, dis / (pit_count + mound_count) * 0.5f + 0.5f);
                    y = max(min(height, y), -height);
                    Instantiate(cube, new Vector3(i, y, j) + terrainGround.position, Quaternion.identity, terrainGround);

                    float l_dis = dis;
                    float b_dis = dis;
                    int l_y = y;
                    int b_y = y;
                    if (i > -(int)(area / 2))
                    {
                        l_dis = 0.0f;
                        l_dis = GeneratePitAndMount(l_dis, pits, pit_count, i - 1, j, TerrainType.Pit);
                        l_dis = GeneratePitAndMount(l_dis, mounds, mound_count, i - 1, j, TerrainType.Mound);
                        l_dis = contrastIntensity * l_dis;
                        l_y = (int)math.lerp(-height - 0.999f, height + 0.999f, l_dis / (pit_count + mound_count) * 0.5f + 0.5f);
                        l_y = max(min(height, l_y), -height);
                    }
                    if (j > -(int)(area / 2))
                    {
                        b_dis = 0.0f;
                        b_dis = GeneratePitAndMount(b_dis, pits, pit_count, i, j - 1, TerrainType.Pit);
                        b_dis = GeneratePitAndMount(b_dis, mounds, mound_count, i, j - 1, TerrainType.Mound);
                        b_dis = contrastIntensity * b_dis;
                        b_y = (int)math.lerp(-height - 0.999f, height + 0.999f, b_dis / (pit_count + mound_count) * 0.5f + 0.5f);
                        b_y = max(min(height, b_y), -height);
                    }

                    int u_l_y = l_y + height;
                    int u_b_y = b_y + height;
                    int u_y = y + height;
                    if (abs(u_l_y - u_y) > 1 || abs(u_b_y - u_y) > 1)
                    {
                        int g_l = u_l_y - u_y;
                        int g_b = u_b_y - u_y;
                        int t = 0;
                        int b = 0;

                        if (g_l > 1 && g_b > 1)
                        {
                            t = max(g_l, g_b);
                        }
                        else if (g_l > 1)
                        {
                            t = g_l;
                        }
                        else if (g_b > 1)
                        {
                            t = g_b;
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
                        for (int k = 0; k < t; k++)
                        {
                            Instantiate(cube, new Vector3(i, y + k + 1, j) + terrainGround.position, Quaternion.identity, terrainGround);
                        }
                        for (int k = 0; k > b; k--)
                        {
                            Instantiate(cube, new Vector3(i, y + k - 1, j) + terrainGround.position, Quaternion.identity, terrainGround);
                        }
                    }
                }
            }
        }
    }
}