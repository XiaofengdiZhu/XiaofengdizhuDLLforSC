using Engine;
using Engine.Audio;
using Engine.Content;
using Engine.Graphics;
using Engine.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class PlayBadApple
    {
        private Texture2D[] m_pictures = new Texture2D[6574];
        public Sound m_music;
        Vector3 position = new Vector3(0, 80, 0);
        Vector3 unitZ = Vector3.UnitZ;
        Vector3 unitY = Vector3.UnitY;
        public PlayBadApple()
        {
            Log.Information("test");
            
            Stream stream = Storage.OpenFile("app:BadApple\\BadApple.ogg", OpenFileMode.Read);
            BinaryReader expr_0C = new BinaryReader(stream);
            SoundBuffer soundBuffer;
            bool flag = expr_0C.ReadBoolean();
            int bytesCount = expr_0C.ReadInt32();
            if (flag)
            {
                MemoryStream memoryStream = new MemoryStream();
                using (StreamingSource streamingSource = Ogg.Stream(stream, false))
                {
                    streamingSource.CopyTo(memoryStream);
                    if (memoryStream.Length > 2147483647L)
                    {
                        throw new InvalidOperationException("Audio data too long.");
                    }
                    memoryStream.Position = 0L;
                    soundBuffer = new SoundBuffer(memoryStream, (int)memoryStream.Length, 1, 44100);
                }
            }
            else soundBuffer = new SoundBuffer(stream, bytesCount, 1, 44100);
            m_music = new Sound(soundBuffer, 1, AudioManager.ToEnginePitch(0), 0, true, false);
            
            for (int i = 0; i < 6574; i++)
            {
                m_pictures[i] = Texture2D.Load("app:BadApple\\BadApple_" + (i + 1).ToString() + ".png");
            }
        }
        public void Play(int m_pictureIndex, Camera camera, PrimitivesRenderer3D primitivesRenderer3d)
        {
            TexturedBatch3D batch = primitivesRenderer3d.TexturedBatch(m_pictures[m_pictureIndex], true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.PointClamp);
            Vector3 p1 = Vector3.Transform(position - 12 * unitZ - 9 * unitY, camera.ViewMatrix);
            Vector3 p2 = Vector3.Transform(position + 12 * unitZ - 9 * unitY, camera.ViewMatrix);
            Vector3 p3 = Vector3.Transform(position + 12 * unitZ + 9 * unitY, camera.ViewMatrix);
            Vector3 p4 = Vector3.Transform(position - 12 * unitZ + 9 * unitY, camera.ViewMatrix);
            batch.QueueQuad(p1, p2, p3, p4, new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f), Color.White);
        }
    }
}
