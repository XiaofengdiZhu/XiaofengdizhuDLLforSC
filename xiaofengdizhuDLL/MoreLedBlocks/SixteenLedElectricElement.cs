using Engine;

namespace Game
{
    public class SixteenLedElectricElement : MountedElectricElement
    {
        public SubsystemGlow m_subsystemGlow;
        public float m_voltage;
        public Color m_color;
        public GlowPoint[] m_glowPoints = new GlowPoint[16];

        public SixteenLedElectricElement(SubsystemElectricity subsystemElectricity, CellFace cellFace) : base(subsystemElectricity, cellFace)
        {
            m_subsystemGlow = subsystemElectricity.Project.FindSubsystem<SubsystemGlow>(true);
        }

        public override void OnAdded()
        {
            CellFace cellFace = CellFaces[0];
            int data = Terrain.ExtractData(SubsystemElectricity.SubsystemTerrain.Terrain.GetCellValue(cellFace.X, cellFace.Y, cellFace.Z));
            int mountingFace = SixteenLedBlock.GetMountingFace(data);
            m_color = LedBlock.LedColors[SixteenLedBlock.GetColor(data)];
            for (int i = 0; i < 16; i++)
            {
                var v = new Vector3(cellFace.X + 0.5f, cellFace.Y + 0.5f, cellFace.Z + 0.5f);
                Vector3 vector = CellFace.FaceToVector3(mountingFace);
                Vector3 vector2 = (mountingFace < 4) ? Vector3.UnitY : Vector3.UnitX;
                var vector3 = Vector3.Cross(vector, vector2);
                m_glowPoints[i] = m_subsystemGlow.AddGlowPoint();
                m_glowPoints[i].Position = v - 0.4375f * CellFace.FaceToVector3(mountingFace) + vector3 * ((i % 4 * -2f + 3f) * 0.125f) + vector2 * ((i / 4 * -2f + 3f) * 0.125f);
                m_glowPoints[i].Forward = vector;
                m_glowPoints[i].Up = vector2;
                m_glowPoints[i].Right = vector3;
                m_glowPoints[i].Color = Color.Transparent;
                m_glowPoints[i].Size = 0.14f;
                m_glowPoints[i].FarSize = 0.14f;
                m_glowPoints[i].FarDistance = 1f;
                m_glowPoints[i].Type = GlowPointType.Square;
            }
        }

        public override void OnRemoved()
        {
            for (int i = 0; i < 16; i++)
            {
                m_subsystemGlow.RemoveGlowPoint(m_glowPoints[i]);
            }
        }

        public override bool Simulate()
        {
            float voltage = m_voltage;
            m_voltage = 0f;
            foreach (ElectricConnection current in Connections)
            {
                if (current.ConnectorType != ElectricConnectorType.Output && current.NeighborConnectorType != ElectricConnectorType.Input)
                {
                    m_voltage = MathUtils.Max(m_voltage, current.NeighborElectricElement.GetOutputVoltage(current.NeighborConnectorFace));
                }
            }
            if (m_voltage != voltage)
            {
                int num = (int)MathUtils.Round(m_voltage * 15f);
                for (int i = 0; i < 4; i++)
                {
                    if ((num & 1 << i) != 0)
                    {
                        m_glowPoints[i].Color = m_color;
                    }
                    else
                    {
                        m_glowPoints[i].Color = Color.Transparent;
                    }
                }
            }
            return false;
        }
    }
}