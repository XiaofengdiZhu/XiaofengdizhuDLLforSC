using System.Xml.Linq;

namespace Game
{
    public class TargetStateWidget : CanvasWidget
    {
        public TargetStateWidget()
        {
            XElement node = ContentManager.Get<XElement>("Widgets/TargetStateWidget");
            WidgetsManager.LoadWidgetContents(this, this, node);
        }
    }
}
