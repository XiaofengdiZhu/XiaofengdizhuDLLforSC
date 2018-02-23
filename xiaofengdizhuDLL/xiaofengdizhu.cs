using System.Collections.Generic;
using System.Reflection;
using Game;
using Engine;
using GameEntitySystem;
using TemplatesDatabase;

namespace xiaofengdizhuDLL
{
    public class xiaofengdizhu
    {
        public static IEnumerable<TypeInfo> getMyEnumerable()
        {
            List<TypeInfo> l1 = new List<TypeInfo>(typeof(BlocksManager).GetTypeInfo().Assembly.DefinedTypes);
            l1.AddRange(typeof(xiaofengdizhu).GetTypeInfo().Assembly.DefinedTypes);
            return l1;
        }
    }
}