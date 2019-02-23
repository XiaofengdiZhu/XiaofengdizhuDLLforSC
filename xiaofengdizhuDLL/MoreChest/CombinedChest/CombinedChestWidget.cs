using Engine;
using System.Xml.Linq;

namespace Game
{
    public class CombinedChestWidget : CanvasWidget
    {
        public ButtonWidget m_pageLeftButton;
        public ButtonWidget m_pageRightButton;
        public ComponentCombinedChest m_componentCombinedChest;
        public GridPanelWidget m_inventoryGrid;
        public GridPanelWidget m_chestGrid;
        public LabelWidget m_pageLabel;
        public ComponentInventoryWithPage m_componentInventoryWithPage;
        public int m_pagesCount;
        public int m_lastPageIndex = -1;

        public CombinedChestWidget(IInventory inventory, ComponentCombinedChest componentCombinedChest)
        {
            m_componentCombinedChest = componentCombinedChest;
            XElement node = ContentManager.Get<XElement>("Widgets/CombinedChestWidget");
            WidgetsManager.LoadWidgetContents(this, this, node);
            m_inventoryGrid = Children.Find<GridPanelWidget>("InventoryGrid", true);
            m_chestGrid = Children.Find<GridPanelWidget>("ChestGrid", true);
            m_pageLeftButton = Children.Find<ButtonWidget>("PageLeftButton", true);
            m_pageRightButton = Children.Find<ButtonWidget>("PageRightButton", true);
            m_pageLabel = Children.Find<LabelWidget>("PageLabel", true);
            m_componentInventoryWithPage = componentCombinedChest.Entity.FindComponent<ComponentInventoryWithPage>(true);
            m_pagesCount = m_componentInventoryWithPage.SlotsCount / (m_chestGrid.RowsCount * m_chestGrid.ColumnsCount);
            int num = m_componentInventoryWithPage.PageIndex * m_chestGrid.RowsCount * m_chestGrid.ColumnsCount;
            for (int i = 0; i < m_chestGrid.RowsCount; i++)
            {
                for (int j = 0; j < m_chestGrid.ColumnsCount; j++)
                {
                    var inventorySlotWidget = new InventorySlotWidget();
                    inventorySlotWidget.AssignInventorySlot(m_componentCombinedChest, num++);
                    m_chestGrid.Children.Add(inventorySlotWidget);
                    m_chestGrid.SetWidgetCell(inventorySlotWidget, new Point2(j, i));
                }
            }
            num = 6;
            for (int k = 0; k < m_inventoryGrid.RowsCount; k++)
            {
                for (int l = 0; l < m_inventoryGrid.ColumnsCount; l++)
                {
                    var inventorySlotWidget2 = new InventorySlotWidget();
                    inventorySlotWidget2.AssignInventorySlot(inventory, num++);
                    m_inventoryGrid.Children.Add(inventorySlotWidget2);
                    m_inventoryGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
                }
            }
        }

        public override void Update()
        {
            if (!m_componentCombinedChest.IsAddedToProject)
                ParentWidget.Children.Remove(this);
            if (m_pageRightButton.IsClicked)
            {
                m_componentInventoryWithPage.PageIndex++;
            }
            if (m_pageLeftButton.IsClicked)
            {
                m_componentInventoryWithPage.PageIndex--;
            }
            m_pageRightButton.IsEnabled = m_componentInventoryWithPage.PageIndex < m_pagesCount - 1;
            m_pageLeftButton.IsEnabled = m_componentInventoryWithPage.PageIndex > 0;
            m_pageLabel.Text = string.Format("{0}/{1}", new object[]
            {
                m_componentInventoryWithPage.PageIndex + 1,
                m_pagesCount
            });
            if (m_componentInventoryWithPage.PageIndex != m_lastPageIndex)
            {
                int num3 = m_chestGrid.ColumnsCount * m_chestGrid.RowsCount;
                int num4 = m_componentInventoryWithPage.PageIndex * num3;
                foreach (Widget widget2 in m_chestGrid.Children)
                {
                    var inventorySlotWidget = widget2 as InventorySlotWidget;
                    if (inventorySlotWidget != null)
                    {
                        if (num4 < m_componentInventoryWithPage.SlotsCount)
                        {
                            inventorySlotWidget.AssignInventorySlot(m_componentInventoryWithPage, num4++);
                        }
                        else
                        {
                            inventorySlotWidget.AssignInventorySlot(null, 0);
                        }
                    }
                }
                m_lastPageIndex = m_componentInventoryWithPage.PageIndex;
            }
        }
    }
}