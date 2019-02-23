using Engine;
using Engine.Graphics;
using System.Collections.Generic;
using System.Globalization;

namespace Game
{
    public class SixteenLedBlock : MountedElectricElementBlock
    {
        public override void Initialize()
        {
            ModelMesh modelMesh = ContentManager.Get<Model>("Models/Leds").FindMesh("OneLed", true);
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(modelMesh.ParentBone);
            for (int i = 0; i < 6; i++)
            {
                Matrix m;
                if (i < 4)
                {
                    m = Matrix.CreateRotationX(1.57079637f) * Matrix.CreateTranslation(0f, 0f, -0.5f) * Matrix.CreateRotationY(i * 3.14159274f / 2f) * Matrix.CreateTranslation(0.5f, 0.5f, 0.5f);
                }
                else if (i == 4)
                {
                    m = Matrix.CreateTranslation(0.5f, 0f, 0.5f);
                }
                else
                {
                    m = Matrix.CreateRotationX(3.14159274f) * Matrix.CreateTranslation(0.5f, 1f, 0.5f);
                }
                m_blockMeshesByFace[i] = new BlockMesh();
                m_blockMeshesByFace[i].AppendModelMeshPart(modelMesh.MeshParts[0], boneAbsoluteTransform * m, false, false, false, false, Color.White);
                m_collisionBoxesByFace[i] = new BoundingBox[]
                {
                    m_blockMeshesByFace[i].CalculateBoundingBox()
                };
            }
            Matrix m2 = Matrix.CreateRotationY(-1.57079637f) * Matrix.CreateRotationZ(1.57079637f);
            m_standaloneBlockMesh = new BlockMesh();
            m_standaloneBlockMesh.AppendModelMeshPart(modelMesh.MeshParts[0], boneAbsoluteTransform * m2, false, false, false, false, Color.White);
        }

        public override IEnumerable<CraftingRecipe> GetProceduralCraftingRecipes()
        {
            int num;
            for (int i = 0; i < 8; i = num)
            {
                var craftingRecipe = new CraftingRecipe
                {
                    ResultCount = 4,
                    ResultValue = Terrain.MakeBlockValue(310, 0, SetColor(0, i)),
                    RemainsCount = 1,
                    RemainsValue = Terrain.MakeBlockValue(90),
                    RequiredHeatLevel = 0f,
                    Description = "Make colored 16-LEDs from copper, glass and paint"
                };
                craftingRecipe.Ingredients[0] = "glass";
                craftingRecipe.Ingredients[1] = "glass";
                craftingRecipe.Ingredients[2] = "glass";
                craftingRecipe.Ingredients[4] = "paintbucket:" + i.ToString(CultureInfo.InvariantCulture);
                craftingRecipe.Ingredients[6] = "copperingot";
                craftingRecipe.Ingredients[7] = "copperingot";
                craftingRecipe.Ingredients[8] = "copperingot";
                yield return craftingRecipe;
                num = i + 1;
            }
            yield break;
        }

        public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
        {
            int mountingFace = GetMountingFace(Terrain.ExtractData(value));
            return face != CellFace.OppositeFace(mountingFace);
        }

        public override int GetFace(int value)
        {
            return GetMountingFace(Terrain.ExtractData(value));
        }

        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            int color = GetColor(Terrain.ExtractData(value));
            return LedBlock.LedColorDisplayNames[color] + " 16-LED";
        }

        //[IteratorStateMachine(typeof(SixteenLedBlock.< GetCreativeValues > d__9))]
        public override IEnumerable<int> GetCreativeValues()
        {
            int num;
            for (int i = 0; i < 8; i = num)
            {
                yield return Terrain.MakeBlockValue(310, 0, SetColor(0, i));
                num = i + 1;
            }
            yield break;
        }

        public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
        {
            int data = SetMountingFace(Terrain.ExtractData(value), raycastResult.CellFace.Face);
            int value2 = Terrain.ReplaceData(value, data);
            return new BlockPlacementData
            {
                Value = value2,
                CellFace = raycastResult.CellFace
            };
        }

        public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
        {
            int color = GetColor(Terrain.ExtractData(oldValue));
            dropValues.Add(new BlockDropValue
            {
                Value = Terrain.MakeBlockValue(310, 0, SetColor(0, color)),
                Count = 1
            });
            showDebris = true;
        }

        public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
        {
            int mountingFace = GetMountingFace(Terrain.ExtractData(value));
            if (mountingFace >= m_collisionBoxesByFace.Length)
            {
                return null;
            }
            return m_collisionBoxesByFace[mountingFace];
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            int mountingFace = GetMountingFace(Terrain.ExtractData(value));
            if (mountingFace < m_blockMeshesByFace.Length)
            {
                generator.GenerateMeshVertices(this, x, y, z, m_blockMeshesByFace[mountingFace], Color.White, null, geometry.SubsetOpaque);
                generator.GenerateWireVertices(value, x, y, z, mountingFace, 1f, Vector2.Zero, geometry.SubsetOpaque);
            }
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
        }

        public override ElectricElement CreateElectricElement(SubsystemElectricity subsystemElectricity, int value, int x, int y, int z)
        {
            return new SixteenLedElectricElement(subsystemElectricity, new CellFace(x, y, z, GetFace(value)));
        }

        public override ElectricConnectorType? GetConnectorType(SubsystemTerrain terrain, int value, int face, int connectorFace, int x, int y, int z)
        {
            int face2 = GetFace(value);
            if (face == face2 && SubsystemElectricity.GetConnectorDirection(face2, 0, connectorFace).HasValue)
            {
                return new ElectricConnectorType?(ElectricConnectorType.Input);
            }
            return null;
        }

        public static int GetColor(int data)
        {
            return data >> 3 & 7;
        }

        public static int SetColor(int data, int color)
        {
            return (data & -57) | (color & 7) << 3;
        }

        public static int GetMountingFace(int data)
        {
            return data & 7;
        }

        public static int SetMountingFace(int data, int face)
        {
            return (data & -8) | (face & 7);
        }

        public const int Index = 310;
        public BlockMesh m_standaloneBlockMesh;
        public BlockMesh[] m_blockMeshesByFace = new BlockMesh[6];
        public BoundingBox[][] m_collisionBoxesByFace = new BoundingBox[6][];
    }
}