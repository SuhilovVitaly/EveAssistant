namespace EveAssistant.Common.Configuration
{
    public class Shortcuts
    {
        public string TacticalOverview => ConfigurationTools.GetConfigFromSectionOptionalStringValue("TacticalOverview", "Shortcuts");
        public string ShipStop => ConfigurationTools.GetConfigFromSectionOptionalStringValue("ShipStop", "Shortcuts");

        public string Approach => ConfigurationTools.GetConfigFromSectionOptionalStringValue("Approach", "Shortcuts");

        public string OpenHoldsAndBays => ConfigurationTools.GetConfigFromSectionOptionalStringValue("OpenHoldsAndBays", "Shortcuts");

        public string OpenOreHold => ConfigurationTools.GetConfigFromSectionOptionalStringValue("OpenOreHold", "Shortcuts");

        public string KeepAtRange => ConfigurationTools.GetConfigFromSectionOptionalStringValue("KeepAtRange", "Shortcuts");

        public string LaunchDrones => ConfigurationTools.GetConfigFromSectionOptionalStringValue("LaunchDrones", "Shortcuts");

        public string NextTarget => ConfigurationTools.GetConfigFromSectionOptionalStringValue("NextTarget", "Shortcuts");

        public string LockTarget => ConfigurationTools.GetConfigFromSectionOptionalStringValue("LockTarget", "Shortcuts");

        public string PrevuesTarget => ConfigurationTools.GetConfigFromSectionOptionalStringValue("PrevuesTarget", "Shortcuts");

        public string ReloadAmmo => ConfigurationTools.GetConfigFromSectionOptionalStringValue("ReloadAmmo", "Shortcuts");

        public string ScoopDrones => ConfigurationTools.GetConfigFromSectionOptionalStringValue("ScoopDrones", "Shortcuts");

        public string DronesAttack => ConfigurationTools.GetConfigFromSectionOptionalStringValue("DronesAttack", "Shortcuts");

        public string ClearScreen => ConfigurationTools.GetConfigFromSectionOptionalStringValue("ClearScreen", "Shortcuts");

        public string UnlockAllTargets => ConfigurationTools.GetConfigFromSectionOptionalStringValue("UnlockAllTargets", "Shortcuts");

        public string ExitStation => ConfigurationTools.GetConfigFromSectionOptionalStringValue("ExitStation", "Shortcuts");

        public string OpenCargo => ConfigurationTools.GetConfigFromSectionOptionalStringValue("OpenCargo", "Shortcuts");

        public string FormFleet => ConfigurationTools.GetConfigFromSectionOptionalStringValue("FormFleet", "Shortcuts");

        public string Orbit => ConfigurationTools.GetConfigFromSectionOptionalStringValue("Orbit", "Shortcuts");

    }
}