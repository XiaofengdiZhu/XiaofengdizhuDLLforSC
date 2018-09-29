using Engine;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
    public class ComponentExplosiveCreatureBehavior : ComponentBehavior, IUpdateable
    {
        private ComponentCreature m_componentCreature;
        private ComponentHealth m_componentHealth;
        private SubsystemExplosions m_subsystemExplosions;
        private SubsystemTime m_subsystemTime;
        private StateMachine m_stateMachine = new StateMachine();
        private bool m_isExplosed;
        private double m_nextUpdateTime;

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
            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
            m_componentHealth = Entity.FindComponent<ComponentHealth>(true);
            m_subsystemExplosions = Project.FindSubsystem<SubsystemExplosions>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_isExplosed = false;
        }

        public void Update(float dt)
        {
            if (!m_isExplosed && m_componentHealth.Health <= 0)
            {
                m_isExplosed = true;
                Vector3 position = this.m_componentCreature.ComponentBody.Position;
                m_subsystemExplosions.AddExplosion((int)position.X, (int)position.Y, (int)position.Z, this.m_componentCreature.ComponentHealth.AttackResilience * 3, false, false);
            }
            if (this.m_subsystemTime.GameTime >= this.m_nextUpdateTime)
            {
                this.m_nextUpdateTime = this.m_subsystemTime.GameTime + 0.5;
                this.m_stateMachine.Update();
            }
        }

        public ComponentExplosiveCreatureBehavior() : base()
        {
        }
    }
}