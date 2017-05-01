using Engine;
using Engine.Input;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
    public class ComponentSkill : Component, IUpdateable
    {
        private ComponentPlayer m_componentPlayer;
        private SubsystemTime m_subsystemTime;
        private SubsystemCamera m_subsystemCamera;
        private SubsystemExplosions m_subsystemExplosions;
        private SkillInput m_skillInput;
        protected override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemCamera = Project.FindSubsystem<SubsystemCamera>(true);
            m_subsystemExplosions = Project.FindSubsystem<SubsystemExplosions>(true);
        }
        public int UpdateOrder
        {
            get
            {
                return 0;
            }
        }
        public void UpdateInput()
        {
            this.m_skillInput = default(SkillInput);
            if (!Window.IsActive)
            {
                return;
            }
            UpdateInputFromMouseAndKeyboard();
        }
        private void UpdateInputFromMouseAndKeyboard()
        {
            m_skillInput.FireBall = (m_skillInput.FireBall | InputManager.IsKeyDownOnce(Key.Y));
        }
        public void Update(float dt)
        {
            UpdateInput();
            SkillInput skillInput;
            if (this.m_componentPlayer.ComponentHealth.Health > 0f && !this.m_componentPlayer.ComponentSleep.IsSleeping && this.m_subsystemCamera.IsEntityControlEnabled(base.Entity))
            {
                skillInput = m_skillInput;
                if (this.m_subsystemCamera.CameraRequiresMovementControls)
                {
                }
            }
            else
            {
                skillInput = default(SkillInput);
            }
            if (skillInput.FireBall)
            {
                Vector3 position = m_componentPlayer.ComponentBody.Position;
                m_subsystemExplosions.AddExplosion((int)position.X, (int)position.Y + 10, (int)position.Z, m_componentPlayer.ComponentHealth.AttackResilience * 3, false, false);
            }
        }
        public SkillInput SkillInput
        {
            get
            {
                return this.m_skillInput;
            }
        }
    }
}
