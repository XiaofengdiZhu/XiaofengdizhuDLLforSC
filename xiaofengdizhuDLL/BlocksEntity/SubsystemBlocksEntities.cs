﻿using System;
using System.Collections.Generic;
using Engine;
using GameEntitySystem;

namespace Game
{
    public class SubsystemBlocksEntities : Subsystem
    {
        public ComponentBlocksEntity GetBlockEntity(int x, int y, int z)
        {
            m_blocksEntities.TryGetValue(new Point3(x, y, z), out ComponentBlocksEntity result);
            return result;
        }
        protected override void OnEntityAdded(Entity entity)
        {
            ComponentBlocksEntity componentBlocksEntity = entity.FindComponent<ComponentBlocksEntity>();
            if (componentBlocksEntity != null)
            {
                foreach(Point3 point in componentBlocksEntity.Coordinates)
                {
                    m_blocksEntities.Add(point, componentBlocksEntity);
                }
            }
        }
        protected override void OnEntityRemoved(Entity entity)
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