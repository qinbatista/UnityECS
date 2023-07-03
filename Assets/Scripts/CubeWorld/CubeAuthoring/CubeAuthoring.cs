using Unity.Entities;
using UnityEngine;
public class CubeAuthoring : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed;
    class Baker : Baker<CubeAuthoring>
    {
        public override void Bake(CubeAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CubeData { speed = authoring.speed });
        }
    }
}
public struct CubeData: IComponentData
{
    public float speed;
}