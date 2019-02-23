using Engine;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
    internal class ComponentCreeperBehavior : ComponentBehavior, IUpdateable
    {
        private ComponentCreature m_componentCreature;
        private ComponentHealth m_componentHealth;
        private ComponentChaseBehavior m_componenttChaseBehavior;
        private ComponentLocomotion m_componenttLocomotion;
        private ComponentPilot m_componenttPilot;
        private SubsystemExplosions m_subsystemExplosions;
        private ShapeshiftParticleSystem m_particleSystem;
        private SubsystemParticles m_subsystemParticles;
        private SubsystemTime m_subsystemTime;
        private StateMachine m_stateMachine = new StateMachine();
        private bool m_isExplosed;
        private bool m_shouldExplose;
        private bool m_explosionPrepared;
        private double m_nextUpdateTime;
        private double m_exploseTime;

        public int UpdateOrder
        {
            get { return 0; }
        }

        public override float ImportanceLevel
        {
            get { return 0; }
        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
            m_componentHealth = Entity.FindComponent<ComponentHealth>(true);
            m_componenttChaseBehavior = Entity.FindComponent<ComponentChaseBehavior>(true);
            m_componenttLocomotion = Entity.FindComponent<ComponentLocomotion>(true);
            m_componenttPilot = Entity.FindComponent<ComponentPilot>(true);
            m_subsystemExplosions = Project.FindSubsystem<SubsystemExplosions>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemParticles = Project.FindSubsystem<SubsystemParticles>(true);
            m_isExplosed = false;
            m_shouldExplose = false;
            m_explosionPrepared = false;
            m_exploseTime = 0;
        }

        public void Update(float dt)
        {
            if (m_subsystemTime.GameTime >= m_nextUpdateTime)
            {
                if (!m_isExplosed && m_componenttChaseBehavior.Target != null)
                {
                    Vector3 position = m_componentCreature.ComponentBody.Position;
                    Vector3 target_position = m_componenttChaseBehavior.Target.ComponentBody.Position;
                    float distance = Vector3.Distance(position, target_position);
                    if (!m_shouldExplose) m_shouldExplose = distance < 1.5;//距离小于1.5应该爆炸
                    else
                    {
                        if (distance < 5 && m_componentHealth.Health > 0)
                        {
                            m_componenttPilot.Stop();//距离小于5且生命值大于0时一直不动
                            if (!m_explosionPrepared)//如果还未准备下次的爆炸
                            {
                                m_particleSystem = new ShapeshiftParticleSystem();
                                m_subsystemParticles.AddParticleSystem(m_particleSystem);
                                m_particleSystem.BoundingBox = m_componentCreature.ComponentBody.BoundingBox;
                                m_exploseTime = m_subsystemTime.GameTime + 1.8;
                                m_explosionPrepared = true;
                            }
                            else if (m_subsystemTime.GameTime >= m_exploseTime)//已准备好爆炸且到达预定爆炸的时间
                            {
                                m_isExplosed = true;
                                m_subsystemExplosions.AddExplosion((int)position.X, (int)position.Y, (int)position.Z, 360, false, false);
                            }
                        }
                        else//距离大于5或生命值为0时取消爆炸
                        {
                            m_shouldExplose = false;
                            m_explosionPrepared = false;
                            m_subsystemParticles.RemoveParticleSystem(m_particleSystem);
                        }
                    }
                }
                else
                {
                    m_shouldExplose = false;
                    m_explosionPrepared = false;
                    m_subsystemParticles.RemoveParticleSystem(m_particleSystem);
                }
                m_nextUpdateTime = m_subsystemTime.GameTime + 0.05;
                m_stateMachine.Update();
            }
        }

        /*public ComponentCreeperBehavior() : base()
        {
        }*/
    }
}