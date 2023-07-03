using Unity.Entities;
using UnityEngine;
public class CubeTagAuthoring : MonoBehaviour
{
    class Baker : Baker<CubeTagAuthoring>
    {
        public override void Bake(CubeTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new CubeTag());
        }
    }
}
public struct CubeTag: IComponentData
{
    public bool tag;
}