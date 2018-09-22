using Engine;
using Engine.Graphics;
using Engine.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesDatabase;

namespace Game
{
    class SubsystemFactorioTransportBeltBLockBehavior : SubsystemBlockBehavior, IDrawable, IUpdateable
    {
        public SubsystemTime m_subsystemTime;
        public PrimitivesRenderer3D m_primitivesRenderer;
        public Dictionary<Point3, FactorioTransportBelt> m_blocks = new Dictionary<Point3, FactorioTransportBelt>();
        public Texture2D[] m_textures;
        public float m_visibilityRange;
        public double m_lastDrawtime = 0;
        public int[] m_drawedTime = new int[4] { 0, 0, 0, 0 };
        public Vector3[] m_floorOffset = new Vector3[4]
        {
            new Vector3(-0.5f,0f,-0.5f),
            new Vector3(0.5f,0f,-0.5f),
            new Vector3(0.5f,0f,0.5f),
            new Vector3(-0.5f,0f,0.5f)
        };
        public Vector3[,,] m_slopeOffset = new Vector3[4, 2, 4] {
            {
                {
                    new Vector3(-0.5f,-0.5f,-0.5f),
                    new Vector3(0.5f,-0.5f,-0.5f),
                    new Vector3(0.5f,0.5f,0.5f),
                    new Vector3(-0.5f,0.5f,0.5f)
                },
                {
                    new Vector3(-0.5f,0.5f,-0.5f),
                    new Vector3(0.5f,0.5f,-0.5f),
                    new Vector3(0.5f,-0.5f,0.5f),
                    new Vector3(-0.5f,-0.5f,0.5f)
                }
            },
            {
                {
                    new Vector3(-0.5f,-0.5f,-0.5f),
                    new Vector3(0.5f,0.5f,-0.5f),
                    new Vector3(0.5f,0.5f,0.5f),
                    new Vector3(-0.5f,-0.5f,0.5f)
                },
                {
                    new Vector3(-0.5f,0.5f,-0.5f),
                    new Vector3(0.5f,-0.5f,-0.5f),
                    new Vector3(0.5f,-0.5f,0.5f),
                    new Vector3(-0.5f,0.5f,0.5f)
                }
            },
            {
                {
                    new Vector3(-0.5f,0.5f,-0.5f),
                    new Vector3(0.5f,0.5f,-0.5f),
                    new Vector3(0.5f,-0.5f,0.5f),
                    new Vector3(-0.5f,-0.5f,0.5f)
                },
                {
                    new Vector3(-0.5f,-0.5f,-0.5f),
                    new Vector3(0.5f,-0.5f,-0.5f),
                    new Vector3(0.5f,0.5f,0.5f),
                    new Vector3(-0.5f,0.5f,0.5f)
                }
            },
            {
                {
                    new Vector3(-0.5f,0.5f,-0.5f),
                    new Vector3(0.5f,-0.5f,-0.5f),
                    new Vector3(0.5f,-0.5f,0.5f),
                    new Vector3(-0.5f,0.5f,0.5f)
                },
                {
                    new Vector3(-0.5f,-0.5f,-0.5f),
                    new Vector3(0.5f,0.5f,-0.5f),
                    new Vector3(0.5f,0.5f,0.5f),
                    new Vector3(-0.5f,-0.5f,0.5f)
                }
            }
        };
        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                    400
                };
            }
        }
        protected override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            string value = valuesDictionary.GetValue<string>("FactorioTransportBelt");
            //this.m_blocks = new DynamicArray<Point3>(HumanReadableConverter.ValuesListFromString<Point3>(';', value));
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_textures = new Texture2D[3]
            {
                ContentManager.Get<Texture2D>("Textures/Factorio/Transport-belt_sprite"),
                ContentManager.Get<Texture2D>("Textures/Factorio/Fast-transport-belt_sprite"),
                ContentManager.Get<Texture2D>("Textures/Factorio/Express-transport-belt_sprite")
            };
            m_primitivesRenderer = Project.FindSubsystem<SubsystemModelsRenderer>(true).PrimitivesRenderer;
            m_visibilityRange = (float)SettingsManager.VisibilityRange;
        }
        protected override void Save(ValuesDictionary valuesDictionary)
        {
            base.Save(valuesDictionary);
            //string value = HumanReadableConverter.ValuesListToString<Point3>(';', this.m_blocks.ToArray<Point3>());
            //valuesDictionary.SetValue<string>("FactorioBeltTransport", value);
        }
        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            Point3 point = new Point3(x, y, z);
            UpdateFTB(value, oldValue, point);
            if (!m_blocks.ContainsKey(point))
            {
                m_blocks.Add(point, new FactorioTransportBelt(point, value));
            }
        }
        public override void OnBlockGenerated(int value, int x, int y, int z, bool isLoaded)
        {
            Point3 point = new Point3(x, y, z);
            UpdateFTB(value, value, point);
            if (!m_blocks.ContainsKey(point))
            {
                m_blocks.Add(point, new FactorioTransportBelt(point, value));
            }
        }
        public static Point3[] m_surroundDirections = new Point3[4]
        {
            new Point3(1,0,0),
            new Point3(-1,0,0),
            new Point3(0,0,1),
            new Point3(0,0,-1)
        };
        public static Point3[] m_surroundDownDirections = new Point3[4]
        {
            new Point3(1,-1,0),
            new Point3(-1,-1,0),
            new Point3(0,-1,1),
            new Point3(0,-1,-1)
        };
        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            Point3 point = new Point3(x, y, z);
            UpdateFTB(newValue, value, point);
            if (m_blocks.ContainsKey(point))
            {
                m_blocks.Remove(point);
            }
        }
        public override void OnBlockModified(int value, int oldValue, int x, int y, int z)
        {
            Point3 point = new Point3(x, y, z);
            UpdateFTB(value, oldValue, point);
            if (m_blocks.ContainsKey(point))
            {
                m_blocks[point] = new FactorioTransportBelt(point, value);
            }
            else
            {
                m_blocks.Add(point, new FactorioTransportBelt(point, value));
            }
        }
        public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
        {
            int value = base.SubsystemTerrain.Terrain.GetCellValue(x, y, z);
            if (y - 1 == neighborY)
            {
                int cellContents = base.SubsystemTerrain.Terrain.GetCellContents(x, neighborY, z);
                if (BlocksManager.Blocks[cellContents].IsTransparent)
                {
                    base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
                }
            }
        }

        public void Update(float dt)
        { }
        public void Draw(Camera camera, int drawOrder)
        {
            try
            {
                if (drawOrder == 10)
                {
                    double nowTime = m_subsystemTime.GameTime;
                    if (nowTime - m_lastDrawtime >= 0.01)
                    {
                        m_drawedTime[3]++;
                        m_drawedTime[2]++;
                        if (m_drawedTime[3] == 1)
                        {
                            m_drawedTime[1]++;
                        }
                        else if (m_drawedTime[3] > 3)
                        {
                            m_drawedTime[0]++;
                            m_drawedTime[1]++;
                            m_drawedTime[3] = 0;
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            if (m_drawedTime[i] > 15)
                            {
                                m_drawedTime[i] = 0;
                            }
                        }
                        m_lastDrawtime = nowTime;
                    }
                    foreach (FactorioTransportBelt block in m_blocks.Values)
                    {
                        Vector3 displayPosition = new Vector3((float)block.position.X + 0.5f, (float)block.position.Y + (block.slopeType.HasValue ? 0.5f : 0.01f), (float)block.position.Z + 0.5f);
                        if (Vector3.Distance(displayPosition, camera.ViewPosition) < m_visibilityRange + 10)
                        {
                            TexturedBatch3D texturedBatch3D = m_primitivesRenderer.TexturedBatch(m_textures[block.color], true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.PointClamp);
                            Vector3[] displayVertices = new Vector3[4];
                            for (int i = 0; i < 4; i++)
                            {
                                displayVertices[i] = Vector3.Transform(displayPosition + (block.slopeType.HasValue ? m_slopeOffset[block.rotation, block.slopeType.Value ? 1 : 0, i] : m_floorOffset[i]), camera.ViewMatrix);
                            }
                            Vector4 textureVertices = FactorioTransportBeltBlock.m_texCoords[block.cornerType.HasValue ? block.cornerType.Value + 4 : block.rotation, m_drawedTime[block.color]];
                            texturedBatch3D.QueueQuad(displayVertices[0], displayVertices[1], displayVertices[2], displayVertices[3], new Vector2(textureVertices.X, textureVertices.Y), new Vector2(textureVertices.Z, textureVertices.Y), new Vector2(textureVertices.Z, textureVertices.W), new Vector2(textureVertices.X, textureVertices.W), Color.White);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Warning("0：" + e.ToString());
            }
        }
        public int[] DrawOrders
        {
            get
            {
                return new int[] { 10 };
            }
        }
        public int UpdateOrder
        {
            get
            {
                return 0;
            }
        }
        public class FactorioTransportBelt
        {
            public FactorioTransportBelt(Point3 position, int value)
            {
                this.position = position;
                if (Terrain.ExtractContents(value) == FactorioTransportBeltBlock.Index)
                {
                    int data = Terrain.ExtractData(value);
                    this.color = FactorioTransportBeltBlock.GetColor(data);
                    this.rotation = FactorioTransportBeltBlock.GetRotation(data);
                    this.cornerType = FactorioTransportBeltBlock.GetCornerType(data);
                    this.slopeType = FactorioTransportBeltBlock.GetSlopeType(data);
                }
                else
                {
                    this.isFTB = false;
                }
            }
            public static FactorioTransportBelt GetFromPosition(Point3 position,Terrain terrain)
            {
                return new FactorioTransportBelt(position, terrain.GetCellValue(position.X, position.Y, position.Z));
            }
            public int Data
            {
                get
                {
                    return FactorioTransportBeltBlock.SetCornerType(FactorioTransportBeltBlock.SetSlopeType(FactorioTransportBeltBlock.SetRotation(FactorioTransportBeltBlock.SetColor(0, color), rotation), slopeType), cornerType);
                }
            }
            public int Value
            {
                get
                {
                    return Terrain.MakeBlockValue(FactorioTransportBeltBlock.Index,0,this.Data);
                }
            }
            public FactorioTransportBelt(Point3 position, int color, int rotation, int? cornerType, bool? isSlope)
            {
                this.position = position;
                this.color = color;
                this.rotation = rotation;
                this.cornerType = cornerType;
                this.slopeType = isSlope;
            }
            public bool isFTB = true;
            public Point3 position;
            public int color;
            public int rotation;
            public int? cornerType;
            public bool? slopeType;
        }
        public void UpdateFTB(int newValue, int oldValue, Point3 position)
        {
            try {
                bool isUpdated = newValue != oldValue;
                List<Point3> list_positionsShouldBeUpdated = new List<Point3>();
                FactorioTransportBelt oldFTB = new FactorioTransportBelt(position, oldValue);
                FactorioTransportBelt newFTB = new FactorioTransportBelt(position, newValue);
                if (newFTB.isFTB)
                {
                    FactorioTransportBelt updatedFTB = UpdateFTBAccordingToNeighbours(newFTB);
                    int updatedData = updatedFTB.Data;
                    if (updatedData != Terrain.ExtractData(newValue))
                    {
                        base.SubsystemTerrain.ChangeCell(position.X, position.Y, position.Z, Terrain.ReplaceData(newValue, updatedData), true);
                        isUpdated = true;
                        if (updatedFTB.slopeType.HasValue)
                        {
                            list_positionsShouldBeUpdated.Add(position + Point3.UnitY + (updatedFTB.slopeType.Value ? FactorioTransportBeltBlock.RotationToDirection(updatedFTB.rotation) : FactorioTransportBeltBlock.RotationToDirection(TurnBack(updatedFTB.rotation))));
                        }
                    }
                }
                if (isUpdated)
                {
                    if (newValue != oldValue && oldFTB.isFTB)
                    {
                        if (oldFTB.slopeType.HasValue)
                        {
                            Point3 tempPosition = position + Point3.UnitY + (oldFTB.slopeType.Value ? FactorioTransportBeltBlock.RotationToDirection(oldFTB.rotation) : FactorioTransportBeltBlock.RotationToDirection(TurnBack(oldFTB.rotation)));
                            if (!list_positionsShouldBeUpdated.Contains(tempPosition))
                            {
                                list_positionsShouldBeUpdated.Add(tempPosition);
                            }
                        }
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        list_positionsShouldBeUpdated.Add(position + m_surroundDirections[i]);
                        list_positionsShouldBeUpdated.Add(position + m_surroundDownDirections[i]);
                    }
                    foreach (Point3 point3 in list_positionsShouldBeUpdated)
                    {
                        int tempValue = base.SubsystemTerrain.Terrain.GetCellValue(point3.X, point3.Y, point3.Z);
                        UpdateFTB(tempValue, tempValue, point3);
                    }
                }
            }
            catch(Exception e)
            {
                Log.Warning(e.ToString());
            }
        }
        public FactorioTransportBelt UpdateFTBAccordingToNeighbours(FactorioTransportBelt FTB)
        {
            if (FTB.isFTB)
            {
                int turnBack = TurnBack(FTB.rotation);
                FactorioTransportBelt backFTB = FactorioTransportBelt.GetFromPosition(FTB.position + FactorioTransportBeltBlock.RotationToDirection(turnBack), base.SubsystemTerrain.Terrain);
                if (!backFTB.isFTB)
                {
                    backFTB = FactorioTransportBelt.GetFromPosition(backFTB.position - Point3.UnitY, base.SubsystemTerrain.Terrain);
                }
                bool flag = false;
                if (!backFTB.isFTB)
                {
                    flag = true;
                    FactorioTransportBelt.GetFromPosition(backFTB.position + new Point3(0, 2, 0), base.SubsystemTerrain.Terrain);
                }
                if (backFTB.isFTB && backFTB.rotation == FTB.rotation)
                {
                    FactorioTransportBelt frontUpFTB = FactorioTransportBelt.GetFromPosition(FTB.position + FactorioTransportBeltBlock.RotationToDirection(FTB.rotation) + Point3.UnitY, base.SubsystemTerrain.Terrain);
                    if (frontUpFTB.isFTB && frontUpFTB.rotation != turnBack)
                    {
                        FTB.slopeType = flag ? null : (bool?)true;
                        FTB.cornerType = null;
                        return FTB;
                    }
                    else
                    {
                        FTB.slopeType = flag ? (bool?)false : null;
                        FTB.cornerType = null;
                        return FTB;
                    }
                }
                else
                {
                    int turnLeft = TurnLeft(FTB.rotation);
                    int turnRight = TurnRight(FTB.rotation);
                    FactorioTransportBelt leftFTB = FactorioTransportBelt.GetFromPosition(FTB.position + FactorioTransportBeltBlock.RotationToDirection(turnLeft), base.SubsystemTerrain.Terrain);
                    if (!leftFTB.isFTB)
                    {
                        leftFTB = FactorioTransportBelt.GetFromPosition(leftFTB.position - Point3.UnitY, base.SubsystemTerrain.Terrain);
                    }
                    FactorioTransportBelt rightFTB = FactorioTransportBelt.GetFromPosition(FTB.position + FactorioTransportBeltBlock.RotationToDirection(turnRight), base.SubsystemTerrain.Terrain);
                    if (!rightFTB.isFTB)
                    {
                        rightFTB = FactorioTransportBelt.GetFromPosition(rightFTB.position - Point3.UnitY, base.SubsystemTerrain.Terrain);
                    }
                    if (leftFTB.isFTB && leftFTB.rotation == turnRight)
                    {
                        if (rightFTB.isFTB && rightFTB.rotation == turnLeft)
                        {
                            FTB.slopeType = null;
                            FTB.cornerType = null;
                            return FTB;
                        }
                        else
                        {
                            FTB.slopeType = null;
                            FTB.cornerType = FactorioTransportBeltBlock.m_rotations2CornerType[FTB.rotation, 0];
                            return FTB;
                        }
                    }
                    else if (rightFTB.isFTB && rightFTB.rotation == turnLeft)
                    {
                        FTB.slopeType = null;
                        FTB.cornerType = FactorioTransportBeltBlock.m_rotations2CornerType[FTB.rotation, 1];
                        return FTB;
                    }
                }
            }
            FTB.slopeType = null;
            FTB.cornerType = null;
            return FTB;
        }
        public static int TurnLeft(int rotation)
        {
            switch(rotation){
                case 0:return 1;
                case 1:return 2;
                case 2:return 3;
                case 3:return 0;
                default: return (rotation + 1) % 4;
            }
        }
        public static int TurnRight(int rotation)
        {
            switch (rotation)
            {
                case 0: return 3;
                case 1: return 0;
                case 2: return 1;
                case 3: return 2;
                default: return (rotation + 3) % 4;
            }
        }
        public static int TurnBack(int rotation)
        {
            switch (rotation)
            {
                case 0: return 2;
                case 1: return 3;
                case 2: return 0;
                case 3: return 1;
                default: return (rotation + 2) % 4;
            }
        }
    }
}