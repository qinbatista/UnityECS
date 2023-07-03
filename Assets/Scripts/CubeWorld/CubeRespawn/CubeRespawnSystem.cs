using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public partial struct CubeRespawnSystem : ISystem
{
    uint updateCounter;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CubeRespawn>();
        // state.RequireForUpdate<CubeTag>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //find if empty, if empty, add new cubes
        // var spinningCubesQuery = SystemAPI.QueryBuilder().WithAll<CubeData>().Build();
        // if (spinningCubesQuery.IsEmpty)
        // {
        //     var prefab = SystemAPI.GetSingleton<CubeRespawn>().prefab;
        //     var instance = state.EntityManager.Instantiate(prefab, 500, Allocator.Temp);
        //     var random = Unity.Mathematics.Random.CreateFromIndex(updateCounter++);
        //     foreach (var entity in instance)
        //     {
        //         var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
        //         transform.ValueRW.Position = random.NextFloat3(new float3(-100, 0, -100), new float3(100, 0, 100));
        //     }
        // }

        //run once when code start
        state.Enabled = false;
        var prefab = SystemAPI.GetSingleton<CubeRespawn>().prefab;
        var instance = state.EntityManager.Instantiate(prefab, 500, Allocator.Temp);
        var random = Unity.Mathematics.Random.CreateFromIndex(updateCounter++);
        foreach (var entity in instance)
        {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            transform.ValueRW.Position = random.NextFloat3(new float3(-100, 0, -100), new float3(100, 0, 100));
        }
    }
}

