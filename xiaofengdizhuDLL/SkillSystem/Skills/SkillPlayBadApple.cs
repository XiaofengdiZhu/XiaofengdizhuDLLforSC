﻿//使用时取消注释
/*
using Engine;
using Engine.Graphics;
using Engine.Input;
using System;

namespace Game
{
    public class SkillPlayBadApple : Skill
    {
        private PlayBadApple m_playBadApple = new PlayBadApple(); 
        private bool m_isPlaying;
        private DateTime m_startTime;
        public override string Name
        {
            get { return "PlayBadApple"; }
        }
        public override bool Input()
        {
            if (Keyboard.IsKeyDownOnce(Key.O))
            {
                if (m_isPlaying)
                {
                    m_playBadApple.m_music.Stop();
                    m_playBadApple.m_music.Play();
                    m_startTime = DateTime.Now;
                }
                else
                {
                    m_playBadApple.m_music.Play();
                    m_startTime = DateTime.Now;
                    m_isPlaying = true;
                }
            }
            else if (Keyboard.IsKeyDownRepeat(Key.O))
            {
                m_playBadApple.m_music.Stop();
                m_isPlaying = false;
            }
            if (m_isPlaying) return true; else return false;
        }
        public override void Action()
        {
            TimeSpan timePlayed = DateTime.Now - m_startTime;
            int m_pictureIndex = (int)(timePlayed.TotalSeconds*30);
            if (m_pictureIndex > 6574)
            {
                m_startTime = DateTime.Now;
                m_pictureIndex = 0;
            }
            Camera camera = componentPlayer.View.ActiveCamera;
            PrimitivesRenderer3D primitivesRenderer3d = commonMethod.project.FindSubsystem<SubsystemModelsRenderer>(true).PrimitivesRenderer;
            m_playBadApple.Play(m_pictureIndex, camera, primitivesRenderer3d);
            Vector3 playerPosition = commonMethod.playerPosition;
            float distance = MathUtils.Sqrt(Vector3.DistanceSquared(playerPosition, new Vector3(0, 80, 0)));
            float volume = 1;
            float minDistance = 4;
            float rolloffFactor = 2;
            if (distance > minDistance) volume = minDistance / (minDistance + MathUtils.Max(rolloffFactor * (distance - minDistance), 0f));
            m_playBadApple.m_music.Volume = volume;
        }
    }

}
*/