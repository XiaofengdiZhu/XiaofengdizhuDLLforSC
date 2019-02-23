using Game;
using System.Collections.Generic;
using System.Reflection;

namespace xiaofengdizhuDLL
{
    public class xiaofengdizhu
    {
        public static IEnumerable<TypeInfo> getMyEnumerable()
        {
            var l1 = new List<TypeInfo>(typeof(BlocksManager).GetTypeInfo().Assembly.DefinedTypes);
            l1.AddRange(typeof(xiaofengdizhu).GetTypeInfo().Assembly.DefinedTypes);
            return l1;
        }
    }
}