using System;

namespace Akashiyaki01c.PlatformDoorPlugin
{
    partial class PlatformDoor
    {
        private TimeSpan vehicleDoorOpeningTime;
        private TimeSpan vehicleDoorClosingTime;
        private TimeSpan platformDoorOpeningStopTime;
        private TimeSpan platformDoorClosingStopTime;

        private void OnEnterZaisenKenchi()
        {
            Console.WriteLine("ZAISEN On");
        }

        private void OnExitZaisenKenchi()
        {
            Console.WriteLine("ZAISEN Off");
            DrawTextureNone();
        }

        private void OnEnterTeiichiKenchi()
        {
            Console.WriteLine("TEI-ICHI On");
        }

        private void OnExitTeiichiKenchi()
        {
            Console.WriteLine("TEI-ICHI Off");
        }

        private void OnEnterTeishaKenchi()
        {
            Console.WriteLine("STOP On");
            StartOpen();
        }

        private void OnExitTeishaKenchi()
        {
            Console.WriteLine("STOP Off");
        }

        private void OnVehicleDoorOpening()
        {
            Console.WriteLine("Vehicle Door Opening");
            vehicleDoorOpeningTime = Native.VehicleState.Time;
            if (DoorStatus == DoorStatus.Closing)
            {
                platformDoorClosingStopTime = Native.VehicleState.Time;
                DoorStatus = DoorStatus.StopClosing;
                StopOpen();
            }
        }

        private void OnVehicleDoorClosing()
        {
            Console.WriteLine("Vehicle Door Closing");
            vehicleDoorClosingTime = Native.VehicleState.Time;
            if (DoorStatus == DoorStatus.Opening)
            {
                platformDoorOpeningStopTime = Native.VehicleState.Time;
                DoorStatus = DoorStatus.StopOpening;
                StopClose();
            }
        }

        private void StartOpen()
        {
            Console.WriteLine("platformdoor Opening");
            DoorStatus = DoorStatus.Opening;
            OpenSound?.PlayLooping(1, 1, 0, 0);
            DrawTextureOpen();
            // DoorRate = 0;
        }

        private void EndOpen()
        {
            Console.WriteLine("platformdoor Opened");
            DoorStatus = DoorStatus.Open;
            OpenSound?.Stop(100);
            if (DoorRate == 0)
            {
                DoorRate = 1;
            }
        }

        private void StopOpen()
        {
            Console.WriteLine("platformdoor Stopped Open");
            OpenSound?.Stop(100);
            CloseSound?.Stop(100);
        }

        private void StartClose()
        {
            Console.WriteLine("platformdoor Closing");
            DoorStatus = DoorStatus.Closing;
            CloseSound?.PlayLooping(1, 1, 0, 0);
            // DoorRate = 1;
        }

        private void EndClose()
        {
            Console.WriteLine("platformdoor Closed");
            DoorStatus = DoorStatus.Close;
            CloseSound?.Stop(100);
            DrawTextureClose();
            if (DoorRate == 1)
            {
                DoorRate = 0;
            }
        }

        private void StopClose()
        {
            Console.WriteLine("platformdoor Stopped Close");
            OpenSound?.Stop(100);
            CloseSound?.Stop(100);
        }
    }
}