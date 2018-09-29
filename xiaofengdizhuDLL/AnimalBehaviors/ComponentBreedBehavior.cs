using Engine;
using GameEntitySystem;
using System;
using TemplatesDatabase;

namespace Game
{
    internal class ComponentBreedBehavior : ComponentBehavior, IUpdateable
    {
        private SubsystemGameInfo m_subsystemGameInfo;
        private SubsystemTime m_subsystemTime;
        private SubsystemBodies m_subsystemBodies;
        private ComponentCreature m_componentCreature;
        private ComponentHealth m_componentHealth;
        private ComponentLocomotion m_componenttLocomotion;
        private ComponentPilot m_componenttPilot;
        private StateMachine m_stateMachine = new StateMachine();
        private Random m_random = new Random();
        public DynamicArray<ComponentBody> m_nearbyBodies = new DynamicArray<ComponentBody>();
        private Guid m_guid;
        public bool m_isBreeded;
        public double m_nextUpdateTime;
        public double m_lastBreedTime;
        public double m_breedCheckTime;
        public double m_breedPeriodTime;
        public float m_breedPeriodTimeRandomOffsetRange;

        public int UpdateOrder
        {
            get
            {
                return 0;
            }
        }

        public override float ImportanceLevel
        {
            get
            {
                return 0;
            }
        }

        protected override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_subsystemGameInfo = Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemBodies = Project.FindSubsystem<SubsystemBodies>(true);
            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
            m_componentHealth = Entity.FindComponent<ComponentHealth>(true);
            m_componenttLocomotion = Entity.FindComponent<ComponentLocomotion>(true);
            m_componenttPilot = Entity.FindComponent<ComponentPilot>(true);
            m_guid = Entity.ValuesDictionary.DatabaseObject.Guid;
            m_isBreeded = true;
            m_breedCheckTime = 0;
            m_breedPeriodTime = valuesDictionary.GetValue<double>("BreedPeriodTime") / 2;
            m_breedPeriodTimeRandomOffsetRange = valuesDictionary.GetValue<float>("BreedPeriodTimeRandomOffsetRange") / 2f;
            m_lastBreedTime = valuesDictionary.GetValue<double>("LastBreedTime");
        }

        protected override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            base.Save(valuesDictionary, entityToIdMap);
            valuesDictionary.SetValue<double>("lastBreedTime", m_lastBreedTime);
        }

        public void Update(float dt)
        {
            if (m_subsystemTime.GameTime >= m_nextUpdateTime && m_componentHealth.Health > 0)
            {
                if (m_subsystemGameInfo.TotalElapsedGameTime > m_breedCheckTime)
                {
                    if (m_lastBreedTime == 0)
                    {
                        m_lastBreedTime = m_subsystemGameInfo.TotalElapsedGameTime;
                    }
                    if (m_breedCheckTime == 0)
                    {
                        m_breedCheckTime = m_lastBreedTime + m_breedPeriodTime + (double)m_random.UniformFloat(-m_breedPeriodTimeRandomOffsetRange, m_breedPeriodTimeRandomOffsetRange);
                        return;
                    }
                    if (m_isBreeded)
                    {
                        m_isBreeded = false;
                    }
                    else
                    {
                        m_lastBreedTime = m_subsystemGameInfo.TotalElapsedGameTime;
                        m_breedCheckTime = m_lastBreedTime + m_breedPeriodTime + (double)m_random.UniformFloat(-m_breedPeriodTimeRandomOffsetRange, m_breedPeriodTimeRandomOffsetRange);
                        m_nearbyBodies.Clear();
                        m_subsystemBodies.FindBodiesAroundPoint(m_componentCreature.ComponentBody.Position.XZ, 16f, m_nearbyBodies);
                        if (m_nearbyBodies.Count > 30)
                        {
                            return;
                        }
                        foreach (ComponentBody body in m_nearbyBodies)
                        {
                            if (body.Entity.ValuesDictionary.DatabaseObject.Guid != m_guid)
                            {
                                m_nearbyBodies.Remove(body);
                            }
                        }
                        if (m_nearbyBodies.Count > 1)
                        {
                            m_componenttPilot.Stop();
                            Entity entity = DatabaseManager.CreateEntity(base.Project, Entity.ValuesDictionary.DatabaseObject.Name, true);
                            entity.FindComponent<ComponentBody>(true).Position = m_componentCreature.ComponentBody.Position;
                            entity.FindComponent<ComponentBody>(true).Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, this.m_random.UniformFloat(0f, 6.28318548f));
                            entity.FindComponent<ComponentSpawn>(true).SpawnDuration = 5f;
                            entity.FindComponent<ComponentBreedBehavior>(true).m_isBreeded = true;
                            Project.AddEntity(entity);
                            m_isBreeded = true;
                            foreach (ComponentBody body in m_nearbyBodies)
                            {
                                ComponentBreedBehavior bb = body.Entity.FindComponent<ComponentBreedBehavior>(true);
                                bb.m_isBreeded = true;
                                bb.m_lastBreedTime = m_lastBreedTime;
                                bb.m_breedCheckTime = m_breedCheckTime + m_breedPeriodTime + (double)m_random.UniformFloat(-m_breedPeriodTimeRandomOffsetRange, m_breedPeriodTimeRandomOffsetRange);
                            }
                        }
                    }
                }
                m_nextUpdateTime = m_subsystemTime.GameTime + 10;
                m_stateMachine.Update();
            }
        }

        public ComponentBreedBehavior() : base()
        {
        }
    }
}