using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Engine;
using Engine.Serialization;
using TemplatesDatabase;

namespace Game
{
    public class SubsystemOreSaplingBlockBehavior : SubsystemBlockBehavior, IUpdateable
    {
        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                    340
                };
            }
        }
        public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
        {
            int cellContents = base.SubsystemTerrain.Terrain.GetCellContents(x, y - 1, z);
            if (BlocksManager.Blocks[cellContents].IsTransparent)
            {
                base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
            }
        }
        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            float num = (this.m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative) ? this.m_random.UniformFloat(2f, 4f) : this.m_random.UniformFloat(480f, 600f);
            this.AddSapling(new SubsystemOreSaplingBlockBehavior.OreSaplingData
            {
                Point = new Point3(x, y, z),
                Type = (OreTreeType)Terrain.ExtractData(value),
                MatureTime = this.m_subsystemGameInfo.TotalElapsedGameTime + (double)num
            });
        }
        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            this.RemoveSapling(new Point3(x, y, z));
        }
        protected override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            this.m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
            this.m_enumerator = this.m_saplings.Values.GetEnumerator();
            foreach (object obj in valuesDictionary.GetValue<ValuesDictionary>("OreSaplings").Values)
            {
                string data = (string)obj;
                this.AddSapling(this.LoadSaplingData(data));
            }
        }
        protected override void Save(ValuesDictionary valuesDictionary)
        {
            ValuesDictionary valuesDictionary2 = new ValuesDictionary();
            valuesDictionary.SetValue<ValuesDictionary>("OreSaplings", valuesDictionary2);
            int num = 0;
            foreach (SubsystemOreSaplingBlockBehavior.OreSaplingData saplingData in this.m_saplings.Values)
            {
                ValuesDictionary valuesDictionary3 = valuesDictionary2;
                int num2 = num;
                num = num2 + 1;
                valuesDictionary3.SetValue<string>(num2.ToString(CultureInfo.InvariantCulture), this.SaveSaplingData(saplingData));
            }
        }
        public int UpdateOrder
        {
            get
            {
                return 0;
            }
        }
        public void Update(float dt)
        {
            for (int i = 0; i < 10; i++)
            {
                if (!this.m_enumerator.MoveNext())
                {
                    this.m_enumerator = this.m_saplings.Values.GetEnumerator();
                    return;
                }
                this.MatureSapling(this.m_enumerator.Current);
            }
        }
        public SubsystemOreSaplingBlockBehavior.OreSaplingData LoadSaplingData(string data)
        {
            string[] array = data.Split(new char[]
            {
                ';'
            });
            if (array.Length != 3)
            {
                throw new InvalidOperationException("Invalid sapling data string.");
            }
            return new SubsystemOreSaplingBlockBehavior.OreSaplingData
            {
                Point = HumanReadableConverter.ConvertFromString<Point3>(array[0]),
                Type = HumanReadableConverter.ConvertFromString<OreTreeType>(array[1]),
                MatureTime = HumanReadableConverter.ConvertFromString<double>(array[2])
            };
        }
        public string SaveSaplingData(SubsystemOreSaplingBlockBehavior.OreSaplingData saplingData)
        {
            this.m_stringBuilder.Length = 0;
            this.m_stringBuilder.Append(HumanReadableConverter.ConvertToString(saplingData.Point));
            this.m_stringBuilder.Append(';');
            this.m_stringBuilder.Append(HumanReadableConverter.ConvertToString(saplingData.Type));
            this.m_stringBuilder.Append(';');
            this.m_stringBuilder.Append(HumanReadableConverter.ConvertToString(saplingData.MatureTime));
            return this.m_stringBuilder.ToString();
        }
        public void MatureSapling(SubsystemOreSaplingBlockBehavior.OreSaplingData saplingData)
        {
            if (this.m_subsystemGameInfo.TotalElapsedGameTime >= saplingData.MatureTime)
            {
                int x = saplingData.Point.X;
                int y = saplingData.Point.Y;
                int z = saplingData.Point.Z;
                TerrainChunk chunkAtCell = base.SubsystemTerrain.Terrain.GetChunkAtCell(x - 6, z - 6);
                TerrainChunk chunkAtCell2 = base.SubsystemTerrain.Terrain.GetChunkAtCell(x - 6, z + 6);
                TerrainChunk chunkAtCell3 = base.SubsystemTerrain.Terrain.GetChunkAtCell(x + 6, z - 6);
                TerrainChunk chunkAtCell4 = base.SubsystemTerrain.Terrain.GetChunkAtCell(x + 6, z + 6);
                if (y<36 && chunkAtCell != null && chunkAtCell.State == TerrainChunkState.Valid && chunkAtCell2 != null && chunkAtCell2.State == TerrainChunkState.Valid && chunkAtCell3 != null && chunkAtCell3.State == TerrainChunkState.Valid && chunkAtCell4 != null && chunkAtCell4.State == TerrainChunkState.Valid)
                {
                    int cellContents = base.SubsystemTerrain.Terrain.GetCellContents(x, y - 1, z);
                    //只在大锗块上生长
                    if (cellContents != 231)
                    {
                        SetBlock(x,y,z,28);
                        RemoveSapling(new Point3(x, y, z));
                        return;
                    }
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = z - 1; j <= z + 1; j++)
                        {
                            int cell2 = base.SubsystemTerrain.Terrain.GetCellValue(i, y - 1, j);
                            //周围一圈高度较高的岩浆才生长
                            if (!(BlocksManager.Blocks[Terrain.ExtractContents(cell2)] is MagmaBlock && (Terrain.ExtractData(cell2) & 15) <2))
                            {
                                if (i == x && j == z) continue;
                                SetBlock(x,y,z,28);
                                RemoveSapling(new Point3(x, y, z));
                                return;
                            }
                        }
                    }
                    SubsystemTerrain.ChangeCell(x, y, z, Terrain.MakeBlockValue(0, 0, 0), false);
                    if (!GrowTree(x, y, z, saplingData.Type))
                    {
                        SetBlock(x,y,z,28);
                        RemoveSapling(new Point3(x, y, z));
                        return;
                    }
                    else if (this.m_subsystemGameInfo.TotalElapsedGameTime > saplingData.MatureTime + 1200.0)
                    {
                        SetBlock(x,y,z,28);
                        RemoveSapling(new Point3(x, y, z));
                        return;
                    }
                    else
                    {
                        for (int i = x - 1; i <= x + 1; i++)
                        {
                            for (int j = z - 1; j <= z + 1; j++)
                            {
                                int cellContents3 = base.SubsystemTerrain.Terrain.GetCellContents(i, y - 1, j);
                                if ((BlocksManager.Blocks[cellContents3] is MagmaBlock))
                                {
                                    SubsystemTerrain.ChangeCell(i, y-1, j, 0, true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    saplingData.MatureTime = this.m_subsystemGameInfo.TotalElapsedGameTime;
                }
            }
        }
        public bool GrowTree(int x, int y, int z, OreTreeType treeType)
        {
            ReadOnlyList<TerrainBrush> treeBrushes = OrePlantsManager.GetTreeBrushes(treeType);
            for (int i = 0; i < 20; i++)
            {
                TerrainBrush terrainBrush = treeBrushes[this.m_random.UniformInt(0, treeBrushes.Count - 1)];
                bool flag = true;
                foreach (TerrainBrush.Cell cell in terrainBrush.Cells)
                {
                    if (cell.Y >= 0 && (cell.X != 0 || cell.Y != 0 || cell.Z != 0))
                    {
                        int cellContents = base.SubsystemTerrain.Terrain.GetCellContents((int)cell.X + x, (int)cell.Y + y, (int)cell.Z + z);
                        if (cellContents != 0 && !(Array.IndexOf(OrePlantsManager.m_treeLeavesByType,cellContents)>-1))
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    terrainBrush.Paint(base.SubsystemTerrain, x, y, z);
                    return true;
                }
            }
            return false;
        }
        public void AddSapling(SubsystemOreSaplingBlockBehavior.OreSaplingData saplingData)
        {
            this.m_saplings[saplingData.Point] = saplingData;
            this.m_enumerator = this.m_saplings.Values.GetEnumerator();
        }
        public void RemoveSapling(Point3 point)
        {
            this.m_saplings.Remove(point);
            this.m_enumerator = this.m_saplings.Values.GetEnumerator();
        }
        public SubsystemGameInfo m_subsystemGameInfo;
        public Dictionary<Point3, SubsystemOreSaplingBlockBehavior.OreSaplingData> m_saplings = new Dictionary<Point3, SubsystemOreSaplingBlockBehavior.OreSaplingData>();
        public Dictionary<Point3, SubsystemOreSaplingBlockBehavior.OreSaplingData>.ValueCollection.Enumerator m_enumerator;
        public Random m_random = new Random();
        public StringBuilder m_stringBuilder = new StringBuilder();
        public class OreSaplingData
        {
            public Point3 Point;
            
            public OreTreeType Type;
            
            public double MatureTime;
        }

        public void SetBlock(int x, int y, int z, int value)
        {
            Terrain terrain = SubsystemTerrain.Terrain;
            terrain.SetCellValueFast(x, y, z, value);
            TerrainChunk chunkAtCell = terrain.GetChunkAtCell(x, z);
            if (chunkAtCell != null)
            {
                chunkAtCell.ModificationCounter++;
                SubsystemTerrain.TerrainUpdater.DowngradeChunkNeighborhoodState(chunkAtCell.Coords, 1, TerrainChunkState.InvalidLight, false);
            }
        }
    }
}
