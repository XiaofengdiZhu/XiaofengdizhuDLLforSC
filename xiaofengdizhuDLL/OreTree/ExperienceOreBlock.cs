using Engine;
using Engine.Graphics;

namespace Game
{
    public class ExperienceOreBlock : OreBlock
    {
        public override void Initialize()
        {
            base.Initialize();
            m_texture = ContentManager.Get<Texture2D>("Textures/ExperienceOre");
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawFlatBlock(primitivesRenderer, value, size, ref matrix, m_texture, Color.White, true, environmentData);
        }

        public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
        {
            return new BlockDebrisParticleSystem(subsystemTerrain, position, 0f, DestructionDebrisScale, Color.White, GetFaceTextureSlot(4, value));
        }

        public const int Index = 330;
        public Texture2D m_texture;
    }
}