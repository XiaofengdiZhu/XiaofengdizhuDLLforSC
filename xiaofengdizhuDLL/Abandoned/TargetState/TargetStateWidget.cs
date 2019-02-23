/*
using System.Xml.Linq;
using Engine;

namespace Game
{
    public class TargetStateWidget : CanvasWidget
    {
        private LabelWidget m_lableName;
        private ValueBarWidget m_healthBar;
        private LabelWidget m_lableAttackPower;
        private LabelWidget m_lableAttackResilience;
        private ComponentBody m_targetBody;
        private double m_disappearTime;
        private bool m_isDeaded;
        public TargetStateWidget()
        {
            XElement node = ContentManager.Get<XElement>("Widgets/TargetStateWidget");
            WidgetsManager.LoadWidgetContents(this, this, node);
            m_lableName = this.Children.Find<LabelWidget>("Name", true);
            m_healthBar = this.Children.Find<ValueBarWidget>("TargetHealthBar", true);
            m_lableAttackPower = this.Children.Find<LabelWidget>("AttackPower", true);
            m_lableAttackResilience = this.Children.Find<LabelWidget>("AttackResilience", true);
            m_isDeaded = false;
        }
        public override void Update()
        {
            if (m_targetBody != null)
            {
                ComponentHealth componentHealth = m_targetBody.Entity.FindComponent<ComponentHealth>();
                if (componentHealth != null)
                {
                    if (componentHealth.DeathTime == null)
                    {
                        m_isDeaded = false;
                        m_lableName.Text = m_targetBody.Entity.ValuesDictionary.DatabaseObject.Name;
                        m_healthBar.Value = componentHealth.Health;
                        m_lableAttackPower.Text = m_targetBody.Entity.FindComponent<ComponentMiner>().AttackPower.ToString();
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
            if (Time.RealTime > m_disappearTime)
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
            m_disappearTime = Time.RealTime + 10;
        }
        public void resetState()
        {
            m_targetBody = null;
            this.IsVisible = false;
            m_lableName.Text = "Non";
            m_healthBar.Value = 0;
            m_lableAttackPower.Text = "Non";
            m_lableAttackResilience.Text = "Non";
        }
        public ComponentBody TargetBody
        {
            get { return m_targetBody; }
            set
            {
                m_targetBody = value;
                this.IsVisible = true;
            }
        }
    }
}
*/