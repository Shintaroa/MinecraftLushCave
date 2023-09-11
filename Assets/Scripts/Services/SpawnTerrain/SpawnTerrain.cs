using Aquarium.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using Unity.Mathematics;

namespace Aquarium.Terrain.SpawnTerrain
{
    [System.Serializable]
    public class SpawnParameter
    {
        public float seed = 0;
        [Range(10, 200)]
        public int zArea = 10;
        [Range(10, 200)]
        public int xArea = 10;
        public Transform parent = null;
        [Range(5, 50)]
        public int height = 10;
        public GameObject gameObject = null;
        public void set(SpawnParameter sp)
        {
            seed = sp.seed;
            zArea = sp.zArea;
            xArea = sp.xArea;
            parent = sp.parent;
            height = sp.height;
            gameObject = sp.gameObject;
        }
    }
    public interface SpawnTerrainInterFace<SpawnParameter>
    {
        SpawnParameter sp { get; set; }
        int[,] Spawn();

    }

    public abstract class SpawnTerrain<SpawnParameter> : SpawnTerrainInterFace<SpawnParameter>
    {
        public SpawnParameter sp { get; set; }

        public int[,] Spawn()
        {
            return null;
        }


        private float GeneratePitAndMount(float dis, Vector2[] pms, int pm_count, int i, int j, TerrainType tt,float[] moundIntensitys, float[] pitIntensitys,int xArea, int zArea, float moundIntensity, float pitIntensity, float[] pitPowIntensitys, float[] moundPowIntensitys)
        {
            float s = 0;
            for (int k = 0; k < pm_count; k++)
            {
                float il = i - pms[k].x;
                float jl = j - pms[k].y;
                if (tt == TerrainType.Mound)
                    s += max(0, pow(1 - sqrt(il * il + jl * jl) / sqrt(xArea*0.5f * xArea * 0.5f + zArea * 0.5f * zArea * 0.5f), moundPowIntensitys[k])) * moundIntensitys[k];
                else
                    s -= max(0, pow(1 - sqrt(il * il + jl * jl) / sqrt(xArea * 0.5f * xArea * 0.5f + zArea * 0.5f  * zArea*0.5f), pitPowIntensitys[k]))* pitIntensitys[k];
            }
            if (tt == TerrainType.Mound)
                dis = dis + s * moundIntensity;
            else
                dis = dis + s * pitIntensity;

            return dis;
        }
        protected GameObject Instantiate(Vector3 loc,GameObject gameObject,Transform parent)
        {
            return GameObject.Instantiate(gameObject, loc, Quaternion.identity, parent);
        }
        protected Vector2[] RandomPitAndMound(int pitAndMount_min_count, int pitAndMount_max_count, float seed, int xArea, int zArea)
        {
            uint pitAndMount_count = (uint)lerp(pitAndMount_min_count, (float)pitAndMount_max_count + 0.9999f, Tool.random(seed));
            return GeneratePitAndMound(pitAndMount_count, seed, xArea, zArea);
        }

        protected Vector2[] GeneratePitAndMound(uint pitAndMount_count, float seed, int xArea, int zArea)
        {
            Vector2[] pitAndMounts = new Vector2[pitAndMount_count];
            for (int i = 0; i < pitAndMount_count; i++)
            {
                float x = lerp(-(int)(xArea / 2), (int)(xArea / 2), Tool.random(i + seed + seed));
                float y = lerp(-(int)(zArea / 2), (int)(zArea / 2), Tool.random(i + seed + seed + seed));
                pitAndMounts[i] = new Vector2(x, y);
            }
            return pitAndMounts;
        }

        protected float GenerateTerrainDis(float dis, Vector2[] pits, Vector2[] mounds, int i, int j, float[] moundIntensitys, float[] pitIntensitys, float contrastIntensity, int xArea, int zArea, float moundIntensity, float pitIntensity, float[] pitPowIntensitys, float[] moundPowIntensitys)
        {
            if (pits != null)
                dis = GeneratePitAndMount(dis, pits, pits.Length, i, j, TerrainType.Pit, moundIntensitys, pitIntensitys, xArea, zArea, moundIntensity, pitIntensity,pitPowIntensitys,moundPowIntensitys);
            if (mounds != null)
                dis = GeneratePitAndMount(dis, mounds, mounds.Length, i, j, TerrainType.Mound, moundIntensitys, pitIntensitys, xArea, zArea, moundIntensity, pitIntensity, pitPowIntensitys, moundPowIntensitys);
            dis = contrastIntensity * dis;
            return dis;
        }

        protected int GenerateTerrainY(float dis, int p_l, int m_l,int height,int x,int z,int i, int j)
        {
            int y = (int)math.lerp(-height - 0.999f, height + 0.999f, dis / (p_l + m_l) * 0.5f + 0.5f);
            float  noise = Mathf.PerlinNoise((float)((i + 0.5 * x)/ x), (float)((j + 0.5 * z) / z));
            Debug.Log(noise);
            //y += (int)((noise-0.5) * 20);
            y = max(min(height, y), -height);
            return y;
        }
    }
}