using Engine;
using Engine.Input;
using GameEntitySystem;
using System.Collections.Generic;
using TemplatesDatabase;

namespace Game
{
    public class SubsystemSkillInput : Subsystem, IUpdateable
    {
        private Dictionary<string, bool> m_skillInputDictionary;
        protected override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            SkillManager.Initialize();
        }
        public void UpdateInput()
        {
            if (!Window.IsActive || GameManager.Project == null)
            {
                return;
            }
            Dictionary<string, bool> skillInputDictionary = new Dictionary<string, bool>();
            foreach (string key in SkillManager.DefaultSkillInputDictionary.Keys)
            {
                skillInputDictionary.Add(key, SkillManager.DefaultSkillInputDictionary[key]);
            }
            m_skillInputDictionary = skillInputDictionary;
            UpdateInputFromMouseAndKeyboard();
        }
        private void UpdateInputFromMouseAndKeyboard()
        {
            Dictionary<string, bool> skillInputDictionary = new Dictionary<string, bool>();
            foreach (string key in m_skillInputDictionary.Keys)
            {
                skillInputDictionary.Add(key,(m_skillInputDictionary[key] || SkillManager.SkillDictionary[key].Input()));
            }
            m_skillInputDictionary = skillInputDictionary;
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
            UpdateInput();
        }
        public Dictionary<string, bool> SkillInputDictionary
        {
            get { return m_skillInputDictionary; }
            set { m_skillInputDictionary = value; }
        }
    }
}
