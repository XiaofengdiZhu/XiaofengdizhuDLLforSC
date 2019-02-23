using Engine;
using GameEntitySystem;
using System.Collections.Generic;

namespace Game
{
    public class SubsystemBlocksEntities : Subsystem
    {
        public ComponentBlocksEntity GetBlockEntity(int x, int y, int z)
        {
            m_blocksEntities.TryGetValue(new Point3(x, y, z), out ComponentBlocksEntity result);
            return result;
        }

        public override void OnEntityAdded(Entity entity)
        {
            ComponentBlocksEntity componentBlocksEntity = entity.FindComponent<ComponentBlocksEntity>();
            if (componentBlocksEntity != null)
            {
                foreach (Point3 point in componentBlocksEntity.Coordinates)
                {
                    m_blocksEntities.Add(point, componentBlocksEntity);
                }
            }
        }

        public override void OnEntityRemoved(Entity entity)
        {
            ComponentBlocksEntity componentBlocksEntity = entity.FindComponent<ComponentBlocksEntity>();
            if (componentBlocksEntity != null)
            {
                foreach (Point3 point in componentBlocksEntity.Coordinates)
                {
                    m_blocksEntities.Remove(point);
                }
            }
        }

        public Dictionary<Point3, ComponentBlocksEntity> m_blocksEntities = new Dictionary<Point3, ComponentBlocksEntity>();
    }
}