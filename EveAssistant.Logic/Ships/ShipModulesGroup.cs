using System.Collections.Generic;

namespace EveAssistant.Logic.Ships
{
    public class ShipModulesGroup
    {
        public List<string> Modules { get; set; }

        public bool IsEnabled { get; set; } = false;

        public ShipModulesGroup()
        {
            Modules = new List<string>();
        }
    }
}