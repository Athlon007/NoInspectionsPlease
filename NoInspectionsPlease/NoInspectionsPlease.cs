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
        public override string Version => "1.1"; //Version

        // Set this to true if you will be load custom assets from Assets folder.
        // This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => false;

        static FsmInt currentWeek;
        static FsmInt nextInspection;
        static FsmInt inspectionIntervalWeeks;

        const int NoInspectionValue = 999999999;

        public override void OnLoad()
        {
            // Called once, when mod is loading after game is fully loaded
            PlayMakerFSM inspectionProcess = GameObject.Find("INSPECTION").transform.Find("Functions/InspectionProcess").gameObject.GetComponent<PlayMakerFSM>();
            currentWeek = inspectionProcess.FsmVariables.GetFsmInt("_CurrentWeek");
            nextInspection = inspectionProcess.FsmVariables.GetFsmInt("_NextInspection");
            inspectionIntervalWeeks = inspectionProcess.FsmVariables.GetFsmInt("_InspectionIntervalWeeks");
            UpdateInterval();
        }

        static void UpdateInterval()
        {
            // Disabling inspection sets the nextInspection value to NoInspectionValue
            if ((bool)disableInspection.GetValue())
            {
                nextInspection.Value = NoInspectionValue;
                return;
            }

            inspectionIntervalWeeks.Value = int.Parse(inspectionFrequency.GetValue().ToString());

            // If player had previous version of NoInspection mod, or had set disableInspection to true, reset the value
            if (nextInspection.Value == NoInspectionValue)
                nextInspection.Value = currentWeek.Value + inspectionIntervalWeeks.Value;
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
