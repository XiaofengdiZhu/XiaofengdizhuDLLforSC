using Engine;
using Engine.Input;
using System;
using System.IO;

namespace Game
{
    public class SkillTest : Skill
    {
        public bool isTesting;
        private double nextTime;

        public override string Name
        {
            get { return "Test"; }
        }

        public override bool Input()
        {
            bool flag = false;
            /*if (Keyboard.IsKeyDownOnce(Key.U))
            {
                isTesting = isTesting ? false : true;
                flag = true;
            }*/
            return flag || (isTesting && subsystems.time.GameTime > nextTime);
        }

        public override void Action()
        {
            nextTime = subsystems.time.GameTime + 2;
            Vector3 position = componentPlayer.ComponentBody.Position;
            DateTime time = DateTime.Now;
            Log.Information(time.Millisecond.ToString());
            commonMethod.copyAndPasteBlock((int)position.X + 1, (int)position.Y + 1, (int)position.Z + 1, (int)position.X + 10, (int)position.Y - 10, (int)position.Z + 10, (int)position.X + 1, (int)position.Y + 10, (int)position.Z + 1);
            time = DateTime.Now;
            Log.Information(time.Millisecond.ToString());
        }
    }

    public class SkillTest1 : Skill
    {
        public override string Name
        {
            get { return "Test1"; }
        }

        public override bool Input()
        {
            //return Keyboard.IsKeyDownOnce(Key.I);
            return false;
        }

        public override void Action()
        {
            Vector3 position = commonMethod.playerPosition;
            commonMethod.exportBlocks((int)position.X + 1, (int)position.Y + 1, (int)position.Z + 1, (int)position.X + 10, (int)position.Y - 10, (int)position.Z + 10, "data:/mydata", "Binary");
            commonMethod.exportBlocks((int)position.X + 1, (int)position.Y + 1, (int)position.Z + 1, (int)position.X + 10, (int)position.Y - 10, (int)position.Z + 10, "data:/mydata.txt", "Text");
            CommonMethod.uploadFile("data:/mydata.txt");
        }
    }

    public class SkillTest2 : Skill
    {
        public override string Name
        {
            get { return "Test2"; }
        }

        public override bool Input()
        {
            //return Keyboard.IsKeyDownOnce(Key.L);
            return false;
        }

        public override void Action()
        {
            //转换dae模型
            var m = new Engine.Content.ModelDataContentWriter
            {
                ModelData = "miku.dae"
            };
            Stream st = Storage.OpenFile("data:/logs\\miku", OpenFileMode.CreateOrOpen);
            m.Write("app:/myDae", st);
            st.Flush();
        }
    }

    public class SkillTest3 : Skill
    {
        public override string Name
        {
            get { return "Test3"; }
        }

        public override bool Input()
        {
            //return Keyboard.IsKeyDownOnce(Key.M);
            return false;
        }

        public override void Action()
        {
            //commonMethod.playerPosition = new Vector3(2250,60,3900);

            commonMethod.fillBox(2275, 10, 3875, 2324, 10, 3924, 46);
            for (int i = 0; i < 12; i++)
            {
                commonMethod.generateMaze(2300, 3900, 48, 48, 11 + i * 4, 3, 46, 15, -1, 46);
            }
            commonMethod.fillBox(2275, 59, 3875, 2324, 59, 3924, 15);
        }

        public Maze m_lastMaze;

        public Maze LastMaze
        {
            get { return m_lastMaze; }
            set { m_lastMaze = value; }
        }
    }

    public class SkillTest4 : Skill
    {
        public override string Name
        {
            get { return "Test4"; }
        }

        public override bool Input()
        {
            //return Keyboard.IsKeyDownOnce(Key.B);
            return false;
        }

        public override void Action()
        {
            try
            {
                ComponentMiner miner = componentPlayer.ComponentMiner;
                Vector3 viewPosition = componentPlayer.View.ActiveCamera.ViewPosition;
                Vector3 viewDirection = componentPlayer.View.ActiveCamera.ViewDirection;
                GameViewWidget gameViewWidget = componentPlayer.View.GameWidget.GameViewWidget;
                var direction = Vector3.Normalize(componentPlayer.View.ActiveCamera.ScreenToWorld(new Vector3(gameViewWidget.WidgetToScreen(gameViewWidget.ActualSize / 2f), 1f), Matrix.Identity) - viewPosition);
                TerrainRaycastResult? terrainRaycastResult = miner.PickTerrainForDigging(viewPosition, direction);
                if (terrainRaycastResult.HasValue)
                {
                    Point3 a = terrainRaycastResult.Value.CellFace.Point;
                    int value = commonMethod.getBlock(a.X, a.Y, a.Z);
                    Vector3 center = viewPosition + new Vector3(0, 10, 0);
                    commonMethod.setBlocks(StaticCommonMethod.get3DSierpinskiTriangle(new Vector3(0, 3, 0), 46, 124, 0));
                    commonMethod.setBlocks(StaticCommonMethod.get3DSierpinskiTriangle(new Vector3(124, 3, 0), 46, 124, 1));
                    commonMethod.setBlocks(StaticCommonMethod.get3DSierpinskiTriangle(new Vector3(248, 3, 0), 46, 124, 2));
                    commonMethod.setBlocks(StaticCommonMethod.get3DSierpinskiTriangle(new Vector3(372, 3, 0), 46, 124, 3));
                    commonMethod.setBlocks(StaticCommonMethod.get3DSierpinskiTriangle(new Vector3(496, 3, 0), 46, 124, 4));
                    commonMethod.displaySmallMessage(a.ToString(), false, false);
                }
            }
            catch (Exception e)
            {
                Log.Warning(e.ToString());
            }
        }
    }

    public class SkillTest5 : Skill
    {
        public override string Name
        {
            get { return "Test5"; }
        }

        public override bool Input()
        {
            //return Keyboard.IsKeyDownOnce(Key.B);
            return false;
        }

        public override void Action()
        {
            try
            {
                ComponentMiner miner = componentPlayer.ComponentMiner;
                Vector3 viewPosition = componentPlayer.View.ActiveCamera.ViewPosition;
                Vector3 viewDirection = componentPlayer.View.ActiveCamera.ViewDirection;
                GameViewWidget gameViewWidget = componentPlayer.View.GameWidget.GameViewWidget;
                var direction = Vector3.Normalize(componentPlayer.View.ActiveCamera.ScreenToWorld(new Vector3(gameViewWidget.WidgetToScreen(gameViewWidget.ActualSize / 2f), 1f), Matrix.Identity) - viewPosition);
                TerrainRaycastResult? terrainRaycastResult = miner.PickTerrainForDigging(viewPosition, direction);
                if (terrainRaycastResult.HasValue)
                {
                    Point3 a = terrainRaycastResult.Value.CellFace.Point;
                    int value = commonMethod.getBlock(a.X, a.Y, a.Z);
                    Vector3 center = viewPosition + new Vector3(0, 10, 0);
                    DateTime time = DateTime.Now;
                    StoreBlocks blocks = StaticCommonMethod.getKoch90(new CellFace(0, 3, 0, 4), 46, 5);
                    commonMethod.setBlocks(blocks);
                    Log.Information((DateTime.Now - time).TotalMilliseconds);
                    Log.Information(blocks.Count);
                    commonMethod.displaySmallMessage(a.ToString(), false, false);
                }
            }
            catch (Exception e)
            {
                Log.Warning(e.ToString());
            }
        }
    }

    public class SkillTest6 : Skill
    {
        public override string Name
        {
            get { return "Test6"; }
        }

        public override bool Input()
        {
            return Keyboard.IsKeyDownOnce(Key.B);
            //return false;
        }

        public override void Action()
        {
            try
            {
                ComponentMiner miner = componentPlayer.ComponentMiner;
                Vector3 viewPosition = componentPlayer.View.ActiveCamera.ViewPosition;
                Vector3 viewDirection = componentPlayer.View.ActiveCamera.ViewDirection;
                GameViewWidget gameViewWidget = componentPlayer.View.GameWidget.GameViewWidget;
                var direction = Vector3.Normalize(componentPlayer.View.ActiveCamera.ScreenToWorld(new Vector3(gameViewWidget.WidgetToScreen(gameViewWidget.ActualSize / 2f), 1f), Matrix.Identity) - viewPosition);
                TerrainRaycastResult? terrainRaycastResult = miner.PickTerrainForDigging(viewPosition, direction);
                if (terrainRaycastResult.HasValue)
                {
                    Point3 cellFace = terrainRaycastResult.Value.CellFace.Point;
                    int value = commonMethod.getBlock(cellFace.X, cellFace.Y, cellFace.Z);
                    if (Terrain.ExtractContents(value) == 400)
                    {
                        int data = Terrain.ExtractData(value);
                        int? cornerType = FactorioTransportBeltBlock.GetCornerType(data);
                        commonMethod.displaySmallMessage(cellFace.X + "," + cellFace.Y + "," + cellFace.Z + " " + FactorioTransportBeltBlock.GetRotation(data) + (cornerType.HasValue ? (" " + cornerType.Value.ToString()) : ""), false, false);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Warning(e.ToString());
            }
        }
    }

    public class SkillTest7 : Skill
    {
        public override string Name
        {
            get { return "Test7"; }
        }

        public override bool Input()
        {
            return Keyboard.IsKeyDownOnce(Key.N);
            //return false;
        }

        public override void Action()
        {
            try
            {
                ComponentMiner miner = componentPlayer.ComponentMiner;
                Vector3 viewPosition = componentPlayer.View.ActiveCamera.ViewPosition;
                Vector3 viewDirection = componentPlayer.View.ActiveCamera.ViewDirection;
                GameViewWidget gameViewWidget = componentPlayer.View.GameWidget.GameViewWidget;
                var direction = Vector3.Normalize(componentPlayer.View.ActiveCamera.ScreenToWorld(new Vector3(gameViewWidget.WidgetToScreen(gameViewWidget.ActualSize / 2f), 1f), Matrix.Identity) - viewPosition);
                TerrainRaycastResult? terrainRaycastResult = miner.PickTerrainForDigging(viewPosition, direction);
                if (terrainRaycastResult.HasValue)
                {
                    CellFace cellFace = terrainRaycastResult.Value.CellFace;
                    Point3 position = cellFace.Point + CellFace.FaceToPoint3(cellFace.Face);
                    int oldValue = commonMethod.getBlock(position.X, position.Y, position.Z);
                    if (Terrain.ExtractContents(oldValue) == 0)
                    {
                        int value = Terrain.MakeBlockValue(400, 0, FactorioTransportBeltBlock.SetCornerType(FactorioTransportBeltBlock.SetColor(0, 0), 0));
                        commonMethod.placeBlock(position.X, position.Y, position.Z, value);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Warning(e.ToString());
            }
        }
    }
}