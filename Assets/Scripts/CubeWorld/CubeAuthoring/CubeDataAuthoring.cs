using Unity.Entities;
using UnityEngine;
public class CubeDataAuthoring : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed;
    class Baker : Baker<CubeDataAuthoring>
    {
        public override void Bake(CubeDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CubeData { speed = authoring.speed });
        }
    }
}
public struct CubeData : IComponentData
{
    public float speed;
}
