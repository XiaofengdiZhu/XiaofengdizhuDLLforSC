using Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class OrePlantsManager
    {
        static OrePlantsManager()
        {
            var random = new Random(33);
            var array = new[]
            {
                3,
                4,
                4,
                5,
                5,
                5,
                6,
                6,
                6,
                7,
                7,
                8
            };
            for (int i = 0; i < 8; i++)
            {
                m_treeBrushesByType[i] = new List<TerrainBrush>();
                for (int j = 0; j < 12; j++)
                {
                    int height = array[j];
                    int branchesCount2 = j;
                    TerrainBrush item2 = CreateTreeBrush(random, m_treeTrunksByType[i], m_treeLeavesByType[i], height, branchesCount2, delegate (int y)
                    {
                        float num = 0.66f;
                        if (y < height / 2 - 1)
                        {
                            num = 0f;
                        }
                        else if (y > height / 2 && y <= height)
                        {
                            num *= 1.5f;
                        }
                        return num;
                    }, delegate (int y)
                    {
                        if (y < height * 0.35f || y > height * 0.75f)
                        {
                            return 0f;
                        }
                        return random.UniformFloat(0f, 0.33f * height);
                    });
                    m_treeBrushesByType[i].Add(item2);
                }
            }
        }

        public static ReadOnlyList<TerrainBrush> GetTreeBrushes(OreTreeType treeType)
        {
            return new ReadOnlyList<TerrainBrush>(m_treeBrushesByType[(int)treeType]);
        }

        public static TerrainBrush CreateTreeBrush(Random random, int woodIndex, int leavesIndex, int height, int branchesCount, Func<int, float> leavesProbabilityByHeight, Func<int, float> branchesLengthByHeight)
        {
            return PlantsManager.CreateTreeBrush(random, woodIndex, leavesIndex, height, branchesCount, leavesProbabilityByHeight, branchesLengthByHeight);
        }

        public static List<TerrainBrush>[] m_treeBrushesByType = new List<TerrainBrush>[EnumUtils.GetEnumValues(typeof(OreTreeType)).Max() + 1];

        public static int[] m_treeTrunksByType =
        {
            67,
            67,
            67,
            67,
            67,
            67,
            67,
            67
        };

        public static int[] m_treeLeavesByType =
        {
            330,
            16,
            41,
            39,
            101,
            112,
            148,
            100
        };
    }
}