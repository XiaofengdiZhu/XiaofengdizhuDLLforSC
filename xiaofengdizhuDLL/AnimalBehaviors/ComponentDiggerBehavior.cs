using Engine;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
    public class ComponentDiggerBehavior : ComponentBehavior, IUpdateable
    {
        private ComponentCreature m_componentCreature;
        private ComponentMiner m_componentMiner;
        private SubsystemTerrain m_subsystemTerrain;
        private StateMachine m_stateMachine = new StateMachine();
        private SubsystemTime m_subsystemTime;
        private Vector3 m_diggingPosition;
        private float bodyHeight;
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
            m_componentMiner = Entity.FindComponent<ComponentMiner>(true);
            m_subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            bodyHeight = Entity.FindComponent<ComponentBody>(true).BoxSize.Y;
            m_isDigging = false;
        }

        public void Update(float dt)
        {
            if (m_subsystemTime.GameTime >= m_nextUpdateTime)
            {
                Vector3 vector3 = this.m_componentCreature.ComponentBody.Position;
                if (!m_isDigging)
                {
                    TerrainRaycastResult? terrainRaycastResult = m_componentMiner.PickTerrainForDigging(vector3, new Vector3(0, -1, 0));
                    if (terrainRaycastResult.HasValue)
                    {
                        if (terrainRaycastResult.Value.Distance < 1.2)
                        {
                            int cellValue = terrainRaycastResult.Value.Value;
                            float digResilience = BlocksManager.Blocks[Terrain.ExtractContents(cellValue)].DigResilience;
                            if (digResilience > 0 && digResilience < 60)
                            {
                                m_digTime = digResilience / 5;
                                m_diggingPosition = vector3;
                                m_startDigTime = m_subsystemTime.GameTime;
                                m_isDigging = true;
                            }
                        }
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
                TerrainRaycastResult? terrainRaycastResult = m_componentMiner.PickTerrainForDigging(m_componentCreature.ComponentBody.Position, new Vector3(0, -1, 0));
                if (terrainRaycastResult.HasValue)
                {
                    if (terrainRaycastResult.Value.Distance < 1.2)
                    {
                        Point3 point3 = terrainRaycastResult.Value.CellFace.Point;
                        m_subsystemTerrain.DestroyCell(1, point3.X, point3.Y, point3.Z, 0, false, false);
                    }
                }
                m_isDigging = false;
            }
        }

        public ComponentDiggerBehavior() : base()
        {
        }
    }
}