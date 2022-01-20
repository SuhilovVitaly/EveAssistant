using System.Drawing;

namespace EveAssistant.Common
{
    public class ScreenZones
    {
        public Rectangle SpeedState { get; set; } = new Rectangle(350, 170, 100, 100);

        public Rectangle Overview { get; set; } = new Rectangle(850, 210, 1200 - 850, 620 - 210);

        public Rectangle SelectedItem { get; set; } = new Rectangle(850, 0, 300, 170);

        public Rectangle HangarItemFilters { get; set; } = new Rectangle(2, 830, 110, 210);

        public Point ActiveShipCargoPoint { get; set; } = new Point(400, 690);

        public Point ActiveShipCargoPointFirst { get; set; } = new Point(170, 690);

        public Point ItemHangarCargoPoint { get; set; } = new Point(1025, 710);
    }
}