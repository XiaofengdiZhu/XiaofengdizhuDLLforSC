using Engine;
using Engine.Audio;
using Engine.Media;
using System;
using System.IO;

namespace Game
{
    public class PlayBadAppleBy4LED
    {
        private Image[] m_pictures = new Image[6574];
        public Sound m_music;
        private Vector3 position = new Vector3(0, 2, 0);
        private Vector3 unitZ = Vector3.UnitZ;
        private Vector3 unitY = Vector3.UnitY;
        public CommonMethod commonMethod = new CommonMethod();

        public PlayBadAppleBy4LED()
        {
            Stream stream = Storage.OpenFile("app:BadAppleBy4LED\\BadApple.ogg", OpenFileMode.Read);
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
                m_pictures[i] = Image.Load("app:BadAppleBy4LED\\BadApple_" + (i + 1).ToString() + ".png");
            }
        }

        public void Play(int m_pictureIndex)
        {
            for (int x = 0; x < 72; x++)
            {
                for (int y = 0; y < 54; y++)
                {
                    try
                    {
                        GlowPoint[] glowPoints = ((FourLedElectricElement)(commonMethod.subsystems.electricity.GetElectricElement(x, 3, y, 4))).m_glowPoints;
                        glowPoints[0].Color = m_pictures[m_pictureIndex].GetPixel(x * 2 + 1, y * 2);
                        glowPoints[1].Color = m_pictures[m_pictureIndex].GetPixel(x * 2 + 1, y * 2 + 1);
                        glowPoints[2].Color = m_pictures[m_pictureIndex].GetPixel(x * 2, y * 2);
                        glowPoints[3].Color = m_pictures[m_pictureIndex].GetPixel(x * 2, y * 2 + 1);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}