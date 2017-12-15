using Engine;
using Engine.Audio;
using Engine.Media;
using System;
using System.Collections.Generic;
using System.IO;

namespace Game
{
    public class PlayBadApple1
    {
        private Image[] m_pictures = new Image[6574];
        public Sound m_music;
        Vector3 position = new Vector3(0, 2, 0);
        Vector3 unitZ = Vector3.UnitZ;
        Vector3 unitY = Vector3.UnitY;
        public CommonMethod commonMethod = new CommonMethod();
        Dictionary<Color, int> color2colorInt = new Dictionary<Color, int>();
        public PlayBadApple1()
        {
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
                m_pictures[i] = Image.Load("app:BadApple3\\BadApple_" + (i + 1).ToString() + ".png");
            }
            color2colorInt = new Dictionary<Color, int>()
            {
                { new Color(0x00,0x00,0x00),0},
                { new Color(0x0f,0x0f,0x0f),1},
                { new Color(0x1f,0x1f,0x1f),2},
                { new Color(0x2f,0x2f,0x2f),3},
                { new Color(0x3f,0x3f,0x3f),4},
                { new Color(0x4f,0x4f,0x4f),5},
                { new Color(0x5f,0x5f,0x5f),6},
                { new Color(0x6f,0x6f,0x6f),7},
                { new Color(0x7f,0x7f,0x7f),8},
                { new Color(0x8f,0x8f,0x8f),9},
                { new Color(0x9f,0x9f,0x9f),10},
                { new Color(0xaf,0xaf,0xaf),11},
                { new Color(0xbf,0xbf,0xbf),12},
                { new Color(0xcf,0xcf,0xcf),13},
                { new Color(0xe7,0xe7,0xe7),14},
                { new Color(0xff,0xff,0xff),15}
            };
        }
        public void Play(int m_pictureIndex)
        {
            for (int x = 0; x < 72; x++)
            {
                for (int y = 0; y < 54; y++)
                {
                    try
                    {
                        commonMethod.setBlock(x, 2, y, Terrain.ReplaceData(72, (1 | color2colorInt[m_pictures[m_pictureIndex].GetPixel(x, y)] << 1)));
                    }
                    catch
                    {
                        commonMethod.setBlock(x, 2, y, Terrain.ReplaceData(72, (1 | 8 << 1)));
                    }
                }
            }
        }
    }
}
