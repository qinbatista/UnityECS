using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
[UpdateAfter(typeof(CubeRespawnSystem))]
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
        //**use command buffer to add component, if no CubeTag, add it, add class type data
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithNone<CubeTag>().WithEntityAccess())
        {
            ecb.AddComponent(entity, new CubeTag { tag = true });
            ecb.AddComponent(entity, new ClassCubeData()); //this is not working, because it is not in the same world as state.EntityManager
        }
        //!disable Entity with CubeTag and tag is false
        foreach (var (cubeTag, entity) in SystemAPI.Query<RefRO<CubeTag>>().WithNone<Disabled>().WithEntityAccess())
        {
            if (!cubeTag.ValueRO.tag)
                ecb.AddComponent<Disabled>(entity);
        }
        //!Enable Entity with disabled component
        foreach (var (cubeTag, entity) in SystemAPI.Query<RefRO<CubeTag>>().WithAll<Disabled>().WithEntityAccess())
        {
            if (cubeTag.ValueRO.tag)
                ecb.RemoveComponent<Disabled>(entity);
        }
        ecb.Playback(state.EntityManager);

        //**foreach complexity O(n)
        // foreach (var (transform, cubeData) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<CubeData>>())
        // {
        //     transform.ValueRW = transform.ValueRO.RotateY(cubeData.ValueRO.speed * deltaTime);
        // }

        //**job complexity O(logN)
        var job = new CubeJob { deltaTime = deltaTime };
        job.ScheduleParallel();

        // **foreach complexity O(n) with IAspect
        // foreach (var (cubeIAspect,entity) in SystemAPI.Query<CubeIAspect>().WithEntityAccess())
        // {
        //     cubeIAspect.Rotate(deltaTime);
        // }





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