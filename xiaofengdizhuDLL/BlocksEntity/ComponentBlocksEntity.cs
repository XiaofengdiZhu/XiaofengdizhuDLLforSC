using Engine;
using GameEntitySystem;
using System;
using System.Text;
using System.Collections.Generic;
using TemplatesDatabase;

namespace Game
{
    public class ComponentBlocksEntity : Component
    {
        private List<Point3> m_coordinates = new List<Point3>();

        public List<Point3> Coordinates
        {
            get { return m_coordinates; }
            set { m_coordinates = value; }
        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            try
            {
                string[] strings = valuesDictionary.GetValue<string>("Coordinates").Split(';');
                for (int i = 0, stringsLength = strings.Length; i < stringsLength; i++)
                {
                    string[] strings1 = strings[i].Split(',');
                    if (strings1.Length == 3)
                        m_coordinates.Add(new Point3(int.Parse(strings1[0]), int.Parse(strings1[1]), int.Parse(strings1[2])));
                }
            }
            catch (Exception e)
            {
                Log.Warning(e.ToString());
            }
        }

        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            var sb = new StringBuilder();
            foreach (Point3 point in Coordinates)
            {
                sb.Append(point.X + "," + point.Y + "," + point.Z + ";");
            }
            valuesDictionary.SetValue("Coordinates", sb.ToString(0, sb.Length - 1));
        }
    }
}