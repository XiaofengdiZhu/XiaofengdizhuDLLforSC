using Engine;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
    public class ComponentTargetState :Component, IUpdateable
    {
        private ComponentPlayer m_componentPlayer;
        private ComponentMiner m_componentMiner;
        private SubsystemTime m_subsystemTime;
        private SubsystemDrawing m_subsystemDrawing;
        private SubsystemInput m_subsystemInput;
        private SubsystemTargetState m_subsystemTargetState;
        private long m_lastMeleeHits;
        protected override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
            m_componentMiner = Entity.FindComponent<ComponentMiner>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemDrawing = Project.FindSubsystem<SubsystemDrawing>(true);
            m_subsystemInput = Project.FindSubsystem<SubsystemInput>(true);
            m_subsystemTargetState = Project.FindSubsystem<SubsystemTargetState>(true);
        }
        public int UpdateOrder
        {
            get
            {
                return 0;
            }
        }
        public void Update(float dt)
        {
            long newMeleeHits = m_componentMiner.ComponentCreature.PlayerStats.MeleeHits;
            if (m_lastMeleeHits != newMeleeHits)
            {
                m_lastMeleeHits = newMeleeHits;
                Vector3 viewPosition3 = m_subsystemDrawing.ViewPosition;
                PlayerInput playerInput = m_subsystemInput.PlayerInput;
                Vector3 vector = Vector3.Normalize(this.m_subsystemDrawing.ScreenToWorld(new Vector3(playerInput.Hit.Value, 1f), Matrix.Identity) - viewPosition3);
                BodyRaycastResult? bodyRaycastResult = m_componentMiner.PickBody(viewPosition3, vector);
                if (bodyRaycastResult.HasValue)
                {
                    m_subsystemTargetState.TargetBody = bodyRaycastResult.Value.ComponentBody;
                    m_subsystemTargetState.prepareDisapear();
                }
            }
        }
    }
}
