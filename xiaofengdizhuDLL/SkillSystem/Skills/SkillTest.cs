using Engine;
using Engine.Graphics;
using Engine.Input;
using Engine.Media;
using System;
using System.IO;
using System.Speech.Synthesis;

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
            if (Keyboard.IsKeyDownOnce(Key.U))
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
            return Keyboard.IsKeyDownOnce(Key.I);
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
            //return Keyboard.IsKeyDownOnce(Key.O);
            return false;
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
            return Keyboard.IsKeyDownOnce(Key.M);
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
    public class SkillTest4 : Skill
    {
        public override string Name
        {
            get { return "Test4"; }
        }
        public override bool Input()
        {
            return Keyboard.IsKeyDown(Key.B);
        }
        public override void Action()
        {
            Camera camera = componentPlayer.View.ActiveCamera;
            PrimitivesRenderer3D primitivesRenderer3d = commonMethod.project.FindSubsystem<SubsystemModelsRenderer>(true).PrimitivesRenderer;
            Vector3 position = commonMethod.playerPosition;
            /*
            for (var i = 0; i < 1000; i++)
            {
                FlatBatch3D flatBatch3D = primitivesRenderer3d.FlatBatch(0, DepthStencilState.DepthRead, null, BlendState.Additive);
                flatBatch3D.QueueTriangle(position + new Vector3(5+i/1000, -10, 5), position + new Vector3(5, 10, 5 + i / 1000), position + new Vector3(5, 10, -5 - i / 1000), Color.Red);
            }
            */
            Vector3 vector = Vector3.Transform(new Vector3(0, 80, 0), camera.ViewMatrix);
            Vector3 right = Vector3.TransformNormal(0.005f * Vector3.Normalize(Vector3.Cross(camera.ViewDirection, camera.ViewUp)), camera.ViewMatrix);
            Vector3 down = Vector3.TransformNormal(-0.005f * Vector3.UnitY, camera.ViewMatrix);
            primitivesRenderer3d.FontBatch(ContentManager.Get<BitmapFont>("Fonts/Pericles32"), 1, DepthStencilState.DepthRead, RasterizerState.CullNoneScissor, BlendState.AlphaBlend, SamplerState.LinearClamp).QueueText("TEST TEST TEST", vector, right, down, Color.Red, TextAnchor.HorizontalCenter | TextAnchor.Bottom);

            TexturedBatch3D batch = primitivesRenderer3d.TexturedBatch(ContentManager.Get<Texture2D>("Textures/Moon1"), true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.PointClamp);
            
            Vector3 position1 = new Vector3(0, 80, 0);
            Vector3 vector2 = position1 - camera.ViewPosition;
            float num = Vector3.Dot(vector2, camera.ViewDirection);
            float num2 = vector2.Length();
            Vector3 v = -(0.01f + 0.02f * num) / num2 * vector2;
            Vector3 unitZ = Vector3.UnitZ;
            Vector3 unitY = Vector3.UnitY;
            Vector3 p1 = Vector3.Transform(position1 - 10 * unitZ - 10 * unitY, camera.ViewMatrix);
            Vector3 p2 = Vector3.Transform(position1 + 10 * unitZ - 10 * unitY, camera.ViewMatrix);
            Vector3 p3 = Vector3.Transform(position1 + 10 * unitZ + 10 * unitY, camera.ViewMatrix);
            Vector3 p4 = Vector3.Transform(position1 - 10 * unitZ + 10 * unitY, camera.ViewMatrix);
            batch.QueueQuad(p1, p2, p3, p4, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(0f, 1f), Color.White);
            
            /*
            Vector3 position1 = new Vector3(0, 80, 0);
            Matrix matrix = Matrix.CreateRotationY(0);
            matrix.Translation = position;
            Matrix value2 = camera.ViewMatrix;
            Vector3 p = matrix.Translation;
            */
            /*
            float timeOfDay = commonMethod.project.FindSubsystem<SubsystemTimeOfDay>(true).TimeOfDay;
            float f = MathUtils.Max(CalculateDawnGlowIntensity(timeOfDay), CalculateDuskGlowIntensity(timeOfDay));
            float num3 = 2f * timeOfDay * 3.14159274f;
            float num4 = MathUtils.Lerp(90f, 160f, f);
            QueueCelestialBody(batch, camera.ViewPosition, Color.Blue, 800, num4, num3);
            */
        }
        private void QueueCelestialBody(TexturedBatch3D batch, Vector3 viewPosition, Color color, float distance, float radius, float angle)
        {
            if (color.A > 0)
            {
                Vector3 vector = new Vector3
                {
                    X = -MathUtils.Sin(angle),
                    Y = -MathUtils.Cos(angle),
                    Z = 0f
                };
                Vector3 unitZ = Vector3.UnitZ;
                Vector3 v = Vector3.Cross(unitZ, vector);
                Vector3 p = viewPosition + vector * distance - radius * unitZ - radius * v;
                Vector3 p2 = viewPosition + vector * distance + radius * unitZ - radius * v;
                Vector3 p3 = viewPosition + vector * distance + radius * unitZ + radius * v;
                Vector3 p4 = viewPosition + vector * distance - radius * unitZ + radius * v;
                batch.QueueQuad(p, p2, p3, p4, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(0f, 1f), color);
            }
        }
        private static float CalculateDawnGlowIntensity(float timeOfDay)
        {
            return MathUtils.Max(1f - MathUtils.Abs(timeOfDay - 0.25f) / 0.100000009f * 2f, 0f);
        }
        private static float CalculateDuskGlowIntensity(float timeOfDay)
        {
            return MathUtils.Max(1f - MathUtils.Abs(timeOfDay - 0.75f) / 0.100000024f * 2f, 0f);
        }
    }
}
