using Engine;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
    public class ComponentDiggerBehavior : ComponentBehavior, IUpdateable
    {
        private ComponentCreature m_componentCreature;
        private SubsystemTerrain m_subsystemTerrain;
        private StateMachine m_stateMachine = new StateMachine();
        private SubsystemTime m_subsystemTime;
        private Vector3 m_diggingPosition;
        private double m_nextUpdateTime;
        private double m_startDigTime;
        private double m_digTime;
        private bool m_isDigging;

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
            m_subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_isDigging = false;
        }

        public void Update(float dt)
        {
            if (m_subsystemTime.GameTime >= m_nextUpdateTime)
            {
                Vector3 vector3 = this.m_componentCreature.ComponentBody.Position;
                if (!m_isDigging)
                {
                    Point3 point3 = new Point3((int)vector3.X, (int)vector3.Y, (int)vector3.Z);
                    int cellValue = this.m_subsystemTerrain.TerrainData.GetCellValue(point3.X, point3.Y - 1, point3.Z);
                    float digResilience = BlocksManager.Blocks[TerrainData.ExtractContents(cellValue)].DigResilience;
                    if (digResilience > 0 && digResilience < 60)
                    {
                        m_digTime = digResilience / 5;
                        m_diggingPosition = vector3;
                        m_startDigTime = m_subsystemTime.GameTime;
                        m_isDigging = true;
                    }
                }
                else if (!vector3.Equals(m_diggingPosition))
                {
                    m_isDigging = false;
                }
                m_nextUpdateTime = m_subsystemTime.GameTime + 0.2;
                m_stateMachine.Update();
            }
            if (m_isDigging && m_subsystemTime.GameTime - m_startDigTime > m_digTime)
            {
                Point3 point3 = new Point3((int)m_diggingPosition.X, (int)m_diggingPosition.Y, (int)m_diggingPosition.Z);
                m_subsystemTerrain.DestroyCell(1, point3.X, point3.Y - 1, point3.Z, 0, false, false);
                m_isDigging = false;
            }
        }
        public ComponentDiggerBehavior() : base() { }

    }
}
