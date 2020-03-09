using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace NoInspectionsPlease
{
    public class NoInspectionsPlease : Mod
    {
        public override string ID => "NoInspectionsPlease"; //Your mod ID (unique)
        public override string Name => "No Inspections, Please!"; //You mod name
        public override string Author => "Athlon"; //Your Username
        public override string Version => "1.2"; //Version

        // Set this to true if you will be load custom assets from Assets folder.
        // This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => false;

        static FsmInt nextInspectionIntervalDays;
        static FsmInt nextInspectionIntervalLetter;
        static FsmInt nextInspectionDay;

        const int NoInspectionValue = 0;

        public override void OnLoad()
        {
            // Called once, when mod is loading after game is fully loaded
            PlayMakerFSM inspectionProcess = GameObject.Find("INSPECTION").transform.Find("Functions/InspectionProcess").gameObject.GetComponent<PlayMakerFSM>();
            nextInspectionIntervalDays = inspectionProcess.FsmVariables.GetFsmInt("_InspectionIntervalDays");
            nextInspectionIntervalLetter = inspectionProcess.FsmVariables.GetFsmInt("_InspectionIntervalLetter");
            nextInspectionDay = inspectionProcess.FsmVariables.GetFsmInt("_NextInspectionDay");
            UpdateInterval();
        }

        static void UpdateInterval()
        {
            // Disabling inspection sets the nextInspection value to NoInspectionValue
            if ((bool)disableInspection.GetValue())
            {
                nextInspectionDay.Value = NoInspectionValue;
                return;
            }

            nextInspectionIntervalLetter.Value = int.Parse(inspectionFrequency.GetValue().ToString()) * 7;
            nextInspectionIntervalDays.Value = nextInspectionIntervalDays.Value + 7;
        }

        static Settings inspectionFrequency = new Settings("inspectionFrequency", "Inspection Frequency (in weeks)", 6, UpdateInterval);
        static Settings disableInspection = new Settings("disableInspection", "Disable Periodic Inspection", false, UpdateInterval);

        // All settings should be created here. 
        // DO NOT put anything else here that settings.
        public override void ModSettings()
        {
            Settings.AddSlider(this, inspectionFrequency, 2, 12);
            Settings.AddCheckBox(this, disableInspection);
        }
    }
}
