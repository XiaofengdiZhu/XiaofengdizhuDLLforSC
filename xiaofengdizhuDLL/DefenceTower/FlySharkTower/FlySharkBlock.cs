using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Engine;
using Engine.Graphics;
using TemplatesDatabase;

namespace Game
{
    // Token: 0x02000091 RID: 145
    public class FlysharkBlock : Block
    {
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
        }

        // Token: 0x0600059E RID: 1438 RVA: 0x000341D0 File Offset: 0x000323D0
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            EggBlock.EggType eggType = ((EggBlock)BlocksManager.Blocks[118]).GetEggTypeByCreatureTemplateName("Shark_Bull_Fly");
            BlocksManager.DrawMeshBlock(primitivesRenderer, eggType.BlockMesh, color, eggType.Scale * size, ref matrix, environmentData);
        }
        public const int Index = 351;
    }
}
