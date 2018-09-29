using Engine;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using TemplatesDatabase;

namespace Game
{
    public class ComponentBlocksEntity : Component
    {
        private List<Point3> m_coordinates = new List<Point3>();

        public List<Point3> Coordinates
        {
            get
            {
                return m_coordinates;
            }
            set
            {
                m_coordinates = value;
            }
        }

        protected override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            try
            {
                string[] strings = valuesDictionary.GetValue<string>("Coordinates").Split(';');
                foreach (string str in strings)
                {
                    string[] strings1 = str.Split(',');
                    if (strings1.Length != 3) continue;
                    m_coordinates.Add(new Point3(int.Parse(strings1[0]), int.Parse(strings1[1]), int.Parse(strings1[2])));
                }
            }
            catch (Exception e)
            {
                Log.Warning(e.ToString());
            }
        }

        protected override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            string str = "";
            foreach (Point3 point in Coordinates)
            {
                str += point.X + "," + point.Y + "," + point.Z + ";";
            }
            valuesDictionary.SetValue<string>("Coordinates", str.Remove(str.Length - 1));
        }
    }
}