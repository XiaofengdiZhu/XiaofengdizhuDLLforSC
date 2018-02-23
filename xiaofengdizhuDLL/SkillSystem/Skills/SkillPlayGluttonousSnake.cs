using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Input;

namespace Game
{
    public class SkillPlayGluttonousSnake : Skill
    {
        private PlayGluttonousSnake m_playGluttonousSnake = new PlayGluttonousSnake(144,108);
        private bool m_isPlaying;
        private DateTime m_lastUpdateTime = DateTime.Now;
        public override string Name
        {
            get { return "PlayBadAppleBy4LED"; }
        }
        public override bool Input()
        {
            if (Keyboard.IsKeyDownOnce(Key.O))
            {
                if (!m_isPlaying)
                {
                    m_playGluttonousSnake.AddSnake(PlayGluttonousSnake.SnakeType.Player0);
                    m_isPlaying = true;
                }
            }
            else if (Keyboard.IsKeyDownRepeat(Key.O))
            {
                m_isPlaying = false;
            }
            if (Keyboard.IsKeyDownOnce(Key.K))
            {
                //commonMethod.displaySmallMessage(m_playGluttonousSnake.FindFruit(m_playGluttonousSnake.m_snakes[1].Head).ToString(),false,false);
                m_playGluttonousSnake.AddSnake(PlayGluttonousSnake.SnakeType.Computer);
            }
            if (m_isPlaying) return true; else return false;
        }
        public override void Action()
        {
            m_playGluttonousSnake.UpdatePlayerSnakesDirection();
            if ((DateTime.Now - m_lastUpdateTime).TotalMilliseconds > 100)
            {
                m_playGluttonousSnake.Update();
                for (int x = 0; x < 72; x++)
                {
                    for (int y = 0; y < 54; y++)
                    {
                        try
                        {
                            GlowPoint[] glowPoints = ((FourLedElectricElement)(commonMethod.subsystems.electricity.GetElectricElement(x, 3, y, 4))).m_glowPoints;
                            glowPoints[0].Color = m_playGluttonousSnake.OutputLayer[x * 2 + 1, y * 2];
                            glowPoints[1].Color = m_playGluttonousSnake.OutputLayer[x * 2 + 1, y * 2 + 1];
                            glowPoints[2].Color = m_playGluttonousSnake.OutputLayer[x * 2, y * 2];
                            glowPoints[3].Color = m_playGluttonousSnake.OutputLayer[x * 2, y * 2 + 1];
                        }
                        catch
                        {
                        }
                    }
                }
                m_lastUpdateTime = DateTime.Now;
            }
        }
    }
}
