using Engine;
using Engine.Graphics;

namespace Game
{
    public class TelescopeBlock : FlatBlock
    {
        public const int Index = 350;

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawFlatBlock(primitivesRenderer, value, size, ref matrix, null, color, false, environmentData);
        }
    }
}