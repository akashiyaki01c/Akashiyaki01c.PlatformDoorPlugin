using System;
namespace Akashiyaki01c.PlatformDoorPlugin
{
    public class Settings
    {
        /// <summary> AssistantTextを表示するか </summary>
        public bool VisibleAssistantText { get; set; } = false;

        /// <summary> ホームドアの閉扉時間 [s] </summary>
        public float PlatformDoorCloseTime { get; set; } = 4.0f;
        /// <summary> ホームドアの開扉時間 [s] </summary>
        public float PlatformDoorOpenTime { get; set; } = 4.0f;

        /// <summary> 開扉時のチャイム音 </summary>
        public string OpenSoundId { get; set; } = "";
        /// <summary> 閉扉時のチャイム音 </summary>
        public string CloseSoundId { get; set; } = "";

        /// <summary> 車両閉扉検知からホームドア閉扉までの遅延時間 [s] </summary>
        public float PlatformDoorCloseDelay { get; set; } = 2.0f;

        /// <summary> 状態灯ストラクチャのId </summary>
        public string DisplayStructureId { get; set; } = "";
        /// <summary> 状態灯ストラクチャの書き換え対象テクスチャId </summary>
        public string DisplayStructureTextureFile { get; set; } = "";
        /// <summary> 無表示時の状態灯画像パス </summary>
        public string NoneImagePath { get; set; } = "";
        /// <summary> 開扉時の状態灯画像パス </summary>
        public string OpenImagePath { get; set; } = "";
        /// <summary> 閉扉時の状態灯画像パス </summary>
        public string CloseImagePath { get; set; } = "";
    }
}
