using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct CubeManagerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CubeData>();
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
        foreach (var cubeIAspect in SystemAPI.Query<CubeIAspect>())
        {
            cubeIAspect.Rotate(deltaTime);
        }

        //use command buffer to add component, if no CubeTag, add it, add class type data
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithNone<CubeTag>().WithEntityAccess())
        {
            ecb.AddComponent(entity, new CubeTag { tag = true });
            ecb.AddComponent(entity, new ClassCubeData()); //this is not working, because it is not in the same world as state.EntityManager
        }
        ecb.Playback(state.EntityManager);
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
public class ClassCubeData : IComponentData
{
    public GameObject RotatorPrefab;

    // Every IComponentData class must have a no-arg constructor.
    public ClassCubeData()
    {
    }
}