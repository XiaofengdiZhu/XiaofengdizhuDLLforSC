/*using Engine;
using Engine.Graphics;
using Engine.Input;
using System;

namespace Game
{
    public class SkillPlayTerrariaByOneLED : Skill
    {
        private PlayTerrariaByOneLED m_playTerrariaByOneLED = new PlayTerrariaByOneLED();
        public GlowPoint[] m_glowPoints = new GlowPoint[57600];
        private bool m_isPlaying;
        private DateTime m_startTime;
        public override string Name
        {
            get { return "PlayTerrariaByOneLED"; }
        }
        public override bool Input()
        {
            if (Keyboard.IsKeyDownOnce(Key.Number9))
            {
                if (m_isPlaying)
                {
                    m_startTime = DateTime.Now;
                }
                else
                {
                    for (int x = 0; x < 320; x++)
                    {
                        for (int y = 0; y < 180; y++)
                        {
                            int index = y * 320 + x;
                                Vector3 vector = new Vector3(0f, 1f, 0f);
                                Vector3 vector2 = Vector3.UnitX;
                                Vector3 vector3 = Vector3.Cross(vector, vector2);
                                m_glowPoints[index] = commonMethod.subsystems.glow.AddGlowPoint();
                                m_glowPoints[index].Position = new Vector3(((float)x)/2f + 0.25f,3.5f, ((float)y)/2f + 0.25f);
                                m_glowPoints[index].Forward = vector;
                                m_glowPoints[index].Up = Vector3.UnitX;
                                m_glowPoints[index].Right = vector3;
                                m_glowPoints[index].Color = Color.White;
                                m_glowPoints[index].Size = 0.26f;
                                m_glowPoints[index].FarSize = 0.26f;
                                m_glowPoints[index].FarDistance = 1f;
                                m_glowPoints[index].Type = GlowPointType.Square;
                        }
                    }
                    m_playTerrariaByOneLED.m_glowPoints = m_glowPoints;
                    m_startTime = DateTime.Now;
                    m_isPlaying = true;
                }
            }
            else if (Keyboard.IsKeyDownRepeat(Key.Number0))
            {
                for(int i = 0; i < 160 * 90; i++)
                {
                    commonMethod.subsystems.glow.RemoveGlowPoint(m_glowPoints[i]);
                }
                m_glowPoints = new GlowPoint[14400];
                m_isPlaying = false;
            }
            if (m_isPlaying) return true; else return false;
        }
        public override void Action()
        {
            TimeSpan timePlayed = DateTime.Now - m_startTime;
            int m_pictureIndex = (int)(timePlayed.TotalSeconds * 8.3333);
            if (m_pictureIndex > 79)
            {
                m_startTime = DateTime.Now;
                m_pictureIndex = 0;
            }
            m_playTerrariaByOneLED.Play(m_pictureIndex);
        }
    }
}
*/