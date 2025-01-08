using System;

namespace Akashiyaki01c.PlatformDoorPlugin
{
    partial class PlatformDoor
    {
        private TimeSpan vehicleDoorOpeningTime;
        private TimeSpan vehicleDoorClosingTime;
        private TimeSpan platformDoorOpeningStopTime;
        private TimeSpan platformDoorClosingStopTime;

        /// <summary> 在線検知信号がONになった際に呼ばれる関数 </summary>
        private void OnEnterZaisenKenchi()
        {
            Console.WriteLine("ZAISEN On");
        }

        /// <summary> 在線検知信号がOFFになった際に呼ばれる関数 </summary>
        private void OnExitZaisenKenchi()
        {
            Console.WriteLine("ZAISEN Off");
            DrawTextureNone();
        }

        /// <summary> 定位置検知信号がONになった際に呼ばれる関数 </summary>
        private void OnEnterTeiichiKenchi()
        {
            Console.WriteLine("TEI-ICHI On");
        }

        /// <summary> 定位置検知信号がOFFになった際に呼ばれる関数 </summary>
        private void OnExitTeiichiKenchi()
        {
            Console.WriteLine("TEI-ICHI Off");
        }

        /// <summary> 停車検知信号がONになった際に呼ばれる関数 </summary>
        private void OnEnterTeishaKenchi()
        {
            Console.WriteLine("STOP On");
            StartOpen();
        }

        /// <summary> 停車検知信号がOFFになった際に呼ばれる関数 </summary>
        private void OnExitTeishaKenchi()
        {
            Console.WriteLine("STOP Off");
        }

        /// <summary> 車掌が車両側の扉を開こうとした際に呼ばれる関数 </summary>
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

        /// <summary> 車掌が車両側の扉を閉めようとした際に呼ばれる関数 </summary>
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
