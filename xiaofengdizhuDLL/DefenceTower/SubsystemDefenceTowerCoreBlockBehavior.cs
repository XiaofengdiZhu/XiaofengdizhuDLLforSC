using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Engine;
using Engine.Serialization;
using TemplatesDatabase;
using GameEntitySystem;

namespace Game
{
    public class SubsystemDefenceTowerCoreBlockBehavior : SubsystemBlockBehavior,IUpdateable
    {
        public SubsystemBodies m_subsystemBodies;
        public SubsystemProjectiles m_subsystemProjectiles;
        public SubsystemTime m_subsystemTime;
        public SubsystemPlayers m_subsystemPlayers;
        public SubsystemSky m_subsystemSky;
        public DynamicArray<ComponentBody> m_componentBodies = new DynamicArray<ComponentBody>();
        Random m_random = new Random();
        public Dictionary<Point3, DefenceTower> m_defenceTowers = new Dictionary<Point3, DefenceTower>();
        public bool m_attackPlayer = false;
        protected override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemBodies = Project.FindSubsystem<SubsystemBodies>(true);
            m_subsystemProjectiles = Project.FindSubsystem<SubsystemProjectiles>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemPlayers = Project.FindSubsystem<SubsystemPlayers>(true);
            m_subsystemSky = Project.FindSubsystem<SubsystemSky>(true);
        }
        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                    360
                };
            }
        }
        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            AddDefenceTower(x, y, z, Terrain.ExtractData(value));
        }
        public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
        {
            int type = Terrain.ExtractData(SubsystemTerrain.Terrain.GetCellValue(x, y, z));
            if (type > 0 && (x == neighborX && y == neighborY + 1 && z == neighborZ && SubsystemTerrain.Terrain.GetCellContents(neighborX, neighborY, neighborZ) != DefenceTowerManager.m_towerBlocks[type].Last() || x != neighborX && y != neighborY && z != neighborZ && SubsystemTerrain.Terrain.GetCellContents(neighborX, neighborY, neighborZ) != 0))
            {
                SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
            }
        }
        public override void OnBlockGenerated(int value, int x, int y, int z, bool isLoaded)
        {
            AddDefenceTower(x, y, z, Terrain.ExtractData(value));
        }
        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            if (Terrain.ExtractData(value) > 0)
            {
                Point3 point3 = new Point3(x, y, z);
                if (m_defenceTowers.ContainsKey(point3))
                {
                    m_defenceTowers.Remove(point3);
                }
            }
        }
        public void Update(float t)
        {
            double nowTime = m_subsystemTime.GameTime;
            foreach (DefenceTower tower in m_defenceTowers.Values)
            {
                if (nowTime > tower.nexeActTime)
                {
                    tower.nexeActTime += DefenceTowerManager.m_towerPeriod[tower.type];
                    if (tower.type == 1)
                    {
                        m_componentBodies.Clear();
                        m_subsystemBodies.FindBodiesAroundPoint(new Vector2(tower.position.X, tower.position.Z), 12, this.m_componentBodies);
                        if (m_componentBodies.Count > 0)
                        {
                            foreach (ComponentBody body in m_componentBodies.Array)
                            {
                                if (!m_attackPlayer && m_subsystemPlayers.IsPlayer(body.Entity) || body.Entity.FindComponent<ComponentHealth>().Health <= 0)
                                {
                                    continue;
                                }
                                FireItem(new Vector3(tower.position) + new Vector3(0.5f), body, 32960, 20f);
                                break;
                            }
                        }
                    }
                    else if (tower.type == 2)
                    {
                        m_componentBodies.Clear();
                        m_subsystemBodies.FindBodiesAroundPoint(new Vector2(tower.position.X, tower.position.Z), 12, this.m_componentBodies);
                        if (m_componentBodies.Count > 0)
                        {
                            foreach (ComponentBody body in m_componentBodies.Array)
                            {
                                if (!m_attackPlayer && m_subsystemPlayers.IsPlayer(body.Entity) || body.Entity.FindComponent<ComponentHealth>().Health <= 0)
                                {
                                    continue;
                                }
                                FireItem(new Vector3(tower.position) + new Vector3(0.5f), body, 82112, 40f);
                                break;
                            }
                        }
                    }
                    else if (tower.type == 3)
                    {
                        m_componentBodies.Clear();
                        m_subsystemBodies.FindBodiesAroundPoint(new Vector2(tower.position.X, tower.position.Z), 12, this.m_componentBodies);
                        if (m_componentBodies.Count > 0)
                        {
                            foreach (ComponentBody body in m_componentBodies.Array)
                            {
                                if (!m_attackPlayer && m_subsystemPlayers.IsPlayer(body.Entity) || body.Entity.FindComponent<ComponentHealth>().Health <= 0)
                                {
                                    continue;
                                }
                                FireItem(new Vector3(tower.position) + new Vector3(0.5f), body, 32982, 120f);
                                break;
                            }
                        }
                    }
                    else if (tower.type == 4)
                    {
                        m_componentBodies.Clear();
                        m_subsystemBodies.FindBodiesAroundPoint(new Vector2(tower.position.X, tower.position.Z), 8, this.m_componentBodies);
                        if (m_componentBodies.Count > 0)
                        {
                            foreach (ComponentBody body in m_componentBodies.Array)
                            {
                                if (!m_attackPlayer && m_subsystemPlayers.IsPlayer(body.Entity) || body.Entity.FindComponent<ComponentHealth>().Health <= 0)
                                {
                                    continue;
                                }
                                Vector3 towerPosition = new Vector3(tower.position);
                                Vector3 impulse = Vector3.Zero;
                                Vector3 bodyDirection = Vector3.Normalize(body.Position - towerPosition);
                                if (Vector3.DistanceSquared(towerPosition, body.Position) > 16)
                                {
                                    impulse -= bodyDirection;
                                }
                                else
                                {
                                    impulse += bodyDirection;
                                }
                                body.ApplyImpulse(impulse);
                                continue;
                            }
                        }
                    }
                    else if (tower.type == 5)
                    {
                        m_componentBodies.Clear();
                        m_subsystemBodies.FindBodiesAroundPoint(new Vector2(tower.position.X, tower.position.Z), 24, this.m_componentBodies);
                        if (m_componentBodies.Count > 0)
                        {
                            foreach (ComponentBody body in m_componentBodies.Array)
                            {
                                if (!m_attackPlayer && m_subsystemPlayers.IsPlayer(body.Entity) || body.Entity.FindComponent<ComponentHealth>().Health <= 0)
                                {
                                    continue;
                                }
                                m_subsystemSky.MakeLightningStrike(body.Position);
                                break;
                            }
                        }
                    }
                }
            }

        }
        public int UpdateOrder
        {
            get
            {
                return 10;
            }
        }
        public bool AddDefenceTower(int x, int y, int z, int type)
        {
            if (type > 0)
            {
                Point3 point3 = new Point3(x, y, z);
                m_defenceTowers.Add(point3, new DefenceTower(point3, type, m_subsystemTime.GameTime));
                return true;
            }
            else
            {
                for (int i = 1; i < DefenceTowerManager.m_towerKindCount; i++)
                {
                    int[] array = DefenceTowerManager.m_towerBlocks[i];
                    bool flag = true;
                    int bottom = y - array.Length;
                    for (int j = 0; j <= array.Length + 1; j++)
                    {
                        if (!flag) break;
                        for (int k = -1; k < 2; k++)
                        {
                            if (!flag) break;
                            for (int l = -1; l < 2; l++)
                            {
                                if (k == 0 && l == 0)
                                {
                                    if (j < array.Length && SubsystemTerrain.Terrain.GetCellContents(x, bottom + j, z) != array[j])
                                    {
                                        flag = false;
                                        break;
                                    }
                                    else if (j > array.Length && SubsystemTerrain.Terrain.GetCellContents(x, bottom + j, z) != 0)
                                    {
                                        flag = false;
                                        break;
                                    }
                                }
                                else if (SubsystemTerrain.Terrain.GetCellContents(x + k, bottom + j, z + l) != 0)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        SubsystemTerrain.ChangeCell(x, y, z, Terrain.ReplaceData(360, i));
                        Point3 point3 = new Point3(x, y, z);
                        m_defenceTowers.Add(point3, new DefenceTower(point3, i, m_subsystemTime.GameTime));
                        return true;
                    }
                }
            }
            return false;
        }
        public void FireItem(Vector3 towerPosition,ComponentBody body,int blockValue,float vx)
        {
            Vector3 bodyPosition = body.Position;
            float[] distance = new float[5];
            for (int i = 0; i < 5; i++)
            {
                distance[i] = Vector3.DistanceSquared(towerPosition + CellFace.FaceToVector3(i) * 0.6f, bodyPosition);
            }
            int minIndex = 0;
            for (int j = 1; j < 5; j++)
            {
                if (distance[j] < distance[minIndex])
                {
                    minIndex = j;
                }
            }
            Vector3 firePosition = towerPosition + CellFace.FaceToVector3(minIndex) * 0.6f;

            float sx = Vector2.Distance(firePosition.XZ, bodyPosition.XZ);
            bodyPosition += body.Velocity * 1.2f * sx / vx;
            sx = Vector2.Distance(firePosition.XZ, bodyPosition.XZ);
            float sy = bodyPosition.Y - firePosition.Y + 0.2f;
            float vy = sy * vx / sx + 5f * sx / vx;
            m_subsystemProjectiles.FireProjectile(blockValue, firePosition, Vector3.Normalize(new Vector3(bodyPosition.X, 0f, bodyPosition.Z) - new Vector3(firePosition.X, 0f, firePosition.Z)) * vx + Vector3.UnitY * vy, Vector3.Zero, null);
        }
        public class DefenceTower
        {
            public DefenceTower(Point3 point3,int type0,double nowTime)
            {
                position = point3;
                type = type0;
                nexeActTime = nowTime + DefenceTowerManager.m_towerPeriod[type];
            }
            public Point3 position;
            public int type;
            public double nexeActTime;
        }
    }
}
