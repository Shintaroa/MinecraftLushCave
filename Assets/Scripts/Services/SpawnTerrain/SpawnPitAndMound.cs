using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Aquarium.Terrain.SpawnTerrain
{
    public class SpawnPitAndMountParameter : SpawnParameter
    {
        [Range(0, 10)]
        public int maxMoundCount = 3;
        [Range(0, 10)]
        public int minMoundCount = 1;
        [Range(0, 10)]
        public int maxPitCount = 3;
        [Range(0, 10)]
        public int minPitCount = 1;

        public void set(SpawnPitAndMountParameter sp)
        {
            seed = sp.seed;
            zArea = sp.zArea;
            xArea = sp.xArea;
            parent = sp.parent;
            height = sp.height;
            gameObject = sp.gameObject;
            maxMoundCount = sp.maxMoundCount;
            minMoundCount = sp.minMoundCount;
            maxPitCount = sp.maxPitCount;
            minPitCount = sp.minMoundCount;
        }
    }
    public class SpawnPitAndMound : SpawnTerrain<SpawnPitAndMountParameter>
    {
        private SpawnPitAndMound() { }

        public SpawnPitAndMound(SpawnPitAndMountParameter sp) 
        {
            this.sp = sp;
        }

        public new SpawnPitAndMountParameter sp;

        public Dictionary<TerrainType, Vector2[]> SpawnDetail()
        {
            Dictionary<TerrainType, Vector2[]> tv_d = new Dictionary<TerrainType, Vector2[]>();
            Vector2[] pits = RandomPitAndMound(sp.minPitCount, sp.maxPitCount, sp.seed, sp.xArea, sp.zArea);
            Vector2[] mounds = RandomPitAndMound(sp.maxPitCount, sp.maxMoundCount, sp.seed + 7489.321f, sp.xArea, sp.zArea);
            tv_d.Add(TerrainType.Mound,mounds);
            tv_d.Add(TerrainType.Pit, pits);
            return tv_d;
        }
    }
}