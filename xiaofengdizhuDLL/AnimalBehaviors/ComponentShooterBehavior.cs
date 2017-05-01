﻿using System.Collections.Generic;
using System.Reflection;
using Game;
using Engine;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
    public class ComponentShooterBehavior : ComponentBehavior, IUpdateable
    {
        private ComponentCreature m_componentCreature;
        private ComponentChaseBehavior m_componenttChaseBehavior;
        private SubsystemTerrain m_subsystemTerrain;
        private StateMachine m_stateMachine = new StateMachine();
        private SubsystemTime m_subsystemTime;
        private SubsystemProjectiles m_subsystemProjectiles;
        private Random m_random = new Random();
        private double m_nextUpdateTime;
        private double m_ChargeTime;
        private float m_distance;

        public int UpdateOrder
        {
            get
            {
                return 0;
            }
        }

        public override float ImportanceLevel
        {
            get
            {
                return 0;
            }
        }

        protected override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
            m_componenttChaseBehavior = Entity.FindComponent<ComponentChaseBehavior>(true);
            m_subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemProjectiles = Project.FindSubsystem<SubsystemProjectiles>(true);
        }

        public void Update(float dt)
        {
            if (m_subsystemTime.GameTime >= m_nextUpdateTime)
            {
                m_distance = 10;
                if (m_componenttChaseBehavior.Target != null)
                {
                    Vector3 position = m_componentCreature.ComponentCreatureModel.EyePosition + m_componentCreature.ComponentBody.Matrix.Right * 0.3f - m_componentCreature.ComponentBody.Matrix.Up * 0.2f + m_componentCreature.ComponentBody.Matrix.Forward * 0.2f;
                    Vector3 target_direction = m_componenttChaseBehavior.Target.ComponentBody.Position - position;
                    m_distance = target_direction.Length();
                    Vector3 direction = Vector3.Normalize(target_direction + m_random.Vector3((m_distance<10)?0.4f:1f, false));
                    int value = TerrainData.MakeBlockValue(192, 0, ArrowBlock.SetArrowType(0, (ArrowBlock.ArrowType)0));//ArrowType:0、1、2、3、4、8
                    float vx = MathUtils.Lerp(0f, 40f, MathUtils.Pow((float)m_ChargeTime / 2f, 0.5f));
                    m_subsystemProjectiles.FireProjectile(value, position, direction * vx + new Vector3(0, 8 * m_distance / vx, 0), Vector3.Zero, m_componentCreature);
                }
                m_ChargeTime = m_random.UniformFloat(1.6f, 2.2f);
                if (m_distance < 10) m_ChargeTime *= 0.7;
                m_nextUpdateTime = m_subsystemTime.GameTime + m_ChargeTime;
                m_stateMachine.Update();
            }
        }
        public ComponentShooterBehavior() : base() { }
    }


}
