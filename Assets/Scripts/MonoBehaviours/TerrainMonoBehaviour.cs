using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using Unity.Mathematics;
using Aquarium.Utility;
using Aquarium.Terrain.SpawnTerrain;
using Aquarium.Terrain.RenderTerrain;
using UnityEditor;
using UnityEngine.UIElements;
using Aquarium.Terrain.SpawnDecoration;

namespace Aquarium.Terrain
{
    public class TerrainMonoBehaviour : MonoBehaviour
    {
        public SpawnBottomParameter spawnBottomParameter;

        public SpawnTopParameter spawnTopParameter;

        public SpawnColumnParameter spawnColumnParameter;

        public SpawnPoolParameter spawnPoolParameter;

        public SpawnTerraceParameter spawnTerraceParameter;

        public RenderBottomParameter renderBottomParameter;

        public RenderTopParameter renderTopParameter;

        public SpawnBottomDecorationParameter spawnBottomDecorationParameter;

        public SpawnTopDecorationParameter spawnTopDecorationParameter;

        private void Start()
        {
            //凹凸
            SpawnPitAndMountParameter sp_mp= new SpawnPitAndMountParameter();
            sp_mp.set(spawnBottomParameter);
            Dictionary<TerrainType, Vector2[]> g_d_m =  new SpawnPitAndMound(sp_mp).SpawnDetail();

            spawnBottomParameter.pits = g_d_m[TerrainType.Pit];
            spawnBottomParameter.mounds = g_d_m[TerrainType.Mound];
            
            //石柱
            spawnColumnParameter.set(spawnBottomParameter);
            Dictionary<TerrainType, Vector4[]> g_c = new SpawnColumn(spawnColumnParameter).SpawnDetail();
            spawnBottomParameter.columns = g_c[TerrainType.Column];

            //水池
            spawnPoolParameter.set(spawnBottomParameter);
            Dictionary<TerrainType, Vector3[]> g_p = new SpawnPool(spawnPoolParameter).SpawnDetail();
            spawnBottomParameter.pools = g_p[TerrainType.Pool];

            //水田
            spawnTerraceParameter.set(spawnBottomParameter);
            Dictionary<TerrainType, Vector3[]> g_t = new SpawnTerrace(spawnTerraceParameter).SpawnDetail();
            spawnBottomParameter.terraces = g_t[TerrainType.Terrace];

            //底部
            BlockInfo b_b_i = new SpawnBottomTerrain(spawnBottomParameter).Spawn();

            //水面
            SpawnWaterParameter s_w_p = new SpawnWaterParameter();
            s_w_p.set(spawnBottomParameter);
            s_w_p.pools = g_p[TerrainType.Pool];
            s_w_p.columns = g_c[TerrainType.Column];
            s_w_p.maxColumnBlock = spawnBottomParameter.maxColumnBlock;
            s_w_p.minColumnBlock = spawnBottomParameter.minColumnBlock;
            s_w_p.maxPoolBlock = spawnBottomParameter.maxPoolBlock;
            s_w_p.minPoolBlock = spawnBottomParameter.minPoolBlock;
            s_w_p.b_i = b_b_i;


            WaterInfo w_i = new SpawnWater(s_w_p).Spawn();

            SpawnWallParameter spawnWallParameter = new SpawnWallParameter();

            spawnWallParameter.b_i = b_b_i;
            spawnWallParameter.set(spawnBottomParameter);
            spawnWallParameter.bottom = (int)(spawnWallParameter.height * 0.5f) * -1 - 1;
            spawnWallParameter.direction = Vector3.down;
            new SpawnWall(spawnWallParameter).Spawn();

            spawnTopParameter.xArea = sp_mp.xArea;
            spawnTopParameter.zArea = sp_mp.zArea;

            sp_mp.set(spawnTopParameter);
            Dictionary<TerrainType, Vector2[]> t_d_m = new SpawnPitAndMound(sp_mp).SpawnDetail();

            spawnTopParameter.pits = t_d_m[TerrainType.Pit];
            spawnTopParameter.mounds = t_d_m[TerrainType.Mound];

            spawnTopParameter.columns = g_c[TerrainType.Column];
            spawnTopParameter.b_y = b_b_i.ij_y;

            BlockInfo t_b_i = new SpawnTopTerrain(spawnTopParameter).Spawn();

            spawnWallParameter.b_i = t_b_i;
            spawnWallParameter.set(spawnTopParameter);
            spawnWallParameter.bottom = 2;
            spawnWallParameter.direction = Vector3.up;
            new SpawnWall(spawnWallParameter).Spawn();

            renderBottomParameter.water_info = w_i;
            renderBottomParameter.zArea = sp_mp.zArea;
            renderBottomParameter.xArea = sp_mp.xArea;
            renderBottomParameter.b_i = b_b_i;
            renderBottomParameter.columns = g_c[TerrainType.Column];
            RenderInfo b_r_i = new RenderBottomTerrain(renderBottomParameter).Render();

            renderTopParameter.zArea = sp_mp.zArea;
            renderTopParameter.xArea = sp_mp.xArea;
            renderTopParameter.b_i = t_b_i;
            renderTopParameter.columns = g_c[TerrainType.Column];
            RenderInfo t_r_i = new RenderTopTerrain(renderTopParameter).Render();

            //todo:Generate Decoration
            spawnBottomDecorationParameter.zArea = sp_mp.zArea;
            spawnBottomDecorationParameter.xArea = sp_mp.xArea;
            spawnBottomDecorationParameter.r_i = b_r_i;
            spawnBottomDecorationParameter.b_b_i = b_b_i;
            spawnBottomDecorationParameter.t_b_i = t_b_i;
            spawnBottomDecorationParameter.height = spawnBottomParameter.height;
            spawnBottomDecorationParameter.parent = spawnBottomParameter.parent;
            new SpawnBottomDecoration(spawnBottomDecorationParameter).Spawn();

            spawnTopDecorationParameter.zArea = sp_mp.zArea;
            spawnTopDecorationParameter.xArea = sp_mp.xArea;
            spawnTopDecorationParameter.r_i = t_r_i;
            spawnTopDecorationParameter.b_i = t_b_i;
            spawnTopDecorationParameter.parent = spawnTopParameter.parent;
            new SpawnTopDecoration(spawnTopDecorationParameter).Spawn();
            //todo:Generate Biome

           // new SpawnBottomBorder(spawnBottomDecorationParameter);
        }
    }

}