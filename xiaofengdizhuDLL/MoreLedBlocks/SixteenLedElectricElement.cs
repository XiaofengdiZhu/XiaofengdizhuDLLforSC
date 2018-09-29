using Engine;

namespace Game
{
    // Token: 0x02000394 RID: 916
    public class SixteenLedElectricElement : MountedElectricElement
    {
        public SubsystemGlow m_subsystemGlow;
        public float m_voltage;
        public Color m_color;
        public GlowPoint[] m_glowPoints = new GlowPoint[16];

        public SixteenLedElectricElement(SubsystemElectricity subsystemElectricity, CellFace cellFace) : base(subsystemElectricity, cellFace)
        {
            this.m_subsystemGlow = subsystemElectricity.Project.FindSubsystem<SubsystemGlow>(true);
        }

        public override void OnAdded()
        {
            CellFace cellFace = base.CellFaces[0];
            int data = Terrain.ExtractData(base.SubsystemElectricity.SubsystemTerrain.Terrain.GetCellValue(cellFace.X, cellFace.Y, cellFace.Z));
            int mountingFace = SixteenLedBlock.GetMountingFace(data);
            this.m_color = LedBlock.LedColors[SixteenLedBlock.GetColor(data)];
            for (int i = 0; i < 16; i++)
            {
                Vector3 v = new Vector3((float)cellFace.X + 0.5f, (float)cellFace.Y + 0.5f, (float)cellFace.Z + 0.5f);
                Vector3 vector = CellFace.FaceToVector3(mountingFace);
                Vector3 vector2 = (mountingFace < 4) ? Vector3.UnitY : Vector3.UnitX;
                Vector3 vector3 = Vector3.Cross(vector, vector2);
                this.m_glowPoints[i] = this.m_subsystemGlow.AddGlowPoint();
                this.m_glowPoints[i].Position = v - 0.4375f * CellFace.FaceToVector3(mountingFace) + vector3 * ((float)((i % 4) * -2f + 3f) * 0.125f) + vector2 * ((float)((i / 4) * -2f + 3f) * 0.125f);
                this.m_glowPoints[i].Forward = vector;
                this.m_glowPoints[i].Up = vector2;
                this.m_glowPoints[i].Right = vector3;
                this.m_glowPoints[i].Color = Color.Transparent;
                this.m_glowPoints[i].Size = 0.14f;
                this.m_glowPoints[i].FarSize = 0.14f;
                this.m_glowPoints[i].FarDistance = 1f;
                this.m_glowPoints[i].Type = GlowPointType.Square;
            }
        }

        public override void OnRemoved()
        {
            for (int i = 0; i < 16; i++)
            {
                this.m_subsystemGlow.RemoveGlowPoint(this.m_glowPoints[i]);
            }
        }

        public override bool Simulate()
        {
            float voltage = this.m_voltage;
            this.m_voltage = 0f;
            foreach (ElectricConnection current in base.Connections)
            {
                if (current.ConnectorType != ElectricConnectorType.Output && current.NeighborConnectorType != ElectricConnectorType.Input)
                {
                    this.m_voltage = MathUtils.Max(this.m_voltage, current.NeighborElectricElement.GetOutputVoltage(current.NeighborConnectorFace));
                }
            }
            if (this.m_voltage != voltage)
            {
                int num = (int)MathUtils.Round(this.m_voltage * 15f);
                for (int i = 0; i < 4; i++)
                {
                    if ((num & 1 << i) != 0)
                    {
                        this.m_glowPoints[i].Color = this.m_color;
                    }
                    else
                    {
                        this.m_glowPoints[i].Color = Color.Transparent;
                    }
                }
            }
            return false;
        }
    }
}