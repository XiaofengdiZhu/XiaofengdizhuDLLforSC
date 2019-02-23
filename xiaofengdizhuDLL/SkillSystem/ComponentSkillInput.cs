using GameEntitySystem;
using System.Collections.Generic;
using TemplatesDatabase;

namespace Game
{
    public class ComponentSkillInput : Component, IUpdateable
    {
        private ComponentPlayer m_componentPlayer;
        private SubsystemTime m_subsystemTime;
        private SubsystemExplosions m_subsystemExplosions;

        private Dictionary<string, bool> m_skillInputDictionary;

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemExplosions = Project.FindSubsystem<SubsystemExplosions>(true);
            SkillManager.Initialize();
        }

        public int UpdateOrder
        {
            get { return 0; }
        }

        public void Update(float dt)
        {
            var skillInputDictionary = new Dictionary<string, bool>();
            foreach (string key in SkillManager.DefaultSkillInputDictionary.Keys)
            {
                skillInputDictionary.Add(key, SkillManager.DefaultSkillInputDictionary[key]);
            }
            m_skillInputDictionary = skillInputDictionary;
            UpdateInputFromMouseAndKeyboard();
            if (m_componentPlayer.ComponentHealth.Health > 0f && !m_componentPlayer.ComponentSleep.IsSleeping && m_componentPlayer.View.ActiveCamera.IsEntityControlEnabled)
            {
                skillInputDictionary = m_skillInputDictionary;
            }
            else
            {
                skillInputDictionary = SkillManager.DefaultSkillInputDictionary;
            }
            foreach (string key in skillInputDictionary.Keys)
            {
                if (skillInputDictionary[key])
                    SkillManager.SkillDictionary[key].Action();
            }
        }

        private void UpdateInputFromMouseAndKeyboard()
        {
            var skillInputDictionary = new Dictionary<string, bool>();
            foreach (string key in m_skillInputDictionary.Keys)
            {
                skillInputDictionary.Add(key, m_skillInputDictionary[key] || SkillManager.SkillDictionary[key].Input());
            }
            m_skillInputDictionary = skillInputDictionary;
        }
    }
}