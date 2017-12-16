using Engine;
using Engine.Audio;
using Engine.Content;
using Engine.Graphics;
using Engine.Media;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class PlayBadAppleByTexturedBatch3D
    {
        private Texture2D[] m_pictures = new Texture2D[6574];
        public Sound m_music;
        Vector3 position = new Vector3(0, 30, 36);
        Vector3 unitZ = Vector3.UnitZ;
        Vector3 unitY = Vector3.UnitY;
        public PlayBadAppleByTexturedBatch3D()
        {
            try
            {
                Stream stream = Storage.OpenFile("app:BadAppleByTexturedBatch3D\\BadApple.ogg", OpenFileMode.Read);
            BinaryReader br = new BinaryReader(stream);
            SoundBuffer soundBuffer;
            bool flag = br.ReadBoolean();
            int bytesCount = br.ReadInt32();
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
                m_pictures[i] = Texture2D.Load("app:BadAppleByTexturedBatch3D\\BadApple_" + (i + 1).ToString() + ".png");
            }
            }
            catch (Exception e) { Log.Information(e.ToString()); }
        }
        public void Play(int m_pictureIndex, Camera camera, PrimitivesRenderer3D primitivesRenderer3d)
        {
            TexturedBatch3D batch = primitivesRenderer3d.TexturedBatch(m_pictures[m_pictureIndex], true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.PointClamp);
            Vector3 p1 = Vector3.Transform(position - 36 * unitZ - 27 * unitY, camera.ViewMatrix);
            Vector3 p2 = Vector3.Transform(position + 36 * unitZ - 27 * unitY, camera.ViewMatrix);
            Vector3 p3 = Vector3.Transform(position + 36 * unitZ + 27 * unitY, camera.ViewMatrix);
            Vector3 p4 = Vector3.Transform(position - 36 * unitZ + 27 * unitY, camera.ViewMatrix);
            batch.QueueQuad(p1, p2, p3, p4, new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f), Color.White);
        }
    }
}
