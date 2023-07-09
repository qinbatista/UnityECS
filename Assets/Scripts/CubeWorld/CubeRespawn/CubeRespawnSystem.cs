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
        //**run once when code start
        state.Enabled = false;


        //**find if empty, if empty, add new cubes
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



        //**instance new cubes with specific number
        var prefab = SystemAPI.GetSingleton<CubeRespawn>().prefab;
        var random = Unity.Mathematics.Random.CreateFromIndex(updateCounter++);
        var instance = state.EntityManager.Instantiate(prefab, 100, Allocator.Temp);
        foreach (var entity in instance)
        {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            transform.ValueRW.Position = random.NextFloat3(new float3(-100, -100, -100), new float3(100, 100, 100));
        }

        //**chunk query
        // EntityQuery query = SystemAPI.QueryBuilder().WithAll<LocalTransform, CubeTag>().Build();
        // NativeArray<ArchetypeChunk> chunks = query.ToArchetypeChunkArray(Allocator.Temp);
        // ComponentTypeHandle<LocalTransform> localTransformTypeHandle = SystemAPI.GetComponentTypeHandle<LocalTransform>(true);
        // foreach (ArchetypeChunk chunk in chunks)
        // {
        //     NativeArray<LocalTransform> localTransforms = chunk.GetNativeArray(ref localTransformTypeHandle);
        //     for (int i = chunk.Count; i < chunk.Count; i++)
        //     {
        //         LocalTransform obstacleTransform = localTransforms[i];
        //         Entity player = state.EntityManager.Instantiate(prefab);
        //         state.EntityManager.SetComponentData(player, new LocalTransform
        //         {
        //             Position = random.NextFloat3(new float3(-100, -100, -100), new float3(100, 100, 100))
        //         });

        //     }
        // }
    }
}

