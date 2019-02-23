using Engine;
using Engine.Graphics;

namespace Game
{
    public class FlysharkBlock : Block
    {
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            EggBlock.EggType eggType = ((EggBlock)BlocksManager.Blocks[118]).GetEggTypeByCreatureTemplateName("Shark_Bull_Fly");
            BlocksManager.DrawMeshBlock(primitivesRenderer, eggType.BlockMesh, color, eggType.Scale * size, ref matrix, environmentData);
        }

        public const int Index = 351;
    }
}