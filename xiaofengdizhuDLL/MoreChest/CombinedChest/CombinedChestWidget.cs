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
            this.m_componentCombinedChest = componentCombinedChest;
            XElement node = ContentManager.Get<XElement>("Widgets/CombinedChestWidget");
            WidgetsManager.LoadWidgetContents(this, this, node);
            this.m_inventoryGrid = this.Children.Find<GridPanelWidget>("InventoryGrid", true);
            this.m_chestGrid = this.Children.Find<GridPanelWidget>("ChestGrid", true);
            this.m_pageLeftButton = this.Children.Find<ButtonWidget>("PageLeftButton", true);
            this.m_pageRightButton = this.Children.Find<ButtonWidget>("PageRightButton", true);
            this.m_pageLabel = this.Children.Find<LabelWidget>("PageLabel", true);
            this.m_componentInventoryWithPage = componentCombinedChest.Entity.FindComponent<ComponentInventoryWithPage>(true);
            m_pagesCount = m_componentInventoryWithPage.SlotsCount / (m_chestGrid.RowsCount * m_chestGrid.ColumnsCount);
            int num = m_componentInventoryWithPage.PageIndex * m_chestGrid.RowsCount * m_chestGrid.ColumnsCount;
            for (int i = 0; i < this.m_chestGrid.RowsCount; i++)
            {
                for (int j = 0; j < this.m_chestGrid.ColumnsCount; j++)
                {
                    InventorySlotWidget inventorySlotWidget = new InventorySlotWidget();
                    inventorySlotWidget.AssignInventorySlot(m_componentCombinedChest, num++);
                    this.m_chestGrid.Children.Add(inventorySlotWidget);
                    this.m_chestGrid.SetWidgetCell(inventorySlotWidget, new Point2(j, i));
                }
            }
            num = 6;
            for (int k = 0; k < this.m_inventoryGrid.RowsCount; k++)
            {
                for (int l = 0; l < this.m_inventoryGrid.ColumnsCount; l++)
                {
                    InventorySlotWidget inventorySlotWidget2 = new InventorySlotWidget();
                    inventorySlotWidget2.AssignInventorySlot(inventory, num++);
                    this.m_inventoryGrid.Children.Add(inventorySlotWidget2);
                    this.m_inventoryGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
                }
            }
        }

        public override void Update()
        {
            if (!this.m_componentCombinedChest.IsAddedToProject)
            {
                base.ParentWidget.Children.Remove(this);
            }
            if (m_pageRightButton.IsClicked)
            {
                m_componentInventoryWithPage.PageIndex++;
            }
            if (m_pageLeftButton.IsClicked)
            {
                m_componentInventoryWithPage.PageIndex--;
            }
            m_pageRightButton.IsEnabled = (m_componentInventoryWithPage.PageIndex < this.m_pagesCount - 1);
            m_pageLeftButton.IsEnabled = (m_componentInventoryWithPage.PageIndex > 0);
            m_pageLabel.Text = string.Format("{0}/{1}", new object[]
            {
                m_componentInventoryWithPage.PageIndex + 1,
                this.m_pagesCount
            });
            if (m_componentInventoryWithPage.PageIndex != this.m_lastPageIndex)
            {
                int num3 = this.m_chestGrid.ColumnsCount * this.m_chestGrid.RowsCount;
                int num4 = this.m_componentInventoryWithPage.PageIndex * num3;
                foreach (Widget widget2 in this.m_chestGrid.Children)
                {
                    InventorySlotWidget inventorySlotWidget = widget2 as InventorySlotWidget;
                    if (inventorySlotWidget != null)
                    {
                        if (num4 < this.m_componentInventoryWithPage.SlotsCount)
                        {
                            inventorySlotWidget.AssignInventorySlot(this.m_componentInventoryWithPage, num4++);
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