# PlatformDoorPlugin

BveEXを用いてホームドアの挙動を再現するプラグインです。

## 機能

* ホームドア開閉時の警告音を鳴らす(再開閉に対応)
* ホームドア状態灯を表示させる

### 再現したホームドア仕様

* 開扉処理
  * 定位置に停車したことを検知すると自動で開扉する。
  * 閉状態で車両が開扉したことを検知すると自動で開扉する。
* 閉扉処理
  * ドアが閉まることを検知すると自動で閉扉する。

### 取得できるタイミング
* 在線検知したとき、しなくなったとき
* 定位置検知したとき、しなくなったとき
* 停車を検知したとき、しなくなったとき
* 車両の扉が開き始めたとき、閉まり始めたとき

### 地上子の設定

## 使い方

[リリースページ](https://github.com/akashiyaki01c/Akashiyaki01c.PlatformDoorPlugin/releases) から `PlatformDoorPlugin.dll` をダウンロードします。

シナリオ内に配置した `PlatformDoorPlugin.dll` と同じディレクトリ内に設定項目を記述した `PlatformDoorPlugin.xml` を配置します。

### PlatformDoorPlugin.xml の内容

```xml
<?xml version="1.0" encoding="utf-8"?>
<Settings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <VisibleAssistantText>false</VisibleAssistantText> <!-- デバッグ用テキストを表示するか -->
    <PlatformDoorCloseTime>4.0</PlatformDoorCloseTime> <!-- ホームドアの閉扉時間 [s] -->
    <PlatformDoorOpenTime>4.0</PlatformDoorOpenTime> <!-- ホームドアの開扉時間 [s] -->
    <OpenSoundId>open</OpenSoundId> <!-- 開扉時のチャイム音 -->
    <CloseSoundId>close</CloseSoundId> <!-- 閉扉時のチャイム音 -->
    <PlatformDoorCloseDelay>2.0</PlatformDoorCloseDelay> <!-- 車両閉扉検知からホームドア閉扉までの遅延時間 [s] -->
    <DisplayStructureId>platformdoor</DisplayStructureId> <!-- 状態灯ストラクチャのId -->
    <DisplayStructureTextureFile>platformdoor-none.png</DisplayStructureTextureFile> <!-- 状態灯ストラクチャの書き換え対象テクスチャId -->
    <NoneImagePath>./none.png</NoneImagePath> <!-- 無表示時の状態灯画像パス -->
    <OpenImagePath>./open.png</OpenImagePath> <!-- 開扉時の状態灯画像パス -->
    <CloseImagePath>./close.png</CloseImagePath> <!-- 閉扉時の状態灯画像パス -->
</Settings>
```

## 使用ライブラリ

### [BveEX.*](https://github.com/automatic9045/BveEX) (PolyForm Noncommercial License 1.0.0)

Copyright (c) 2022- automatic9045