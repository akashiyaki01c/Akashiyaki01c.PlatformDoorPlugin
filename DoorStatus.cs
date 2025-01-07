using System;
namespace Akashiyaki01c.PlatformDoorPlugin
{
    public enum DoorStatus
    {
        /// <summary>
        /// 閉状態
        /// </summary>
        Close,
        /// <summary>
        /// 開状態
        /// </summary>
        Open,
        /// <summary>
        /// 閉操作中
        /// </summary>
        Closing,
        /// <summary>
        /// 開操作中
        /// </summary>
        Opening,
        /// <summary>
        /// 閉操作の停止中
        /// </summary>
        StopClosing,
        /// <summary>
        /// 開操作の停止中
        /// </summary>
        StopOpening,
    }
}
