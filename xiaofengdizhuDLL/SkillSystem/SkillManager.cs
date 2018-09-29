using Engine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game
{
    public static class SkillManager
    {
        public static Dictionary<string, Skill> m_skillDictionary;
        public static Dictionary<string, bool> m_defaultSkillInputDictionary;

        public static void Initialize()
        {
            Dictionary<string, Skill> skillDictionary = new Dictionary<string, Skill>();
            Dictionary<string, bool> defaultSkillInputDictionary = new Dictionary<string, bool>();
            List<TypeInfo> list = new List<TypeInfo>(typeof(SkillManager).GetTypeInfo().Assembly.DefinedTypes);
            foreach (TypeInfo current in list)
            {
                if (current.IsSubclassOf(typeof(Skill)) && !current.IsAbstract)
                {
                    Skill skill = (Skill)Activator.CreateInstance(current.AsType());
                    if (skill.Name != null)
                    {
                        if (skillDictionary.ContainsKey(skill.Name))
                        {
                            Log.Warning("There are at least two \"" + skill.Name + "\" in your DLL, the SkillManager will only load the first one");
                        }
                        else
                        {
                            skillDictionary.Add(skill.Name, skill);
                            defaultSkillInputDictionary.Add(skill.Name, false);
                        }
                    }
                }
            }
            m_skillDictionary = skillDictionary;
            m_defaultSkillInputDictionary = defaultSkillInputDictionary;
            foreach (Skill skill in m_skillDictionary.Values)
            {
            }
        }

        public static Dictionary<string, Skill> SkillDictionary
        {
            get { return m_skillDictionary; }
            set { m_skillDictionary = value; }
        }

        public static Dictionary<string, bool> DefaultSkillInputDictionary
        {
            get { return m_defaultSkillInputDictionary; }
        }
    }
}