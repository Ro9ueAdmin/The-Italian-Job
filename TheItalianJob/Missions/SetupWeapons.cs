using SimpleMissions;
using SimpleMissions.Attributes;

using GTA;
using GTA.Math;

namespace TheItalianJob.Missions
{
    [MissionInfo("The Italian Job - Weapons", "italianSetupWeapon", -434.0146f, -2166.265f, 10.32089f, MissionType.HeistSetup, false, "italianSetupIssi")]
    public class SetupWeapons : Mission
    {
        WeaponMissionState state = WeaponMissionState.Start;
        WeaponMissionState previousState = WeaponMissionState.HijackVan;

        Vehicle issi;
        Vehicle van;
        Blip vanBlip;
        Blip garageBlip;

        Ped driver;
        Ped passenger;

        public override void Start()
        {
            this.pay = 500;
            issi = World.CreateVehicle(new Model("issi2"), new Vector3(-445.293f, -2179.457f, 10.31818f), 181.4f);
            van = World.CreateVehicle(new Model("fbi2"), new Vector3(2538.767f, -582.5017f, 65.25363f));
            vanBlip = van.AddBlip();
            vanBlip.Color = BlipColor.Blue;
            vanBlip.ShowRoute = true;
            driver = van.CreatePedOnSeat(VehicleSeat.Driver, new Model("s_m_y_swat_01"));
            passenger = van.CreatePedOnSeat(VehicleSeat.Passenger, new Model("s_m_y_swat_01"));
            driver.Task.CruiseWithVehicle(van, 30, 786603);
            driver.Weapons.Give(GTA.Native.WeaponHash.CarbineRifleMk2, 5000, true, true);
            passenger.Weapons.Give(GTA.Native.WeaponHash.CarbineRifleMk2, 5000, true, true);
            garageBlip = World.CreateBlip(new Vector3(-439.8835f, -2179.444f, 9.94509f));
            garageBlip.Color = BlipColor.Yellow;
            garageBlip.ShowRoute = false;
            garageBlip.Alpha = 0;

            Main.spawnedVehicles.Add(van);
            Main.spawnedVehicles.Add(issi);
            Main.spawnedPeds.Add(driver);
            Main.spawnedPeds.Add(passenger);
            Main.blips.Add(vanBlip);
            Main.blips.Add(garageBlip);
        }

        public override void Tick()
        {
            if (!van.IsAlive) Fail("The van was destroyed!");

            if (state != WeaponMissionState.LoseHeat && Game.Player.WantedLevel > 0)
            {
                previousState = state;
                state = WeaponMissionState.LoseHeat;
            }

            switch (state)
            {
                case WeaponMissionState.Start:
                    UI.ShowSubtitle("Hijack the ~b~Van.", 15000);
                    state = WeaponMissionState.HijackVan;
                    break;
                case WeaponMissionState.HijackVan:
                case WeaponMissionState.ReturnToVan:
                    if (Game.Player.Character.CurrentVehicle == van)
                    {
                        state = WeaponMissionState.BringVanToGarage;
                        vanBlip.Alpha = 0;
                        garageBlip.Alpha = 255;
                        garageBlip.ShowRoute = true;
                        UI.ShowSubtitle("Bring the ~b~Van~w~ to the ~y~Garage.", 15000);
                    }
                    break;
                case WeaponMissionState.BringVanToGarage:
                    if (Game.Player.Character.CurrentVehicle != van)
                    {
                        state = WeaponMissionState.ReturnToVan;
                        vanBlip.Alpha = 255;
                        garageBlip.Alpha = 0;
                        garageBlip.ShowRoute = false;
                        UI.ShowSubtitle("Get back into the ~b~Van", 15000);
                    }
                    else if (World.GetDistance(new Vector3(-439.8835f, -2179.444f, 9.94509f), Game.Player.Character.Position) <= 5)
                    {
                        Pass();
                    }
                    break;

                case WeaponMissionState.LoseHeat:
                    UI.ShowSubtitle("Lose the heat.", 1);
                    if (Game.Player.WantedLevel == 0) state = previousState;
                    break;
            }
        }

        public override void End()
        {
            Game.Player.Character.Task.LeaveVehicle();

            if (van != null)
            {
                van.FreezePosition = true;
                van.IsInvincible = true;
            }

            if (issi != null) issi.MarkAsNoLongerNeeded();
            if (van != null) van.MarkAsNoLongerNeeded();
            if (vanBlip != null) vanBlip.Remove();
            if (garageBlip != null)
                garageBlip.Remove();
            if (driver != null)
            {
                driver.MarkAsNoLongerNeeded();
                if (passenger != null)
                    passenger.MarkAsNoLongerNeeded();
            }
        }

        internal enum WeaponMissionState
        {
            Start, HijackVan, ReturnToVan, BringVanToGarage, LoseHeat
        }
    }
}
