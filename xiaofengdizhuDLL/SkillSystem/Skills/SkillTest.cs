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
            if (InputManager.IsKeyDownOnce(Key.U))
            {
                isTesting = isTesting ? false : true;
                flag = true;
            }
            return flag || (isTesting && subsystems.time.GameTime > nextTime);
        }
        public override void Action()
        {
            nextTime = subsystems.time.GameTime + 2;
            Vector3 position = componentPlayer.ComponentBody.Position;
            DateTime time = DateTime.Now;
            Log.Information(time.Millisecond.ToString());
            commonMethod.copyAndPasteBlock((int)position.X+1, (int)position.Y+1, (int)position.Z+1, (int)position.X + 10, (int)position.Y-10,(int)position.Z + 10, (int)position.X + 1, (int)position.Y + 10, (int)position.Z + 1);
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
            return InputManager.IsKeyDownOnce(Key.I);
        }
        public override void Action()
        {
            Vector3 position = commonMethod.playerPosition;
            commonMethod.exportBlocks((int)position.X + 1, (int)position.Y + 1, (int)position.Z + 1, (int)position.X + 10, (int)position.Y - 10, (int)position.Z + 10, "data:/mydata","Binary");
            commonMethod.exportBlocks((int)position.X + 1, (int)position.Y + 1, (int)position.Z + 1, (int)position.X + 10, (int)position.Y - 10, (int)position.Z + 10, "data:/mydata.txt", "Text");
            commonMethod.uploadFile("data:/mydata.txt");
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
            return InputManager.IsKeyDownOnce(Key.O);
        }
        public override void Action()
        {
            Vector3 position = commonMethod.playerPosition;
            commonMethod.setBlocks((int)position.X, (int)position.Y+10, (int)position.Z,commonMethod.getBlocks((int)position.X, (int)position.Y-10, (int)position.Z, 10, 10, 10).RotateFrowAxisAngle(new Vector3(0,1,0),0.7854f));
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
            return InputManager.IsKeyDownOnce(Key.M);
        }
        public override void Action()
        {
            //commonMethod.playerPosition = new Vector3(2250,60,3900);
            
            commonMethod.fillBox(2275, 10, 3875, 2324, 10, 3924, 46);
            for(int i = 0; i < 12; i++)
            {
                commonMethod.generateMaze(2300, 3900, 48, 48, 11+i*4, 3, 46, 15, -1, 46);
            }
            commonMethod.fillBox(2275, 59, 3875, 2324, 59, 3924, 15);
        }
        public Maze m_lastMaze;
        public Maze LastMaze {
            get
            {
                return m_lastMaze;
            }
            set
            {
                m_lastMaze = value;
            }
        }
    }
}
