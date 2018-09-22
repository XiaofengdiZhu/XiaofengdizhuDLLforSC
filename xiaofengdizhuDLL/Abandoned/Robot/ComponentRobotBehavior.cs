/*using System.Collections.Generic;
using System.Reflection;
using Game;
using Engine;
using GameEntitySystem;
using TemplatesDatabase;
using System;
using System.Globalization;
using System.Linq;

namespace Game
{
    class ComponentRobotBehavior : ComponentBehavior, IUpdateable
    {
        private SubsystemGameInfo m_subsystemGameInfo;
        private SubsystemTime m_subsystemTime;
        private SubsystemBodies m_subsystemBodies;
        private SubsystemTerrain m_subsystemTerrain;
        private ComponentCreature m_componentCreature;
        private ComponentHealth m_componentHealth;
        private ComponentBody m_componentBody;
        private ComponentLocomotion m_componenttLocomotion;
        private ComponentPilot m_componenttPilot;
        private StateMachine m_stateMachine = new StateMachine();
        private Random m_random = new Random();
        public DynamicArray<ComponentBody> m_nearbyBodies = new DynamicArray<ComponentBody>();
        public double m_nextUpdateTime;
        public double m_lastBreedTime;
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
            m_subsystemGameInfo = Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemBodies = Project.FindSubsystem<SubsystemBodies>(true);
            m_subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
            m_componentHealth = Entity.FindComponent<ComponentHealth>(true);
            m_componentBody = Entity.FindComponent<ComponentBody>(true);
            m_componenttLocomotion = Entity.FindComponent<ComponentLocomotion>(true);
            m_componenttPilot = Entity.FindComponent<ComponentPilot>(true);
        }
        public void Update(float dt)
        {
            if (m_subsystemTime.GameTime >= m_nextUpdateTime && m_componentHealth.Health>0)
            {
                Log.Information(FindFruit(new Point3((int)m_componentBody.Position.X, (int)m_componentBody.Position.Y, (int)m_componentBody.Position.Z), 10));
                m_nextUpdateTime = m_subsystemTime.GameTime + 10;
                m_stateMachine.Update();
            }
        }

        public ComponentRobotBehavior() : base() { }

        public Point3 FindFruit(Point3 position, int content)
        {
            Dictionary<Point3, int> close = new Dictionary<Point3, int>() { { position, 0 } };
            Dictionary<Point3, int> open = new Dictionary<Point3, int>();
            bool flag = m_random.Bool();
            List<Point3> sixDirection = new List<Point3>() {
                Point3.UnitX, -Point3.UnitX, Point3.UnitY, -Point3.UnitY, Point3.UnitZ, -Point3.UnitZ
            };
            
            for (int directionIndex = 0; directionIndex < 6; directionIndex++)
            {
                Point3 nextPosition = position + sixDirection[directionIndex];
                int nextContent = m_subsystemTerrain.Terrain.GetCellContents(nextPosition.X, nextPosition.Y, nextPosition.Z);
                if (nextContent == content)
                {
                    return nextPosition;
                }
                else if(m_subsystemTerrain.Terrain.IsCellValid(nextPosition.X, nextPosition.Y, nextPosition.Z) && nextContent == 0 && !open.ContainsKey(nextPosition))
                {
                    open.Add(nextPosition, 1);
                }
            }
            Point3 fruitPosition = Point3.Zero;
            bool founded = false;
            int circled = 0;
            while (open.Count > 0)
            {
                circled++;
                KeyValuePair<Point3, int> minOpen = open.First();
                foreach (KeyValuePair<Point3, int> dic in open)
                {
                    if (dic.Value < minOpen.Value)
                    {
                        minOpen = dic;
                    }
                }
                for (int directionIndex = 0; directionIndex < 6; directionIndex++)
                {
                    Point3 nextPosition = minOpen.Key + sixDirection[directionIndex];
                    int nextContent = m_subsystemTerrain.Terrain.GetCellContents(nextPosition.X, nextPosition.Y, nextPosition.Z);
                    if (nextContent == content)
                    {
                        fruitPosition =  nextPosition;
                        founded = true;
                        break;
                    }
                    else if (m_subsystemTerrain.Terrain.IsCellValid(nextPosition.X, nextPosition.Y, nextPosition.Z) && nextContent == 0)
                    {
                        if (close.ContainsKey(nextPosition) && minOpen.Value + 1 < close[nextPosition])
                        {
                            close[nextPosition] = minOpen.Value + 1;
                        }
                        else if (!open.ContainsKey(nextPosition))
                        {
                            open.Add(nextPosition, minOpen.Value + 1);
                        }
                    }
                }
                if (!close.ContainsKey(minOpen.Key)) close.Add(minOpen.Key, minOpen.Value);
                open.Remove(minOpen.Key);
                if (founded)
                {
                    if (!close.ContainsKey(fruitPosition)) close.Add(fruitPosition, minOpen.Value + 1);
                    break;
                }
                if (circled > 1000) break;
            }
            return fruitPosition;
        }
    }
}
*/