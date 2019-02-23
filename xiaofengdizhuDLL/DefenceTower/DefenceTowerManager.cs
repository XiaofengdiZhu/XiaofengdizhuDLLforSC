using System.Collections.Generic;

namespace Game
{
    public static class DefenceTowerManager
    {
        static DefenceTowerManager()
        {
        }

        public static int m_towerKindCount = 7;

        public static string[] m_towerName = new string[]
        {
            "Defence",
            "Arrow",
            "Bolt",
            "Bullet",
            "Gravity",
            "Lightning",
            "Unknown"
        };

        public static List<int[]> m_towerBlocks = new List<int[]>()
        {
            new int[0],
            new[]{ 150,150,150},//大煤块
            new[]{71,71,71 },//大孔雀石
            new[]{47,47,47 },//大铜块
            new[]{46,46,46,46 },//大铁块
            new[]{231,231,231,231 },//大锗块
            new[]{126,126,126,126,126}//大钻块
        };

        public static double[] m_towerPeriod = new double[]
        {
            double.MaxValue,
            2,
            2,
            0.2,
            0.05,
            5,
            double.MaxValue
        };
    }
}