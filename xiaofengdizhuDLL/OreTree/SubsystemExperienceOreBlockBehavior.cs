using Engine;
using Engine.Graphics;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using TemplatesDatabase;
namespace Game
{
    public class SubsystemExperienceOreBlockBehavior : SubsystemBlockBehavior, IDrawable
    {
        public SubsystemAudio m_subsystemAudio;
        public SubsystemTime m_subsystemTime;
        public Random m_random = new Random();
        public PrimitivesRenderer3D m_primitivesRenderer;
        public DrawBlockEnvironmentData m_drawBlockEnvironmentData = new DrawBlockEnvironmentData();
        public Texture2D m_texture;
        public Dictionary<Point3, ExperienceOre> m_blocks = new Dictionary<Point3, ExperienceOre>();
        public float m_visibilityRange;
        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                    330
                };
            }
        }

        protected override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
            m_texture = ContentManager.Get<Texture2D>("Textures/ExperienceOrb");
            m_primitivesRenderer = Project.FindSubsystem<SubsystemModelsRenderer>(true).PrimitivesRenderer;
            m_visibilityRange = (float)SettingsManager.VisibilityRange;
        }
        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            Point3 point = new Point3(x, y, z);
            if (!m_blocks.ContainsKey(point))
            {
                m_blocks.Add(point, new ExperienceOre(x, y, z));
            }
        }
        public override void OnBlockGenerated(int value, int x, int y, int z, bool isLoaded)
        {
            Point3 point = new Point3(x, y, z);
            if (!m_blocks.ContainsKey(point))
            {
                m_blocks.Add(point, new ExperienceOre(x, y, z));
            }
        }
        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            Point3 point = new Point3(x, y, z);
            if (m_blocks.ContainsKey(point))
            {
                m_blocks.Remove(point);
            }
            m_subsystemAudio.PlaySound("Audio/ExperienceCollected", 0.3f, this.m_random.UniformFloat(-0.1f, 0.4f), 0f, 0f);
        }
        public void Draw(Camera camera, int drawOrder)
        {
            if (drawOrder == 10)
            {
                double nowTime = m_subsystemTime.GameTime;
                float dt = m_subsystemTime.GameTimeDelta;
                foreach (ExperienceOre block in m_blocks.Values)
                {
                    Vector3 blockPosition = new Vector3(block.position.X + 0.5f, block.position.Y + 0.5f, block.position.Z + 0.5f);
                    foreach (ExperienceOrb orb in block.orbs)
                    {
                        if (Vector3.Distance(orb.position, camera.ViewPosition) < m_visibilityRange + 10)
                        {
                            if (nowTime >= orb.timeToStopMoving)
                            {
                                orb.nextPosition = blockPosition + m_random.Vector3(0.5f);
                                orb.timeToStopMoving += Vector3.Distance(orb.position, orb.nextPosition) * MathUtils.Clamp(orb.size * orb.size, 0.015f, 0.09f) * m_random.UniformFloat(120f, 200f);
                            }
                            else
                            {
                                Vector3 speed = (orb.nextPosition - orb.position) / (float)(orb.timeToStopMoving - nowTime) * dt;
                                orb.position += speed;
                            }
                            if (nowTime >= orb.timeToStopResizing)
                            {
                                orb.nextSize = m_random.UniformFloat(0.03f, 0.3f);
                                orb.timeToStopResizing += Math.Abs(orb.nextSize - orb.size) * MathUtils.Clamp(0.07f - orb.size * orb.size, 0.02f, 0.07f) * m_random.UniformFloat(600f, 900f);
                            }
                            else
                            {
                                float speed = (orb.nextSize - orb.size) / (float)(orb.timeToStopResizing - nowTime) * dt;
                                orb.size = MathUtils.Clamp(orb.size + speed, 0.03f, 0.3f);
                            }
                            Vector3 v1 = Vector3.Normalize(Vector3.Cross(camera.ViewDirection, Vector3.UnitY));
                            Vector3 v2 = -Vector3.Normalize(Vector3.Cross(camera.ViewDirection, v1));
                            Vector3 p1 = Vector3.Transform(orb.position + orb.size * (-v1 - v2), camera.ViewMatrix);
                            Vector3 p2 = Vector3.Transform(orb.position + orb.size * (v1 - v2), camera.ViewMatrix);
                            Vector3 p3 = Vector3.Transform(orb.position + orb.size * (-v1 + v2), camera.ViewMatrix);
                            Vector3 p4 = Vector3.Transform(orb.position + orb.size * (v1 + v2), camera.ViewMatrix);
                            TexturedBatch3D texturedBatch3D = m_primitivesRenderer.TexturedBatch(m_texture, true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.AnisotropicWrap);
                            texturedBatch3D.QueueQuad(p1, p3, p4, p2, new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), Color.White);
                        }
                    }
                }
            }
        }
        public int[] DrawOrders
        {
            get
            {
                return new int[] { 10 };
            }
        }
        public class ExperienceOre
        {
            public Random m_random = new Random();
            public ExperienceOre(int x, int y, int z)
            {
                position = new Point3(x, y, z);
                orbs = new ExperienceOrb[3];
                Vector3 vector3 = new Vector3((float)x + 0.5f, (float)y + 0.5f, (float)z + 0.5f);
                orbs[0] = new ExperienceOrb(vector3 + m_random.Vector3(0.5f), m_random.UniformFloat(0.03f, 0.3f));
                orbs[1] = new ExperienceOrb(vector3 + m_random.Vector3(0.5f), m_random.UniformFloat(0.03f, 0.3f));
                orbs[2] = new ExperienceOrb(vector3 + m_random.Vector3(0.5f), m_random.UniformFloat(0.03f, 0.3f));
            }
            public Point3 position;
            public ExperienceOrb[] orbs;
        }
        public class ExperienceOrb
        {
            public ExperienceOrb(Vector3 point, float num)
            {
                position = point;
                nextPosition = point;
                timeToStopMoving = 0;
                size = num;
                nextSize = num;
                timeToStopResizing = 0;
            }
            public Vector3 position;
            public Vector3 nextPosition;
            public double timeToStopMoving;
            public float size;
            public float nextSize;
            public double timeToStopResizing;
        }
    }
}