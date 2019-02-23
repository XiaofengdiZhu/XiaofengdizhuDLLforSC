using Engine;
using Engine.Graphics;

namespace Game
{
    public class AutoCollectPickablesHoleBlock : Block
    {
        public override void Initialize()
        {
            base.Initialize();
            m_texture = ContentManager.Get<Texture2D>("Textures/Round32");
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 0.4f, ref matrix, m_texture, Color.Black, true, environmentData);
        }

        public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
        {
            return new BlockDebrisParticleSystem(subsystemTerrain, position, 0f, DestructionDebrisScale, Color.White, GetFaceTextureSlot(4, value));
        }

        public const int Index = 321;
        public Texture2D m_texture;
    }
}