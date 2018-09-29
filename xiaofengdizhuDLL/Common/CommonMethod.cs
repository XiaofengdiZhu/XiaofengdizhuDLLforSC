using Engine;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class CommonMethod
    {
        public CommonSubsystems subsystems = new CommonSubsystems();

        public Project project
        {
            get
            {
                return GameManager.Project;
            }
        }

        public ReadOnlyList<ComponentPlayer> componentPlayers
        {
            get
            {
                return subsystems.players.ComponentPlayers;
            }
        }

        public ComponentPlayer componentPlayer
        {
            get
            {
                return componentPlayers[0];
            }
        }

        //常用方法
        public Vector3 playerPosition
        {
            get
            {
                return componentPlayer.ComponentBody.Position;
            }
            set
            {
                componentPlayer.ComponentBody.Position = value;
            }
        }

        //获取指定位置方块Value
        public int getBlock(int x, int y, int z)
        {
            return subsystems.terrain.Terrain.GetCellValue(x, y, z);
        }

        //方块Value转方块ID
        public int BlockValue2ID(int value)
        {
            //TerrainData.ExtractContents
            return value & 1023;
        }

        //会引发方块行为的方块放置（和手动放置效果相同）
        public void placeBlock(int x, int y, int z, int value)
        {
            placeBlock(x, y, z, value, false);
        }

        public void placeBlock(int x, int y, int z, int value, bool toSetBlock)
        {
            if (!toSetBlock)
            {
                subsystems.terrain.ChangeCell(x, y, z, value, true);
            }
            else
            {
                setBlock(x, y, z, value);
            }
        }

        //单纯的方块放置
        public void setBlock(int x, int y, int z, int value)
        {
            SubsystemTerrain subsystemTerrain = subsystems.terrain;
            Terrain terrain = subsystemTerrain.Terrain;
            terrain.SetCellValueFast(x, y, z, value);
            TerrainChunk chunkAtCell = terrain.GetChunkAtCell(x, z);
            if (chunkAtCell != null)
            {
                chunkAtCell.ModificationCounter++;
                subsystemTerrain.TerrainUpdater.DowngradeChunkNeighborhoodState(chunkAtCell.Coords, 1, TerrainChunkState.InvalidLight, false);
            }
        }

        public void addExplosion(int x, int y, int z, float pressure, bool isIncendiary, bool noExplosionSound)
        {
            subsystems.explosions.AddExplosion(x, y, z, pressure, isIncendiary, noExplosionSound);
        }

        public void makeLightningStrike(int x, int y, int z)
        {
            subsystems.sky.MakeLightningStrike(new Vector3(x, y, z));
        }

        //地形
        public void fillBox(int x1, int y1, int z1, int x2, int y2, int z2, int value)
        {
            int dx = Math.Abs(x1 - x2), dy = Math.Abs(y1 - y2), dz = Math.Abs(z1 - z2);
            if (x1 > x2)
            {
                int temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (y1 > y2)
            {
                int temp = x1;
                y1 = y2;
                y2 = temp;
            }
            if (z1 > z2)
            {
                int temp = x1;
                z1 = z2;
                z2 = temp;
            }
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    for (int z = z1; z <= z2; z++)
                    {
                        placeBlock(x, y, z, value);
                    }
                }
            }
        }

        //画三角面
        public void drawTriangle(Vector3 v1, Vector3 v2, Vector3 v3, int value)
        {
            setBlocks(StaticCommonMethod.getTriangle(v1, v2, v3, value).ClearDuplicatePosition());
        }

        //画直线
        public void drawLine(int x1, int y1, int z1, int x2, int y2, int z2, int value)
        {
            setBlocks(StaticCommonMethod.getLine3D(x1, y1, z1, x2, y2, z2, value));
        }

        public void drawCircle(int centerX, int centerY, int centerZ, int diameter, int value)
        {
            setBlocks(StaticCommonMethod.getCircle(centerX, centerZ, diameter, value).Translate3D(0, 20, 0));
        }

        public void UpdateColor2colorInt()
        {
            StaticCommonMethod.color2colorInt = new Dictionary<Color, int>() {
            { SubsystemPalette.GetColor(subsystems.terrain, 0),0},
            { SubsystemPalette.GetColor(subsystems.terrain, 1),1},
            { SubsystemPalette.GetColor(subsystems.terrain, 2),2},
            { SubsystemPalette.GetColor(subsystems.terrain, 3),3},
            { SubsystemPalette.GetColor(subsystems.terrain, 4),4},
            { SubsystemPalette.GetColor(subsystems.terrain, 5),5},
            { SubsystemPalette.GetColor(subsystems.terrain, 6),6},
            { SubsystemPalette.GetColor(subsystems.terrain, 7),7},
            { SubsystemPalette.GetColor(subsystems.terrain, 8),8},
            { SubsystemPalette.GetColor(subsystems.terrain, 9),9},
            { SubsystemPalette.GetColor(subsystems.terrain, 10),10},
            { SubsystemPalette.GetColor(subsystems.terrain, 11),11},
            { SubsystemPalette.GetColor(subsystems.terrain, 12),12},
            { SubsystemPalette.GetColor(subsystems.terrain, 13),13},
            { SubsystemPalette.GetColor(subsystems.terrain, 14),14},
            { SubsystemPalette.GetColor(subsystems.terrain, 15),15}
        };
        }

        public void copyAndPasteBlock(int x1, int y1, int z1, int x2, int y2, int z2, int x3, int y3, int z3)
        {
            int dx = Math.Abs(x1 - x2), dy = Math.Abs(y1 - y2), dz = Math.Abs(z1 - z2);
            if (x1 > x2)
            {
                int temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (y1 > y2)
            {
                int temp = x1;
                y1 = y2;
                y2 = temp;
            }
            if (z1 > z2)
            {
                int temp = x1;
                z1 = z2;
                z2 = temp;
            }
            for (int x = 0; x <= dx; x++)
            {
                for (int z = 0; z <= dz; z++)
                {
                    for (int y = 0; y <= dy; y++)
                    {
                        var value = getBlock(x1 + x, y1 + y, z1 + z);
                        if (BlockValue2ID(value) == 0) continue;
                        setBlock(x3 + x, y3 + y, z3 + z, value);
                    }
                }
            }
        }

        public void exportBlocks(int x1, int y1, int z1, int x2, int y2, int z2, string path, string way)
        {
            int dx = Math.Abs(x1 - x2), dy = Math.Abs(y1 - y2), dz = Math.Abs(z1 - z2);
            if (x1 > x2)
            {
                int temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (y1 > y2)
            {
                int temp = x1;
                y1 = y2;
                y2 = temp;
            }
            if (z1 > z2)
            {
                int temp = x1;
                z1 = z2;
                z2 = temp;
            }
            StoreBlocks storeBlocks = getBlocks(x1, y1, z1, dx, dy, dz);
            exportBlocks(storeBlocks, path, way);
        }

        public void exportBlocks(StoreBlocks storeBlocks, string path, string way)
        {
            Stream stream = Storage.OpenFile(path, OpenFileMode.Create);
            exportBlocks(storeBlocks, stream, way);
        }

        public void exportBlocks(StoreBlocks storeBlocks, Stream stream, string way)
        {
            switch (way)
            {
                case "Binary":
                    using (BinaryWriter binaryWriter = new BinaryWriter(stream))
                    {
                        binaryWriter.Write(storeBlocks.Count);
                        foreach (StoreBlock storeBlock in storeBlocks)
                        {
                            binaryWriter.Write(storeBlock.X);
                            binaryWriter.Write(storeBlock.Y);
                            binaryWriter.Write(storeBlock.Z);
                            binaryWriter.Write(storeBlock.Value);
                        }
                    }
                    break;

                case "Text":
                    using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
                    {
                        int blockCount = storeBlocks.Count;
                        //string s = blockCount.ToString();
                        streamWriter.Write(blockCount.ToString());
                        foreach (StoreBlock storeBlock in storeBlocks)
                        {
                            //s += ";";
                            //s += storeBlock.ToString();
                            streamWriter.Write(";" + storeBlock.ToString());
                        }
                        //streamWriter.Write(s);
                    }
                    break;
            }
        }

        public StoreBlocks getBlocks(int x1, int y1, int z1, int dx, int dy, int dz)
        {
            StoreBlocks storeBlocks = new StoreBlocks();
            for (int x = 0; x <= dx; x++)
            {
                for (int y = 0; y <= dy; y++)
                {
                    for (int z = 0; z <= dz; z++)
                    {
                        int value = getBlock(x1 + x, y1 + y, z1 + z);
                        if (BlockValue2ID(value) == 0) continue;
                        storeBlocks.Add(x, y, z, value);
                    }
                }
            }
            return storeBlocks;
        }

        public StoreBlocks importBlocks(string path, string way)
        {
            Stream stream = Storage.OpenFile(path, OpenFileMode.Read);
            return importBlocks(stream, way);
        }

        public StoreBlocks importBlocks(Stream stream, string way)
        {
            StoreBlocks storeBlocks = new StoreBlocks();
            int blockCount = 0;
            switch (way)
            {
                case "Binary":
                    using (BinaryReader binaryReader = new BinaryReader(stream))
                    {
                        blockCount = binaryReader.ReadInt32();
                        for (int i = 0; i < blockCount; i++)
                        {
                            storeBlocks.Add(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
                        }
                    }
                    break;

                case "Text":
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string[] s = streamReader.ReadToEnd().Split(';');
                        blockCount = int.Parse(s[0]);
                        for (int i = 1; i <= blockCount; i++)
                        {
                            string[] s1 = s[i].Split(',');
                            storeBlocks.Add(int.Parse(s1[0]), int.Parse(s1[1]), int.Parse(s1[2]), int.Parse(s1[3]));
                        }
                    }
                    break;
            }
            return storeBlocks;
        }

        public void setBlocks(StoreBlocks storeBlocks)
        {
            if (storeBlocks.IsAbsolute)
            {
                foreach (StoreBlock storeBlock in storeBlocks)
                {
                    setBlock(storeBlock.X, storeBlock.Y, storeBlock.Z, storeBlock.Value);
                }
            }
        }

        public void setBlocks(int x1, int y1, int z1, StoreBlocks storeBlocks)
        {
            if (!storeBlocks.IsAbsolute)
            {
                foreach (StoreBlock storeBlock in storeBlocks)
                {
                    setBlock(x1 + storeBlock.X, y1 + storeBlock.Y, z1 + storeBlock.Z, storeBlock.Value);
                }
            }
        }

        //生成迷宫，floorValue、ceilingValue、borderValue为-1代表不生成地板、天花板、外墙，bottomY为-1会在地形表面生成整个迷宫
        public Maze generateMaze(int centerX, int centerZ, int sizeX, int sizeZ, int bottomY, int height, int wallValue, int floorValue, int ceilingValue, int borderValue)
        {
            Maze maze = new Maze(sizeX, sizeZ);
            bool[,] mazeArray = maze.GetBoolArray();
            int startX = centerX - (sizeX >> 1);
            int startZ = centerZ - (sizeZ >> 1);
            int endX = startX + sizeX;
            int endZ = startZ + sizeZ;
            for (int x = startX; x < endX; x++)
            {
                for (int z = startZ; z < endZ; z++)
                {
                    if (bottomY == -1) bottomY = subsystems.terrain.Terrain.GetChunkAtCell(x, z).GetTopHeightFast(x, z);
                    if (floorValue != -1)
                    {
                        placeBlock(x, bottomY, z, floorValue);
                    }
                    else
                    {
                        bottomY -= 1;
                    }
                    if (mazeArray[x - startX, z - startZ])
                    {
                        for (int y = 1; y <= height; y++)
                        {
                            placeBlock(x, bottomY + y, z, wallValue);
                        }
                    }
                    if (ceilingValue != -1)
                    {
                        placeBlock(x, bottomY + height + 1, z, ceilingValue);
                    }
                    if (borderValue != -1)
                    {
                        int borderHeight = height + ((ceilingValue != -1) ? 1 : 0);
                        for (int y = 0; y <= borderHeight; y++)
                        {
                            if (x == startX)
                            {
                                placeBlock(x - 1, bottomY + y, z, borderValue);
                                if (z == startZ)
                                {
                                    placeBlock(x - 1, bottomY + y, z - 1, borderValue);
                                }
                                else if (z == endZ - 1)
                                {
                                    placeBlock(x - 1, bottomY + y, z + 1, borderValue);
                                }
                            }
                            else if (x == (endX - 1))
                            {
                                placeBlock(x + 1, bottomY + y, z, borderValue);
                                if (z == startZ)
                                {
                                    placeBlock(x + 1, bottomY + y, z - 1, borderValue);
                                }
                                else if (z == endZ - 1)
                                {
                                    placeBlock(x + 1, bottomY + y, z + 1, borderValue);
                                }
                            }
                            if (z == startZ)
                            {
                                placeBlock(x, bottomY + y, z - 1, borderValue);
                            }
                            else if (z == endZ - 1)
                            {
                                placeBlock(x, bottomY + y, z + 1, borderValue);
                            }
                        }
                    }
                }
            }
            return maze;
        }

        public void displaySmallMessage(string text, bool blinking, bool playNotificationSound)
        {
            componentPlayer.ComponentGui.DisplaySmallMessage(text, blinking, playNotificationSound);
        }

        public void displayLargeMessage(string largeText, string smallText, float duration, float delay)
        {
            componentPlayer.ComponentGui.DisplayLargeMessage(largeText, smallText, duration, delay);
        }

        public void packFile(string filePath2Pack)
        {
            packFile(filePath2Pack, filePath2Pack + ".zip");
        }

        public void packFile(string filePath2Pack, string targetFilePath)
        {
            Stream stream = Storage.OpenFile(filePath2Pack, OpenFileMode.Read);
            string fileName = Storage.GetFileName(filePath2Pack);
            packFile(stream, fileName, targetFilePath);
        }

        public void packFile(Stream file2Pack, string fileName, string targetFilePath)
        {
            Stream tmpStream = Storage.OpenFile(targetFilePath, OpenFileMode.Create);
            using (ZipArchive zipArchive = ZipArchive.Create(tmpStream, true))
            {
                zipArchive.AddStream(fileName, file2Pack);
            }
        }

        public void uploadFile(string path)
        {
            DialogsManager.ShowDialog(null, new SelectExternalContentProviderDialog("Select Upload Destination", false, delegate (IExternalContentProvider provider)
              {
                  try
                  {
                      string fileName = Storage.GetFileName(path);
                      CancellableBusyDialog busyDialog = new CancellableBusyDialog("Uploading file", false);
                      DialogsManager.ShowDialog(null, busyDialog);
                      Task.Run(delegate
                      {
                          Stream stream = Storage.OpenFile(path, OpenFileMode.Read);
                          try
                          {
                              stream.Position = 0L;
                              Dispatcher.Dispatch(delegate
                              {
                                  busyDialog.LargeMessage = "Uploading file";
                                  provider.Upload(fileName, stream, busyDialog.Progress, delegate
                                  {
                                      DialogsManager.HideDialog(busyDialog);
                                      Utilities.Dispose<Stream>(ref stream);
                                  }, delegate (Exception error)
                                  {
                                      DialogsManager.HideDialog(busyDialog);
                                      DialogsManager.ShowDialog(null, new MessageDialog("Error", error.Message, "OK", null, null));
                                      Utilities.Dispose<Stream>(ref stream);
                                  });
                              }, false);
                          }
                          catch (Exception ex)
                          {
                              DialogsManager.HideDialog(busyDialog);
                              DialogsManager.ShowDialog(null, new MessageDialog("Error", ex.Message, "OK", null, null));
                              Utilities.Dispose<Stream>(ref stream);
                          }
                      });
                  }
                  catch (Exception ex)
                  {
                      DialogsManager.ShowDialog(null, new MessageDialog("Error", ex.Message, "OK", null, null));
                  }
              }));
        }

        /*public static void unpackFile(string path)
        {
            string path2place = path + Storage.GetFileName(path) + "/";
            unpackFile(path, path2place);
        }
        public void unpackFile(string path, string path2place)
        {
            Stream stream = Storage.OpenFile(path, OpenFileMode.Read);
            unpackFile( stream, path2place);
        }
        public static void unpackFile(Stream stream, string path2place)
        {
            Storage.CreateDirectory(path2place);
            using (ZipArchive zipArchive = ZipArchive.Open(stream, true))
            {
                foreach (ZipArchiveEntry current in zipArchive.ReadCentralDir())
                {
                    string text = current.FilenameInZip.Replace('\\', '/');
                    using (Stream outStream = Storage.OpenFile(path2place + Storage.GetFileName(text), OpenFileMode.Create))
                    {
                        zipArchive.ExtractFile(current, outStream);
                    }
                }
            }
        }*/
    }
}