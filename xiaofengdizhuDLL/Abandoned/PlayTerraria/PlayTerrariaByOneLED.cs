using Engine.Media;

namespace Game
{
    public class PlayTerrariaByOneLED
    {
        private Image[] m_pictures = new Image[79];
        public GlowPoint[] m_glowPoints;
        public CommonMethod commonMethod = new CommonMethod();

        public PlayTerrariaByOneLED()
        {
            for (int i = 0; i < 79; i++)
            {
                m_pictures[i] = Image.Load("app:TerrariaByOneLED\\Terraria_" + (i + 1).ToString() + ".png");
            }
        }

        public void Play(int m_pictureIndex)
        {
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 180; y++)
                {
                    try
                    {
                        m_glowPoints[y * 320 + x].Color = m_pictures[m_pictureIndex].GetPixel(x, y);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}