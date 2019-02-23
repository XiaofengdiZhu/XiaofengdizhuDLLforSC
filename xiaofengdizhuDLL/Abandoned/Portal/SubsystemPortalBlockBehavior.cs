/*
using Engine;
using Engine.Graphics;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using TemplatesDatabase;
namespace Game
{
    public class SubsystemPortalBlockBehavior : SubsystemBlockBehavior, IDrawable,IUpdateable
    {
        public SubsystemTime m_subsystemTime;
        public SubsystemPickables m_subsystemPickables;
        public Random m_random = new Random();
        public PrimitivesRenderer3D m_primitivesRenderer;
        public DrawBlockEnvironmentData m_drawBlockEnvironmentData = new DrawBlockEnvironmentData();
        public Texture2D m_texture;
        public Dictionary<Point3, Portal> m_blocks = new Dictionary<Point3, Portal>();
        public float m_visibilityRange;
        private double m_nextUpdateTime;
        public override int[] HandledBlocks
        {
            get { return new[] { 370 }; }
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemPickables = Project.FindSubsystem<SubsystemPickables>(true);
            m_texture = ContentManager.Get<Texture2D>("Textures/Round32");
            m_primitivesRenderer = Project.FindSubsystem<SubsystemModelsRenderer>(true).PrimitivesRenderer;
            m_visibilityRange = (float)SettingsManager.VisibilityRange;
        }
        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            Point3 point = new Point3(x, y, z);
            if (!m_blocks.ContainsKey(point))
            {
                m_blocks.Add(point, new Portal(point, m_subsystemTime.GameTime));
            }
        }
        public override void OnBlockGenerated(int value, int x, int y, int z, bool isLoaded)
        {
            Point3 point = new Point3(x, y, z);
            if (!m_blocks.ContainsKey(point))
            {
                m_blocks.Add(point, new Portal(point, m_subsystemTime.GameTime));
            }
        }
        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            Point3 point = new Point3(x, y, z);
            if (m_blocks.ContainsKey(point))
            {
                m_blocks.Remove(point);
            }
        }
        public void Draw(Camera camera, int drawOrder)
        {
            try
            {
                if (drawOrder == 10)
                {
                    double nowTime = m_subsystemTime.GameTime;
                    foreach (Portal hole in m_blocks.Values)
                    {
                        Vector3 blockPosition = new Vector3((float)hole.position.X + 0.5f, (float)hole.position.Y + 1.05f, (float)hole.position.Z + 0.5f);
                        if (Vector3.Distance(blockPosition, camera.ViewPosition) < m_visibilityRange + 10)
                        {
                            double timePassed = (nowTime - hole.spawnTime) * 0.5;
                            float size = (float)(Math.Sin(7 * timePassed) / (7 * Math.Sin(timePassed))) * 0.08f + 0.4f;
                            Vector3 v1 = Vector3.Normalize(Vector3.Cross(camera.ViewDirection, Vector3.UnitY));
                            Vector3 v2 = -Vector3.Normalize(Vector3.Cross(camera.ViewDirection, v1));
                            Vector3 p1 = Vector3.Transform(blockPosition - size*v1 - 2f*size*v2, camera.ViewMatrix);
                            Vector3 p2 = Vector3.Transform(blockPosition + size * v1 - 2f * size * v2, camera.ViewMatrix);
                            Vector3 p3 = Vector3.Transform(blockPosition - size * v1 + 2f * size * v2, camera.ViewMatrix);
                            Vector3 p4 = Vector3.Transform(blockPosition + size * v1 + 2f * size * v2, camera.ViewMatrix);
                            TexturedBatch3D texturedBatch3D = m_primitivesRenderer.TexturedBatch(m_texture, true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.AnisotropicWrap);
                            texturedBatch3D.QueueQuad(p1, p3, p4, p2, new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), Color.Cyan);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Warning("0：" + e.ToString());
            }
        }

        public void Update(float dt)
        {
            if (m_subsystemTime.GameTime >= m_nextUpdateTime&&m_blocks.Count > 0)
            {
                m_nextUpdateTime += 0.2;
            }
        }

        public int[] DrawOrders
        {
            get { return new[] { 10 }; }
        }

        public int UpdateOrder
        {
            get { return 0; }
        }

        public class Portal
        {
            public Portal(Point3 point, double time)
            {
                position = point;
                spawnTime = time;
            }
            public Point3 position;
            public double spawnTime;
        }
    }
}
*/