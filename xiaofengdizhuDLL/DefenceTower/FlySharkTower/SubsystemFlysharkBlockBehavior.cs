using Engine;
using GameEntitySystem;
using System;
using TemplatesDatabase;

namespace Game
{
    public class SubsystemFlysharkBlockBehavior : SubsystemBlockBehavior
    {
        public SubsystemBodies m_subsystemBodies;
        public DynamicArray<ComponentBody> m_componentBodies = new DynamicArray<ComponentBody>();
        private Random m_random = new Random();

        public override void Load(ValuesDictionary valuesDictionary)
        {
            m_subsystemBodies = Project.FindSubsystem<SubsystemBodies>(true);
        }

        public override int[] HandledBlocks
        {
            get { return new[]
                {
                    351
                }; }
        }

        public override bool OnHitAsProjectile(CellFace? cellFace, ComponentBody componentBody, WorldItem worldItem)
        {
            Entity entity = DatabaseManager.CreateEntity(Project, "Shark_Bull_Fly", true);
            entity.FindComponent<ComponentBody>(true).Position = worldItem.Position;
            entity.FindComponent<ComponentBody>(true).Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, m_random.UniformFloat(0f, 6.28318548f));
            entity.FindComponent<ComponentSpawn>(true).SpawnDuration = 0.25f;
            try
            {
                m_componentBodies.Clear();
                m_subsystemBodies.FindBodiesAroundPoint(new Vector2(worldItem.Position.X, worldItem.Position.Z), 3, m_componentBodies);
                if (m_componentBodies.Count > 0)
                {
                    ComponentCreature componentCreature = m_componentBodies.Array[0].Entity.FindComponent<ComponentCreature>();
                    entity.FindComponent<ComponentChaseBehavior>(true).Attack(componentCreature, 24f, 120f, true);
                }
            }
            catch (Exception e)
            {
                Log.Information(e.ToString());
            }
            Project.AddEntity(entity);
            return true;
        }
    }
}