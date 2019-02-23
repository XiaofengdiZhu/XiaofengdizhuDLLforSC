using System.Collections.Generic;

namespace Game
{
    public class DefenceTowerCoreBlock : CubeBlock
    {
        public const int Index = 360;

        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            int num = Terrain.ExtractData(value);
            string name = "";
            name = DefenceTowerManager.m_towerName[num];
            return name.Length > 0 ? name + " Tower Core" : "Defence Tower Core";
        }

        public override IEnumerable<int> GetCreativeValues()
        {
            yield return Terrain.MakeBlockValue(360, 0, 0);
            yield return Terrain.MakeBlockValue(360, 0, 1);
            yield return Terrain.MakeBlockValue(360, 0, 2);
            yield return Terrain.MakeBlockValue(360, 0, 3);
            yield return Terrain.MakeBlockValue(360, 0, 4);
            yield return Terrain.MakeBlockValue(360, 0, 5);
            yield return Terrain.MakeBlockValue(360, 0, 6);
            yield break;
        }
    }
}