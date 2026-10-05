// Decompiled with JetBrains decompiler
// Type: KMod.EventType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace KMod;

public enum EventType
{
  LoadError,
  NotFound,
  InstallInfoInaccessible,
  OutOfOrder,
  ExpectedActive,
  ExpectedInactive,
  ActiveDuringCrash,
  InstallFailed,
  Installed,
  Uninstalled,
  CannotInstall,
  VersionUpdate,
  AvailableContentChanged,
  RestartRequested,
  BadWorldGen,
  Deactivated,
  DisabledEarlyAccess,
  DownloadFailed,
}
