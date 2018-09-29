/*
using System;
using Engine;
using Engine.Graphics;

namespace Game
{
    public class PortalBlock : Block
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
            DrawFlatBlock(primitivesRenderer, value, 0.35f,0.7f, ref matrix, this.m_texture, Color.Cyan, true, environmentData);
            DrawCubeBlock(primitivesRenderer, 208, 0.1f, ref matrix, Color.Gray, environmentData);
        }
        public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
        {
            return new BlockDebrisParticleSystem(subsystemTerrain, position, 0f, this.DestructionDebrisScale, Color.White, this.GetFaceTextureSlot(4, value));
        }
        public const int Index = 370;
        public Texture2D m_texture;
        public static void DrawFlatBlock(PrimitivesRenderer3D primitivesRenderer, int value, float sizeX,float sizeY, ref Matrix matrix, Texture2D texture, Color color, bool isEmissive, DrawBlockEnvironmentData environmentData)
        {
            environmentData = (environmentData ?? BlocksManager.m_defaultEnvironmentData);
            if (!isEmissive)
            {
                float s = LightingManager.LightIntensityByLightValue[environmentData.Light];
                color = Color.MultiplyColorOnly(color, s);
            }
            Vector3 translation = matrix.Translation + new Vector3(0f,0.2f,0f);
            Vector3 right;
            Vector3 up;
            if (environmentData.BillboardDirection != null)
            {
                right = Vector3.Normalize(Vector3.Cross(environmentData.BillboardDirection.Value, Vector3.UnitY));
                up = -Vector3.Normalize(Vector3.Cross(environmentData.BillboardDirection.Value, right));
            }
            else
            {
                right = matrix.Right;
                up = matrix.Up;
            }
            Vector3 p = translation - sizeX*right - sizeY*up;
            Vector3 vector2 = translation + sizeX * right - sizeY * up;
            Vector3 vector3 = translation - sizeX * right + sizeY * up;
            Vector3 p2 = translation + sizeX * right + sizeY * up;
            if (environmentData.ViewProjectionMatrix != null)
            {
                Matrix value2 = environmentData.ViewProjectionMatrix.Value;
                Vector3.Transform(ref p, ref value2, out p);
                Vector3.Transform(ref vector2, ref value2, out vector2);
                Vector3.Transform(ref vector3, ref value2, out vector3);
                Vector3.Transform(ref p2, ref value2, out p2);
            }
            int num = Terrain.ExtractContents(value);
            Block block = BlocksManager.Blocks[num];
            Vector4 vector4;
            if (texture == null)
            {
                texture = ((environmentData.SubsystemTerrain != null) ? environmentData.SubsystemTerrain.SubsystemAnimatedTextures.AnimatedBlocksTexture : BlocksTexturesManager.DefaultBlocksTexture);
                vector4 = BlocksManager.m_slotTexCoords[block.GetFaceTextureSlot(-1, value)];
            }
            else
            {
                vector4 = new Vector4(0f, 0f, 1f, 1f);
            }
            TexturedBatch3D texturedBatch3D = primitivesRenderer.TexturedBatch(texture, true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.PointClamp);
            texturedBatch3D.QueueQuad(p, vector3, p2, vector2, new Vector2(vector4.X, vector4.W), new Vector2(vector4.X, vector4.Y), new Vector2(vector4.Z, vector4.Y), new Vector2(vector4.Z, vector4.W), color);
            if (environmentData.BillboardDirection == null)
            {
                texturedBatch3D.QueueQuad(p, vector2, p2, vector3, new Vector2(vector4.X, vector4.W), new Vector2(vector4.Z, vector4.W), new Vector2(vector4.Z, vector4.Y), new Vector2(vector4.X, vector4.Y), color);
            }
        }
        public static void DrawCubeBlock(PrimitivesRenderer3D primitivesRenderer, int value, float sizeY, ref Matrix matrix, Color color, DrawBlockEnvironmentData environmentData)
        {
            environmentData = (environmentData ?? BlocksManager.m_defaultEnvironmentData);
            Texture2D texture = (environmentData.SubsystemTerrain != null) ? environmentData.SubsystemTerrain.SubsystemAnimatedTextures.AnimatedBlocksTexture : BlocksTexturesManager.DefaultBlocksTexture;
            TexturedBatch3D texturedBatch3D = primitivesRenderer.TexturedBatch(texture, true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.PointClamp);
            float s = LightingManager.LightIntensityByLightValue[environmentData.Light];
            color = Color.MultiplyColorOnly(color, s);
            Vector3 translation = matrix.Translation-new Vector3(0f,0.6f,0f);
            Vector3 vector = matrix.Right * 0.4f;
            Vector3 v = matrix.Up * sizeY;
            Vector3 v2 = matrix.Forward * 0.4f;
            Vector3 p = translation + 0.5f * (-vector - v - v2);
            Vector3 vector2 = translation + 0.5f * (vector - v2);
            Vector3 vector3 = translation + 0.5f * (-vector + v - v2);
            Vector3 vector4 = translation + 0.5f * (vector + v - v2);
            Vector3 vector5 = translation + 0.5f * (-vector + v2);
            Vector3 vector6 = translation + 0.5f * (vector + v2);
            Vector3 vector7 = translation + 0.5f * (-vector + v + v2);
            Vector3 p2 = translation + 0.5f * (vector + v + v2);
            if (environmentData.ViewProjectionMatrix != null)
            {
                Matrix value2 = environmentData.ViewProjectionMatrix.Value;
                Vector3.Transform(ref p, ref value2, out p);
                Vector3.Transform(ref vector2, ref value2, out vector2);
                Vector3.Transform(ref vector3, ref value2, out vector3);
                Vector3.Transform(ref vector4, ref value2, out vector4);
                Vector3.Transform(ref vector5, ref value2, out vector5);
                Vector3.Transform(ref vector6, ref value2, out vector6);
                Vector3.Transform(ref vector7, ref value2, out vector7);
                Vector3.Transform(ref p2, ref value2, out p2);
            }
            int num = Terrain.ExtractContents(value);
            Block block = BlocksManager.Blocks[num];
            Vector4 vector8 = BlocksManager.m_slotTexCoords[block.GetFaceTextureSlot(0, value)];
            Color color2 = Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(-matrix.Forward));
            texturedBatch3D.QueueQuad(p, vector3, vector4, vector2, new Vector2(vector8.X, vector8.W), new Vector2(vector8.X, vector8.Y), new Vector2(vector8.Z, vector8.Y), new Vector2(vector8.Z, vector8.W), color2);
            vector8 = BlocksManager.m_slotTexCoords[block.GetFaceTextureSlot(2, value)];
            color2 = Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(matrix.Forward));
            texturedBatch3D.QueueQuad(vector5, vector6, p2, vector7, new Vector2(vector8.Z, vector8.W), new Vector2(vector8.X, vector8.W), new Vector2(vector8.X, vector8.Y), new Vector2(vector8.Z, vector8.Y), color2);
            vector8 = BlocksManager.m_slotTexCoords[block.GetFaceTextureSlot(5, value)];
            color2 = Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(-matrix.Up));
            texturedBatch3D.QueueQuad(p, vector2, vector6, vector5, new Vector2(vector8.X, vector8.Y), new Vector2(vector8.Z, vector8.Y), new Vector2(vector8.Z, vector8.W), new Vector2(vector8.X, vector8.W), color2);
            vector8 = BlocksManager.m_slotTexCoords[block.GetFaceTextureSlot(4, value)];
            color2 = Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(matrix.Up));
            texturedBatch3D.QueueQuad(vector3, vector7, p2, vector4, new Vector2(vector8.X, vector8.W), new Vector2(vector8.X, vector8.Y), new Vector2(vector8.Z, vector8.Y), new Vector2(vector8.Z, vector8.W), color2);
            vector8 = BlocksManager.m_slotTexCoords[block.GetFaceTextureSlot(1, value)];
            color2 = Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(-matrix.Right));
            texturedBatch3D.QueueQuad(p, vector5, vector7, vector3, new Vector2(vector8.Z, vector8.W), new Vector2(vector8.X, vector8.W), new Vector2(vector8.X, vector8.Y), new Vector2(vector8.Z, vector8.Y), color2);
            vector8 = BlocksManager.m_slotTexCoords[block.GetFaceTextureSlot(3, value)];
            color2 = Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(matrix.Right));
            texturedBatch3D.QueueQuad(vector2, vector4, p2, vector6, new Vector2(vector8.X, vector8.W), new Vector2(vector8.X, vector8.Y), new Vector2(vector8.Z, vector8.Y), new Vector2(vector8.Z, vector8.W), color2);
        }
    }
}
*/