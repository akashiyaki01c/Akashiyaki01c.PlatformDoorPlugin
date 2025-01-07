using System;
using BveEx.Extensions.Native;
using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;
using Mackoy.Bvets;

namespace Akashiyaki01c.PlatformDoorPlugin
{
    [Plugin(PluginType.MapPlugin)]
    partial class PlatformDoor : AssemblyPluginBase
    {
        private INative Native;

        private readonly AssistantText assistantText;

        private DoorStatus DoorStatus = DoorStatus.Open;
        // ホームドアが開いている率
        private double DoorRate = 1.00;

        private DoorState beforeDoorStateOpen;
        private DoorState beforeDoorStateClose;

        // ホームドア音源
        private Sound OpenSound;
        private Sound CloseSound;

        private bool _zaisenKenchi = false;
        /// <summary> 在線検知信号 </summary>
        internal bool ZaisenKenchi
        {
            get => _zaisenKenchi;
            set
            {
                if (value == _zaisenKenchi)
                {
                    return;
                }
                _zaisenKenchi = value;
                if (value)
                {
                    OnEnterZaisenKenchi();
                }
                else
                {
                    OnExitZaisenKenchi();
                }
            }
        }

        private bool _teiichiKenchi = false;
        /// <summary> 定位置検知信号 </summary>
        internal bool TeiichiKenchi
        {
            get => _teiichiKenchi;
            set
            {
                if (value == _teiichiKenchi)
                {
                    return;
                }
                _teiichiKenchi = value;
                if (value)
                {
                    OnEnterTeiichiKenchi();
                }
                else
                {
                    OnExitTeiichiKenchi();
                }
            }
        }
        /// <summary> 定位置検知開始時刻 </summary>
        private TimeSpan _teiichiKenchiStartTime = new TimeSpan(0);

        private bool _teishaKenchi = false;
        /// <summary> 停車検知信号 </summary>
        internal bool TeishaKenchi
        {
            get => _teishaKenchi;
            set
            {
                if (value == _teishaKenchi)
                {
                    return;
                }
                _teishaKenchi = value;
                if (value)
                {
                    OnEnterTeishaKenchi();
                }
                else
                {
                    OnExitTeishaKenchi();
                }
            }
        }

        public PlatformDoor(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
            Native = Extensions.GetExtension<INative>();
            Native.BeaconPassed += OnBeaconPassed;
            assistantText = new AssistantText(new AssistantSettings() { 
                Scale = 40,
            });
            BveHacker.Assistants.Items.Add(assistantText);
        }

        public override void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
            Native.BeaconPassed -= OnBeaconPassed;
            BveHacker.Assistants.Items.Remove(assistantText);
        }

        public override void Tick(TimeSpan elapsed)
        {
            assistantText.Text = $"{DoorStatus} {DoorRate:P}";

            CheckTeiichi();
            if (IsStartDoorClose())
            {
                OnVehicleDoorClosing();
            }
            if (IsStartDoorOpen())
            {
                OnVehicleDoorOpening();
            }
            CheckDoorSensor();
            if (DoorStatus == DoorStatus.Opening)
            {
                DoorRate += 0.2 * elapsed.TotalSeconds;
                if (DoorRate >= 1)
                {
                    DoorRate = 1;
                    EndOpen();
                }
            }
            else if (DoorStatus == DoorStatus.Closing)
            {
                DoorRate -= 0.2 * elapsed.TotalSeconds;
                if (DoorRate <= 0)
                {
                    DoorRate = 0;
                    EndClose();
                }
            }
        }

        private void OnBeaconPassed(object sender, BeaconPassedEventArgs e)
        {
            switch (e.Type)
            {
                case 1000: // 在線センサー検知
                    ZaisenKenchi = true;
                    break;
                case 1001: // 在線センサー非検知
                    ZaisenKenchi = false;
                    TeiichiKenchi = false;
                    TeishaKenchi = false;
                    break;
                case 1002: // 定位置センサー検知
                    ZaisenKenchi = true;
                    TeiichiKenchi = true;
                    break;
                case 1003: // 定位置センサー非検知
                    TeiichiKenchi = false;
                    TeishaKenchi = false;
                    _teiichiKenchiStartTime = new TimeSpan(0);
                    break;
            }
        }
        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            OpenSound = e.Scenario.Map.Sounds["open"];
            CloseSound = e.Scenario.Map.Sounds["close"];
            OnScenarioCreatedStructure(e);
        }
        private void CheckTeiichi()
        {
            if (TeiichiKenchi && !TeishaKenchi)
            {
                float speed = Native.VehicleState.Speed;
                TimeSpan time = Native.VehicleState.Time;
                if (_teiichiKenchiStartTime.Ticks == 0)
                {
                    // 定位置検知開始時刻を更新
                    _teiichiKenchiStartTime = time;
                }
                if (speed > 1.0)
                {
                    // 1km/h以上の時、定位置検知開始時刻を更新
                    _teiichiKenchiStartTime = time;
                }
                else
                {
                    // 定位置検知開始時刻から0.5秒経過すると、停車検知信号をTrueにする
                    if (time - _teiichiKenchiStartTime > new TimeSpan(0, 0, 1))
                    {
                        TeishaKenchi = true;
                    }
                }
            }
        }
        private void CheckDoorSensor()
        {
            if (vehicleDoorClosingTime != TimeSpan.Zero && 
                DoorStatus == DoorStatus.Open && 
                Native.VehicleState.Time - vehicleDoorClosingTime > new TimeSpan(0, 0, 2))
            {
                vehicleDoorClosingTime = TimeSpan.Zero;
                StartClose();
            }
            else if (vehicleDoorClosingTime != TimeSpan.Zero && 
                DoorStatus == DoorStatus.StopOpening && 
                Native.VehicleState.Time - platformDoorOpeningStopTime > new TimeSpan(0, 0, 1))
            {
                platformDoorOpeningStopTime = TimeSpan.Zero;
                StartClose();
            }
            else if (vehicleDoorClosingTime != TimeSpan.Zero && 
                Native.VehicleState.Time - vehicleDoorClosingTime > new TimeSpan(0, 0, 2))
            {
                vehicleDoorClosingTime = TimeSpan.Zero;
            }

            if (platformDoorClosingStopTime != TimeSpan.Zero && 
                DoorStatus == DoorStatus.StopClosing && 
                Native.VehicleState.Time - platformDoorClosingStopTime > new TimeSpan(0, 0, 1))
            {
                vehicleDoorOpeningTime = TimeSpan.Zero;
                platformDoorClosingStopTime = TimeSpan.Zero;
                StartOpen();
            }

        }
        private bool IsStartDoorClose()
        {
            var nowRightState = BveHacker.Scenario.Vehicle.Doors.GetSide(DoorSide.Right).CarDoors[0].State;
            var nowLeftState = BveHacker.Scenario.Vehicle.Doors.GetSide(DoorSide.Left).CarDoors[0].State;
            var nowState = (nowRightState == DoorState.Close && nowLeftState == DoorState.Close)
                    ? DoorState.Close : DoorState.Open;
            var result = nowState == DoorState.Close && beforeDoorStateClose == DoorState.Open;
            beforeDoorStateClose = nowState;
            return result;
        }
        private bool IsStartDoorOpen()
        {
            var nowRightState = BveHacker.Scenario.Vehicle.Doors.GetSide(DoorSide.Right).CarDoors[0].State;
            var nowLeftState = BveHacker.Scenario.Vehicle.Doors.GetSide(DoorSide.Left).CarDoors[0].State;
            var nowState = (nowRightState == DoorState.Close && nowLeftState == DoorState.Close)
                    ? DoorState.Close : DoorState.Open;
            var result = nowState == DoorState.Open && beforeDoorStateOpen == DoorState.Close;
            beforeDoorStateOpen = nowState;
            return result;
        }
    }
}
