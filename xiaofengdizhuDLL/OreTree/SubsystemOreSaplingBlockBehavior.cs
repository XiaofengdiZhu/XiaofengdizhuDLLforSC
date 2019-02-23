using Engine;
using Engine.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TemplatesDatabase;

namespace Game
{
    public class SubsystemOreSaplingBlockBehavior : SubsystemBlockBehavior, IUpdateable
    {
        public override int[] HandledBlocks
        {
            get { return new[] { 340 }; }
        }

        public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
        {
            int cellContents = SubsystemTerrain.Terrain.GetCellContents(x, y - 1, z);
            if (BlocksManager.Blocks[cellContents].IsTransparent)
                SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
        }

        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            float num = (m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative) ? m_random.UniformFloat(2f, 4f) : m_random.UniformFloat(480f, 600f);
            AddSapling(new OreSaplingData
            {
                Point = new Point3(x, y, z),
                Type = (OreTreeType)Terrain.ExtractData(value),
                MatureTime = m_subsystemGameInfo.TotalElapsedGameTime + num
            });
        }

        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            RemoveSapling(new Point3(x, y, z));
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemGameInfo = Project.FindSubsystem<SubsystemGameInfo>(true);
            m_enumerator = m_saplings.Values.GetEnumerator();
            foreach (object obj in valuesDictionary.GetValue<ValuesDictionary>("OreSaplings").Values)
            {
                AddSapling(LoadSaplingData((string)obj));
            }
        }

        public override void Save(ValuesDictionary valuesDictionary)
        {
            var valuesDictionary2 = new ValuesDictionary();
            valuesDictionary.SetValue("OreSaplings", valuesDictionary2);
            int num = 0;
            foreach (OreSaplingData saplingData in m_saplings.Values)
            {
                int num2 = num;
                num = num2 + 1;
                valuesDictionary2.SetValue(num2.ToString(CultureInfo.InvariantCulture), SaveSaplingData(saplingData));
            }
        }

        public int UpdateOrder
        {
            get { return 0; }
        }

        public void Update(float dt)
        {
            for (int i = 0; i < 10; i++)
            {
                if (!m_enumerator.MoveNext())
                {
                    m_enumerator = m_saplings.Values.GetEnumerator();
                    return;
                }
                MatureSapling(m_enumerator.Current);
            }
        }

        public static OreSaplingData LoadSaplingData(string data)
        {
            string[] array = data.Split(';');
            if (array.Length != 3)
            {
                throw new InvalidOperationException("Invalid sapling data string.");
            }
            return new OreSaplingData
            {
                Point = HumanReadableConverter.ConvertFromString<Point3>(array[0]),
                Type = HumanReadableConverter.ConvertFromString<OreTreeType>(array[1]),
                MatureTime = HumanReadableConverter.ConvertFromString<double>(array[2])
            };
        }

        public string SaveSaplingData(OreSaplingData saplingData)
        {
            m_stringBuilder.Length = 0;
            m_stringBuilder.Append(HumanReadableConverter.ConvertToString(saplingData.Point));
            m_stringBuilder.Append(';');
            m_stringBuilder.Append(HumanReadableConverter.ConvertToString(saplingData.Type));
            m_stringBuilder.Append(';');
            m_stringBuilder.Append(HumanReadableConverter.ConvertToString(saplingData.MatureTime));
            return m_stringBuilder.ToString();
        }

        public void MatureSapling(OreSaplingData saplingData)
        {
            if (m_subsystemGameInfo.TotalElapsedGameTime >= saplingData.MatureTime)
            {
                int x = saplingData.Point.X;
                int y = saplingData.Point.Y;
                int z = saplingData.Point.Z;
                TerrainChunk chunkAtCell = SubsystemTerrain.Terrain.GetChunkAtCell(x - 6, z - 6);
                TerrainChunk chunkAtCell2 = SubsystemTerrain.Terrain.GetChunkAtCell(x - 6, z + 6);
                TerrainChunk chunkAtCell3 = SubsystemTerrain.Terrain.GetChunkAtCell(x + 6, z - 6);
                TerrainChunk chunkAtCell4 = SubsystemTerrain.Terrain.GetChunkAtCell(x + 6, z + 6);
                if (y < 36 && chunkAtCell != null && chunkAtCell.State == TerrainChunkState.Valid && chunkAtCell2 != null && chunkAtCell2.State == TerrainChunkState.Valid && chunkAtCell3 != null && chunkAtCell3.State == TerrainChunkState.Valid && chunkAtCell4 != null && chunkAtCell4.State == TerrainChunkState.Valid)
                {
                    int cellContents = SubsystemTerrain.Terrain.GetCellContents(x, y - 1, z);
                    //只在大锗块上生长
                    if (cellContents != 231)
                    {
                        SetBlock(x, y, z, 28);
                        RemoveSapling(new Point3(x, y, z));
                        return;
                    }
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = z - 1; j <= z + 1; j++)
                        {
                            int cell2 = SubsystemTerrain.Terrain.GetCellValue(i, y - 1, j);
                            //周围一圈高度较高的岩浆才生长
                            if (!(BlocksManager.Blocks[Terrain.ExtractContents(cell2)] is MagmaBlock && (Terrain.ExtractData(cell2) & 15) < 2))
                            {
                                if (i == x && j == z) continue;
                                SetBlock(x, y, z, 28);
                                RemoveSapling(new Point3(x, y, z));
                                return;
                            }
                        }
                    }
                    SubsystemTerrain.ChangeCell(x, y, z, Terrain.MakeBlockValue(0, 0, 0), false);
                    if (!GrowTree(x, y, z, saplingData.Type))
                    {
                        SetBlock(x, y, z, 28);
                        RemoveSapling(new Point3(x, y, z));
                        return;
                    }
                    if (m_subsystemGameInfo.TotalElapsedGameTime > saplingData.MatureTime + 1200.0)
                    {
                        SetBlock(x, y, z, 28);
                        RemoveSapling(new Point3(x, y, z));
                        return;
                    }
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = z - 1; j <= z + 1; j++)
                        {
                            int cellContents3 = SubsystemTerrain.Terrain.GetCellContents(i, y - 1, j);
                            if (BlocksManager.Blocks[cellContents3] is MagmaBlock)
                            {
                                SubsystemTerrain.ChangeCell(i, y - 1, j, 0, true);
                            }
                        }
                    }
                }
                else
                {
                    saplingData.MatureTime = m_subsystemGameInfo.TotalElapsedGameTime;
                }
            }
        }

        public bool GrowTree(int x, int y, int z, OreTreeType treeType)
        {
            ReadOnlyList<TerrainBrush> treeBrushes = OrePlantsManager.GetTreeBrushes(treeType);
            for (int i = 0; i < 20; i++)
            {
                TerrainBrush terrainBrush = treeBrushes[m_random.UniformInt(0, treeBrushes.Count - 1)];
                bool flag = true;
                foreach (TerrainBrush.Cell cell in terrainBrush.Cells)
                {
                    if (cell.Y >= 0 && (cell.X != 0 || cell.Y != 0 || cell.Z != 0))
                    {
                        int cellContents = SubsystemTerrain.Terrain.GetCellContents(cell.X + x, cell.Y + y, cell.Z + z);
                        if (cellContents != 0 && Array.IndexOf(OrePlantsManager.m_treeLeavesByType, cellContents) <= -1)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    terrainBrush.Paint(SubsystemTerrain, x, y, z);
                    return true;
                }
            }
            return false;
        }

        public void AddSapling(OreSaplingData saplingData)
        {
            m_saplings[saplingData.Point] = saplingData;
            m_enumerator = m_saplings.Values.GetEnumerator();
        }

        public void RemoveSapling(Point3 point)
        {
            m_saplings.Remove(point);
            m_enumerator = m_saplings.Values.GetEnumerator();
        }

        public SubsystemGameInfo m_subsystemGameInfo;
        public Dictionary<Point3, OreSaplingData> m_saplings = new Dictionary<Point3, OreSaplingData>();
        public Dictionary<Point3, OreSaplingData>.ValueCollection.Enumerator m_enumerator;
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