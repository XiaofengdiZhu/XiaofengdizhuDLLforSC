﻿using Engine;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
    public class SubsystemCombinedChestBlockBehavior : SubsystemBlockBehavior
    {
        private SubsystemTerrain m_subsystemTerrain;
        public SubsystemBlocksEntities m_subsystemBlocksEntities;
        public SubsystemAudio m_subsystemAudio;

        public override int[] HandledBlocks
        {
            get { return new[]
                {
                    320
                }; }
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemBlocksEntities = Project.FindSubsystem<SubsystemBlocksEntities>(true);
            m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
        }

        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            ComponentBlocksEntity blocksEntity = null;
            Log.Information(m_subsystemTerrain.Terrain.GetCellValue(x + 1, y, z) + " " + m_subsystemTerrain.Terrain.GetCellValue(x - 1, y, z) + " " + m_subsystemTerrain.Terrain.GetCellValue(x + 1, y, z + 1) + " " + m_subsystemTerrain.Terrain.GetCellValue(x, y, z - 1));
            if (m_subsystemTerrain.Terrain.GetCellContents(x + 1, y, z) == 320)
            {
                blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(x + 1, y, z);
            }
            else if (m_subsystemTerrain.Terrain.GetCellContents(x - 1, y, z) == 320)
            {
                blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(x - 1, y, z);
            }
            else if (m_subsystemTerrain.Terrain.GetCellContents(x, y, z + 1) == 320)
            {
                blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(x, y, z + 1);
            }
            else if (m_subsystemTerrain.Terrain.GetCellContents(x, y, z - 1) == 320)
            {
                blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(x, y, z - 1);
            }
            if (blocksEntity != null && blocksEntity.Coordinates.Count < 2)
            {
                var point = new Point3(x, y, z);
                blocksEntity.Coordinates.Add(point);
                m_subsystemBlocksEntities.m_blocksEntities.Add(point, blocksEntity);
            }
            else
            {
                var valuesDictionary = new ValuesDictionary();
                valuesDictionary.PopulateFromDatabaseObject(Project.GameDatabase.Database.FindDatabaseObject("CombinedChest", Project.GameDatabase.EntityTemplateType, true));
                valuesDictionary.GetValue<ValuesDictionary>("BlocksEntity").SetValue("Coordinates", x + "," + y + "," + z);
                Project.AddEntity(Project.CreateEntity(valuesDictionary));
            }
        }

        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            ComponentBlocksEntity blocksEntity = null;
            if (m_subsystemTerrain.Terrain.GetCellContents(x + 1, y, z) == 320)
            {
                blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(x + 1, y, z);
            }
            else if (m_subsystemTerrain.Terrain.GetCellContents(x - 1, y, z) == 320)
            {
                blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(x - 1, y, z);
            }
            else if (m_subsystemTerrain.Terrain.GetCellContents(x, y, z + 1) == 320)
            {
                blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(x, y, z + 1);
            }
            else if (m_subsystemTerrain.Terrain.GetCellContents(x, y, z - 1) == 320)
            {
                blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(x, y, z - 1);
            }
            if (blocksEntity != null)
            {
                var point = new Point3(x, y, z);
                blocksEntity.Coordinates.Remove(point);
                m_subsystemBlocksEntities.m_blocksEntities.Remove(point);
            }
            else
            {
                blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(x, y, z);
                if (blocksEntity != null)
                {
                    Vector3 position = new Vector3(x, y, z) + new Vector3(0.5f);
                    foreach (IInventory inventory in blocksEntity.Entity.FindComponents<IInventory>())
                    {
                        inventory.DropAllItems(position);
                    }
                    Project.RemoveEntity(blocksEntity.Entity, true);
                }
            }
        }

        public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
        {
            ComponentBlocksEntity blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
            if (blocksEntity != null && blocksEntity.Coordinates.Count == 2 && componentMiner.ComponentPlayer != null)
            {
                ComponentCombinedChest ComponentCombinedChest = blocksEntity.Entity.FindComponent<ComponentCombinedChest>(true);
                componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new CombinedChestWidget(componentMiner.Inventory, ComponentCombinedChest);
                AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                return true;
            }
            return false;
        }

        public override void OnHitByProjectile(CellFace cellFace, WorldItem worldItem)
        {
            if (!worldItem.ToRemove)
            {
                ComponentBlocksEntity blocksEntity = m_subsystemBlocksEntities.GetBlockEntity(cellFace.X, cellFace.Y, cellFace.Z);
                if (blocksEntity != null)
                {
                    var inventory = blocksEntity.Entity.FindComponent<ComponentCombinedChest>(true);
                    var pickable = worldItem as Pickable;
                    int num = (pickable != null) ? pickable.Count : 1;
                    int num2 = ComponentInventoryBase.AcquireItems(inventory, worldItem.Value, num);
                    if (num2 < num)
                    {
                        m_subsystemAudio.PlaySound("Audio/PickableCollected", 1f, 0f, worldItem.Position, 3f, true);
                    }
                    if (num2 <= 0)
                    {
                        worldItem.ToRemove = true;
                        return;
                    }
                    if (pickable != null)
                    {
                        pickable.Count = num2;
                    }
                }
            }
        }
    }
}