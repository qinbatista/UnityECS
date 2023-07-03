using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
public partial struct CubeManagerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CubeTag>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        //foreach complexity O(n)
        // foreach (var (transform, cubeData) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<CubeData>>())
        // {
        //     transform.ValueRW = transform.ValueRO.RotateY(cubeData.ValueRO.speed * deltaTime);
        // }

        //job complexity O(logN)
        // var job = new CubeJob { deltaTime = deltaTime };
        // job.Schedule();

        //foreach complexity O(n) with IAspect
        foreach (var (cubeIAspect,cubeTag) in SystemAPI.Query<CubeIAspect,CubeTag>())
        {
            cubeIAspect.Rotate(deltaTime);
        }
    }
}
[BurstCompile]
partial struct CubeJob : IJobEntity
{
    public float deltaTime;
    void Execute(ref LocalTransform transform, ref CubeData cubeData, ref CubeTag tag)
    {
        transform = transform.RotateY(cubeData.speed * deltaTime);
    }
}
