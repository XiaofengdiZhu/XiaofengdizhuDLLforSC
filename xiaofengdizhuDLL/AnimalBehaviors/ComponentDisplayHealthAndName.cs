using Engine;
using Engine.Graphics;
using Engine.Media;
using GameEntitySystem;
using System;
using TemplatesDatabase;

namespace Game
{
    class ComponentDisplayHealthAndNameBehavior : Component, IDrawable
    {
        private PrimitivesRenderer3D m_primitivesRenderer = new PrimitivesRenderer3D();

        private ComponentCreature m_componentCreature;
        private ComponentHealth m_componentHealth;

        private int[] m_drawOrders = new int[1];
        private string m_displayName;
        private float m_height;
        private bool m_isPlayer = false;

        public int[] DrawOrders
        {
            get
            {
                return this.m_drawOrders;
            }
        }
        protected override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_primitivesRenderer = Project.FindSubsystem<SubsystemModelsRenderer>(true).PrimitivesRenderer;
            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
            m_componentHealth = Entity.FindComponent<ComponentHealth>(true);
            m_isPlayer = (Entity.FindComponent<ComponentPlayer>() != null);
            m_displayName = m_componentCreature.DisplayName;
            m_height = m_componentCreature.ComponentBody.BoxSize.Y;
        }
        public void Draw(Camera camera, int drawOrder)
        {
            if (!m_isPlayer)
            {
                Vector3 vector = Vector3.Transform(m_componentCreature.ComponentBody.Position + Vector3.UnitY * m_height + new Vector3(0, 0.4f, 0), camera.ViewMatrix);
                Vector3 vector2 = Vector3.Transform(m_componentCreature.ComponentBody.Position + Vector3.UnitY * m_height + new Vector3(0, 0.2f, 0), camera.ViewMatrix);
                if (vector.Z < 0f)
                {
                    Color color = Color.Lerp(Color.White, Color.Transparent, MathUtils.Saturate((vector.Length() - 16f) / 3f));
                    if (color.A > 6)
                    {
                        Vector3 right = Vector3.TransformNormal(0.005f * Vector3.Normalize(Vector3.Cross(camera.ViewDirection, camera.ViewUp)), camera.ViewMatrix);
                        Vector3 down = Vector3.TransformNormal(-0.005f * Vector3.UnitY, camera.ViewMatrix);
                        BitmapFont font = ContentManager.Get<BitmapFont>("Fonts/Pericles32");
                        m_primitivesRenderer.FontBatch(font, 1, DepthStencilState.DepthRead, RasterizerState.CullNoneScissor, BlendState.AlphaBlend, SamplerState.LinearClamp).QueueText(m_displayName, vector, right, down, color, TextAnchor.HorizontalCenter | TextAnchor.Bottom);
                        m_primitivesRenderer.FontBatch(font, 1, DepthStencilState.DepthRead, RasterizerState.CullNoneScissor, BlendState.AlphaBlend, SamplerState.LinearClamp).QueueText(Math.Round((m_componentHealth.Health * 100), 1).ToString() + "%", vector2, right, down, color, TextAnchor.HorizontalCenter | TextAnchor.Bottom);
                    }
                }
                //m_primitivesRenderer.Flush(camera.ProjectionMatrix, true, 2147483647);
            }
            return;
        }
    }
}
