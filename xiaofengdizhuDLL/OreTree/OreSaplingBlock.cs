using Engine;
using Engine.Graphics;
using System;
using System.Collections.Generic;

namespace Game
{
    public class OreSaplingBlock : CrossBlock
    {
        public override void Initialize()
        {
            Model model = ContentManager.Get<Model>("Models/ChristmasTree");
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("StandTrunk", true).ParentBone);
            Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Leaves", true).ParentBone);
            Matrix boneAbsoluteTransform3 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Decorations", true).ParentBone);
            Color color = BlockColorsMap.SpruceLeavesColorsMap.Lookup(4, 15);
            this.m_leavesBlockMesh.AppendModelMeshPart(model.FindMesh("Leaves", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateTranslation(0.5f, 0f, 0.5f), false, false, true, false, Color.White);
            this.m_standTrunkBlockMesh.AppendModelMeshPart(model.FindMesh("StandTrunk", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0.5f, 0f, 0.5f), false, false, false, false, Color.White);
            this.m_decorationsBlockMesh.AppendModelMeshPart(model.FindMesh("Decorations", true).MeshParts[0], boneAbsoluteTransform3 * Matrix.CreateTranslation(0.5f, 0f, 0.5f), false, false, false, false, Color.White);
            this.m_litDecorationsBlockMesh.AppendModelMeshPart(model.FindMesh("Decorations", true).MeshParts[0], boneAbsoluteTransform3 * Matrix.CreateTranslation(0.5f, 0f, 0.5f), true, false, false, false, Color.White);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("StandTrunk", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0f, -1f, 0f), false, false, false, false, Color.White);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Leaves", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateTranslation(0f, -1f, 0f), false, false, true, false, color);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Decorations", true).MeshParts[0], boneAbsoluteTransform3 * Matrix.CreateTranslation(0f, -1f, 0f), false, false, false, false, Color.White);
            base.Initialize();
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, size, ref matrix, environmentData);
        }
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            Color[] oreColor = new Color[]
            {
                new Color(255,205,97),
                new Color(12,12,12),
                new Color(46,206,163),
                new Color(112,60,29),
                new Color(161,161,88),
                new Color(78,166,234),
                new Color(64,64,64),
                new Color(222,222,222),
            };
            Color color = BlockColorsMap.SpruceLeavesColorsMap.Lookup(generator.Terrain, x, z);
            generator.GenerateMeshVertices(this, x, y, z, this.m_standTrunkBlockMesh, Color.White, null, geometry.SubsetOpaque);
            generator.GenerateMeshVertices(this, x, y, z, this.m_litDecorationsBlockMesh, oreColor[Terrain.ExtractData(value)], null, geometry.SubsetOpaque);
            generator.GenerateMeshVertices(this, x, y, z, this.m_leavesBlockMesh, color, null, geometry.SubsetAlphaTest);
            generator.GenerateWireVertices(value, x, y, z, 4, 0.01f, Vector2.Zero, geometry.SubsetOpaque);
        }
        public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
        {
            Color color = BlockColorsMap.SpruceLeavesColorsMap.Lookup(subsystemTerrain.Terrain, Terrain.ToCell(position.X), Terrain.ToCell(position.Z));
            return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, this.DestructionDebrisScale, color, this.DefaultTextureSlot);
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            int num = Terrain.ExtractData(value);
            string name = "";
            switch (num)
            {
                case 0:name = "Experience";break;
                case 1:name = "Coal";break;
                case 2: name = "Copper"; break;
                case 3: name = "Iron"; break;
                case 4: name = "Sulphur"; break;
                case 5: name = "Diamond"; break;
                case 6: name = "Germanium"; break;
                case 7: name = "Saltpeter"; break;
            }
            if (name.Length > 0)return name += " Ore Sapling"; else return "Ore Sapling";
        }
        public override IEnumerable<int> GetCreativeValues()
        {
            yield return Terrain.MakeBlockValue(340, 0, 0);
            yield return Terrain.MakeBlockValue(340, 0, 1);
            yield return Terrain.MakeBlockValue(340, 0, 2);
            yield return Terrain.MakeBlockValue(340, 0, 3);
            yield return Terrain.MakeBlockValue(340, 0, 4);
            yield return Terrain.MakeBlockValue(340, 0, 5);
            yield return Terrain.MakeBlockValue(340, 0, 6);
            yield return Terrain.MakeBlockValue(340, 0, 7);
            yield break;
        }
        public override IEnumerable<CraftingRecipe> GetProceduralCraftingRecipes()
        {
            string[] array = new string[]
            {
                "diamondblock",
                "coalblock",
                "copperblock",
                "ironblock",
                "sulphurchunk",
                "diamondblock",
                "semiconductorblock",
                "saltpeterchunk"
            };
            for (int i = 0; i < 8; i++)
            {
                CraftingRecipe craftingRecipe = new CraftingRecipe
                {
                    ResultCount = 1,
                    ResultValue = Terrain.MakeBlockValue(340, 0, i),
                    RequiredHeatLevel = 0f,
                    Description = "Make an ore sapling"
                };
                craftingRecipe.Ingredients[1] = "target";
                craftingRecipe.Ingredients[3] = array[i];
                craftingRecipe.Ingredients[4] = "christmastree";
                craftingRecipe.Ingredients[5] = "diamondblock";
                craftingRecipe.Ingredients[6] = "battery";
                craftingRecipe.Ingredients[7] = "magnet";
                craftingRecipe.Ingredients[8] = "battery";
                if (i == 5)
                {
                    craftingRecipe.Ingredients[0] = "diamondblock";
                    craftingRecipe.Ingredients[2] = "diamondblock";
                }
                yield return craftingRecipe;
            }
            yield break;
            //yield break;
        }
        public const int Index = 340;
        public BlockMesh m_standaloneBlockMesh = new BlockMesh();
        public BlockMesh m_leavesBlockMesh = new BlockMesh();
        public BlockMesh m_standTrunkBlockMesh = new BlockMesh();
        public BlockMesh m_decorationsBlockMesh = new BlockMesh();
        public BlockMesh m_litDecorationsBlockMesh = new BlockMesh();
    }
}
