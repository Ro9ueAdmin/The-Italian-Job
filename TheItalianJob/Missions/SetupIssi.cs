using SimpleMissions;
using SimpleMissions.Attributes;

using GTA;
using System.Collections.Generic;
using GTA.Math;
using System;

namespace TheItalianJob.Missions
{
    [MissionInfo("The Italian Job - Issi", "italianSetupIssi",  -434.0146f, -2166.265f, 10.32089f, MissionType.HeistSetup, false, "None", Characters.All)]
    public class SetupIssi : Mission
    {
        // All of the variables the mission uses. These MUST be static if you intend to modify them at runtime.
        MissionState state = MissionState.GoToCar;
        Vehicle issi;
        Blip issiBlip;
        Blip mansionBlip;
        Blip garageBlip;

        /// <summary>
        /// The mission setup. This spawns things like vehicles, blips, etc
        /// </summary>
        public override void Start()
        {
            this.pay = 1000;

            issi = World.CreateVehicle(new Model("issi2"), new Vector3(-926.974f, 11.819f, 47.719f), 214.167f);
            Main.spawnedVehicles.Add(issi);
            issiBlip = issi.AddBlip();
            issiBlip.Name = "Issi";
            issiBlip.Color = BlipColor.Blue;

            mansionBlip = World.CreateBlip(new Vector3(-926.974f, 11.819f, 47.719f));
            mansionBlip.Color = BlipColor.Yellow;
            mansionBlip.Name = "Mansion";
            mansionBlip.ShowRoute = true;
            Main.blips.Add(mansionBlip);
            Main.blips.Add(issiBlip);
        }

        /// <summary>
        /// Called once per frame, this is where all of the actual mission logic is
        /// </summary>
        public override void Tick()
        {
            if (issi.IsDead) Fail("The Issi was destroyed!");

            switch (state)
            {
                // Go to the Mansion objective
                case MissionState.GoToCar:
                    UI.ShowSubtitle("Go to the ~y~Mansion", 1);
                    if (World.GetDistance(issi.Position, Game.Player.Character.Position) <= 25)
                    {
                        state = MissionState.StealCar;
                        UI.ShowSubtitle("Steal the ~b~Issi", 15000);
                    }
                    break;


                case MissionState.StealCar:
                    if (mansionBlip != null)
                    {
                        Main.blips.Remove(mansionBlip);
                        mansionBlip.Remove();
                        mansionBlip = null;
                    }
                    if (Game.Player.Character.CurrentVehicle == issi)
                    {
                        state = MissionState.GoToGarage;
                        UI.ShowSubtitle("Drive the ~b~Issi~w~ to the ~y~Garage.", 15000);
                    }
                    break;


                case MissionState.GoToGarage:
                    if (issiBlip.Alpha != 0) issiBlip.Alpha = 0;
                    if (garageBlip == null)
                    {
                        garageBlip = World.CreateBlip(new Vector3(-439.8835f, -2179.444f, 9.94509f));
                        garageBlip.Color = BlipColor.Yellow;
                        garageBlip.Name = "Garage";
                        garageBlip.ShowRoute = true;
                        Main.blips.Add(garageBlip);
                    }
                    if (garageBlip.Alpha == 0) garageBlip.Alpha = 255;
                    if (Game.Player.Character.CurrentVehicle != issi)
                    {
                        state = MissionState.ReturnToCar;
                        UI.ShowSubtitle("Get back into the ~b~Issi", 1);
                    }
                    else if (World.GetDistance(new Vector3(-439.8835f, -2179.444f, 9.94509f), Game.Player.Character.CurrentVehicle.Position) <= 5f)
                    {
                        Game.Player.Character.Task.LeaveVehicle(issi, true);
                        Pass();
                    }
                    break;


                case MissionState.ReturnToCar:
                    issiBlip.Alpha = 255;
                    garageBlip.Alpha = 0;
                    if (Game.Player.Character.CurrentVehicle == issi)
                    {
                        state = MissionState.GoToGarage;
                        UI.ShowSubtitle("Drive the ~b~Issi~w~ to the ~y~Garage.", 15000);
                    }
                    break;
            }
        }

        /// <summary>
        /// Cleans up after the mission by removing blips and marking the vehicle as not needed
        /// </summary>
        public override void End()
        {
            if (Main.blips.Contains(garageBlip)) Main.blips.Remove(garageBlip);
            if (Main.blips.Contains(issiBlip)) Main.blips.Remove(issiBlip);
            if (Main.blips.Contains(mansionBlip)) Main.blips.Remove(mansionBlip);
            if (issiBlip != null) issiBlip.Remove();
            if (garageBlip != null) garageBlip.Remove();
            if (mansionBlip != null) mansionBlip.Remove();
            if (Main.spawnedVehicles.Contains(issi)) Main.spawnedVehicles.Remove(issi);
            if (issi != null)
            {
                issi.LockStatus = VehicleLockStatus.CannotBeTriedToEnter;
                issi.MarkAsNoLongerNeeded();
            }
        }

        /// <summary>
        /// The state that the mission is in, this is just a simple way to manage objectives
        /// </summary>
        private enum MissionState
        {
            GoToCar, StealCar, ReturnToCar, GoToGarage
        }
    }
}