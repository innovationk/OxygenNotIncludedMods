# OxygenNotIncludedMods

Personal mod named SlowRunMod for the video game Oxygen Not Included following [Cairath tutorial](https://github.com/Cairath/Oxygen-Not-Included-Modding/wiki) 

## Requirement

- Visual Studio Community 
- [dotPeek](https://www.jetbrains.com/decompiler/download/#section=web-installer) to decompile game libraries and to be able to read the source code.

## Cheatsheet

OriginalSources : exported file games from C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

Required game files should be copied from C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed to SlowRunMod\lib

Open the project > Right click on the solution and Manage NuGet > install Lib.Harmony

## FYI

Remember to compile it as Release version.

Move only the mod's dll (not any of the game files that are being referenced) in:
%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\Dev\
└── SlowRunMod\
    ├── SlowRunMod.dll
    ├── mod.yaml
    └── mod_info.yaml

On other platforms, the directories are:
    Linux: ~/.config/unity3d/Klei/OxygenNotIncluded/mods
    Mac: ~/Library/Application Support/unity.Klei.Oxygen Not Included/mods