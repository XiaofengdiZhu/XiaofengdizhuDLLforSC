using GameEntitySystem;
using System;
using TemplatesDatabase;

namespace Game
{
    public class SubsystemTargetState : Subsystem, IUpdateable
    {
        private SubsystemPlayer m_subsystemPlayer;
        private SubsystemTime m_subsystemTime;
        private SubsystemGameInfo m_subsystemGameInfo;
        private ContainerWidget m_targetStateWidget;
        private LabelWidget m_lableName;
        private LabelWidget m_lableAttackPower;
        private LabelWidget m_lableAttackResilience;
        private ValueBarWidget m_healthBar;
        private SubsystemGui m_subsystemGui;
        private ComponentBody m_targetBody;
        private double m_disappearTime;
        private bool m_isDeaded;
        protected override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemPlayer = Project.FindSubsystem<SubsystemPlayer>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemGameInfo = Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemGui = Project.FindSubsystem<SubsystemGui>(true);
            m_targetStateWidget = m_subsystemGameInfo.GameWidget.Children.Find<ContainerWidget>("TargetState", true);
            m_lableName = m_targetStateWidget.Children.Find<LabelWidget>("Name", true);
            m_healthBar = m_targetStateWidget.Children.Find<ValueBarWidget>("TargetHealthBar", true);
            m_lableAttackPower = m_targetStateWidget.Children.Find<LabelWidget>("AttackPower", true);
            m_lableAttackResilience = m_targetStateWidget.Children.Find<LabelWidget>("AttackResilience", true);
            m_isDeaded = false;
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
            if (TargetBody != null)
            {
                ComponentHealth componentHealth = m_targetBody.Entity.FindComponent<ComponentHealth>();
                if (componentHealth != null)
                {
                    if (componentHealth.DeathTime == null)
                    {
                        m_isDeaded = false;
                        m_targetStateWidget.IsVisible = true;
                        m_lableName.Text = m_targetBody.Entity.ValuesDictionary.DatabaseObject.Name;
                        m_healthBar.Value = componentHealth.Health;
                        m_lableAttackPower.Text = TargetBody.Entity.FindComponent<ComponentMiner>().AttackPower.ToString();
                        m_lableAttackResilience.Text = componentHealth.AttackResilience.ToString();
                    }
                    else
                    {
                        if (m_isDeaded)
                        {
                            m_healthBar.Value = 0;
                            prepareDisapear();
                            m_targetBody = null;
                        }
                        m_isDeaded = true;
                    }
                }
            }
            if(m_subsystemTime.RealTime > m_disappearTime)
            {
                ComponentChaseBehavior chase;
                if (m_targetBody != null && (chase = m_targetBody.Entity.FindComponent<ComponentChaseBehavior>()) != null && chase.Target != null && chase.Target.Entity.ValuesDictionary.DatabaseObject.Name == "Player")
                {
                    prepareDisapear();
                }
                else
                {
                    resetState();
                }
            }

        }
        public void prepareDisapear()
        {
            m_disappearTime = m_subsystemTime.RealTime + 10;
        }
        public void resetState()
        {
            m_targetBody = null;
            m_targetStateWidget.IsVisible = false;
            m_lableName.Text = "Non";
            m_healthBar.Value = 0;
            m_lableAttackPower.Text = "Non";
            m_lableAttackResilience.Text = "Non";
        }
        public ComponentBody TargetBody
        {
            get
            {
                return m_targetBody;
            }
            set
            {
                m_targetBody = value;
            }
        }
    }
}
