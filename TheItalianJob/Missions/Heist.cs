using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using SimpleMissions;
using SimpleMissions.Attributes;

namespace TheItalianJob.Missions
{
    [MissionInfo("The Italian Job", "italianHeist", -434.0146f, -2166.265f, 10.32089f, MissionType.Heist, false, "italianSetupWeapon", Characters.All)]
    public class Heist : Mission
    {
        Vector3 truck1Spawn = new Vector3(-125.3826f, 1048.887f, 228.3891f);
        Vector3 truck2Spawn = new Vector3(-203.4017f, 1050.366f, 233.4528f);
        Vector3 truck3Spawn = new Vector3(-84.28207f, 901.2555f, 235.1829f);
        Vehicle truck1, truck2, truck3, issi;
        Blip truck1Blip, truck2Blip, truck3Blip, tunnelBlip, trainBlip;
        Ped truck1Driver, truck2Driver, truck3Driver;
        Ped truck1Pass, truck2Pass, truck3Pass;
        Entity gold;

        MissionState state = MissionState.Start;
        List<Vehicle> searchedVehicles = new List<Vehicle>();

        public override void Start()
        {
            pay = 50000000;

            // Create the vehicles and their blips
            truck1 = World.CreateVehicle(new Model("stockade"), truck1Spawn);
            truck2 = World.CreateVehicle(new Model("stockade"), truck2Spawn);
            truck3 = World.CreateVehicle(new Model("stockade"), truck3Spawn);
            issi = World.CreateVehicle(new Model("issi2"), new Vector3(22.32739f, -649.8838f, 16.08807f));
            issi.InstallModKit();
            truck1.PrimaryColor = VehicleColor.BrushedBlackSteel;
            truck2.PrimaryColor = VehicleColor.BrushedBlackSteel;
            truck3.PrimaryColor = VehicleColor.BrushedBlackSteel;
            truck1.SecondaryColor = VehicleColor.BrushedBlackSteel;
            truck2.SecondaryColor = VehicleColor.BrushedBlackSteel;
            truck3.SecondaryColor = VehicleColor.BrushedBlackSteel;
            truck1Blip = truck1.AddBlip();
            truck2Blip = truck2.AddBlip();
            truck3Blip = truck3.AddBlip();
            truck1Blip.Color = BlipColor.Blue;
            truck2Blip.Color = BlipColor.Blue;
            truck3Blip.Color = BlipColor.Blue;

            // Create the drivers
            truck1Driver = truck1.CreatePedOnSeat(VehicleSeat.Driver, new Model("s_m_m_armoured_01"));
            truck2Driver = truck2.CreatePedOnSeat(VehicleSeat.Driver, new Model("s_m_m_armoured_01"));
            truck3Driver = truck3.CreatePedOnSeat(VehicleSeat.Driver, new Model("s_m_m_armoured_01"));
            truck1Driver.Weapons.Give(GTA.Native.WeaponHash.AssaultrifleMk2, 5000, true, true);
            truck2Driver.Weapons.Give(GTA.Native.WeaponHash.AssaultrifleMk2, 5000, true, true);
            truck3Driver.Weapons.Give(GTA.Native.WeaponHash.AssaultrifleMk2, 5000, true, true);
            truck1Driver.Weapons.Give(GTA.Native.WeaponHash.PistolMk2, 5000, true, true);
            truck2Driver.Weapons.Give(GTA.Native.WeaponHash.PistolMk2, 5000, true, true);
            truck3Driver.Weapons.Give(GTA.Native.WeaponHash.PistolMk2, 5000, true, true);
            truck1Driver.Armor = 100;
            truck2Driver.Armor = 100;
            truck3Driver.Armor = 100;
            truck1Driver.RelationshipGroup = (int)Relationship.Dislike;
            truck2Driver.RelationshipGroup = (int)Relationship.Dislike;
            truck3Driver.RelationshipGroup = (int)Relationship.Dislike;

            // Create the passengers
            truck1Pass = truck1.CreatePedOnSeat(VehicleSeat.Passenger, new Model("s_m_m_armoured_01"));
            truck2Pass = truck2.CreatePedOnSeat(VehicleSeat.Passenger, new Model("s_m_m_armoured_01"));
            truck3Pass = truck3.CreatePedOnSeat(VehicleSeat.Passenger, new Model("s_m_m_armoured_01"));
            truck1Pass.Weapons.Give(GTA.Native.WeaponHash.AssaultrifleMk2, 5000, true, true);
            truck2Pass.Weapons.Give(GTA.Native.WeaponHash.AssaultrifleMk2, 5000, true, true);
            truck3Pass.Weapons.Give(GTA.Native.WeaponHash.AssaultrifleMk2, 5000, true, true);
            truck1Pass.Weapons.Give(GTA.Native.WeaponHash.PistolMk2, 5000, true, true);
            truck2Pass.Weapons.Give(GTA.Native.WeaponHash.PistolMk2, 5000, true, true);
            truck3Pass.Weapons.Give(GTA.Native.WeaponHash.PistolMk2, 5000, true, true);
            truck1Pass.Armor = 100;
            truck2Pass.Armor = 100;
            truck3Pass.Armor = 100;
            truck1Pass.RelationshipGroup = (int)Relationship.Dislike;
            truck2Pass.RelationshipGroup = (int)Relationship.Dislike;
            truck3Pass.RelationshipGroup = (int)Relationship.Dislike;

            // Driver AI
            truck1Driver.Task.CruiseWithVehicle(truck1, 25, 786603);
            truck2Driver.Task.DriveTo(truck2, new Vector3(-942.4401f, -2752.101f, 13.40687f), 10, 20, 786603);
            truck3Driver.Task.CruiseWithVehicle(truck3, 25, 786603);

            // Add the gold
            gold = World.CreateProp(new Model("prop_large_gold"), Vector3.Zero, false, false);
            gold.AttachTo(truck2, truck2.GetBoneIndex("chassis"), new Vector3(0, -2f, .82f), new Vector3(0, 0, 0));

            // Blips
            tunnelBlip = World.CreateBlip(new Vector3(-64.07829f, -538.8095f, 31.85758f));
            tunnelBlip.Color = BlipColor.Yellow;
            tunnelBlip.ShowRoute = false;
            tunnelBlip.Alpha = 0;
            trainBlip = World.CreateBlip(new Vector3(513.2259f, -639.1615f, 24.27212f));
            trainBlip.Color = BlipColor.Yellow;
            trainBlip.Alpha = 0;
            trainBlip.ShowRoute = false;

            // Add all of the the vehicles, peds and blips to the lists in Main
            Main.spawnedVehicles.Add(truck1);
            Main.spawnedVehicles.Add(truck2);
            Main.spawnedVehicles.Add(truck3);
            Main.spawnedVehicles.Add(issi);
            Main.spawnedPeds.Add(truck1Driver);
            Main.spawnedPeds.Add(truck2Driver);
            Main.spawnedPeds.Add(truck3Driver);
            Main.spawnedPeds.Add(truck1Pass);
            Main.spawnedPeds.Add(truck2Pass);
            Main.spawnedPeds.Add(truck3Pass);
            Main.blips.Add(truck1Blip);
            Main.blips.Add(truck2Blip);
            Main.blips.Add(truck3Blip);
            Main.blips.Add(tunnelBlip);
            Main.blips.Add(trainBlip);

            MissionFunc.SendLesterText("They've sent out two extra trucks as decoys. You'll have to find the one with the gold in it.");
            MissionFunc.SendLesterText("And remember, don't let them get to the airport!");
        }

        public override void Tick()
        {
            if (truck2.IsDead) Fail("The gold was destroyed.");
            if (issi.IsDead) Fail("The issi was destroyed.");
            if (World.GetDistance(new Vector3(-942.4401f, -2752.101f, 13.40687f), truck2.Position) <= 100) Fail("The gold made it to the airport.");

            switch (state)
            {
                case MissionState.Start:
                    UI.ShowSubtitle("Find the ~b~truck~w~ carrying the gold", 15000);
                    state = MissionState.FindGold;
                    break;
                case MissionState.FindGold:
                    if (World.GetDistance(truck1.Position, Game.Player.Character.Position) <= 25)
                    {
                        MissionFunc.SendLesterText("That's not our truck. Keep looking.");
                        truck1Blip.Alpha = 0;
                        searchedVehicles.Add(truck1);
                    }
                    else if(World.GetDistance(truck3.Position, Game.Player.Character.Position) <= 25)
                    {
                        MissionFunc.SendLesterText("That's not our truck. Keep looking.");
                        truck3Blip.Alpha = 0;
                        searchedVehicles.Add(truck3);
                    }
                    else if (World.GetDistance(truck2.Position, Game.Player.Character.Position) <= 25)
                    {
                        MissionFunc.SendLesterText("That's the one! Hijack it then bring it down to the transfer point.");
                        UI.ShowSubtitle("Hijack the ~b~truck.", 15000);
                        state = MissionState.HijackGold;
                    }
                    break;
                case MissionState.ReturnToTruck:
                case MissionState.HijackGold:
                    if(Game.Player.Character.CurrentVehicle == truck2)
                    {
                        MissionFunc.SendLesterText("You got the gold? Good. Bring it to the transfer point in the tunnel.");
                        state = MissionState.GoToTunnel;
                        truck2Blip.Alpha = 0;
                        tunnelBlip.Alpha = 255;
                        tunnelBlip.ShowRoute = true;
                        UI.ShowSubtitle("Go to the ~y~transfer point~w~ in the tunnels.", 15000);
                    }
                    if (truck2Driver.GetKiller() == Game.Player.Character || truck2Pass.GetKiller() == Game.Player.Character)
                        Game.Player.WantedLevel = 5;
                    break;
                case MissionState.GoToTunnel:
                    if(Game.Player.Character.CurrentVehicle != truck2)
                    {
                        state = MissionState.ReturnToTruck;
                        truck2Blip.Alpha = 255;
                        tunnelBlip.Alpha = 0;
                        tunnelBlip.ShowRoute = false;
                        MissionFunc.SendLesterText("Stop wasting time and get back into the truck!");
                        UI.ShowSubtitle("Get back into the ~b~truck.", 15000);
                    }
                    if(World.GetDistance(new Vector3(-64.07829f, -538.8095f, 31.85758f), Game.Player.Character.Position) <= 7.5f && tunnelBlip.Position == new Vector3(-64.07829f, -538.8095f, 31.85758f))
                    {
                        tunnelBlip.Position = new Vector3(13.52527f, -636.3681f, 16.0861f);
                    }
                    if(World.GetDistance(new Vector3(13.52527f, -636.3681f, 16.0861f), Game.Player.Character.Position) <= 5 && tunnelBlip.Position != new Vector3(-64.07829f, -538.8095f, 31.85758f))
                    {
                        Game.Player.Character.Task.LeaveVehicle();
                        tunnelBlip.Alpha = 0;
                        tunnelBlip.ShowRoute = false;
                        truck2.FreezePosition = true;
                        truck2.IsInvincible = true;
                        truck2.LockStatus = VehicleLockStatus.CannotBeTriedToEnter;
                        truck2.OpenDoor(VehicleDoor.BackLeftDoor, true, true);
                        truck2.OpenDoor(VehicleDoor.BackRightDoor, true, true);
                        
                        if (gold.Exists())
                            gold.Delete();

                        state = MissionState.GetIntoIssi;
                        UI.ShowSubtitle("Get into the issi.", 15000);
                    }
                    break;
                case MissionState.GetIntoIssi:
                case MissionState.ReturnToIssi:
                    if (Game.Player.Character.CurrentVehicle == issi)
                    {
                        if (Game.Player.WantedLevel > 0)
                        {
                            state = MissionState.LoseHeat;
                            UI.ShowSubtitle("Lose the heat.", 15000);
                        }
                        else
                        {
                            state = MissionState.GoToTrainYard;
                            UI.ShowSubtitle("Go to the ~y~train yard.", 15000);
                        }
                    }
                    break;
                case MissionState.LoseHeat:
                    if(Game.Player.WantedLevel == 0)
                    {
                        state = MissionState.GoToTrainYard;
                        trainBlip.Alpha = 255;
                        trainBlip.ShowRoute = true;
                        MissionFunc.SendLesterText("Alright, we're almost done this job. Just drop the gold off at the train yard and we'll be good.");
                        UI.ShowSubtitle("Go to the ~y~train yard.", 15000);
                    }
                    if (Game.Player.Character.CurrentVehicle != issi)
                    {
                        state = MissionState.ReturnToIssi;
                        UI.ShowSubtitle("Get back into the issi.", 15000);
                    }
                    break;
                case MissionState.GoToTrainYard:
                    if(Game.Player.Character.CurrentVehicle != issi)
                    {
                        state = MissionState.ReturnToIssi;
                        UI.ShowSubtitle("Get back into the issi.");
                    }
                    else if(World.GetDistance(new Vector3(513.2259f, -639.1615f, 24.27212f), Game.Player.Character.Position) <= 10)
                        Pass();
                    break;
            }
        }

        public override void End()
        {
            Game.Player.Character.Task.LeaveVehicle();
            issi.LockStatus = VehicleLockStatus.CannotBeTriedToEnter;
            issi.FreezePosition = true;
            issi.IsInvincible = true;

            // Clean up everything
            truck1.MarkAsNoLongerNeeded();
            truck2.MarkAsNoLongerNeeded();
            truck3.MarkAsNoLongerNeeded();
            truck1Driver.MarkAsNoLongerNeeded();
            truck2Driver.MarkAsNoLongerNeeded();
            truck3Driver.MarkAsNoLongerNeeded();
            truck1Pass.MarkAsNoLongerNeeded();
            truck2Pass.MarkAsNoLongerNeeded();
            truck3Pass.MarkAsNoLongerNeeded();
            truck1Blip.Remove();
            truck2Blip.Remove();
            truck3Blip.Remove();
            trainBlip.Remove();
            tunnelBlip.Remove();
            if (gold.Exists())
            {
                gold.MarkAsNoLongerNeeded();
                gold.Delete();
            }

            MissionFunc.SendLesterText("I've deposited your cut into your bank account. Enjoy.");
        }

        enum MissionState
        {
            Start, FindGold, HijackGold, LoseHeat, ReturnToTruck, ReturnToIssi, GetIntoIssi, GoToTunnel, GoToTrainYard
        }
    }
}