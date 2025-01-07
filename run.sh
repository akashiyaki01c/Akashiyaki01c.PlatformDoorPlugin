#!/bin/bash

msbuild -p:Configuration=Release
cp ./bin/Release/Akashiyaki01c.PlatformDoorPlugin.dll ./TestScenario/Basic/Akashiyaki01c.PlatformDoorPlugin.dll
wine ~/Winapp/Bve5.8/BveTs.exe ./TestScenario/scenario.ini