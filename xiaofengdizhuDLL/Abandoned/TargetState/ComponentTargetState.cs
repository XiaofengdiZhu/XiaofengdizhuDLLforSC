/*
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
        private TargetStateWidget targetStateWidget;
        protected override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
            m_componentMiner = Entity.FindComponent<ComponentMiner>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemDrawing = Project.FindSubsystem<SubsystemDrawing>(true);
            targetStateWidget = m_componentPlayer.View.GameWidget.Children.Find<TargetStateWidget>("TargetState", true);
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
            if (m_componentPlayer.ComponentInput.PlayerInput.Hit.HasValue)
            {
                Vector3 viewPosition3 = this.m_componentPlayer.View.ActiveCamera.ViewPosition;
                Vector3 vector = Vector3.Normalize(this.m_componentPlayer.View.ActiveCamera.ScreenToWorld(new Vector3(m_componentPlayer.ComponentInput.PlayerInput.Hit.Value, 1f), Matrix.Identity) - viewPosition3);
                BodyRaycastResult? bodyRaycastResult = m_componentMiner.PickBody(viewPosition3, vector);
                if (bodyRaycastResult.HasValue)
                {
                    targetStateWidget.TargetBody = bodyRaycastResult.Value.ComponentBody;
                    targetStateWidget.prepareDisapear();
                }
            }
        }
    }
}
*/