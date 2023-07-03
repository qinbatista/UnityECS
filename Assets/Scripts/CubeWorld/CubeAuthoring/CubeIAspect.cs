using Unity.Entities;
using Unity.Transforms;
readonly partial struct CubeIAspect: IAspect
{
    readonly RefRW<LocalTransform> transform;
    readonly RefRO<CubeData> cubeData;
    public void Rotate(float deltaTime)
    {
        transform.ValueRW  = transform.ValueRW.RotateY(cubeData.ValueRO.speed * deltaTime);
    }
}