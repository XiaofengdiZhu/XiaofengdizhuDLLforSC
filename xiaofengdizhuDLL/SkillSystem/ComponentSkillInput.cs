using Engine;
using GameEntitySystem;
using System.Collections.Generic;
using TemplatesDatabase;

namespace Game
{
    public class ComponentSkillInput : Component, IUpdateable
    {
        private ComponentPlayer m_componentPlayer;
        private SubsystemTime m_subsystemTime;
        private SubsystemCamera m_subsystemCamera;
        private SubsystemExplosions m_subsystemExplosions;
        private SubsystemSkillInput m_subsystemSkillInput;
        protected override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemSkillInput = Project.FindSubsystem<SubsystemSkillInput>(true);
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
        public void Update(float dt)
        {
            Dictionary<string, bool> skillInputDictionary = new Dictionary<string, bool>(); ;
            if (this.m_componentPlayer.ComponentHealth.Health > 0f && !this.m_componentPlayer.ComponentSleep.IsSleeping && this.m_subsystemCamera.IsEntityControlEnabled(base.Entity))
            {
                skillInputDictionary = m_subsystemSkillInput.SkillInputDictionary;
                if (this.m_subsystemCamera.CameraRequiresMovementControls)
                {
                }
            }
            else
            {
                skillInputDictionary = SkillManager.DefaultSkillInputDictionary;
            }
            foreach(string key in skillInputDictionary.Keys)
            {
                if(skillInputDictionary[key])
                SkillManager.SkillDictionary[key].Action();
            }
        }
    }
}
