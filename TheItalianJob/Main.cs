using System;
using System.Collections.Generic;

using SimpleMissions;
using GTA;
using TheItalianJob.Missions;

namespace TheItalianJob
{
    public class Main : Script
    {
        public static List<Vehicle> spawnedVehicles = new List<Vehicle>();
        public static List<Ped> spawnedPeds = new List<Ped>();
        public static List<Blip> blips = new List<Blip>();

        public Main()
        {
            Func.RegisterMission(typeof(SetupIssi));
            Func.RegisterMission(typeof(SetupWeapons));
            Func.RegisterMission(typeof(Heist));
            Aborted += OnAbort;
        }

        private void OnAbort(object sender, EventArgs e)
        {
            // Cleanup
            foreach (Vehicle vehicle in spawnedVehicles) vehicle.MarkAsNoLongerNeeded();
            foreach (Ped ped in spawnedPeds) ped.MarkAsNoLongerNeeded();
            foreach (Blip blip in blips) blip.Remove();
        }
    }
}
