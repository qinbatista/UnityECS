using Unity.Entities;
using UnityEngine;
public class CubeRespawnAuthoring : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject CubePrefab;
    class Baker : Baker<CubeRespawnAuthoring>
    {
        public override void Bake(CubeRespawnAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new CubeRespawn
            {
                prefab = GetEntity(authoring.CubePrefab, TransformUsageFlags.None)
            });
        }
    }
}
public struct CubeRespawn : IComponentData
{
    public Entity prefab;
}